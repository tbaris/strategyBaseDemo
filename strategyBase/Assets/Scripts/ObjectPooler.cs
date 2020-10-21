using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public abstract class ObjectPooler<TObject> where TObject : Component
{
    public TObject Prefab;
    
    private  Queue<TObject> _objects = new Queue<TObject>();

    

   

    public virtual TObject Get()// gets object from pool if there is one in or creates new objcet
    {
        /*
        if (Application.isEditor && _objects.Count > 20000)
        {
            
            Debug.Log("pooled object limit reached");
            EditorApplication.isPaused = true;
            

        }*/

        if (_objects.Count == 0)
        {
            AddObjects();
           
        }


        return _objects.Dequeue();
    }

    private void AddObjects()//adds object to pool
    {
        var newObject = GameObject.Instantiate(Prefab);
        newObject.gameObject.SetActive(false);
       
        _objects.Enqueue(newObject);
      
    }

    public void ReturnToPool(TObject returnThis)//return given objects an disables it
    {
        returnThis.gameObject.SetActive(false);
        _objects.Enqueue(returnThis);
    }
    
}



