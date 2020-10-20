using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
    //pooled menu button behaviour
    public class PooledButton : Button
    {
   
        public GameObject building;
  
        public BuildingMenuPool MyPool;

        protected override void Awake()
        {
            base.Awake();
            this.onClick.AddListener(SendTypeToSpawn);
         }

        private void SendTypeToSpawn()//if clicked send info to game controller
        {
            GameController.Instance.StartBuilding(building);
        }

        public void ReturnToPool()
        {
            MyPool.ReturnToPool(this);
        }




    }
}
