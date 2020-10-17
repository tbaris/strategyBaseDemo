using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PooledObject : MonoBehaviour
{

    public GameObjectPool MyPool;

    public void ReturnToPool()
    {
        MyPool.ReturnToPool(this);
    }
}
