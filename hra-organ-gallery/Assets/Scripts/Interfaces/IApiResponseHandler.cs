using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HRAOrganGallery
{
    public interface IApiResponseHandler
    {
        public void Deserialize(string rawWebResponse);

        public void GetJsonFromWeb();
    }
}
