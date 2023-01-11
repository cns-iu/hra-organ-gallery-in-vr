using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class VersionShower : MonoBehaviour
{
    private void Awake()
    {
        if (TryGetComponent(out TMP_Text output)) {
            output.text = "Current version:\n" + Application.version;
        }
    }
}
