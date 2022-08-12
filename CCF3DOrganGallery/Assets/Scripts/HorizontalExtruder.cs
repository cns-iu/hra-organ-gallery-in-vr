using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

public enum BodySystem { undefined, integumentary, nervous, respiratory, cardio, digestive, musculoskeletal, lymphatic, urinary, fetal, reproductive }

public class HorizontalExtruder : MonoBehaviour
{
    public KeyHandler upArrowHandler = null;
    public KeyHandler downArrowHandler = null;

    [SerializeField] private float currentStep;
    [SerializeField] private float offset;
    [SerializeField] private float scalingFactor;
    [SerializeField] private float maxDistance;
    [SerializeField] private string filename;
    [SerializeField] private Dictionary<string, string> mappings = new Dictionary<string, string>();

    private List<SystemObjectPair> SystemsObjs = new List<SystemObjectPair>();
    private string[] systems;

    private void Awake()
    {
        ReadCsv();
        systems = System.Enum.GetNames(typeof(BodySystem));
    }

    void ReadCsv()
    {
        using (var reader = new StreamReader("Assets/Data/" + filename + ".csv"))
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
        SceneBuilder.OnSceneBuilt += GetSystemAndDefaultPosition;
        upArrowHandler.keyHeld += AdjustExtrusion;
        downArrowHandler.keyHeld += AdjustExtrusion;
    }

    private void OnDestroy()
    {
        SceneBuilder.OnSceneBuilt -= GetSystemAndDefaultPosition;
        upArrowHandler.keyHeld -= AdjustExtrusion;
        downArrowHandler.keyHeld -= AdjustExtrusion;
    }

    void AdjustExtrusion(KeyCode key)
    {
        float direction = 0;
        switch (key)
        {
            case KeyCode.UpArrow:
                direction = 1;
                break;
            case KeyCode.DownArrow:
                direction = -1;
                break;
            default:
                break;
        }
        Extrude(direction);
    }

    void Extrude(float direction)
    {
        currentStep += Time.deltaTime * direction;

        if (currentStep > 1)
        {
            currentStep = 1;
        }
        else if (currentStep < 0)
        {
            currentStep = 0;
        }

        for (int i = 0; i < SystemsObjs.Count; i++)
        {
            var list = SystemsObjs[i].GameObjects;

            foreach (var item in list)
            {
                Vector3 defaultPosition = item.GetComponent<OrganData>().DefaultPosition;
                Vector3 maxPosition = new Vector3(
                    defaultPosition.x,
                    defaultPosition.y,
                    defaultPosition.z - maxDistance * i
                    );
                item.transform.position = Vector3.Lerp(defaultPosition, maxPosition, currentStep * i * offset * scalingFactor);
            }
        }

    }

    void GetSystemAndDefaultPosition()
    {
        OrganData[] organs = FindObjectsOfType<OrganData>();

        for (int i = 0; i < organs.Length; i++)
        {
            OrganData organData = organs[i];
            _ = mappings.TryGetValue(organData.SceneGraph, out string system);
            organData.BodySystem = (BodySystem)Enum.Parse(typeof(BodySystem), system);
            organData.DefaultPosition = organData.gameObject.transform.position;

        }



        for (int i = 0; i < systems.Length; i++)
        {
            List<GameObject> gameObjects = new List<GameObject>();
            OrganData[] organDataComponents = FindObjectsOfType<OrganData>();

            foreach (var organData in organDataComponents)
            {
                if (organData.BodySystem == (BodySystem)Enum.Parse(typeof(BodySystem), systems[i]))
                {
                    gameObjects.Add(organData.gameObject);
                }

            }

            SystemsObjs.Add(
               new SystemObjectPair(systems[i], gameObjects));

        }
    }
}

struct SystemObjectPair
{
    public string System;
    public List<GameObject> GameObjects;

    public SystemObjectPair(string system, List<GameObject> list)
    {
        this.System = system;
        this.GameObjects = list;
    }
}
