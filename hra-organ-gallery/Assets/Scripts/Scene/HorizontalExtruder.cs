using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.InputSystem;

public enum BodySystem { undefined, integumentary, nervous, respiratory, cardio, digestive, musculoskeletal, lymphatic, urinary, fetal, reproductive }

public class HorizontalExtruder : MonoBehaviour
{
    public static event Action<List<SystemObjectPair>> OnBodySystemsReady;

    public List<SystemObjectPair> SystemsObjs = new List<SystemObjectPair>();



    [SerializeField] private string bodySystemsData;

    [SerializeField] private SceneBuilder sceneBuilder;



    private Dictionary<string, string> mappings = new Dictionary<string, string>();
    private string[] systems;

    private void Awake()
    {
        ReadCsv();
        systems = System.Enum.GetNames(typeof(BodySystem));
    }

    void ReadCsv()
    {
        using (var reader = Utils.ReadCsv(bodySystemsData))
        {
            while (!reader.EndOfStream)
            {
                var line = reader.ReadLine();
                string sceneGraph = line.Split(',')[0];

                if (sceneGraph != "sceneGraph" && sceneGraph != "")
                {
                    mappings.Add(sceneGraph, line.Split(',')[2]);
                }
            }
        }
    }

    private void OnEnable()
    {
        SceneBuilder.OnSceneBuilt += GetSystemAndDefaultPosition; //remove getting default pos + rot from Kumar's code once pulled in


    }



    private void OnDestroy()
    {
        SceneBuilder.OnSceneBuilt -= GetSystemAndDefaultPosition;

    }



    void SetAllActiveOrgansToDefaultPosition()
    {
        for (int i = 0; i < sceneBuilder.Organs.Count; i++)
        {
            sceneBuilder.Organs[i].transform.position = sceneBuilder.Organs[i].GetComponent<OrganData>().DefaultPosition;
        }
    }


    void GetSystemAndDefaultPosition()
    {
        //OrganData[] organs = FindObjectsOfType<OrganData>();
        List<OrganData> allOrgans = new List<OrganData>();
        foreach (var o in sceneBuilder.Organs)
        {
            allOrgans.Add(o.GetComponent<OrganData>());
        }

        for (int i = 0; i < allOrgans.Count; i++)
        {
            OrganData organData = allOrgans[i];
            _ = mappings.TryGetValue(organData.SceneGraph, out string system);
            organData.BodySystem = (BodySystem)Enum.Parse(typeof(BodySystem), system);
            organData.DefaultPosition = organData.gameObject.transform.position;
        }

        for (int i = 0; i < systems.Length; i++)
        {
            List<GameObject> gameObjects = new List<GameObject>();

            foreach (var organData in allOrgans)
            {
                if (organData.BodySystem == (BodySystem)Enum.Parse(typeof(BodySystem), systems[i]))
                {
                    gameObjects.Add(organData.gameObject);
                }

            }
            SystemsObjs.Add(
               new SystemObjectPair(systems[i], gameObjects));
        }

        AssignOrgansIntoSexedList();
        OnBodySystemsReady?.Invoke(SystemsObjs);
    }

    void AssignOrgansIntoSexedList()
    {
        for (int i = 0; i < SystemsObjs.Count; i++)
        {
            for (int n = 0; n < SystemsObjs[i].GameObjects.Count; n++)
            {
                switch (SystemsObjs[i].GameObjects[n].GetComponent<OrganData>().DonorSex.ToLower())
                {
                    case "male":
                        SystemsObjs[i].GameObjectsBySex[0].Add(SystemsObjs[i].GameObjects[n]);
                        break;
                    case "female":
                        SystemsObjs[i].GameObjectsBySex[1].Add(SystemsObjs[i].GameObjects[n]);
                        break;
                    default:
                        break;
                }
            }
        }
    }
}

[Serializable]
public struct SystemObjectPair
{
    public string System;
    public List<GameObject> GameObjects;
    public List<List<GameObject>> GameObjectsBySex;
    [SerializeField]
    public Vector3 SystemPosition
    {
        get { return new Vector3(GameObjects[0].transform.position.x, GameObjects[0].transform.position.y, GameObjects[0].transform.position.z); }
    }

    public SystemObjectPair(string system, List<GameObject> list)
    {
        this.System = system;
        this.GameObjects = list;
        this.GameObjectsBySex = new List<List<GameObject>>() {
            new List<GameObject>(),
            new List<GameObject>()
        };

    }
}

