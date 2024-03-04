using Newtonsoft.Json.Bson;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace HRAOrganGallery
{
    public interface IStateSelect
    {
        void OnSelect();
        void OnDeselect(UserInputState state);

    }
}
