using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.InputSystem;

public enum BodySystem { undefined, integumentary, nervous, respiratory, cardio, digestive, musculoskeletal, lymphatic, urinary, fetal, reproductive }

public class HorizontalExtruder : MonoBehaviour
{
    public static event Action<float[]> ExtrusionUpdate;
    public static event Action<List<SystemObjectPair>> OnBodySystemsReady;

    public InputActionReference RightHandJoyStickAxis;
    public InputActionReference LeftHandJoyStickAxis;
    public KeyHandler upArrowHandler = null;
    public KeyHandler downArrowHandler = null;
    public KeyHandler leftArrowHandler = null;
    public KeyHandler rightArrowHandler = null;
    public List<SystemObjectPair> SystemsObjs = new List<SystemObjectPair>();
    [field: SerializeField] public float CurrentStepOne { get; set; }
    [field: SerializeField] public float CurrentStepTwo { get; set; }

    [SerializeField] private float maxDistanceOne;
    [SerializeField] private float maxDistanceTwo;
    [SerializeField] private string bodySystemsData;
    [SerializeField] private bool canExtrudeOne = false;
    [SerializeField] private bool canExtrudeTwo = false;
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
        SceneBuilder.OnSceneBuilt += () => { canExtrudeOne = true; };
        upArrowHandler.keyHeld += AdjustExtrusionOne;
        downArrowHandler.keyHeld += AdjustExtrusionOne;
        leftArrowHandler.keyHeld += AdjustExtrusionTwo;
        rightArrowHandler.keyHeld += AdjustExtrusionTwo;
        RightHandJoyStickAxis.action.performed += AdjustExtrusionOne;
        LeftHandJoyStickAxis.action.performed += AdjustExtrusionTwo;
    }

    private void OnDestroy()
    {
        SceneBuilder.OnSceneBuilt -= GetSystemAndDefaultPosition;
        upArrowHandler.keyHeld -= AdjustExtrusionOne;
        downArrowHandler.keyHeld -= AdjustExtrusionOne;
        leftArrowHandler.keyHeld -= AdjustExtrusionTwo;
        rightArrowHandler.keyHeld -= AdjustExtrusionTwo;
        RightHandJoyStickAxis.action.performed -= AdjustExtrusionOne;
        LeftHandJoyStickAxis.action.performed -= AdjustExtrusionTwo;
    }

    void AdjustExtrusionTwo(InputAction.CallbackContext context)
    {
        if (!canExtrudeTwo) return;

        float direction = context.action.ReadValue<Vector2>().x < 0 ? -1 : 1;


        CurrentStepTwo += Time.deltaTime * direction;

        if (CurrentStepTwo > 1)
        {
            CurrentStepTwo = 1;
        }
        else if (CurrentStepTwo < 0)
        {
            CurrentStepTwo = 0;
        }

        ExtrusionUpdate?.Invoke(new float[2] { CurrentStepOne, CurrentStepTwo });

        for (int i = 2; i < SystemsObjs.Count; i++)
        {

            foreach (var sex in SystemsObjs[i].GameObjectsBySex)
            {
                for (int k = 0; k < sex.Count; k++)
                {
                    float sideValue = sex[k].GetComponent<OrganData>().DonorSex.ToLower() == "male" ? -1 : 1;
                    Vector3 defaultPosition = sex[k].GetComponent<OrganData>().DefaultPositionExtruded;
                    Vector3 maxPosition = new Vector3(
                        defaultPosition.x + maxDistanceTwo * sideValue * k,
                        defaultPosition.y,
                        defaultPosition.z
                        );
                    sex[k].transform.position = Vector3.Lerp(defaultPosition, maxPosition, CurrentStepTwo);
                }

            }

            canExtrudeOne = !(CurrentStepTwo > 0f);
        }
    }

    void AdjustExtrusionTwo(KeyCode key)
    {
        if (!canExtrudeTwo) return;

        float direction = 0;
        switch (key)
        {
            case KeyCode.RightArrow:
                direction = 1;
                break;
            case KeyCode.LeftArrow:
                direction = -1;
                break;
            default:
                break;
        }

        CurrentStepTwo += Time.deltaTime * direction;

        if (CurrentStepTwo > 1)
        {
            CurrentStepTwo = 1;
        }
        else if (CurrentStepTwo < 0)
        {
            CurrentStepTwo = 0;
        }

        ExtrusionUpdate?.Invoke(new float[2] { CurrentStepOne, CurrentStepTwo });

        for (int i = 2; i < SystemsObjs.Count; i++)
        {

            foreach (var sex in SystemsObjs[i].GameObjectsBySex)
            {
                for (int k = 0; k < sex.Count; k++)
                {
                    float sideValue = sex[k].GetComponent<OrganData>().DonorSex.ToLower() == "male" ? -1 : 1;
                    Vector3 defaultPosition = sex[k].GetComponent<OrganData>().DefaultPositionExtruded;
                    Vector3 maxPosition = new Vector3(
                        defaultPosition.x + maxDistanceTwo * sideValue * k,
                        defaultPosition.y,
                        defaultPosition.z
                        );
                    sex[k].transform.position = Vector3.Lerp(defaultPosition, maxPosition, CurrentStepTwo);
                }

            }

            canExtrudeOne = !(CurrentStepTwo > 0f);
        }
    }

    void AdjustExtrusionOne(InputAction.CallbackContext context)
    {
        if (!canExtrudeOne) return;

        float direction = context.action.ReadValue<Vector2>().y < 0f ? -1 : 1;
        ExtrudeOne(direction);
    }
    void AdjustExtrusionOne(KeyCode key)
    {
        if (!canExtrudeOne) return;
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
        ExtrudeOne(direction);
    }

    void ExtrudeOne(float direction)
    {
        CurrentStepOne += Time.deltaTime * direction;

        if (CurrentStepOne > 1)
        {
            CurrentStepOne = 1;
        }
        else if (CurrentStepOne < 0)
        {
            CurrentStepOne = 0;
        }

        //ExtrusionUpdate
        ExtrusionUpdate?.Invoke(new float[2] { CurrentStepOne, CurrentStepTwo });

        //srart at index = 2 to leave skin in default place
        for (int i = 2; i < SystemsObjs.Count; i++)
        {
            var list = SystemsObjs[i].GameObjects;

            foreach (var item in list)
            {
                Vector3 defaultPosition = item.GetComponent<OrganData>().DefaultPosition;
                Vector3 maxPosition = new Vector3(
                    defaultPosition.x,
                    defaultPosition.y,
                    defaultPosition.z - maxDistanceOne * i
                    );
                item.transform.position = Vector3.Lerp(defaultPosition, maxPosition, CurrentStepOne);
                item.GetComponent<OrganData>().DefaultPositionExtruded = maxPosition;
            }
        }

        canExtrudeTwo = CurrentStepOne == 1;

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

