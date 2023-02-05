using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIData : MonoBehaviour
{
    //Sparar lista �ver m�ltavlor och hinder
    public List<Transform> targets = null;
    public Collider2D[] obstacles = null;
    //Den aktuella m�ltavlan
    public Transform currentTarget;

    public int GetTargetCount() => targets == null ? 0 : targets.Count;
}
