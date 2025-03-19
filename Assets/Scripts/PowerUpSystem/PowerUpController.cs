using UnityEngine;

public class PowerUpController : MonoBehaviour
{
    [SerializeField] private KawaiAttack kawaiAttack;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public KawaiAttack GetKawaiAttack()
    {
        return kawaiAttack;
    }
}
