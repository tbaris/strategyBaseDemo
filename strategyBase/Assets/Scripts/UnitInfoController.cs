using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal.VersionControl;
using UnityEngine;
using UnityEngine.UI;

public class UnitInfoController : MonoBehaviour
{
    [SerializeField] private Image buildingImage;
    [SerializeField] private GameObject productionList;
    private List<ProductionButton> _activeButtonList;
    private GameObject _selectedGo;
    private void Awake()
    {
        GameController.SelectedAObject += RefreshBuildingInfo;
        _activeButtonList = new List<ProductionButton>();
    }

    private void RefreshBuildingInfo(object sender, GameController.SelectedAObjectArgs e)
    {
       
        if (e.SelectedGameObject == null)
        {
            _selectedGo = null;

            if (_activeButtonList.Count > 0)
            {
                foreach (var button in _activeButtonList)
                {
                    Destroy(button?.gameObject);
                }
                _activeButtonList.Clear();
            }

        }
        else if(e.SelectedGameObject.GetComponent<Building>())
        {
           
            _selectedGo = e.SelectedGameObject;

            if (_activeButtonList.Count > 0)
            {
                foreach (var button in _activeButtonList)
                {
                    Destroy(button?.gameObject);
                }
                _activeButtonList.Clear();
            }

            Building selectedBuilding = e.SelectedGameObject.GetComponent<Building>();
            buildingImage.sprite = selectedBuilding.sprite;
            if (selectedBuilding.products.Count > 0)
            {
                for (int i = 0; i < selectedBuilding.products.Count; i++)
                {
                    ProductionButton productButton = Instantiate(selectedBuilding.products[i]);
                    productButton.transform.SetParent(productionList.transform);
                    _activeButtonList.Add(productButton);
                    productButton.spawnBuilding=e.SelectedGameObject;


                }
            }
        }else if (e.SelectedGameObject.GetComponent<Unit>())
        {
            _selectedGo = e.SelectedGameObject;

            if (_activeButtonList.Count > 0)
            {
                foreach (var button in _activeButtonList)
                {
                    Destroy(button?.gameObject);
                }
                _activeButtonList.Clear();
            }
            buildingImage.sprite = e.SelectedGameObject.GetComponent<Unit>().sprite;
        }
    }

    
}
