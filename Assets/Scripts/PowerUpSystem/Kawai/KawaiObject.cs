using System;
using DG.Tweening;
using UnityEngine;

public class KawaiObject : MonoBehaviour, IPowerUpObject
{
    [SerializeField] private Animator animator;
    private AnimController _animController;


    [SerializeField] private float movementSpeed;
    private bool _moveTowardsTarget;

    [SerializeField] private Transform _target;
    
    public void Start()
    {
        _animController = new AnimController(animator);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            InitiateAttackOnTarget(_target);
        }

        if (_moveTowardsTarget)
        {
            //NOTE: Object Move Towards Target and Attach When Reach
            transform.position = Vector3.MoveTowards(transform.position, _target.position, movementSpeed*Time.deltaTime);
            transform.LookAt(_target);
            
            Vector3 direction = _target.position - transform.position;
            float distanceSqr = direction.sqrMagnitude;
            if (distanceSqr <= 1f)
            {
                _moveTowardsTarget = false;
                Attack();
            }
        }
    }

    public void InitiateAttackOnTarget(Transform target)
    {
        _target = target;
        _moveTowardsTarget = true;
        _animController.SetMove();
    }
    
    private void Attack()
    {
        transform.DOLocalJump(_target.position, 1, 1, 0.5f);
        _animController.SetAttack();
    }
    

    public PowerUpType GetPowerUpType()
    {
        return PowerUpType.KawaiAttack;
    }

    private void OnAnimatorMove()
    {
    }

    private void Reset()
    {
        _moveTowardsTarget = false;
        _target = null;
    }
}