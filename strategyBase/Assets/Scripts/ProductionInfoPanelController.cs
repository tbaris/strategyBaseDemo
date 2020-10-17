using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class ProductionInfoPanelController : MonoBehaviour
{
    [SerializeField] private GameObject gameInfoPanel;
    [FormerlySerializedAs("ProductionInfoPanel")] [SerializeField] private GameObject productionInfoPanel;
    private void Awake()
    {
        GameController.SelectedAObject += SwitchPanel;
    }

    private void SwitchPanel(object sender, GameController.SelectedAObjectArgs e)
    {
        if (e.SelectedGameObject == null)
        {
            gameInfoPanel.gameObject.SetActive(true);
            productionInfoPanel.gameObject.SetActive(false);
        }
        else
        {
            gameInfoPanel.gameObject.SetActive(false);
            productionInfoPanel.gameObject.SetActive(true);
        }
    }
}
