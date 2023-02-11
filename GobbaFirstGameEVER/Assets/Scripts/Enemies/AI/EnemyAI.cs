using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemyAI : MonoBehaviour
{
    [SerializeField]
    private List<SteeringBehaviour> steeringBehaviours;

    [SerializeField]
    private List<Detector> detectors;

    [SerializeField]
    private AIData aiData;

    [SerializeField]
    private float detectionDelay = 0.05f, aiUpdateDelay = 0.06f, attackDelay = 1f;

    [SerializeField]
    private float attackDistance = 0.5f;
    [SerializeField]
    private float attackMinDistance = 0f;

    public UnityEvent<Vector3> OnAttackPressed;
    public UnityEvent<Vector2> OnMovementInput, OnPointerInput;

    [SerializeField]
    private Vector2 movementInput;

    [SerializeField]
    private ContextSolver movementDirectionSolver;

    bool following = false;
    private void OnEnable()
    {
        InvokeRepeating("PerformDetection", 0, detectionDelay);
    }

    private void Start()
    {
    }
    private void PerformDetection()
    {
        foreach (Detector detector in detectors)
        {
            detector.Detect(aiData);
        }
    }

    private void Update()
    {
        if (aiData.currentTarget != null)
        {
            OnPointerInput?.Invoke(aiData.currentTarget.position);
            if (following == false)
            {
                following = true;
                StartCoroutine(ChaseAndAttack());
            }
        }
        else if (aiData.GetTargetCount() > 0)
        {
            aiData.currentTarget = aiData.targets[0];
        }
        OnMovementInput?.Invoke(movementInput);
    }
    private IEnumerator ChaseAndAttack()
    {
        if (aiData.currentTarget == null)
        {
            movementInput = Vector2.zero;
            following = false;
            yield return null;
        }
        else
        {
            float distance = Vector2.Distance(aiData.currentTarget.position, transform.position);

            if (distance < attackDistance && distance > attackMinDistance)
            {
                //Attack Logic
                movementInput = Vector2.zero;

                Vector3 direction = (aiData.currentTarget.position - transform.position).normalized;

                
                OnAttackPressed?.Invoke(direction);

                yield return new WaitForSeconds(attackDelay);
                StartCoroutine(ChaseAndAttack());
            }
            else
            {
                //FollowLogic
                movementInput = movementDirectionSolver.GetDirectionToMove(steeringBehaviours, aiData);
                if (distance < attackMinDistance)
                {
                    movementInput = -movementInput;
                }
                yield return new WaitForSeconds(aiUpdateDelay);
                StartCoroutine(ChaseAndAttack());
            }
        }
    }
}
