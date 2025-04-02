using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetDetectionController : MonoBehaviour
{
    private PlayerController playerControl;
    
    [Space]
    [Header("Target Detection")]
    public LayerMask whatIsEnemy;
    public bool canChangeTarget = true;

    [Tooltip("Detection Range: \n Player range for detecting potential targets.")]
    [Range(0f, 15f)] public float detectionRange = 10f;

    [Tooltip("Dot Product Threshold \nHigher Values: More strict alignment required \nLower Values: Allows for broader targeting")]
    [Range(0f, 1f)] public float dotProductThreshold = 0.15f;

    private Camera mainCamera;
    private EnemySystem currentTarget;

    [Space]
    [Header("Debug")]
    public bool debug;
    public Transform checkPos;
    
    private FiniteStateManager _finiteStateManager;
    private EnemyManager _enemyManager;

    void Awake()
    {
        mainCamera = Camera.main;
    }

    private void Start()
    {
        _finiteStateManager = WorldController.GetInstance().GetPlayerController().GetStateManager();
        _enemyManager = EnemyManager.GetInstance();
        StartCoroutine(RunEveryXms());
    }

    private IEnumerator RunEveryXms()
    {
        while (true)
        {
            yield return new WaitForSeconds(.1f); // Wait for 'x' milliseconds
            GetEnemyInInputDirection(_finiteStateManager.GetInput().move, _enemyManager.GetEnemies());
        }
    }

    #region Get Enemy In Input Direction

    public void GetEnemyInInputDirection(Vector2 _inputDirection, List<EnemySystem> allTargets)
    {
        if(allTargets.Count == 0)
        {
            return;
        }
        if (canChangeTarget)
        {
            if (currentTarget != null)
            {
                if(Vector3.Distance(GetPlayerControl().transform.position, currentTarget.transform.position) <=1)
                {
                    return;
                }
            }
            // Vector3 inputDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")).normalized;
            Vector3 inputDirection = _inputDirection;

            if (inputDirection != Vector3.zero)
            {
                inputDirection = mainCamera.transform.TransformDirection(inputDirection);
                inputDirection.y = 0;
                inputDirection.Normalize();


                EnemySystem closestEnemy = GetClosestEnemyInDirection(inputDirection, allTargets);

                if (closestEnemy != null && (Vector3.Distance(GetPlayerControl().transform.position, closestEnemy.transform.position)) <= detectionRange)
                {
                    GetPlayerControl().GetStateManager().GetAttackState().ChangeTarget(closestEnemy);
                    // Do something with the closest enemy in the input direction
                    Debug.Log("Closest enemy in direction: " + closestEnemy.name);
                    // OnEnemyFound?.Invoke(closestEnemy);
                }
            }

        }
    }
    
    EnemySystem GetClosestEnemyInDirection(Vector3 inputDirection, List<EnemySystem> allTargets)
    {
        EnemySystem closestEnemy = null;
        float maxDotProduct = dotProductThreshold; // Start with the threshold value

        foreach (EnemySystem enemy in allTargets)
        {
            Vector3 enemyDirection = (enemy.transform.position - GetPlayerControl().transform.position).normalized;
            float dotProduct = Vector3.Dot(inputDirection, enemyDirection);

            if (dotProduct > maxDotProduct)
            {
                maxDotProduct = dotProduct;
                closestEnemy = enemy;
            }
        }
        
        currentTarget = closestEnemy;
        return closestEnemy;
    }
    
    public PlayerController GetPlayerControl()
    {
        if(playerControl == null)
        {
            playerControl = WorldController.GetInstance().GetPlayerController();
        }
        return playerControl;
    }

    #endregion
}
