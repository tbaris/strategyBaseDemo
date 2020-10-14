using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenResChangeCheck : MonoBehaviour
{
    private Vector2 resolution;
    public static event EventHandler<OnScreenResChangeArgs> OnScreenResChange;
    public static ScreenResChangeCheck Instance;
    public class OnScreenResChangeArgs : EventArgs
    {
        public Vector2 res;
    }

    private void Awake()
    {
        Instance = this;
        resolution = new Vector2(Screen.width, Screen.height);

    }

    private void Update()
    {
        if (resolution.x != Screen.width || resolution.y != Screen.height)
        {
           

            OnScreenResChange?.Invoke(this, new OnScreenResChangeArgs{ res = resolution});

            resolution.x = Screen.width;
            resolution.y = Screen.height;
        }
    }

  

}
