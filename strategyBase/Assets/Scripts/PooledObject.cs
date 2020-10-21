using UnityEngine;

namespace Assets.Scripts
{
    public class PooledObject : MonoBehaviour
    {

        public GameObjectPool MyPool;

        public void ReturnToPool()
        {
            MyPool.ReturnToPool(this);
        }
    }
}
