using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using System;
using UnityEngine.Networking;
using UnityEngine.XR.Interaction.Toolkit;

public class CCFAPISPARQLQueryTester : MonoBehaviour
{
    [SerializeField] private string baseUrl;
    [SerializeField] private string iri;
    [SerializeField] private string endpoint;
    [SerializeField] private string format;

    //documentation for SPARQL query through grlc: http://grlc.io/api/hubmapconsortium/ccf-grlc/ccf/#/default/get_cell_by_location
    //url with iri descending colon as query string: http://grlc.io/api-git/hubmapconsortium/ccf-grlc/subdir/ccf//cell_by_location?location=http://purl.obolibrary.org/obo/UBERON_0001158&endpoint=https%3A%2F%2Fccf-api.hubmapconsortium.org%2Fv1%2Fsparql?format=application%2Fjson

    public async void CalAPI(SelectEnterEventArgs args) {

        string iri = args.interactableObject.transform.gameObject.GetComponent<TissueBlockData>().CcfAnnotations[0];
        string url = baseUrl + iri + endpoint + format;
        Debug.Log(url);
        response = await Get(url);
    }

    public SPARQLAPIResponse response;

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
        [SerializeField] public CellIriLabel[] pairs;
    }

    [Serializable]
    public class CellIriLabel
    {
        public string cell_label;
        public string cell_iri;
    }
    //priv method void that subscribes to event after controller emits even (when user clicks on TB) returns new IRI (uses request URL to retrieve)
}
