using Assets.Scripts.Scene;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace HRAOrganGallery
{
    public class TissueBlockRefOrganLoader : MonoBehaviour, IApiResponseHandler<RuiLocationOrganMapping>
    {
        public static TissueBlockRefOrganLoader Instance { get; private set; }
        public RuiLocationOrganMapping ruiLocationMapping;
        public void Deserialize(string rawWebResponse)
        {
            throw new System.NotImplementedException();
        }

        public Task GetNodes()
        {
            throw new System.NotImplementedException();
        }

        public Task<RuiLocationOrganMapping> ShareData()
        {
            throw new System.NotImplementedException();
        }

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(this);
            }
            else
            {
                Instance = this;
            }
        }


    }

    public class RuiLocationOrganMapping
    {
        [SerializeField] public RuiToOrgan[] mappings;
    }

    public class RuiToOrgan
    {
        public string rui_location;
        public string reference_organ;
    }
}
