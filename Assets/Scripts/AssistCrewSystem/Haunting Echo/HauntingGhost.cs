using System;
using UnityEngine;

public class HauntingGhost : MonoBehaviour, IAssistCrewObject
{
    [SerializeField] private Animator animator;
    private AnimController _animController;

    private EnemySystem _target;
    private Transform _targetTransform;
    
    [SerializeField] private float movementSpeed;
    private bool _moveTowardsTarget = false;
    
    [Space] [SerializeField] private ParticleSystem attackBlastParticle;

    private void Start()
    {
        _animController = new AnimController(animator);
    }

    private void Update()
    {
        if (_moveTowardsTarget)
        {
            //NOTE: Object Move Towards Target and Attach When Reach
            transform.position = Vector3.MoveTowards(transform.position, _targetTransform.position, movementSpeed*Time.deltaTime);
            transform.LookAt(_targetTransform);
            
            Vector3 direction = _targetTransform.position - transform.position;
            float distanceSqr = direction.sqrMagnitude;
            if (distanceSqr <= 0.25f)
            {
                _moveTowardsTarget = false;
                Attack();
            }
        }
    }
    
    private void Attack()
    {
        ParticleSystem particleSystem = ObjectPool.GetInstance().GetObject(attackBlastParticle.gameObject).GetComponent<ParticleSystem>();
        particleSystem.transform.position = _targetTransform.position;
        
        _target.InitStunEffect(GameManager.GetInstance().GetStunTimer());
        
        Reset();
        ObjectPool.GetInstance().ReturnToPool(gameObject);
        _animController.SetAttack();
    }

    public void Init(EnemySystem target)
    {
        _target = target;
        _targetTransform = target.GetHeadPos();
        _moveTowardsTarget = true;
    }
    
    private void Reset()
    {
        _moveTowardsTarget = false;
        _target = null;
    }

    public PowerUpType GetPowerUpType()
    {
        return PowerUpType.HauntingEcho;
    }
}
