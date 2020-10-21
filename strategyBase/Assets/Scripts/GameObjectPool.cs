

namespace Assets.Scripts
{
    public class GameObjectPool : ObjectPooler<PooledObject>
    {

        //pool class for other objects

        public GameObjectPool(PooledObject poolingObject)
        {
            Prefab = poolingObject;

        }

    



    }
}
