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
        public static event Action<TButtonPayloadType> OnCLick;

        //A Collider so it can interact with the user's pointing device
        public BoxCollider Collider { get; set; }
        public TFeatureType Feature { get; set; }
        public Material ActiveMaterial { get; set; }
        public Material InactiveMaterial { get; set; }

        public Renderer Renderer { get; set; }

        private void TurnOff(TFeatureType f) { }

        private void ChangeColor() { }
    }
}