using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HRAOrganGallery
{
    public interface IPauseCollision
    {
       static event Action<bool> OnCollideWithPriorityLayer;
    }
}
