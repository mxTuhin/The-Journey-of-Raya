using System;
using UnityEngine;

public class EnemySystem : MonoBehaviour
{
    
    public static Action<EnemySystem> OnEnemySpawn;
    public static Action<EnemySystem> OnEnemyDeath;

    private void OnEnable()
    {
        OnEnemySpawn?.Invoke(this);
    }
    
    private void OnDisable()
    {
        OnEnemyDeath?.Invoke(this);
    }
    
    public void Dead()
    {
        
    }
}
