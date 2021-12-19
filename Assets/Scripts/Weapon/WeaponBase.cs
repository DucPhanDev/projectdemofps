using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathologicalGames;

public class WeaponBase : MonoBehaviour
{
    [SerializeField] Transform bulletTrans;
    [SerializeField] Transform bulletSpawnPos;

    private int currentAmmo;
    private int maxAmmo;

    public Animator weaponAnimator;

    public virtual void Setup()
    {
        
    }

    public virtual void OnShoot(Vector3 worldPoint)
    {
        Transform go = PoolManager.Pools["BulletPlayer_Pool"].Spawn(bulletTrans, bulletSpawnPos.position, Quaternion.identity);
        go.SetParent(null);
        go.GetComponent<BulletPlayer>().Setup();
        go.LookAt(worldPoint);
    }
}
