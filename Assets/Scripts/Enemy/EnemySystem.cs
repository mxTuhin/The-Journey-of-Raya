using System;
using System.Collections;
using UnityEngine;

public class EnemySystem : Humanoid
{
    [SerializeField] private Transform headPos;
    [SerializeField] private Transform bodyPos;
    
    [Header("Stun System")]
    [SerializeField] private ParticleSystem stunParticle;

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
