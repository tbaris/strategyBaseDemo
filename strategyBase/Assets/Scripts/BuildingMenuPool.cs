namespace Assets.Scripts
{
    //class to create pool for menu buttons
    public class BuildingMenuPool : ObjectPooler<PooledButton>
    {
        public BuildingMenuPool(PooledButton poolingObject ) 
        {
            Prefab = poolingObject;
      
        }

   
    }
}