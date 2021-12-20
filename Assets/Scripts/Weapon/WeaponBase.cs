using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathologicalGames;

public class WeaponBase : MonoBehaviour
{
    #region Serialize Field
    [SerializeField] protected Transform bulletTrans;
    [SerializeField] protected Transform bulletSpawnPos;
    [SerializeField] protected Transform effectGunFlash;
    [SerializeField] protected Transform effectBlood;
    [SerializeField] protected Transform effectImpact;
    [SerializeField] protected AudioSource audioSource;

    [SerializeField] protected AudioClip audioFireClip;
    [SerializeField] protected AudioClip audioReloadClip;

    [SerializeField] protected WeaponAnimatorController weaponAnimatorController;

    #endregion

    #region Private Variables
    private int currentAmmo;
    private int maxAmmo;
    private IEnumerator ieOnReload;
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
            PlayerUIController.Instance.OnUpdateCurrentAmmoText(currentAmmo);
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
    public float rateOfFire;
    public bool isReloading;
    #endregion

    public virtual void Setup()
    {
        _CurrentAmmo =_MaxAmmo = 30;
    }

    public virtual void OnReload()
    {
        if(!isReloading && currentAmmo<maxAmmo)
        {
            audioSource.PlayOneShot(audioReloadClip);
            isReloading = true;
            ieOnReload = IEOnActionReload();
            StartCoroutine(ieOnReload);    
        }
        
    }

    private IEnumerator IEOnActionReload()
    {
        weaponAnimatorController.ResetTrigger_fire();
        weaponAnimatorController.SetTrigger_reload();
        yield return new WaitForSeconds(3f);
        isReloading = false;
        _CurrentAmmo = _MaxAmmo;
    }

    public virtual void OnShoot(Ray ray)
    {
        if(!isReloading)
        {
            Transform effectFlashTrans = PoolManager.Pools["GunFlash_Pool"].Spawn(effectGunFlash, bulletSpawnPos);
            effectFlashTrans.localPosition = Vector3.zero;
            effectBlood.transform.forward = bulletSpawnPos.transform.forward;
            weaponAnimatorController.SetTrigger_fire();
            _CurrentAmmo -= 1;
            RaycastHit rcHit;
            audioSource.PlayOneShot(audioFireClip);
            if (Physics.Raycast(ray, out rcHit))
            {
                Transform objectHit = rcHit.transform;
                EnemyZombie zombie = objectHit.GetComponent<EnemyZombie>();
                if (zombie != null)
                {
                    PoolManager.Pools["Blood_Pool"].Spawn(effectBlood, rcHit.point, Quaternion.identity);
                    zombie.OnGotHit(30);
                }
                else
                {
                    PoolManager.Pools["DirtImpact_Pool"].Spawn(effectImpact, rcHit.point, Quaternion.identity);
                }
            }
        }
    }
}
