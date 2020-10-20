using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class ProductionButton : MonoBehaviour
    {
        public GameObject prefab;
        public GameObject spawnBuilding;
        private void Awake()
        {
            this.GetComponent<Button>().onClick.AddListener(OrderSpawn);//orders its building to spawn this buttons object
        }

        private void OrderSpawn()
        {
            spawnBuilding.GetComponent<Building>()?.SpawnUnit(prefab);
        }
    }
}
