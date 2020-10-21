using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class UnitInfoController : MonoBehaviour
    {
        [SerializeField] private Image selectedUnitImage;
        [SerializeField] private GameObject productionList;
        [SerializeField] private TextMeshProUGUI selectedUnitName;
        private List<ProductionButton> _activeButtonList;
        //private GameObject _selectedGo;
        private void Awake()
        {
            GameController.SelectedAObject += RefreshBuildingInfo;
            _activeButtonList = new List<ProductionButton>();
        }

      

        private void RefreshBuildingInfo(object sender, GameController.SelectedAObjectArgs e)// sets info panels sprite and buttons for selected unit
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
            else if(e.SelectedGameObject.GetComponent<PlayableObject>())
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

                Unit selectedUnit= e.SelectedGameObject.GetComponent<Unit>();
                selectedUnitImage.sprite = selectedUnit.sprite;
                selectedUnitName.text  = selectedUnit.name;
                if (selectedUnit.GetComponent<Building>() && selectedUnit.GetComponent<Building>().products.Count > 0)
                {
                    foreach (var t in selectedUnit.GetComponent<Building>().products)
                    {
                        ProductionButton productButton = Instantiate(t, productionList.transform, true);
                        _activeButtonList.Add(productButton);
                        productButton.spawnBuilding=e.SelectedGameObject;
                    }
                }
            }
            else
            {
                if (_activeButtonList.Count > 0)
                {
                    foreach (var button in _activeButtonList)
                    {
                        Destroy(button?.gameObject);
                    }
                    _activeButtonList.Clear();
                }
            }
        }

    
    }
}
