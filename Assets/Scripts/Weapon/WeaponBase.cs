using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathologicalGames;

public class WeaponBase : MonoBehaviour
{
    #region Serialize Field
    [SerializeField] protected Transform bulletTrans;
    [SerializeField] protected Transform bulletSpawnPos;
    #endregion

    #region Private Variables
    private int currentAmmo;
    private int maxAmmo;
    #endregion

    #region Protected Variables
    protected int _CurrentAmmo
    {
        get
        {
            return currentAmmo;
        }
        set
        {
            currentAmmo = value;
            if (currentAmmo == 0)
                OnReload();
        }
    }
    protected int _MaxAmmo
    {
        get
        {
            return maxAmmo;
        }
        set
        {
            maxAmmo = value;
        }
    }
    #endregion

    #region Public Variables
    public Animator weaponAnimator;
    #endregion

    public virtual void Setup()
    {
        _CurrentAmmo = _MaxAmmo;
    }

    public virtual void OnReload()
    {
        
    }

    public virtual void OnShoot(Vector3 worldPoint)
    {
        _CurrentAmmo -= 1;
        Transform go = PoolManager.Pools["BulletPlayer_Pool"].Spawn(bulletTrans, bulletSpawnPos.position, Quaternion.identity);
        go.SetParent(null);
        go.GetComponent<BulletPlayer>().Setup();
        go.LookAt(worldPoint);
    }
}
