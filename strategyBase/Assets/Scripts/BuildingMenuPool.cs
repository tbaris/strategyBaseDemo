namespace Assets.Scripts
{
    public class BuildingMenuPool : ObjectPooler<PooledButton>
    {
        public BuildingMenuPool(PooledButton poolingObject )
        {
            Prefab = poolingObject;
      
        }

   
    }
}