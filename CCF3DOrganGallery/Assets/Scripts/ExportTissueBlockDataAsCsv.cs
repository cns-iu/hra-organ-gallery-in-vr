using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using System.IO;

public class ExportTissueBlockDataAsCsv : MonoBehaviour
{
    [SerializeField] private GameObject sceneBuilder;
    [SerializeField] private string filepath;

    private void OnEnable()
    {
        SceneBuilder.OnSceneBuilt += () =>
        {
            filepath = $"{Application.persistentDataPath}/Output/tissueBlocks.csv";
            List<GameObject> blocks = sceneBuilder.GetComponent<SceneBuilder>().TissueBlocks;

            StringBuilder sb = new StringBuilder();
            sb.AppendLine("Consortium name[entity ID],HuBMAP ID,Tissue Block Sex,HRA Organ,HRA Organ Sex,HRA Organ Uberon ID,HRA GLB File");
            Debug.Log(sb.ToString());
            for (int i = 0; i < blocks.Count; i++)
            {
                TissueBlockData data = blocks[i].GetComponent<TissueBlockData>();
                OrganData organ = blocks[i].transform.parent.gameObject.GetComponent<OrganData>();
                List<string> toAdd = new List<string>();
                toAdd.Add(data.EntityId);
                toAdd.Add(data.HubmapId);
                toAdd.Add(data.DonorSex);
                toAdd.Add(data.gameObject.transform.parent.GetChild(0).name);
                toAdd.Add(organ.DonorSex);
                toAdd.Add(organ.RepresentationOf);
                toAdd.Add(organ.SceneGraph);

                sb.AppendLine(String.Join(',', toAdd));


                //
            }
            Debug.Log(sb.ToString());

            File.WriteAllText(filepath, sb.ToString());
        };
    }
}
