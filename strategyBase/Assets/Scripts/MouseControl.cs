using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MouseControl : MonoBehaviour
{
    

    public static MouseControl Instance;
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    void Update()
    {
        if(EventSystem.current.IsPointerOverGameObject())
            return;

        if (Input.GetMouseButtonDown(0))
        {

            GameController.Instance.OnLeftClick();
        }
        if (Input.GetMouseButtonDown(1))
        {

            GameController.Instance.OnRightClick();
        }
        
        if (Input.GetMouseButtonDown(2))
        {

            GameController.Instance.OnMiddleClick();
        }
    }

    public GridCell GetGridPosOfCursor()
    {
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        
     
        return GridManager.Instance.GetCellAdress(worldPosition);
        
    }


}
