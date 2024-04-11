using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponHolder : MonoBehaviour
{
    public GameObject[] weapons;
    private int currentWeapon = 0;

    // Start is called before the first frame update
    void Start()
    {
        weapons[currentWeapon].SetActive(true);
    }

    public void ChangeWeaponTo(int weaponNumber)
    {
        if(weaponNumber >= 0 && weaponNumber <= weapons.Length-1)
        {
            weapons[currentWeapon].SetActive(false);
            currentWeapon = weaponNumber;
            weapons[currentWeapon].SetActive(true);
        }
        else
        {
            Debug.Log("Weapon " + weaponNumber + " doesnt exist.");
        }
    }

    public void ChangeWeaponUp(bool up)
    {
        weapons[currentWeapon].SetActive(false);

        if(up)
        {
            if ((currentWeapon + 1) <= weapons.Length - 1)
            {
              currentWeapon += 1;
              weapons[currentWeapon].SetActive(true);
            }
            else
            {
              currentWeapon = 0;
              weapons[currentWeapon].SetActive(true);
            }
        }
        else
        {
            if ((currentWeapon - 1) >= 0)
            {
                currentWeapon -= 1;
                weapons[currentWeapon].SetActive(true);
            }
            else
            {
                currentWeapon = weapons.Length - 1;
                weapons[currentWeapon].SetActive(true);
            }
        }
    }

    public void Attack()
    {
        weapons[currentWeapon].GetComponent<Weapon>().Attack();
    }

    public string GetWeaponName()
    {
        return weapons[currentWeapon].name;
    }
}
