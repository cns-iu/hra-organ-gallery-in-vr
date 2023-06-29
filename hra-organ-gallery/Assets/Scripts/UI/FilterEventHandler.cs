using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

namespace Assets.Scripts.UI
{
    public class FilterEventHandler : MonoBehaviour
    {
        public static event Action OnFilterComplete;
        public static event Action<List<string>> OnFilterCompleteWithOrgans;

        [SerializeField] private Button filterButton;
        [SerializeField] private bool canUserSetFilter = true;
        [SerializeField] private List<Toggle> toggles;
        [SerializeField] private List<string> newOrgansToShow;
        [SerializeField] private GameObject filterCanvas;
        [SerializeField] private float threshold = 0.2f;

        private void OnEnable()
        {

        }

        private void Start()
        {
            toggles = GetComponent<OrganToggleGroup>().Toggles;

            filterButton.onClick.AddListener(
              () =>
              {
                  if (canUserSetFilter)
                  {
                      newOrgansToShow.Clear();
                      for (int i = 0; i < toggles.Count; i++)
                      {
                          if (toggles[i].isOn) newOrgansToShow.Add(toggles[i].name);
                      }
                      OnFilterComplete?.Invoke();
                      OnFilterCompleteWithOrgans?.Invoke(newOrgansToShow);
                  }
              }
                );
        }
    }
}