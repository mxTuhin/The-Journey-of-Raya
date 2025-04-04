using System;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

public class AttackState : CharacterState
{
    
    [Space]
    [Header("Combat System")]
    private bool isAttacking = false;
    public Transform target;
    [SerializeField] private Transform attackPos;
    [Tooltip("Offset Stoping Distance")][SerializeField] private float quickAttackDeltaDistance;
    [Tooltip("Offset Stoping Distance")][SerializeField] private float heavyAttackDeltaDistance;
    [SerializeField] private float knockbackForce = 10f; 
    [SerializeField] private float airknockbackForce = 10f; 
    [SerializeField] private float attackRange = 1f;
    [SerializeField] private float reachTime = 0.3f;
    [SerializeField] private LayerMask enemyLayer;
    
    [Space]
    [Header("Debug")]
    [SerializeField] private bool debug;
    
    private bool attackFinished = false;

    private EnemyManager enemyManager;
    
    private AttackType currentAttackType;

    private void Start()
    {
        enemyManager = EnemyManager.GetInstance();
    }

    void Update()
    {
        HandleInput();
    }
    
    private void FixedUpdate()
    {
        if(target == null)
        {
            return;
        }

        if((Vector3.Distance(stateManager.GetController().transform.position, target.position) >= enemyManager.GetTargetDetectionController().detectionRange))
        {
            NoTarget();
        }
    }
    
    void HandleInput()
    {
        if (stateManager.IsLightAttack)
        {
            Attack(AttackType.Light);
        }

        if (stateManager.IsHeavyAttack)
        {
            Attack(AttackType.Heavy);
        }

    }
    
    
    #region Attack, PerformAttack, Reset Attack, Change Target
  

    public void Attack(AttackType attackType)
    {
        if (isAttacking || attackType == AttackType.None)
        {
            return;
        }

        attackFinished = false;
        
        currentAttackType = attackType;
        // enemyManager.GetTargetDetectionController().GetEnemyInInputDirection(stateManager.GetInput().move, enemyManager.GetEnemies());
        stateManager.CanMove = false;
        enemyManager.GetTargetDetectionController().canChangeTarget = false;
        RandomAttackAnim(attackType);
       
    }

    private void RandomAttackAnim(AttackType attackType)
    {
        switch (attackType) 
        {
            case AttackType.Light: //Quick Attack

                QuickAttack();
                break;

            case AttackType.Heavy: //Heavy Attack
                HeavyAttack();
                break;

        }
       
    }

    void QuickAttack()
    {
        int attackIndex = Random.Range(1, 4);
        if (debug)
        {
            Debug.Log(attackIndex + "Light Attack");
        }

        switch (attackIndex)
        {
            case 1: //punch

                if (target != null)
                {
                    MoveTowardsTarget(target.position, quickAttackDeltaDistance, AnimController.AnimPunch);
                    isAttacking = true;
                }
                else
                {
                    PerformAttackAnimation(AnimController.AnimPunch);
                    // stateManager.CanMove = true;
                    enemyManager.GetTargetDetectionController().canChangeTarget = true;
                }

                break;

            case 2: //kick

                if (target != null)
                {
                    MoveTowardsTarget(target.position, quickAttackDeltaDistance, AnimController.AnimKick);
                    isAttacking = true;
                }
                else
                {
                    PerformAttackAnimation(AnimController.AnimKick);
                    // stateManager.CanMove = true;
                    enemyManager.GetTargetDetectionController().canChangeTarget = true;
                }
                   

                break;

            case 3: //mmakick

                if (target != null)
                {
                    MoveTowardsTarget(target.position, quickAttackDeltaDistance, AnimController.AnimMmaKick);

                    isAttacking = true;
                }
                else
                {
                    PerformAttackAnimation(AnimController.AnimMmaKick);
                    // stateManager.CanMove = true;
                    enemyManager.GetTargetDetectionController().canChangeTarget = true;
                }
               

                break;
        }
    }

    void HeavyAttack()
    {
        int attackIndex = Random.Range(1, 3);
        //int attackIndex = 2;
        if (debug)
        {
            Debug.Log(attackIndex + "Heavy Attack");
        }

        switch (attackIndex)
        {
            case 1: //heavyAttack1

                if (target != null)
                {
                    //MoveTowardsTarget(target.position, kickDeltaDistance, "heavyAttack1");
                    FaceThis(target.position);
                    _animController.SetBool(AnimController.AnimHeavyAttack1, true);
                    isAttacking = true;
                  
                }
                else
                {
                    _animController.SetBool(AnimController.AnimHeavyAttack1, true);
                    enemyManager.GetTargetDetectionController().canChangeTarget = true;
                    // stateManager.CanMove = true;
                }


                break;

            case 2: //heavyAttack2

                if (target != null)
                {
                    //MoveTowardsTarget(target.position, kickDeltaDistance, "heavyAttack2");
                    FaceThis(target.position);
                    _animController.SetBool(AnimController.AnimHeavyAttack2, true);
                    isAttacking = true;
                }
                else
                {
                    _animController.SetBool(AnimController.AnimHeavyAttack2, true);
                    // stateManager.CanMove = true;
                    enemyManager.GetTargetDetectionController().canChangeTarget = true;
                }

                break;
        }
    }

    public override void ResetAttack() // Animation Event ---- for Reset Attack
    {
        if(debug)
            Debug.Log("Reset Inside");
        _animController.SetBool(AnimController.AnimPunch, false);
        _animController.SetBool(AnimController.AnimKick, false);
        _animController.SetBool(AnimController.AnimMmaKick, false);
        _animController.SetBool(AnimController.AnimHeavyAttack1, false);
        _animController.SetBool(AnimController.AnimHeavyAttack2, false);
        stateManager.CanMove = true;
        enemyManager.GetTargetDetectionController().canChangeTarget = true;
        isAttacking = false;
        FinishAttack();
    }

    public override void PerformAttack() // Animation Event ---- for Attacking Targets
    {
        // Assuming we have a melee attack with a short range
       
        Collider[] hitEnemies = Physics.OverlapSphere(attackPos.position, attackRange, enemyLayer);

        foreach (Collider enemy in hitEnemies)
        {
            Rigidbody enemyRb = enemy.GetComponent<Rigidbody>();
            EnemySystem enemySystem = enemy.GetComponent<EnemySystem>();
            if (enemyRb != null)
            {
                // Calculate knockback direction
                Vector3 knockbackDirection = enemy.transform.position - stateManager.GetController().transform.position;
                knockbackDirection.y = airknockbackForce; // Keep the knockback horizontal

                // Apply force to the enemy
                enemyRb.isKinematic = false;
                enemyRb.AddForce(knockbackDirection.normalized * knockbackForce, ForceMode.Impulse);
                enemySystem.SpawnHitVfx(enemySystem.transform.position, currentAttackType);
            }
        }
    }

    private EnemySystem oldTarget;
    private EnemySystem currentTarget;
    public override void ChangeTarget(EnemySystem _target)
    {
        
        if(target != null)
        {
            //oldTarget = target_.GetComponent<EnemyBase>(); //clear old target
            // oldTarget.ActiveTarget(false);
        }
       
        target = _target.transform;

        oldTarget = _target; //set current target
        currentTarget = _target;
        // currentTarget.ActiveTarget(true);

    }

    private void NoTarget() // When player gets out of range of current Target
    {
        // currentTarget.ActiveTarget(false);
        currentTarget = null;
        oldTarget = null;
        target = null;
    }

    #endregion


    #region MoveTowards, Target Offset and FaceThis
    public void MoveTowardsTarget(Vector3 target_, float deltaDistance, int animHashId)
    {

        PerformAttackAnimation(animHashId);
        FaceThis(target_);
        Vector3 finalPos = TargetOffset(target_, deltaDistance);
        finalPos.y = 0;
        stateManager.GetController().transform.DOMove(finalPos, reachTime);

    }

    public override void GetClose() // Animation Event ---- for Moving Close to Target
    {
        Vector3 getCloseTarget = Vector3.zero;
        if (target == null)
        {
            if(oldTarget!=null)
                getCloseTarget = oldTarget.transform.position;
        }
        else
        {
            getCloseTarget = target.position;
        }

        if (getCloseTarget != Vector3.zero)
        {
            FaceThis(getCloseTarget);
            Vector3 finalPos = TargetOffset(getCloseTarget, 1.4f);
            finalPos.y = 0;
            stateManager.GetController().transform.DOMove(finalPos, 0.2f);
        }
    }

    void PerformAttackAnimation(int animHashId)
    {
        _animController.SetBool(animHashId, true);
    }

    public Vector3 TargetOffset(Vector3 target, float deltaDistance)
    {
        Vector3 position;
        position = target;
        return Vector3.MoveTowards(position, stateManager.GetController().transform.position, deltaDistance);
    }

    public void FaceThis(Vector3 target)
    {
        Vector3 target_ = new Vector3(target.x, target.y, target.z);
        Quaternion lookAtRotation = Quaternion.LookRotation(target_ - stateManager.GetController().transform.position);
        lookAtRotation.x = 0;
        lookAtRotation.z = 0;
        stateManager.GetController().transform.DOLocalRotateQuaternion(lookAtRotation, 0.2f);
    }
    #endregion

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPos.position, attackRange); // Visualize the attack range
    }

    private void FinishAttack()
    {
        stateManager.IsLightAttack = false;
        stateManager.IsHeavyAttack = false;
        currentAttackType = AttackType.None;
        stateManager.AttackType = AttackType.None;
        isAttacking = false;
        attackFinished = true;
        
        Invoke(nameof(ChangeBackToMoveState), 0.125f);
    }
    
    private void ChangeBackToMoveState()
    {
        if (attackFinished)
        {
            stateManager.ChangeState(stateManager.moveState);
        }
    }
    
    public override void EnterState()
    {
        base.EnterState();
        Attack(stateManager.AttackType);
        Debug.Log("Entering Attack State");
    }


    public override void ExitState()
    {
        base.ExitState();
        Debug.Log("Exiting Attack State");
    }
}

public enum AttackType
{
    None,
    Light,
    Heavy,
    Special,
    Ruin
}