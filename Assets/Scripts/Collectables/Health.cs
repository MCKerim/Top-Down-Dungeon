using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : Collectable
{
    [SerializeField] private int health;

    public override void Collect()
    {
        playerTransform.GetComponent<PlayerController>().AddHealth(health);
        InstantiateParticles();
        Destroy(gameObject);
    }
}
