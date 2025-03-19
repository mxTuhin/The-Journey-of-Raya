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
    
    public virtual void ActivatePowerUp(Transform target)
    {
        //NOTE: Send Command To UiManager
    }

    public virtual void ActivatePowerUp()
    {
        //NOTE: Send Command To UiManager
    }
    
    public virtual void DeactivatePowerUp()
    {
        //NOTE: Send Command To UiManager
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