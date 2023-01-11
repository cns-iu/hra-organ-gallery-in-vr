using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

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
        //set 0.2 as threshold to account for inaccuracies when getting input on Quest 2 natively
        HorizontalExtruder.ExtrusionUpdate += (v) => { canUserSetFilter = v[0] <= threshold; };
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
