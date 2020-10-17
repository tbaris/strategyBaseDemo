﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public abstract class Unit : MonoBehaviour
{
    public new string name;
    public Vector2Int baseSize;
    public Sprite sprite;
    public Button pooledButton;

}
