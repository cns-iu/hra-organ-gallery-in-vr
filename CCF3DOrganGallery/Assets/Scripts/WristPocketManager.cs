using System.Collections.Generic;
using UnityEngine;

public class WristPocketManager : MonoBehaviour
{
    private readonly Dictionary<GameObject, GameObject> _tissueBlockParentOrgans = new Dictionary<GameObject, GameObject>();
    
    public List<GameObject> wristPocket;
    
    private bool _isSceneBuilt;
    
    private void OnEnable()
    {
        SceneBuilder.OnSceneBuilt += () => _isSceneBuilt = true;
    }

    private void OnDestroy()
    {
        SceneBuilder.OnSceneBuilt -= () => _isSceneBuilt = true;
    }
    
    // Start is called before the first frame update
    void Start()
    {
        wristPocket = new List<GameObject>();
        TissueBlockSelectActions.OnHover += hit => Debug.Log("hi");
        TissueBlockSelectActions.OnSelected += hit => Debug.Log("heyo");
        // TissueBlockSelectActions.OnSelected += SwitchParent;
        Debug.Log("Abhijeet2");
    }

    // private void SwitchParent(RaycastHit hit)
    // {   
    //     Debug.Log("Abhijeet1");
    //     wristPocket[0].transform.SetParent(GameObject.Find("VH_F_Kidney_Left_v1.1").transform);
    //     Debug.Log("Abhijeet");
    // }

    // Update is called once per frame
    
    void Update()
    {

        // Records structure used to get information back from a raycast.
        // Casting new ray into the scene
        // Ray ray = new Ray(transform.position, transform.TransformDirection(Vector3.forward));
        //
        // // If our ray hits a collider somewhere in the scene, do:
        // if (Physics.Raycast(ray, out var hit, Mathf.Infinity))
        // {
        //     // Debug.Log((hit.collider.name)); // Logs the name of the gameObject our Raycast hits
        //     // Compare if the object's tag was "TissueBlock"
        //     if (hit.collider.CompareTag("TissueBlock"))
        //     {
        //         if (!_isSceneBuilt) return;
        //         if(!GetComponent<WristPocketManager>().wristPocket.Contains(gameObject))
        //         {
        //             
        //         }
        //     }
        // }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("TissueBlock"))
        {
            Debug.Log("Added Tissue");
            var tissue = other.gameObject;
            //_tissueBlockParentOrgans.Add(tissue, tissue.transform.parent.gameObject);
            wristPocket.Add(tissue);
            tissue.transform.SetParent(transform);
            tissue.transform.position = transform.position;
            
        }
    }
    
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("TissueBlock"))
        {
            Debug.Log("Removed Tissue " + other.gameObject.name);
            var tissue = other.gameObject;
            //_tissueBlockParentOrgans.Add(tissue, tissue.transform.parent.gameObject);
            wristPocket.Remove(tissue);
            
            // new WaitForSeconds(1.5f);
            tissue.transform.SetParent(GameObject.Find("VH_F_Kidney_Left_v1.1").transform);
            Debug.Log("Tissue parent name: "+tissue.transform.parent.name);
        }
    }
}
