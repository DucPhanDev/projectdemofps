using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathologicalGames;

public class PlayerController : SingletonMono<PlayerController>
{
    [SerializeField] private int maxHp;
    [SerializeField] private int currentHp;
    [SerializeField] private List<WeaponBase> lstWeapons;

    private int weaponIndex;
    private float timeSinceLastFire;

    public WeaponBase currentWeapon;

    private void Start()
    {
        Setup();
    }

    private void Setup()
    {
        weaponIndex = 0;
        foreach (WeaponBase e in lstWeapons)
        {
            e.gameObject.SetActive(false);
            e.Setup();
        }
        SwitchWeapon();
    }

    private void SwitchWeapon()
    {
        if (currentWeapon == null)
        {
            currentWeapon = lstWeapons[weaponIndex];
            currentWeapon.gameObject.SetActive(true);
        }
        else
        {
            if(weaponIndex == 0)
            {
                weaponIndex = 1;
                lstWeapons[weaponIndex].gameObject.SetActive(true);
                currentWeapon = lstWeapons[weaponIndex];
            }
            else
            {
                weaponIndex = 1;
                lstWeapons[weaponIndex].gameObject.SetActive(true);
                currentWeapon = lstWeapons[weaponIndex];
            }
        }
    }

    private void Update()
    {
        timeSinceLastFire += Time.deltaTime;
    }
    public void OnPressReload()
    {
        currentWeapon.OnReload();
    }
    public void OnShoot(Ray ray)
    {
        if(timeSinceLastFire >= currentWeapon.rateOfFire)
        {
            currentWeapon.OnShoot(ray);
            timeSinceLastFire = 0;
        }
    }

}
