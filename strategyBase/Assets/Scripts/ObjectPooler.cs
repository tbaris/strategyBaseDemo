using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public abstract class ObjectPooler<TObject> where TObject : Component
{
    public TObject prefab;
   
 //   public  static ObjectPooler<TObject> Instance { get; private set; }
    private  Queue<TObject> objects = new Queue<TObject>();

    

    private void Awake()
    {
      //  Instance = this;
    }

    public virtual TObject Get()
    {
        if (Application.isEditor && objects.Count > 20000)
        {
            
            Debug.Log("pooled object limit reached");
            EditorApplication.isPaused = true;
            

        }
        if (objects.Count == 0)
        {
            AddObjects();
           
        }


        return objects.Dequeue();
    }

    private void AddObjects()
    {
        var newObject = GameObject.Instantiate(prefab);
        newObject.gameObject.SetActive(false);
       
        objects.Enqueue(newObject);
      
    }

    public void ReturnToPool(TObject returnThis)
    {
        returnThis.gameObject.SetActive(false);
        objects.Enqueue(returnThis);
    }
    
}



