using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public enum BodySystem { undefined, cardio, digestive, fetal, integumentary, lymphatic, musculoskeletal, nervous, reproductive, respiratory, urinary }

public class HorizontalExtruder : MonoBehaviour
{
    [SerializeField] private int currentStep;
    [SerializeField] private float offset;
    [SerializeField] private string filename;
    [SerializeField] private Dictionary<string, string> mappings = new Dictionary<string, string>();
    // Start is called before the first frame update

    private void Awake()
    {
        ReadCsv();
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

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            AssignSystem();
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            Extrude();
        }
    }

    void Extrude()
    {
        string[] systems = System.Enum.GetNames(typeof(BodySystem));
        for (int i = 0; i < systems.Length; i++)
        {
            Debug.Log(systems[i]);
            List<GameObject> gameObjects = new List<GameObject>();
            OrganData[] g = GameObject.FindObjectsOfType<OrganData>();

            foreach (var go in g)
            {
                if (go.BodySystem == (BodySystem)Enum.Parse(typeof(BodySystem), systems[i]))
                {
                    gameObjects.Add(go.gameObject);
                }

            }
            Debug.Log(gameObjects.Count);
            foreach (var item in gameObjects)
            {
                item.transform.Translate(0f, 0f, -offset * i);
            }
        }
    }

    void AssignSystem()
    {
        foreach (var kvp in mappings)
        {
            Debug.Log("Key: " + kvp.Key);
            Debug.Log("Value" + kvp.Value);
        }
        Debug.Log("here");
        OrganData[] organs = GameObject.FindObjectsOfType<OrganData>();

        for (int i = 0; i < organs.Length; i++)
        {
            Debug.Log(mappings[organs[i].GetComponent<OrganData>().SceneGraph]);
            string system;

            bool hasValue = mappings.TryGetValue(organs[i].GetComponent<OrganData>().SceneGraph, out system);
            Debug.Log("system: " + system);
            organs[i].GetComponent<OrganData>().BodySystem = (BodySystem)Enum.Parse(typeof(BodySystem), system);
        }
    }
}
