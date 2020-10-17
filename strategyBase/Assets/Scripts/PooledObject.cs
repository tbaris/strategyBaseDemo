using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PooledObject : MonoBehaviour
{

    public GameObjectPool myPool;

    public void returnToPool()
    {
        myPool.ReturnToPool(this);
    }
}
