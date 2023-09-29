using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Mathematics;
using UnityEngine;

namespace HRAOrganGallery
{
    public interface IApiResponseHandler<TReturn>
    {
        public TReturn T { get; set; }
        public void Deserialize(string rawWebResponse);

        /// <summary>
        /// A method to retrieve data using a WebLoader
        /// </summary>
        /// <returns>A Task</returns>
        public Task GetNodes();

        /// <summary>
        /// A method to expose the retrieved web data
        /// </summary>
        /// <returns>A Task</returns>
        public Task<TReturn> ShareData();
    }

}
