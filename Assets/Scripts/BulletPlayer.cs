using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathologicalGames;
public class BulletPlayer : MonoBehaviour
{
    [SerializeField] private float bulletSpeed = 5;

    public void Setup()
    {
       
    }
    void Update()
    {
        transform.Translate(Vector3.forward * Time.deltaTime * bulletSpeed);
    }

}
