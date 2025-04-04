using System;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

public class KawaiObject : MonoBehaviour, IAssistCrewObject
{
    [SerializeField] private Animator animator;
    private AnimController _animController;


    [SerializeField] private float movementSpeed;
    private bool _moveTowardsTarget;

    private EnemySystem _target;
    private Vector3 _targetPosition;

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
            transform.position = Vector3.MoveTowards(transform.position, _targetPosition, movementSpeed*Time.deltaTime);
            transform.LookAt(_targetPosition);
            
            Vector3 direction = _targetPosition - transform.position;
            float distanceSqr = direction.sqrMagnitude;
            if (distanceSqr <= 1f)
            {
                _moveTowardsTarget = false;
                Attack();
            }
        }
    }

    public void InitiateAttackOnTarget(EnemySystem target)
    {
        _target = target;
        _targetPosition = target.GetBodyPos().position + Random.insideUnitSphere * 0.5f;
        _moveTowardsTarget = true;
        _animController.SetMove();
    }
    
    private void Attack()
    {
        transform.DOLocalJump(_targetPosition, 1, 1, 0.5f).OnComplete(() =>
        {
            ParticleSystem particleSystem = ObjectPool.GetInstance().GetObject(attackBlastParticle.gameObject).GetComponent<ParticleSystem>();
            particleSystem.transform.position = _targetPosition;
            
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