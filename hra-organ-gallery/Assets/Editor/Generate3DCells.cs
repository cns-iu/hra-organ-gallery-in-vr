using System.Collections.Generic;
using System.IO;
using System.Net;
using Newtonsoft.Json;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;
using Assets.Scripts.Shared;

/// <summary>
/// A class to make a call to the HRA 3D Cell Generation API with a cell summary
/// </summary>
public class Generate3DCells : EditorWindow
{
    private SOHraApiAsCellSummaries _summaries;

    private CellTypeAnnotationTool
        _cellTypeAnnotationTool = CellTypeAnnotationTool.Azimuth;

    private string
        _apiUrl = "https://apps.humanatlas.io/api/v1/mesh-3d-cell-population";

    private string _sex = "Male";

    private string _organ = "Left Kidney";

    private string _asLabel = "outer cortex of kidney";

    private string _response = "";

    private string
        _file =
            "https://ccf-ontology.hubmapconsortium.org/objects/v1.2/VH_M_Kidney_L.glb";

    private string _fileSubpath = "VH_M_outer_cortex_of_kidney_L";

    private int _maxCells = 6000;

    private string _savePath = "Assets/Resources";

    private string _saveFileName = "3d_cell_positions.csv";

    private string
        description =
            "Use this window to (1) take a ScriptableObject with the AS-CT HRApop (serialized from downloaded CSV)" +
            ", (2) adjust parameters (tool, sex, organ, AS), then (3) call the hra-3d-cell-generation-api to" +
            "get 3D cells positions. These are then saved to the SAVE FOLDER and the SAVE FILE NAME.";

    private Dictionary<string, string>
        _lookup =
            new Dictionary<string, string>()
            {
                {
                    "https://cdn.humanatlas.io/digital-objects/ref-organ/kidney-male-left/v1.3/assets/3d-vh-m-kidney-l.glb",
                    "Left Kidney"
                },
                { "VH_M_outer_cortex_of_kidney_L", "outer cortex of kidney" },
                { "VH_M_renal_pyramid_L_a", "renal pyramid" },
                { "VH_M_renal_papilla_L_a", "renal papilla"},
                { "https://cdn.humanatlas.io/digital-objects/ref-organ/heart-female/v1.3/assets/3d-vh-f-heart.glb", "Heart"},
                {"VH_F_interventricular_septum","interventricular septum" }
                
            };

    [
        MenuItem(
            "Tools/2. Visualize hra-pop Data/1. Call HRA 3D Cell Generation API")
    ]
    public static void ShowWindow()
    {
        GetWindow<Generate3DCells>("Call 3D Cell Generation API");
    }

    private void OnGUI()
    {
        // Display the description with word wrapping
        EditorGUILayout.LabelField(description, Utils.GetStyleForDescription());

        // Display the description with word wrapping
        EditorGUILayout.LabelField(description, Utils.GetStyleForDescription());

        GUILayout.Label("URL", EditorStyles.boldLabel);
        _apiUrl = EditorGUILayout.TextField("API URL", _apiUrl);

        GUILayout.Label("Input", EditorStyles.boldLabel);

        // Display the ScriptableObject field
        _summaries =
            (SOHraApiAsCellSummaries)
            EditorGUILayout
                .ObjectField("Cell Summary",
                _summaries,
                typeof (SOHraApiAsCellSummaries),
                false);

        GUILayout.Label("Filters", EditorStyles.boldLabel);
        _cellTypeAnnotationTool =
            (CellTypeAnnotationTool)
            EditorGUILayout
                .EnumPopup("Cell Type Annotation Tool",
                CellTypeAnnotationTool.Azimuth);
        _sex = EditorGUILayout.TextField("Sex", _sex);

        GUILayout.Label("Parameters", EditorStyles.boldLabel);
        _file = EditorGUILayout.TextField("File Name", _file);
        _fileSubpath = EditorGUILayout.TextField("File Node", _fileSubpath);

        //not super elegant and safe yet --needs fix
        _maxCells =
            int
                .Parse(EditorGUILayout
                    .TextField("Max. Number of Cells", _maxCells.ToString()));

        if (GUILayout.Button("Call API"))
        {
            CallApi (_summaries);
        }

        GUILayout.Label("Response", EditorStyles.boldLabel);
        _response = EditorGUILayout.TextField("", _response);

        GUILayout.Label("Save Data", EditorStyles.boldLabel);
        _savePath = EditorGUILayout.TextField("Save to Folder", _savePath);
        _saveFileName = EditorGUILayout.TextField("Save As", _saveFileName);

        if (GUILayout.Button("Save to CSV"))
        {
            SaveToCsv (_response);
        }
    }

    private void SaveToCsv(string response)
    {
        File.WriteAllText($"{_savePath}/{_saveFileName}", response);
        AssetDatabase.Refresh();
        Debug
            .Log("CSV downloaded and saved to: " +
            $"{_savePath}/{_saveFileName}");
    }

    private void CallApi(SOHraApiAsCellSummaries summary)
    {
        //get distribution from Scriptable Object from HRA API
        Dictionary<string, float> distribution = GetDistribution(summary);

        //formulate the request with the distribution and user input: file, file subpath (scene node for anatomical structure), and max. number of cells
        string json =
            FormulateRequest(_file, _fileSubpath, _maxCells, distribution);

        // make the POST request
        using (WebClient client = new WebClient())
        {
            try
            {
                UnityWebRequest request = new UnityWebRequest(_apiUrl, "POST");
                client.Headers.Add("User-Agent", "UnityEditor");
                client.Headers[HttpRequestHeader.ContentType] =
                    "application/json";
                _response = client.UploadString(_apiUrl, "POST", json);
            }
            catch (WebException webEx)
            {
                var response = (HttpWebResponse) webEx.Response;
                if (response != null)
                {
                    Debug.LogError("HTTP Status Code: " + response.StatusCode);
                    using (
                        var reader =
                            new StreamReader(response.GetResponseStream())
                    )
                    {
                        Debug.LogError("Response: " + reader.ReadToEnd());
                    }
                }
                else
                {
                    Debug.LogError("Failed to download CSV: " + webEx.Message);
                }
            }
            catch (IOException ioEx)
            {
                Debug.LogError("Failed to save CSV: " + ioEx.Message);
            }
        }
    }

    private string
    FormulateRequest(
        string file,
        string fileSubpath,
        int maxNodes,
        Dictionary<string, float> distribution
    )
    {
        foreach (var key in distribution)
        {
            // Debug.Log (key.GetType());
        }

        // Create the JSON object
        var postData =
            new {
                file = file,
                file_subpath = fileSubpath,
                num_nodes = maxNodes,
                node_distribution = distribution
            };

        string json = JsonConvert.SerializeObject(postData);

        return json;
    }

    private Dictionary<string, float>
    GetDistribution(SOHraApiAsCellSummaries summary)
    {
        Dictionary<string, float> result = new Dictionary<string, float>();
        _summaries
            .rows
            .ForEach(r =>
            {
                bool matchesSex = r.sex == _sex;
                bool matchesTool =
                    r.tool.ToLower() ==
                    _cellTypeAnnotationTool.ToString().ToLower();
                bool matchesOrgan = r.organ == _lookup[_file];
                bool matchesAnatomicalStructure =
                    r.asLabel == _lookup[_fileSubpath];
                if (
                    matchesSex &&
                    matchesTool &&
                    matchesOrgan &&
                    matchesAnatomicalStructure
                )
                {
                    result.Add(r.cellLabel, r.cellPercentage);
                }
            });

        return result;
    }
}
