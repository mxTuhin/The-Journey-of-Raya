using UnityEditor.Animations;
using UnityEngine;

public class Humanoid : MonoBehaviour
{
    [SerializeField] private Animator animator;
    private AnimController _animController;
    
    [Space]
    [SerializeField] protected HealthController healthController;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        _animController = new AnimController(animator);
    }
    
    public void UpdateAnimator(Animator animator)
    {
        _animController.UpdateAnimator(animator);
    }
    
    public HealthController GetHealthController()
    {
        return healthController;
    }
    
    public AnimController AnimController
    {
        get => _animController;
        set => _animController = value;
    }
}
