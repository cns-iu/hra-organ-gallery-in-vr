using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HRAOrganGallery
{
    public class ForwardGrid : MonoBehaviour
    {
        RaycastHit hitData;
        Ray ray;
        [SerializeField]
        Camera cam;
        [SerializeField]
        LineRenderer lineRend;
        [SerializeField]
        private GameObject scaleBarPrefab;
        [SerializeField]
        private Vector3 originPosition;
        [SerializeField]
        private GameObject scaleBar;

        private GameObject instantiatedScaleBar;

        // Start is called before the first frame update
        void Start()
        {
            //lineRend.startColor = Color.black;
            //lineRend.endColor = Color.black;
            Debug.Log("In Forward Grid");
        }

        //void InstatiateOnHover()
        //{

        //}

        // Update is called once per frame
        void Update()
        {
            originPosition = scaleBar.transform.position;
            lineRend.SetPosition(0, originPosition);
            Debug.DrawLine(scaleBar.transform.position, scaleBar.transform.position + scaleBar.transform.forward * 5, Color.blue);

            ray = new Ray(originPosition, transform.forward);
            //ray = cam.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hitData, 100))
            {
                lineRend.enabled = true;
                //lineRend.SetPosition(0, originPosition);
                lineRend.SetPosition(1, hitData.point);
                string hitTag = hitData.collider.tag;
                //if (hitTag == "Organ" && scaleBar.transform.childCount == 0)
                if (hitTag == "Organ")
                {   
                    Debug.Log("Organ Hit");
                    //Instantiate(scaleBarPrefab, hitData.point, Quaternion.identity, scaleBar.transform);

                    if (instantiatedScaleBar == null)
                    {
                        instantiatedScaleBar = Instantiate(scaleBarPrefab, hitData.point, Quaternion.identity);
                        instantiatedScaleBar.transform.SetParent(scaleBar.transform, true);
                    }
                    else
                    {
                        // Update the position of the instantiated scale bar to the hit point
                        instantiatedScaleBar.transform.position = hitData.point;
                    }
                }
            }
            else
            {
                lineRend.SetPosition(1, originPosition + ray.direction);
                //if ray doesn't exist/ ray doesn't hit the organ, delete the instantiated scalebar. 
                if (instantiatedScaleBar != null)
                {
                    Destroy(instantiatedScaleBar);
                    instantiatedScaleBar = null; // Reset the reference to null
                }
            }
        }
    }
}
