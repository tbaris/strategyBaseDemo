using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class PooledButton : Button
    {
   
        public GameObject building;
  
        public BuildingMenuPool MyPool;

        protected override void Awake()
        {
            base.Awake();
            this.onClick.AddListener(SendTypeToSpawn);
         }

        private void SendTypeToSpawn()
        {
            GameController.Instance.StartBuilding(building);
        }

        public void ReturnToPool()
        {
            MyPool.ReturnToPool(this);
        }




    }
}
