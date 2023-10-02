using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace HRAOrganGallery
{
    /// <summary>
    /// A class to describe an organ call button on the keyboard
    /// </summary>
    /// 

    public enum Sex { None, Male, Female }
    public enum Laterality { None, Left, Right };

    public class OrganCallButton : MonoBehaviour
    {
        public static event Action<string, string> OnCLick;

        [Header("Data")]
        public string organIri;
        public Sex organSex;
        public Laterality organLaterality;

        private Button button;

        private void Awake()
        {
            button = GetComponent<Button>();

            button.onClick.AddListener(
                () => { OnCLick(organIri.ToString(), organSex.ToString()); }
                );
        }
    }
}
