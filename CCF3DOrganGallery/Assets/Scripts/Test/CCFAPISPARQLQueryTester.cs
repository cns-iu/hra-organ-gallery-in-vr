using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using TMPro;
using UnityEngine.InputSystem;
using System.Net;
using static CellTypeInfoLogger;
using System.Threading.Tasks;
using System;
using UnityEngine.Networking;
using Unity.VisualScripting;

public class CCFAPISPARQLQueryTester : MonoBehaviour
{
    [SerializeField] private string baseUrl;
    [SerializeField] private List<string> iris = new List<string>();
    [SerializeField] private string endpoint;
    [SerializeField] private string format;

    //documentation: http://grlc.io/api/hubmapconsortium/ccf-grlc/ccf/#/default/get_cell_by_location
    //url: http://grlc.io/api-git/hubmapconsortium/ccf-grlc/subdir/ccf//cell_by_location
    //?location=http%3A%2F%2Fpurl.obolibrary.org%2Fobo%2FUBERON_0002113
    //&endpoint=https%3A%2F%2Fccf-api.hubmapconsortium.org%2Fv1%2Fsparql
    //?format=application%2Fjson

    //http://grlc.io/api-git/hubmapconsortium/ccf-grlc/subdir/ccf//cell_by_location?location=http%3A%2F%2Fpurl.obolibrary.org%2Fobo%2FUBERON_0002113&endpoint=https%3A%2F%2Fccf-api.hubmapconsortium.org%2Fv1%2Fsparql?format=application%2Fjson

    [SerializeField] private InputActionReference fButtonPressed;
    [SerializeField] private TMP_Text text;

    private void OnEnable()
    {
        fButtonPressed.action.performed += CallAPI;
        fButtonPressed.action.performed += (InputAction.CallbackContext ctx) => { Debug.Log("called"); };
    }

    private void OnDisable()
    {
        fButtonPressed.action.performed -= CallAPI;
    }


    async private void Start()
    {
        string url = baseUrl + iris[0] + endpoint + format;
        Debug.Log(url);
        text.text = await Get("http://grlc.io/api-git/hubmapconsortium/ccf-grlc/subdir/ccf//cell_by_location?location=http%3A%2F%2Fpurl.obolibrary.org%2Fobo%2FUBERON_0002113&endpoint=https%3A%2F%2Fccf-api.hubmapconsortium.org%2Fv1%2Fsparql?format=application%2Fjson");
    }

    async void CallAPI(InputAction.CallbackContext ctx)
    {
        Debug.Log("works");
        string url = baseUrl + iris[0] + "&location=" + endpoint;
        text.text = await Get(url);
    }

    public async Task<string> Get(string url)
    {
        try
        {
            using var www = UnityWebRequest.Get(url);
            var operation = www.SendWebRequest();

            while (!operation.isDone)
                await Task.Yield();

            if (www.result != UnityWebRequest.Result.Success)
                Debug.LogError($"Failed: {www.error}");

            var result = www.downloadHandler.text;

            var text = www.downloadHandler.text;

            // _nodeArray = JsonUtility.FromJson<NodeArray>(
            //     "{ \"pairs\":" +
            //     text
            //     + "}"
            //     );
            //return _nodeArray;
            Debug.Log("Response: " + result);
            return result;
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
        public string string_label;
        public string cell_iri;
    }
}

