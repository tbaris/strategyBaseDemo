using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class UnitInfoController : MonoBehaviour
    {
        [SerializeField] private Image buildingImage;
        [SerializeField] private GameObject productionList;
        private List<ProductionButton> _activeButtonList;
        //private GameObject _selectedGo;
        private void Awake()
        {
            GameController.SelectedAObject += RefreshBuildingInfo;
            _activeButtonList = new List<ProductionButton>();
        }

      

        private void RefreshBuildingInfo(object sender, GameController.SelectedAObjectArgs e)
        {
       
            if (e.SelectedGameObject == null)
            {
              //  _selectedGo = null;

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
           
               // _selectedGo = e.SelectedGameObject;

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
                    foreach (var t in selectedBuilding.products)
                    {
                        ProductionButton productButton = Instantiate(t, productionList.transform, true);
                        _activeButtonList.Add(productButton);
                        productButton.spawnBuilding=e.SelectedGameObject;
                    }
                }
            }else if (e.SelectedGameObject.GetComponent<PlayableObject>())
            {
               // _selectedGo = e.SelectedGameObject;

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
}
