using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    public float delay;
    protected bool canShoot = true;
    protected float timer;

    private void Update()
    {
        if (!canShoot)
        {
            if (timer <= 0)
            {
                canShoot = true;
            }
            else
            {
                timer -= 1f * Time.deltaTime;
            }
        }
    }

    public abstract void Attack();
}
