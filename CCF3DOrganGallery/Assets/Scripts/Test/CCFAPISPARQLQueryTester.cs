using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using System;
using UnityEngine.Networking;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.Events;
using System.Text;

public class CCFAPISPARQLQueryTester : MonoBehaviour
{
    [SerializeField] private string baseUrl;
    [SerializeField] private string iri;
    [SerializeField] private string endpoint;
    [SerializeField] private string format;
    [SerializeField] private XRRayInteractor interactor;
    private UnityAction<SelectEnterEventArgs> _selectAction;

    //documentation for SPARQL query through grlc: http://grlc.io/api/hubmapconsortium/ccf-grlc/ccf/#/default/get_cell_by_location
    //url with iri descending colon as query string: http://grlc.io/api-git/hubmapconsortium/ccf-grlc/subdir/ccf//cell_by_location?location=http://purl.obolibrary.org/obo/UBERON_0001158&endpoint=https%3A%2F%2Fccf-api.hubmapconsortium.org%2Fv1%2Fsparql?format=application%2Fjson

    public async void CallAPI(SelectEnterEventArgs args) {
        // assigns string iri to link from CCFAnnotations element 0
        // adds everything to variable URL and outputs into untiy console
        // outputs into unity console 
        // makes api call with Get(url)

        string iri = args.interactableObject.transform.gameObject.GetComponent<TissueBlockData>().CcfAnnotations[0];
        string url = baseUrl + iri + endpoint + format;
        Debug.Log("URL corresponding to " + "TB NAME HERE \n"  + url);
        response = await Get(url);
    }

    private SPARQLAPIResponse response = new SPARQLAPIResponse();

    public string Pairs
    {
        get
        {
            StringBuilder sb = new StringBuilder();
            if (response.pairs.Length > 0)
            {
                foreach (var p in response.pairs)
                {
                    sb.Append(p + "\n");
                }
            }
            return sb.ToString();
        }
    }

    public async Task<SPARQLAPIResponse> Get(string url)
    {
        try
        {
            using var www = UnityWebRequest.Get(url);
            var operation = www.SendWebRequest();

            while (!operation.isDone)
                await Task.Yield();

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

