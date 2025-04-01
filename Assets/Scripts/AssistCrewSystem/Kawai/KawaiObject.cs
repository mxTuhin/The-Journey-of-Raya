using System;
using DG.Tweening;
using UnityEngine;

public class KawaiObject : MonoBehaviour, IAssistCrewObject
{
    [SerializeField] private Animator animator;
    private AnimController _animController;


    [SerializeField] private float movementSpeed;
    private bool _moveTowardsTarget;

    private Transform _target;

    [Space] [SerializeField] private ParticleSystem attackBlastParticle;
    
    public void Start()
    {
        _animController = new AnimController(animator);
    }
    

    private void Update()
    {
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
        transform.DOLocalJump(_target.position, 1, 1, 0.5f).OnComplete(() =>
        {
            ParticleSystem particleSystem = ObjectPool.GetInstance().GetObject(attackBlastParticle.gameObject).GetComponent<ParticleSystem>();
            particleSystem.transform.position = _target.position;
            
            Reset();
            ObjectPool.GetInstance().ReturnToPool(gameObject);
        });
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