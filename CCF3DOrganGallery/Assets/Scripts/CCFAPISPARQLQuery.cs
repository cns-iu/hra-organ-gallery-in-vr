using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using System;
using UnityEngine.Networking;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.Events;
using System.Text;

public class CCFAPISPARQLQuery : MonoBehaviour
{
    [SerializeField] private string baseUrl;
    [SerializeField] private string iri;
    [SerializeField] private string endpoint;
    [SerializeField] private string format;
    [SerializeField] private XRRayInteractor interactor;
    private UnityAction<SelectEnterEventArgs> _selectAction;

    //documentation for SPARQL query through grlc: http://grlc.io/api/hubmapconsortium/ccf-grlc/ccf/#/default/get_cell_by_location
    //url with iri descending colon as query string: http://grlc.io/api-git/hubmapconsortium/ccf-grlc/subdir/ccf//cell_by_location?location=http://purl.obolibrary.org/obo/UBERON_0001158&endpoint=https%3A%2F%2Fccf-api.hubmapconsortium.org%2Fv1%2Fsparql?format=application%2Fjson


    public async void CallAPI(SelectEnterEventArgs args)
    {
        // assigns string iri to link from CCFAnnotations element 0
        // adds everything to variable URL and outputs into untiy console
        // outputs into unity console 
        // makes api call with Get(url)
        string[] iris = args.interactableObject.transform.gameObject.GetComponent<TissueBlockData>().CcfAnnotations;
        cellLabelSet.Clear();
        for (int i = 0; i < iris.Length; i++)
        {
            string url = baseUrl + iris[i] + endpoint + format;
            Debug.Log($"now calling: {url}");
            response = await Get(url);
            Debug.Log($"received from {url}: {response.pairs[0].cell_label}");
            cellLabelSet.Add(response.pairs[0].cell_label);
        }

        Debug.Log($"ran all {iris.Length} API queries");
        
    }
/* TESTING HASHSET BELOW
    void Awake() {
        HashSet<string> test = new HashSet<string>();
        test.Add("label1");
        test.Add("label1");
        foreach (var s in test) {
            Debug.Log(s);
        }
    }*/
    
    [SerializeField] private SPARQLAPIResponse response = new SPARQLAPIResponse();
    HashSet<string> cellLabelSet = new HashSet<string>();
    public string Pairs //pairs method to reference when SetPaneltext is called.
    {
        get
        {   
            //loop through hashset below and return string of labels

            StringBuilder sb = new StringBuilder();

            foreach (var label in cellLabelSet)
            {
                sb.Append(label);
            }

            sb.Replace("\"\"", ",");
            sb.Replace("\"", "");
            int count = 0;

            return sb.ToString();
        }
    }
/*
    public void clearSet(SelectExitEventargs args)
    {
        //use this method to set cellLabelSet
        args.interactableObject.
    }*/

    public async Task<SPARQLAPIResponse> Get(string url)
    {
        try
        {
            using var www = UnityWebRequest.Get(url);
            var operation = www.SendWebRequest();

            while (!operation.isDone)
            {
                float progress = Mathf.Clamp01(operation.progress / .9f);
                //Debug.Log(operation.progress);
                await Task.Yield();
            }


            if (www.result != UnityWebRequest.Result.Success)
                Debug.LogError($"Failed: {www.error}");

            var text = www.downloadHandler.text;

            response = JsonUtility.FromJson<SPARQLAPIResponse>(
                "{ \"pairs\":" +
                text
                + "}"
                );

            return response;

        }
        catch (Exception ex)
        {
            Debug.LogError($"{nameof(Get)} failed: {ex.Message}");
            return default;
        }
    }

    [Serializable]
    public class SPARQLAPIResponse
    {
        [SerializeField] public CellIriLabel[] pairs = new CellIriLabel[2];
    }

    [Serializable]
    public class CellIriLabel
    {
        public string cell_label;
        public string cell_iri;
    }

}
