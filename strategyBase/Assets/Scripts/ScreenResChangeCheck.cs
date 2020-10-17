using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenResChangeCheck : MonoBehaviour
{
    public class OnScreenResChangeArgs : EventArgs
    {
        public Vector2 Res;
    }
    private Vector2 _resolution;
    public static event EventHandler<OnScreenResChangeArgs> OnScreenResChange;
    public static ScreenResChangeCheck Instance;
   

    private void Awake()
    {
        Instance = this;
        _resolution = new Vector2(Screen.width, Screen.height);

    }

    private void Update()
    {
        if (_resolution.x != Screen.width || _resolution.y != Screen.height)
        {
            _resolution.x = Screen.width;
            _resolution.y = Screen.height;

            OnScreenResChange?.Invoke(this, new OnScreenResChangeArgs{ Res = _resolution});

            
        }
    }

  

}
