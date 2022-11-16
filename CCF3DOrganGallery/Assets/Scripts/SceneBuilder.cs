using System;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using UnityEngine.Networking;
using TMPro;
using UnityEngine.InputSystem;

public class SceneBuilder : MonoBehaviour
{
    public delegate void SceneBuilt();
    public static event SceneBuilt OnSceneBuilt;

    public static event Action OnOrgansLoaded;

    public List<GameObject> TissueBlocks;
    public List<GameObject> Organs;
    public List<string> MaleEntityIds;
    public List<string> FemaleEntityIds;
    public List<IdTypeMapping> mappingTissueBlocksWithCT = null;
    public List<GameObject> TissueBlocksWithCT;

    public int NumberOfHubmapIds
    {
        get { return numberOfHubmapIds; }
    }

    [SerializeField] private SceneConfiguration sceneConfiguration;
    [SerializeField] private GameObject preTissueBlock;
    [SerializeField] private GameObject preTeleportationAnchor;
    [SerializeField] private DataFetcher dataFetcher;
    [SerializeField] private NodeArray nodeArray;
    [SerializeField] private GameObject loaderParent;

    private int modelsLoaded;
    private int numberOfHubmapIds;

    // Reference to button that instructs tissue-blocks / organs to float back to original position
    public InputActionReference floatBackInputActionReference;

    // Dict to store default position, rotation adn scale values of organs
    private readonly Dictionary<GameObject, List<Vector3>> _organDefaults = new Dictionary<GameObject, List<Vector3>>();

    //driver code 
    private async void Start()
    {
        sceneConfiguration = GetComponent<SceneConfiguration>();
        await GetNodes(sceneConfiguration.BuildUrl());
        await GetOrgans();

        OnOrgansLoaded?.Invoke();

        CreateAndPlaceTissueBlocks();
        ParentTissueBlocksToOrgans(TissueBlocks, Organs);

        for (int i = 0; i < Organs.Count; i++)
        {
            OrganData data = Organs[i].GetComponent<OrganData>();
            Organs[i].gameObject.SetActive(sceneConfiguration.IdsOrgansToShow.Contains(data.RepresentationOf));
        }
    }

    List<GameObject> GetOrgansToShow(List<string> list)
    {
        List<GameObject> result = new List<GameObject>();

        return result;
    }

    private void OnEnable()
    {
        //assign the list of tissue blocks with CTs from GitHubChecker to the global property tissueBlocksWithCT 
        GitHubChecker.GitHubCTChecked += (list) => { mappingTissueBlocksWithCT = list; };
    }

    public async Task GetNodes(string url)
    {
        DataFetcher httpClient = dataFetcher;
        nodeArray = await httpClient.Get(url);
    }

    public async Task GetOrgans()
    {
        List<Task<GameObject>> tasks = new List<Task<GameObject>>();
        List<GameObject> loaders = new List<GameObject>();

        foreach (var node in nodeArray.nodes)
        {
            if (node.scenegraph == null) break;
            GameObject g = new GameObject()
            {
                name = "Loader"
            };
            g.AddComponent<ModelLoader>();
            loaders.Add(g);
            g.transform.parent = loaderParent.transform;
            Task<GameObject> t = g.GetComponent<ModelLoader>().GetModel(node.scenegraph);
            tasks.Add(t);
        }

        await Task.WhenAll(tasks);

        for (int i = 0; i < tasks.Count; i++)
        {
            Organs.Add(tasks[i].Result);
            SetOrganData(tasks[i].Result, nodeArray.nodes[i]);
        }

        for (int i = 0; i < Organs.Count; i++)
        {
            //add teleportation anchors and set label
            GameObject anchor = Instantiate(preTeleportationAnchor);
            anchor.SetActive(true);
            anchor.transform.parent = Organs[i].transform;

            //add tooltip to teleportation anchor label
            TMP_Text label = anchor.GetComponentInChildren<TMP_Text>();
            label.text = Organs[i].GetComponent<OrganData>().tooltip;

            //place organ
            PlaceOrgan(Organs[i], nodeArray.nodes[i]);
            SetOrganOpacity(Organs[i], nodeArray.nodes[i].opacity);
            //AddPullout(Organs[i]);
        }

    }

    void AddPullout(GameObject organWrapper)
    {
        var organChild = organWrapper.transform.GetChild(0);
        organChild.gameObject.AddComponent<OnExtrudeActivateFloat>();
    }

    private async Task GetAllHubmapIds(List<GameObject> tissueBlocks)
    {
        for (int i = 0; i < tissueBlocks.Count; i++)
        {
            TissueBlockData data = tissueBlocks[i].GetComponent<TissueBlockData>();

            // take entity id
            string entityId = data.EntityId;

            //get hubmap id
            HubmapIdHolder response = new HubmapIdHolder();

            if (entityId.Contains("hubmap"))
            {
                response = await Get(entityId, response);

                //assign hubmap id
                data.HubmapId = response.hubmap_id;
            }
        }
    }

    private async Task<HubmapIdHolder> Get(string url, HubmapIdHolder response)
    {
        try
        {
            using var www = UnityWebRequest.Get(url);
            var operation = www.SendWebRequest();

            while (!operation.isDone)
                await Task.Yield();

            if (www.result != UnityWebRequest.Result.Success)
                Debug.LogError($"Failed: {www.error}");

            var result = www.downloadHandler.text;

            var text = www.downloadHandler.text;
            response = JsonUtility.FromJson<HubmapIdHolder>(text);
            return response;
        }
        catch (Exception ex)
        {
            Debug.LogError($"{nameof(Get)} failed: {ex.Message}");
            return default;
        }
    }
    void PlaceOrgan(GameObject organ, Node node) //-1, 1, -1 -> for scale
    {
        Matrix4x4 reflected = ReflectZ() * MatrixExtensions.BuildMatrix(node.transformMatrix);
        organ.transform.position = reflected.GetPosition();
        organ.transform.rotation = new Quaternion(0f, 0f, 0f, 1f); //hard-coded to avoid bug when running natively on Quest 2
        organ.transform.localScale = new Vector3(
            reflected.lossyScale.x,
            reflected.lossyScale.y,
            -reflected.lossyScale.z
        );

    }
    public static void SetOrganOpacity(GameObject organWrapper, float alpha)
    {
        List<Transform> list = new List<Transform>();
        list = LeavesFinder.FindLeaves(organWrapper.transform.GetChild(0), list);

        foreach (var item in list)
        {
            Renderer renderer = item.GetComponent<MeshRenderer>();

            if (renderer == null) continue;
            Color updatedColor = renderer.material.color;
            updatedColor.a = alpha;
            renderer.material.color = updatedColor;

            Shader standard;
            standard = Shader.Find("Standard");
            renderer.material.shader = standard;
            MaterialExtensions.ToFadeMode(renderer.material);
        }


    }

    void CreateAndPlaceTissueBlocks()
    {
        for (int i = 1; i < nodeArray.nodes.Length; i++)
        {
            if (nodeArray.nodes[i].scenegraph != null) continue;
            Matrix4x4 reflected = ReflectZ() * MatrixExtensions.BuildMatrix(nodeArray.nodes[i].transformMatrix);
            GameObject block = Instantiate(
                preTissueBlock,
                reflected.GetPosition(),
                reflected.rotation
       );
            block.transform.localScale = reflected.lossyScale * 2f;
            SetTissueBlockData(block, nodeArray.nodes[i]);
            SetCellTypeData(block);
            TissueBlocks.Add(block);
        }
    }

    Matrix4x4 ReflectZ()
    {
        var result = new Matrix4x4(
            new Vector4(1, 0, 0, 0),
            new Vector4(0, 1, 0, 0),
            new Vector4(0, 0, -1, 0),
            new Vector4(0, 0, 0, 1)
        );
        return result;
    }

    void SetTissueBlockData(GameObject obj, Node node)
    {
        TissueBlockData dataComponent = obj.AddComponent<TissueBlockData>();
        dataComponent.EntityId = node.entityId;
        dataComponent.Name = node.name;
        dataComponent.Tooltip = node.tooltip;
        dataComponent.CcfAnnotations = node.ccf_annotations;
    }

    void SetCellTypeData(GameObject obj)
    {
        obj.AddComponent<CellTypeData>();
        obj.AddComponent<CellTypeDataFetcher>();
    }

    void SetOrganData(GameObject obj, Node node)
    {
        OrganData dataComponent = obj.AddComponent<OrganData>();
        dataComponent.SceneGraph = node.scenegraph;
        dataComponent.RepresentationOf = node.representation_of;
        dataComponent.tooltip = node.tooltip;
    }

    async void ParentTissueBlocksToOrgans(List<GameObject> tissueBlocks, List<GameObject> organs)
    {
        // Add back to AssignEntityIdsToDonorSexLists if delay bug
        MaleEntityIds = await GetEntityIdsBySex("https://ccf-api.hubmapconsortium.org/v1/tissue-blocks?sex=male");
        FemaleEntityIds = await GetEntityIdsBySex("https://ccf-api.hubmapconsortium.org/v1/tissue-blocks?sex=female");

        // assign donor sex to organ
        await GetOrganSex();

        // assign donor sex to tissue block and parent to organ
        for (int i = 0; i < TissueBlocks.Count; i++)
        {
            TissueBlockData tissueData = TissueBlocks[i].GetComponent<TissueBlockData>();
            if (MaleEntityIds.Contains(tissueData.EntityId))
            {
                tissueData.DonorSex = "Male";
            }
            else
            {
                tissueData.DonorSex = "Female";
            }

            for (int j = 0; j < Organs.Count; j++)
            {
                OrganData organData = Organs[j].GetComponent<OrganData>();

                foreach (var annotation in tissueData.CcfAnnotations)
                {
                    if (organData.RepresentationOf == annotation && organData.DonorSex == tissueData.DonorSex)
                    {
                        TissueBlocks[i].transform.parent = Organs[j].transform.GetChild(0).transform;
                        break;
                    }
                }
            }
        }

        var tasks = new List<Task>();
        for (int i = 0; i < tissueBlocks.Count; i++)
        {
            var progressHubmapIds = new Progress<bool>((value) =>
            {
                if (value) numberOfHubmapIds++;
            });

            tasks.Add(tissueBlocks[i].GetComponent<HuBMAPIDFetcher>().FromEntityIdGetHubmapId(progressHubmapIds));
        }


        tasks.Add(GetTissueBlocksWithCellTypes());


        await Task.WhenAll(tasks);

        // trigger OnSceneBuilt event
        OnSceneBuilt?.Invoke();
    }

    private async Task GetTissueBlocksWithCellTypes()
    {
        while (mappingTissueBlocksWithCT.Count == 0)
        {
            await Task.Yield();
        }

        for (int i = 0; i < TissueBlocks.Count; i++)
        {
            TissueBlockData data = TissueBlocks[i].GetComponent<TissueBlockData>();
            for (int j = 0; j < mappingTissueBlocksWithCT.Count; j++)
            {
                IdTypeMapping mapping = mappingTissueBlocksWithCT[j];

                if (data.HubmapId == mapping.ParentId)
                {
                    if (!TissueBlocksWithCT.Contains(data.gameObject))
                    {
                        TissueBlocksWithCT.Add(data.gameObject);
                    }

                }
            }
        }
    }

    public async Task<List<string>> GetEntityIdsBySex(string url)
    {
        List<string> result = new List<string>();
        DataFetcher httpClient = dataFetcher;
        NodeArray nodeArray = await httpClient.Get(url);
        foreach (var node in nodeArray.nodes)
        {
            result.Add(node.jsonLdId);
        }
        return result;
    }

    public async Task GetOrganSex()
    {
        DataFetcher httpClient = dataFetcher;
        NodeArray nodeArray = await httpClient.Get("https://ccf-api.hubmapconsortium.org/v1/reference-organs");
        // Debug.Log(nodeArray.nodes.Length);
        foreach (var organ in Organs)
        {
            OrganData organData = organ.GetComponent<OrganData>();

            foreach (var node in nodeArray.nodes)
            {
                // Debug.Log("file: " + node.reference_organ);
                if (organData.SceneGraph == node.glbObject.file)
                {
                    organData.DonorSex = node.sex;
                }
            }
        }
    }

    public void InitializeOrganDefaultValues(GameObject o) // o = organ
    {
        var values = new List<Vector3>() { o.transform.position, o.transform.rotation.eulerAngles, o.transform.localScale };
        if (!_organDefaults.ContainsKey(o))
        {
            _organDefaults.Add(o, values);
        }
        // Calls UpdateOrganDefaultValues() function from FloatBackOrgan script attached to each organ
        o.GetComponent<FloatBackOrgan>().UpdateOrganDefaultValues(values);
    }

    public List<Vector3> GetDefaultValuesForOrgan(GameObject organ)
    {
        return _organDefaults[organ];
    }
}

public class HubmapIdArray
{
    [SerializeField] public HubmapIdHolder[] hubmapIdHolder;
}

public class HubmapIdHolder
{
    public string hubmap_id;
}