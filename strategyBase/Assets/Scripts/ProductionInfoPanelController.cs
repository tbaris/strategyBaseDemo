using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProductionInfoPanelController : MonoBehaviour
{
    [SerializeField] private GameObject gameInfoPanel;
    [SerializeField] private GameObject ProductionInfoPanel;
    private void Awake()
    {
        GameController.SelectedAObject += SwitchPanel;
    }

    private void SwitchPanel(object sender, GameController.SelectedAObjectArgs e)
    {
        if (e.SelectedGameObject == null)
        {
            gameInfoPanel.gameObject.SetActive(true);
            ProductionInfoPanel.gameObject.SetActive(false);
        }
        else
        {
            gameInfoPanel.gameObject.SetActive(false);
            ProductionInfoPanel.gameObject.SetActive(true);
        }
    }
}
