using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HRAOrganGallery
{
    public class LineDrawerGPU : MonoBehaviour
    {
        public Material lineMaterial;

        private ComputeBuffer buffer;

        private void Start()
        {
            Vector3[] lineData = new Vector3[60000]; // 30k lines => 60k vertices (2 vertices per line)

            for (int i = 0; i < 30000; i++)
            {
                Vector3 start = new Vector3(i % 100, 0, i / 100);
                Vector3 end = start + Vector3.up * 5;
                lineData[i * 2] = start;
                lineData[i * 2 + 1] = end;
            }

            buffer = new ComputeBuffer(lineData.Length, sizeof(float) * 3);
            buffer.SetData (lineData);

            lineMaterial.SetBuffer("lineBuffer", buffer);
        }

        private void OnRenderObject()
        {
            lineMaterial.SetPass(0);
            Graphics.DrawProceduralNow(MeshTopology.Lines, 60000);
        }

        private void OnDestroy()
        {
            buffer.Release();
        }
    }
}
