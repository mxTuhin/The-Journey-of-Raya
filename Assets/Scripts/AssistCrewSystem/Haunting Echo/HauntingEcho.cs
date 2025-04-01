using DG.Tweening;
using UnityEngine;

public class HauntingEcho : AssistCrewBase
{
    [SerializeField] private HauntingGhostSpawner hauntingGhostSpawner;
    
    public override void ActivatePowerUp(Transform target)
    {
        base.ActivatePowerUp();
        
        HauntingGhostSpawner _hauntingGhostSpawner = ObjectPool.GetInstance().GetObject(this.hauntingGhostSpawner.gameObject).GetComponent<HauntingGhostSpawner>();
        
        _hauntingGhostSpawner.transform.position = WorldController.GetInstance().GetPlayerController().transform.position+Vector3.up;
        
        Transform spawnPos = WorldController.GetInstance().GetPlayerController().transform;
        ParticleSystem particleSystem = ObjectPool.GetInstance().GetObject(powerUpSpawnParticle.gameObject).GetComponent<ParticleSystem>();
        particleSystem.transform.position = spawnPos.position;
        
        _hauntingGhostSpawner.transform.DOScale(Vector3.one, 0.35f).SetEase(Ease.OutQuint).OnComplete(() =>
        {
            _hauntingGhostSpawner.transform.DOScale(new Vector3(1, 0.65f, 1), 0.15f).SetEase(Ease.InQuint).OnComplete(
                () =>
                {
                    _hauntingGhostSpawner.transform.DOScale(Vector3.one, 0.15f).SetEase(Ease.OutQuint).OnComplete(() =>
                    {
                        _hauntingGhostSpawner.Init(_hauntingGhostSpawner.transform);
                        Reset(_hauntingGhostSpawner);
                    });
                });
        });
        _hauntingGhostSpawner.transform.DOMove(spawnPos.transform.position+Vector3.up*3, 0.35f).SetEase(Ease.OutQuint);

    }
    
    
    public override void DeactivatePowerUp()
    {
        base.DeactivatePowerUp();
    }

    private void Reset(HauntingGhostSpawner _hauntingGhostSpawner)
    {
        _hauntingGhostSpawner.transform.localScale = Vector3.zero;
        ObjectPool.GetInstance().ReturnToPool(_hauntingGhostSpawner.gameObject);
    }
    
}
