using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mine : MonoBehaviour, ISetDamage
{
    public bool isActive;

    public ParticleSystem explosion;

    public float Damage { get; set; }

    private void OnTriggerEnter(Collider other)
    {
        if(isActive)
        {
            ITakeDamage damageTaker = other.gameObject.GetComponent<ITakeDamage>();

            if (damageTaker != null)
            {
                damageTaker.TakeDamage(Damage);
                Instantiate(explosion, transform.position, explosion.transform.rotation);
                Destroy(gameObject);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        isActive = true;
    }
}
