using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts;
using UnityEngine;
using static ScreenResChangeCheck;

public class CameraMove : MonoBehaviour
{
    public static CameraMove Instance;

    private float _camSize;
    private Camera _mainCamera;
   
    private Vector2 _resolution;
    private float _verticalLimit;
    private float _horizontalLimit;
    private Vector2 _gridUpRightCorner;
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

        ScreenResChangeCheck.OnScreenResChange += SetCamLimits;

    }

    private void Start()
    {
        _mainCamera = Camera.main;
        _camSize = _mainCamera.orthographicSize;
        _resolution = new Vector2(Screen.height, Screen.width);
        _gridUpRightCorner = GridManager.Instance.gridUpRightCorner;
        SetCamLimits(this, new OnScreenResChangeArgs { Res = _resolution });
    }

    void Update()
    {
        

        if (Input.GetAxis("Horizontal") !=0f || Input.GetAxis("Vertical") != 0f)
        {
            MoveCamera();
        }
    }

    private void SetCamLimits(object sender, ScreenResChangeCheck.OnScreenResChangeArgs e)
    {
        float _screenHeight = e.Res.x;
        float _screenWidth = e.Res.y;
      
            _verticalLimit = _camSize;
            _horizontalLimit = _camSize * _screenWidth / _screenHeight;
      
    }

    private void MoveCamera()
    {
        float hInputAxis = Input.GetAxis("Horizontal");
        float vInputAxis = Input.GetAxis("Vertical");
        transform.position = new Vector3(
            Mathf.Clamp(transform.position.x + hInputAxis, _horizontalLimit, _gridUpRightCorner.y - _horizontalLimit), 
            Mathf.Clamp(transform.position.y + vInputAxis,_verticalLimit,_gridUpRightCorner.x - _verticalLimit), 
            transform.position.z);
    }
}
