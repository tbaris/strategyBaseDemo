using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public abstract class ObjectPooler<TObject> where TObject : Component
{
    public TObject Prefab;
   
 //   public  static ObjectPooler<TObject> Instance { get; private set; }
    private  Queue<TObject> _objects = new Queue<TObject>();

    

    private void Awake()
    {
      //  Instance = this;
    }

    public virtual TObject Get()
    {
        if (Application.isEditor && _objects.Count > 20000)
        {
            
            Debug.Log("pooled object limit reached");
            EditorApplication.isPaused = true;
            

        }
        if (_objects.Count == 0)
        {
            AddObjects();
           
        }


        return _objects.Dequeue();
    }

    private void AddObjects()
    {
        var newObject = GameObject.Instantiate(Prefab);
        newObject.gameObject.SetActive(false);
       
        _objects.Enqueue(newObject);
      
    }

    public void ReturnToPool(TObject returnThis)
    {
        returnThis.gameObject.SetActive(false);
        _objects.Enqueue(returnThis);
    }
    
}



