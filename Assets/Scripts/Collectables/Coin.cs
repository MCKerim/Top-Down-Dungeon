using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : Collectable
{
    public override void Collect()
    {
        playerTransform.GetComponent<PlayerController>().AddCoins(1);
        InstantiateParticles();
        Destroy(gameObject);
    }
}
