using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace HRAOrganGallery
{
    public class Scale : MonoBehaviour
    {
        //[SerializeField]
        //private TMPro 

        private float ogOrganScale;
        private float ogBarScale;
        //public property to get or set the label text
        public string ScaleBarLabel { get { return _scaleBarLabel; } set { _scaleBarLabel = value; } }
        [SerializeField] private string _scaleBarLabel;

        private TextMeshProUGUI duplicateScaleText;

        private Vector3 initialOrganScale;
        private Vector3 initialBarScale;

        // Start is called before the first frame update
        void Start()
        {
            //initializing initial scales of the organ, scale bar
            initialOrganScale = BarManager.Instance.organCylinder.transform.localScale;
            initialBarScale = BarManager.Instance.scaleBar.transform.localScale;
        }

        // Update is called once per frame
        void Update()
        {            
            //calculating the scale change of the organ and applying it to the bar
            Vector3 currentOrganScale = BarManager.Instance.organCylinder.transform.localScale;
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
