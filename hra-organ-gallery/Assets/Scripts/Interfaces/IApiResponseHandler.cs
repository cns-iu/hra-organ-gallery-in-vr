using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace HRAOrganGallery
{
    public interface IApiResponseHandler
    {
        public void Deserialize(string rawWebResponse);

        public Task GetNodes();
    }
}
