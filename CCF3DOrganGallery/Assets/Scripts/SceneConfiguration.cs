using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

public class SceneConfiguration : MonoBehaviour
{
    public string Url;

    [SerializeField] private string baseUrl = "https://ccf-api--staging.herokuapp.com/v1/scene";
    [SerializeField]
    private List<string> uberonIds = new List<string>();
    [SerializeField] private string sex;
    [SerializeField] const string ontologyQueryString = "&ontology-terms=http://purl.obolibrary.org/obo/UBERON_";
    [SerializeField] const string sexQueryString = "?sex=";

    public string BuildUrl()
    {
        if (uberonIds.Count > 0)
        {
            Url = baseUrl + sexQueryString + sex + ontologyQueryString + uberonIds[0];

            for (int i = 1; i < uberonIds.Count; i++)
            {
                Url += ontologyQueryString + uberonIds[i];
            }
        }
        else
        {
            Url = baseUrl + sexQueryString + sex;
        }
        return Url;
    }
    //heart: 0000948
    //left kidney: 0004538
    //skin: 0002097
}





