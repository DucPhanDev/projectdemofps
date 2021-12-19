using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathologicalGames;

public class PlayerController : SingletonMono<PlayerController>
{
    [SerializeField] private int maxHp;
    [SerializeField] private int currentHp;

    public WeaponBase currentWeapon;
}
