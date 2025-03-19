using UnityEngine;

public class PowerUpBase : MonoBehaviour
{
    [SerializeField] private PowerUpType powerUpType;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected virtual void Start()
    {
        
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        
    }

    #region Getters

    public PowerUpType GetPowerUpType()
    {
        return powerUpType;
    }

    #endregion

    #region Setters

    

    #endregion
}

public enum PowerUpType
{
    KawaiAttack,
}

public interface IPowerUpObject
{
    PowerUpType GetPowerUpType();
}