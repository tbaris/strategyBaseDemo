using UnityEngine;

namespace Assets.Scripts
{
    public class ProductionInfoPanelController : MonoBehaviour
    {
        [SerializeField] private GameObject gameInfoPanel;
        [SerializeField] private GameObject productionInfoPanel;
        private void Awake()
        {
            GameController.SelectedAObject += SwitchPanel;
        }

        private void Start()
        {
            gameInfoPanel?.gameObject.SetActive(true);
            productionInfoPanel?.gameObject.SetActive(false);
        }

        private void SwitchPanel(object sender, GameController.SelectedAObjectArgs e)//switches between panels when a cell clicked 
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
}
