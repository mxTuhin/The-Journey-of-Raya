using System;
using System.Collections.Generic;
using UnityEngine;

public class HauntingGhostSpawner : MonoBehaviour
{
    [SerializeField] private HauntingGhost hauntingGhost;
    [SerializeField] private ParticleSystem spawnFlashParticle;


    private int _enemyToTarget = 3;
    public int EnemyToTarget => _enemyToTarget;

    private void Start()
    {
        // _enemyToTarget = //TODO: GET COUNTER FROM SAVED DATA
    }


    public void Init(Transform spawnPos)
    {
        Debug.Log(spawnPos.position);
        List<EnemySystem> enemies = EnemyManager.GetInstance().GetMultipleNearestEnemies(WorldController.GetInstance().GetPlayerController().transform.position, _enemyToTarget);

        if (enemies.Count > 0)
        {
            ParticleSystem particleSystem = ObjectPool.GetInstance().GetObject(spawnFlashParticle.gameObject).GetComponent<ParticleSystem>();
            particleSystem.transform.position = spawnPos.position;
        }
        
        for (int i = 0; i < _enemyToTarget; i++)
        {
            if (i < enemies.Count)
            {
                HauntingGhost hauntingGhost = ObjectPool.GetInstance().GetObject(this.hauntingGhost.gameObject).GetComponent<HauntingGhost>();
                hauntingGhost.transform.position = spawnPos.transform.position;
                hauntingGhost.Init(enemies[i]);
            }
        }
        
    }
}
