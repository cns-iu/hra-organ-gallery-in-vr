using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Newtonsoft.Json;
using System;
using Assets.Scripts.Utils;

namespace HRAOrganGallery
{
    public class CellTypeLoader : MonoBehaviour
    {
        [SerializeField] private string _fileName;

        private void Awake()
        {
            //This code should be changed later to get enriched RUI locations from the web
            LoadJson();
        }

        private void LoadJson()
        {
            var asset = Resources.Load<TextAsset>(_fileName);
            Debug.Log(asset); 
            //using (var reader = Utils.ReadTextFile(_fileName))
            //{
            //    while (!reader.EndOfStream)
            //    {
            //        string json = reader.ReadToEnd();
            //        Debug.Log(json);
            //        //List<Item> items = JsonConvert.DeserializeObject<List<Item>>(json);
            //    }
            //}
        }

        private class Item
        {
            public int millis;
            public string stamp;
            public DateTime datetime;
            public string light;
            public float temp;
            public float vcc;
        }
    }
}
