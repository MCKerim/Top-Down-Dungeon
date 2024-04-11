using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileWeapon : Weapon
{
    public GameObject projectile;
    public float damage;

    public ParticleSystem shootParticle;

    public override void Attack()
    {
        if(canShoot)
        {
            if(shootParticle != null)
            {
                Instantiate(shootParticle, transform.position, transform.rotation);
            }

            GameObject projectileObject = Instantiate(projectile, transform.position, transform.rotation);
            projectileObject.GetComponent<ISetDamage>().Damage = damage;

            canShoot = false;
            timer = delay;
        }
    }
}
