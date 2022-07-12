using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneConfiguration : MonoBehaviour
{
    public string Url;

    [SerializeField] private string baseUrl = "https://ccf-api--staging.herokuapp.com/v1/scene";
    [SerializeField]
    private List<string> showTissueBlocksForOrgans = new List<string>();
    [SerializeField] private string sex;
    [SerializeField] const string ontologyQueryString = "&ontology-terms=http://purl.obolibrary.org/obo/UBERON_";
    [SerializeField] const string sexQueryString = "?sex=";

    public string BuildUrl()
    {
        if (showTissueBlocksForOrgans.Count > 0)
        {
            Url = baseUrl + sexQueryString + sex + ontologyQueryString + showTissueBlocksForOrgans[0];

            for (int i = 1; i < showTissueBlocksForOrgans.Count; i++)
            {
                Url += ontologyQueryString + showTissueBlocksForOrgans[i];
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





