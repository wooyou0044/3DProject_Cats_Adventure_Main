using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPool
{
    Queue<GameObject> pool;
    GameObject weaponPrefab;

    public WeaponPool(GameObject prefab, int size)
    {
        weaponPrefab = prefab;
        pool = new Queue<GameObject>();

        for(int i=0; i<size; i++)
        {
            GameObject weapon = Object.Instantiate(prefab);
            weapon.SetActive(false);
            pool.Enqueue(weapon);
        }
    }

    public GameObject GetWeapon()
    {
        if(pool.Count > 0)
        {
            return pool.Dequeue();
        }

        return null;
    }

    public void ReturnWeapon(GameObject weapon)
    {
        weapon.SetActive(false);
        pool.Enqueue(weapon);
    }
}
