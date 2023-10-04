using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HRAOrganGallery
{
    public class KeyboardButtonToggler : MonoBehaviour
    {
        [field: SerializeField] public OrganCallButton CurrentlyActiveOrganButton;

        private void Awake()
        {
            //OrganCallButton.OnCLick += (iris) => { CurrentlyActiveOrganButton = iris.};
        }
    }
}
