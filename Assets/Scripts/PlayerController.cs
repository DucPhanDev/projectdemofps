using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathologicalGames;

public class PlayerController : SingletonMono<PlayerController>
{
    [SerializeField] private int maxHp;
    [SerializeField] private int currentHp;
    [SerializeField] private List<WeaponBase> lstWeapons;

    public WeaponBase currentWeapon;

    private void Start()
    {
        Setup();
    }

    private void Setup()
    {
        foreach (WeaponBase e in lstWeapons)
        {
            e.Setup();
            e.gameObject.SetActive(false);
        }
        lstWeapons[0].gameObject.SetActive(true);
        currentWeapon = lstWeapons[0];
    }
}
