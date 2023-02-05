using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIData : MonoBehaviour
{
    //Sparar lista över måltavlor och hinder
    public List<Transform> targets = null;
    public Collider2D[] obstacles = null;
    //Den aktuella måltavlan
    public Transform currentTarget;

    public int GetTargetCount() => targets == null ? 0 : targets.Count;
}
