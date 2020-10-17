using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayableObject : Unit
{
    public int factionId; //Change to enum later
    [FormerlySerializedAs("Health")] public int health;
    
}
