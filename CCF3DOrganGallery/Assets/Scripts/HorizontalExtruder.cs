using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public enum BodySystem { undefined, integumentary, nervous, respiratory, cardio, digestive, musculoskeletal, lymphatic, urinary, fetal, reproductive }

public class HorizontalExtruder : MonoBehaviour
{
    [SerializeField] private float currentStep;
    [SerializeField] private float offset;
    [SerializeField] private float maxDistance;
    [SerializeField] private string filename;
    [SerializeField] private Dictionary<string, string> mappings = new Dictionary<string, string>();
    private string[] systems;
    // Start is called before the first frame update

    private void Awake()
    {
        ReadCsv();
        systems = System.Enum.GetNames(typeof(BodySystem));
        UserInputModule.KeyPressed += (key) => Debug.Log("detected key press" + key);
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
    }

    private void OnDestroy()
    {
        SceneBuilder.OnSceneBuilt -= GetSystemAndDefaultPosition;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            Extrude();
        }
    }

    void Extrude()
    {
        for (int i = 0; i < systems.Length; i++)
        {
            List<GameObject> gameObjects = new List<GameObject>();
            OrganData[] organDataComponents = GameObject.FindObjectsOfType<OrganData>();

            foreach (var organData in organDataComponents)
            {
                if (organData.BodySystem == (BodySystem)Enum.Parse(typeof(BodySystem), systems[i]))
                {
                    gameObjects.Add(organData.gameObject);
                }

            }
            foreach (var item in gameObjects)
            {
                //item.transform.Translate(0f, 0f, -offset * i * currentStep
                Vector3 defaultPosition = item.GetComponent<OrganData>().DefaultPosition;
                Vector3 maxPosition = new Vector3(
                    defaultPosition.x,
                    defaultPosition.y,
                    defaultPosition.z - maxDistance * i
                    );
                item.transform.position = Vector3.Lerp(defaultPosition, maxPosition, currentStep * i * offset);
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
    }
}
