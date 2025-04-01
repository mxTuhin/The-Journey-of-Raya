using UnityEngine;

public class AssistCrewController : MonoBehaviour
{
    [SerializeField] private KawaiAttack kawaiAttack;
    [SerializeField] private HauntingEcho hauntingEcho;

    private EnemyManager _enemyManager;
    private PlayerController _playerController;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _enemyManager = EnemyManager.GetInstance();
        _playerController = WorldController.GetInstance().GetPlayerController();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Y))
        {
            kawaiAttack.ActivatePowerUp(_enemyManager.GetNearestEnemy(_playerController.transform.position).transform);
        }
        else if (Input.GetKeyDown(KeyCode.U))
        {
            hauntingEcho.ActivatePowerUp(_playerController.transform);
        }
    }
    
    public KawaiAttack GetKawaiAttack()
    {
        return kawaiAttack;
    }
    
    public HauntingEcho GetHauntingEcho()
    {
        return hauntingEcho;
    }
}
