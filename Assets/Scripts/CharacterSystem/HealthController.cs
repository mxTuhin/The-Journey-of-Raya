using UnityEngine;

public class HealthController: MonoBehaviour, IDamageable
{
    public float currentHealth { get; private set;  }
    public float MaxHealth { get; set; }
    public event IDamageable.TakeDamageEvent OnTakeDamage;
    public event IDamageable.DeathEvent OnDeath;
    public bool TakeDamage(float damage, DamageType damageType = DamageType.Normal)
    {
        throw new System.NotImplementedException();
    }

    public bool IsDead => currentHealth <= 0;

    public void Init(int maxHealth)
    {
        MaxHealth = maxHealth;
        currentHealth = maxHealth;
    }
    public void Init(int maxHealth, int currentHealth)
    {
        MaxHealth = maxHealth;
        this.currentHealth = currentHealth;
    }
    
    public void Heal(int amount)
    {
        currentHealth += amount;
        if (currentHealth > MaxHealth)
        {
            currentHealth = MaxHealth;
        }
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
    }
    
}

public interface IDamageable
{
    public float currentHealth { get; }

    public float MaxHealth { get; }

    public delegate void TakeDamageEvent(float damage, Vector3 position);
    public event TakeDamageEvent OnTakeDamage;

    public delegate void DeathEvent();
    public event DeathEvent OnDeath;

    //public bool TakeDamage(ShooterData data);
    public bool TakeDamage(float damage, DamageType damageType = DamageType.Normal);

}

public enum DamageType
{
    Normal,
    Fire,
    Ice,
    Poison
}