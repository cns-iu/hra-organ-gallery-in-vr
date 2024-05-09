using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace HRAOrganGallery
{
    public class OnButtonClickResize : MonoBehaviour
    {
        [SerializeField] private List<Button> _legendButtons;
        [SerializeField] private GameObject _verticalLayout;

        private void Start()
        {
            _legendButtons = _verticalLayout.GetComponentsInChildren<Button>().ToList();
        }
    }
}
