using Assets.Scripts.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

namespace HRAOrganGallery
{
    public enum Sex { Male, Female }
    public enum Laterality { Left, Right }

    /// <summary>
    /// An interface describing a keyboard button. 
    /// </summary>
    /// <typeparam name="TButtonPayloadType">The type that the OnClick event shares</typeparam>
    /// <typeparam name="TFeatureType">The type of button (organ IRI, sex, or laterality)</typeparam>
    public interface IKeyboardButton<TButtonPayloadType, TFeatureType>
    {
        public static event Action<TButtonPayloadType> OnClick;

        //A Collider so it can interact with the user's pointing device
        public TFeatureType Feature { get; set; }
        public Material PressedMaterial { get; set; }
        public Material ReadyMaterial { get; set; }
        public Material DisabledMaterial { get; set; }
        public Renderer Renderer { get; set; }

        public void SetUpXRInteraction() { }
        public void SetVisibility() { }
        public void AutoSwitch() { }
        public void ChangeColor() { }


    }
}
