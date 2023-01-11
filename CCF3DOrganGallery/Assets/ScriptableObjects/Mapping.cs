using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewMapping")]
public class Mapping : ScriptableObject
{
    public List<OrganProperties> pairs;
    [Serializable]
    public struct OrganProperties
    {
        public string RepresentationOf;
        public string SceneGraph;
        public string Tooltip;
        public string SceneGraphNode;

        public OrganProperties(string arg1, string arg2, string arg3, string arg4) => (RepresentationOf, SceneGraph, Tooltip, SceneGraphNode) = (arg1, arg2, arg3, arg4);
    }

}
