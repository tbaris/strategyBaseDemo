using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal.VersionControl;
using UnityEngine;
using UnityEngine.UI;

public class UnitInfoController : MonoBehaviour
{
    [SerializeField] private Image buildingImage;
    [SerializeField] private GameObject productionList;
    private List<productionButton> activeButtonList;
    private GameObject selectedGO;
    private void Awake()
    {
        GameController.SelectedAObject += RefreshBuildingInfo;
        activeButtonList = new List<productionButton>();
    }

    private void RefreshBuildingInfo(object sender, GameController.SelectedAObjectArgs e)
    {
        if (e.SelectedGameObject == null)
        {
            selectedGO = null;

            if (activeButtonList.Count > 0)
            {
                foreach (var button in activeButtonList)
                {
                    Destroy(button?.gameObject);
                }
                activeButtonList.Clear();
            }

        }
        else
        {
            selectedGO = e.SelectedGameObject;

            if (activeButtonList.Count > 0)
            {
                foreach (var button in activeButtonList)
                {
                    Destroy(button?.gameObject);
                }
                activeButtonList.Clear();
            }

            Building selectedBuilding = e.SelectedGameObject.GetComponent<Building>();
            buildingImage.sprite = selectedBuilding.sprite;
            if (selectedBuilding.products.Count > 0)
            {
                for (int i = 0; i < selectedBuilding.products.Count; i++)
                {
                    productionButton productButton = Instantiate(selectedBuilding.products[i]);
                    productButton.transform.SetParent(productionList.transform);
                    activeButtonList.Add(productButton);
                    productButton.spawnBuilding=e.SelectedGameObject;


                }
            }
        }
    }

    
}
