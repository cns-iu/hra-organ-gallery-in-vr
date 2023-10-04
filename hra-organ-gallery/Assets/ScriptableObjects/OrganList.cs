using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HRAOrganGallery
{
    public class OrganList : ScriptableObject
    {
       [SerializeField] public List<Pairs> OrganNames = new List<Pairs>();
    }

    [Serializable]
    public class Pairs
    {
        [SerializeField] public string name;
        [SerializeField] public string iri;
    }

}
