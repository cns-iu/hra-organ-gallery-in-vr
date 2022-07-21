using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using UnityEngine.Networking;

public class SceneBuilder : MonoBehaviour
{
    public delegate void SceneBuilt();
    public static event SceneBuilt OnSceneBuilt;
    public List<GameObject> TissueBlocks;
    public List<GameObject> Organs;
    public List<string> MaleEntityIds;
    public List<string> FemaleEntityIds;

    [SerializeField] private SceneConfiguration sceneConfiguration;
    [SerializeField] private GameObject preTissueBlock;
    [SerializeField] private DataFetcher dataFetcher;
    [SerializeField] private NodeArray nodeArray;
    [SerializeField] private GameObject loaderParent;

    private int modelsLoaded;
    private int numberOfHubmapIds;
    public int NumberOfHubmapIds
    {
        get { return numberOfHubmapIds; }
    }

    //driver code 
    private async void Start()
    {
        sceneConfiguration = GetComponent<SceneConfiguration>();
        await GetNodes(sceneConfiguration.BuildUrl());
        await GetOrgans();

        CreateAndPlaceTissueBlocks();
        ParentTissueBlocksToOrgans(TissueBlocks, Organs);
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

            await Task.Yield();

            SetOrganData(t.Result, node);
            Organs.Add(t.Result);
        }

        await Task.WhenAll(tasks);

        Debug.Log("got em all");

        foreach (var o in Organs)
        {
            foreach (var node in nodeArray.nodes)
            {
                if (o.GetComponent<OrganData>().SceneGraph == node.scenegraph)
                {
                    PlaceOrgan(o, node);
                    Debug.Log("setting opacity for: " + node.reference_organ);
                    SetOrganOpacity(o, node.opacity);
                }
            }
        }
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
                // Debug.Log(data.HubmapId);
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
        organ.transform.rotation = reflected.rotation;
        organ.transform.localScale = new Vector3(
            reflected.lossyScale.x,
            reflected.lossyScale.y,
            -reflected.lossyScale.z
        );
    }

    void SetOrganOpacity(GameObject organWrapper, float alpha)
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
                        TissueBlocks[i].transform.parent = Organs[j].transform;
                        break;
                    }
                }
            }
        }

        var tasks = new List<Task>();
        for (int i = 0; i < tissueBlocks.Count; i++)
        {
            var progress = new Progress<bool>((value) =>
            {
                if (value) numberOfHubmapIds++;
            });

            tasks.Add(tissueBlocks[i].GetComponent<HuBMAPIDFetcher>().FromEntityIdGetHubmapId(progress));
        }

        await Task.WhenAll(tasks);

        // trigger OnSceneBuilt event
        OnSceneBuilt?.Invoke();
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

    private class HubmapIdArray
    {
        [SerializeField] public HubmapIdHolder[] hubmapIdHolder;
    }

    private class HubmapIdHolder
    {
        public string hubmap_id;
    }

}
