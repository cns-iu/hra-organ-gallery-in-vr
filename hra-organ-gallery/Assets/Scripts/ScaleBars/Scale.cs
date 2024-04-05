using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace HRAOrganGallery
{
    public class Scale : MonoBehaviour
    {
        [SerializeField]
        private GameObject organCylinder;
        [SerializeField]
        private GameObject scaleBar;
        [SerializeField]
        private Transform duplicateScaleBar;
        [SerializeField]
        private GameObject scaleBarPrefab;
        //private TMPro 
        //[SerializeField]
        //private TMPro 

        private float ogOrganScale;
        private float ogBarScale;
        private Vector3 initialOrganScale;
        private Vector3 initialBarScale;



        // Start is called before the first frame update
        void Start()
        {
            initialOrganScale = organCylinder.transform.localScale;
            initialBarScale = scaleBar.transform.localScale;
            ogBarScale = scaleBar.transform.localScale.magnitude;
            ogOrganScale = organCylinder.transform.localScale.magnitude;
        }

        // Update is called once per frame
        void Update()
        {
            Vector3 currentOrganScale = organCylinder.transform.localScale;

            Vector3 scaleChange = new Vector3(currentOrganScale.x / initialOrganScale.x, 1, 1);
            scaleBar.transform.localScale = Vector3.Scale(initialBarScale, scaleChange);
            if (scaleChange.x >= 1.9f )
            {
                //Debug.Log(scaleChange);
                Instantiate(scaleBarPrefab, duplicateScaleBar.position, duplicateScaleBar.rotation);
            }

            if(scaleChange.x >= 0.9f)
            {
                Instantiate(scaleBarPrefab, duplicateScaleBar.position, duplicateScaleBar.rotation);
            }


        }
    }
}
