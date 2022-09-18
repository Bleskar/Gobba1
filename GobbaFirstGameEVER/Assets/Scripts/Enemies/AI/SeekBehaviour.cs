using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SeekBehaviour : SteeringBehaviour
{
    [SerializeField]
    private float targetRechedThreshold = 0.5f;

    [SerializeField]
    private bool showGizmo = true;

    bool reachedLastTarget = true;

    //Gizmos
    private Vector2 targetPositionCached;
    private float[] interestTemp;

    public override (float[] danger, float[] interest) GetSteering(float[] danger, float[] interest, AIData aiData)
    {
        if (reachedLastTarget)
        {
            if (aiData.targets == null || aiData.targets.Count <= 0)
            {
                aiData.currentTarget = null;
                return (danger, interest);
            }
            else
            {
                reachedLastTarget = false;
                aiData.currentTarget = aiData.targets.OrderBy
                    (target => Vector2.Distance(target.position, transform.position)).First();
            }
        }

        if (aiData.currentTarget != null && aiData.targets != null && aiData.targets.Contains(aiData.currentTarget))
        {
            targetPositionCached = aiData.currentTarget.position;
        }

        if (Vector2.Distance(transform.position, targetPositionCached) < targetRechedThreshold)
        {
            reachedLastTarget = true;
            aiData.currentTarget = null;
            return (danger, interest);
        }

        Vector2 directionToTarget = (targetPositionCached - (Vector2)transform.position);
        for (int i = 0; i < interest.Length; i++)
        {
            float result = Vector2.Dot(directionToTarget.normalized, Directions.eightDirections[i]);

            if (result > 0)
            {
                float valueToPutIn = result;
                if (valueToPutIn > interest[i])
                {
                    interest[i] = valueToPutIn;
                }
            }
        }
        interestTemp = interest;
        return (danger, interest);
    }

    private void OnDrawGizmos()
    {
        if (showGizmo == false)
        {
            return;
        }

        Gizmos.DrawSphere(targetPositionCached, 0.2f);

        if (Application.isPlaying && interestTemp != null)
        {
            if (interestTemp != null)
            {
                Gizmos.color = Color.green;
                for (int i = 0; i < interestTemp.Length; i++)
                {
                    Gizmos.DrawRay(transform.position, Directions.eightDirections[i] * interestTemp[i]);
                }
                if (reachedLastTarget == false)
                {
                    Gizmos.color = Color.red;
                    Gizmos.DrawSphere(targetPositionCached, 0.1f);
                }
            }
        }
    }
}
