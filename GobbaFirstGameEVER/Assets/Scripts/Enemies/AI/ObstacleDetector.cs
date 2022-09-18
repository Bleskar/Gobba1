using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleDetector : Detector
{
    [SerializeField]
    private float detectionRadius = 2;

    [SerializeField]
    private LayerMask layerMask;

    Collider2D[] colliders;

    public override void Detect(AIData aiData)
    {
        colliders = Physics2D.OverlapCircleAll(transform.position, detectionRadius, layerMask);
        aiData.obstacles = colliders;
    }
    

}
