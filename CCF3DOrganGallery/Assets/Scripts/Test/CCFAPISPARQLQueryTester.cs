using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using System;
using UnityEngine.Networking;

public class CCFAPISPARQLQueryTester : MonoBehaviour
{
    [SerializeField] private string baseUrl;
    [SerializeField] private string iri;
    [SerializeField] private string endpoint;
    [SerializeField] private string format;

    //documentation for SPARQL query through grlc: http://grlc.io/api/hubmapconsortium/ccf-grlc/ccf/#/default/get_cell_by_location
    //url with iri descending colon as query string: http://grlc.io/api-git/hubmapconsortium/ccf-grlc/subdir/ccf//cell_by_location?location=http://purl.obolibrary.org/obo/UBERON_0001158&endpoint=https%3A%2F%2Fccf-api.hubmapconsortium.org%2Fv1%2Fsparql?format=application%2Fjson

    async private void Start()
    {
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
}

