using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class KawaiAttack : AssistCrewBase
{
    [SerializeField] private List<KawaiObject> kawaiSlimeObjects;
    [SerializeField] private int objectSpawnCount;
    
    private IEnumerator _spawnRoutine;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void Start()
    {
        base.Start();
    }
    
    public override void ActivatePowerUp(Transform target)
    {
        base.ActivatePowerUp();
        if(_spawnRoutine!=null) 
            StopCoroutine(_spawnRoutine);
        _spawnRoutine = SpawnKawaiSlimeRoutine(target);
        StartCoroutine(_spawnRoutine);
    }

    private IEnumerator SpawnKawaiSlimeRoutine(Transform target)
    {
        Transform spawnPos = WorldController.GetInstance().GetPlayerController().transform;
        ParticleSystem particleSystem = ObjectPool.GetInstance().GetObject(powerUpSpawnParticle.gameObject).GetComponent<ParticleSystem>();
        particleSystem.transform.position = spawnPos.position;
        for(int i=0; i<objectSpawnCount; i++)
        {
            KawaiObject kawaiObject = ObjectPool.GetInstance().GetObject(kawaiSlimeObjects[Random.Range(0, kawaiSlimeObjects.Count)].gameObject).
                GetComponent<KawaiObject>();
            Transform kawaiTransform = kawaiObject.transform;
            kawaiTransform.position = spawnPos.position+new Vector3(Random.Range(-1, 1), -1, Random.Range(-1, 1));
            kawaiObject.transform.DOMove(new Vector3(kawaiTransform.position.x, kawaiTransform.position.y+1, kawaiTransform.position.z), 0.75f).OnComplete(() =>
            {
                kawaiObject.InitiateAttackOnTarget(target);
            });
            yield return new WaitForSeconds(Random.Range(0.05f, 0.1f));
        }
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        base.Update();
    }
    
    public void UpdateKawaiSpawnCount(int amount)
    {
        objectSpawnCount += amount;
    }
}
