using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemySystem : Humanoid
{
    [SerializeField] private Transform headPos;
    [SerializeField] private Transform bodyPos;
    
    [Header("Stun System")]
    [SerializeField] private ParticleSystem stunParticle;

    [SerializeField] private ParticleSystem hitParticle;
    [SerializeField] private ParticleSystem heavyHitParticle;

    private bool _isStunned;
    
    
    public static Action<EnemySystem> OnEnemySpawn;
    public static Action<EnemySystem> OnEnemyDeath;

    private void Start()
    {
        Init();
    }
    

    //TODO: INIT FROM SPAWNER
    public void Init()
    {
        OnEnemySpawn?.Invoke(this);
        healthController.Init(100); //NOTE: Add From Enemy Data
    }

    public void Dead()
    {
        OnEnemyDeath?.Invoke(this);
    }
    
    public Transform GetHeadPos()
    {
        return headPos;
    }
    
    public void InitStunEffect(float timer)
    {
        _isStunned = true;
        ParticleSystem particleSystem = ObjectPool.GetInstance().GetObject(stunParticle.gameObject).GetComponent<ParticleSystem>();
        particleSystem.transform.position = headPos.position+new Vector3(0, 0.25f, 0);

        StartCoroutine(StopStunEffect(timer, particleSystem));
    }

    private IEnumerator StopStunEffect(float timer, ParticleSystem particleSystem)
    {
        yield return new WaitForSeconds(timer);
        particleSystem.Stop();
        yield return new WaitForSeconds(0.25f);
        ObjectPool.GetInstance().ReturnToPool(particleSystem.gameObject);
        _isStunned = false;
    }
    
    public Transform GetBodyPos()
    {
        return bodyPos;
    }
    
    public void SpawnHitVfx(Vector3 hitPos, AttackType attackType)
    {
        ParticleSystem particleSystem = ObjectPool.GetInstance().GetObject(hitParticle.gameObject).GetComponent<ParticleSystem>();
        particleSystem.transform.position = hitPos+Random.insideUnitSphere*0.25f;
        particleSystem.Play();
        GetAnimController.CrossFadeInFixedTime(AnimController.GetHit, 0.15f);
        
        if(attackType == AttackType.Heavy)
        {
            ParticleSystem heavyParticle = ObjectPool.GetInstance().GetObject(heavyHitParticle.gameObject).GetComponent<ParticleSystem>();
            heavyParticle.transform.position = hitPos+Random.insideUnitSphere*0.5f;
            heavyParticle.Play();
        }
    }
}

public enum EnemyType
{
    Normal,
    Elite,
    Boss
}

public enum EnemyActionArea
{
    Ground,
    Air
}
