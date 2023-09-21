using Assets.Scripts.Shared;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum LoggerType { Environment, Data, User }

namespace HRAOrganGallery.Assets.Scripts
{
    [AddComponentMenu("_AndreasBueckle/Services/Logging")]
    public class ConsoleLogger : MonoBehaviour
    {
        public LoggerType type;

        [Header("Settings")]
        [SerializeField] private bool _showLogs = true;
        [SerializeField] private string _prefix;
        [SerializeField] private Color _prefixColor;
        private string _hexColor;

        private void OnValidate()
        {
            _hexColor = "#" + ColorUtility.ToHtmlStringRGBA(_prefixColor);
        }

        public void Log(object message, Object sender)
        {
            if (!_showLogs) return;

            Debug.Log($"{("[" + _prefix + "]" + sender.ToString()).Color(_hexColor)} says: " + message);
        }
    }
}

