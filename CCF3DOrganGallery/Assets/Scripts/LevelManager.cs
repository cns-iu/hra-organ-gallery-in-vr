using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private Button[] _buttons = new Button[2];

    event Action Open;

    private void Awake()
    {
        foreach (var btn in _buttons)
        {
            int sceneNumber = int.Parse(btn.gameObject.name);
            btn.onClick.AddListener(() => { SceneManager.LoadScene(sceneNumber); }); ;
        }
    }
}
