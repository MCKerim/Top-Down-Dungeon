using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : MonoBehaviour, ISetDamage
{
    private Rigidbody rb;

    public float speed;
    public float Damage { get; set; }

    public ParticleSystem explosionPrefab;

    // Start is called before the first frame update
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        rb.velocity = transform.forward * speed;
        //rb.AddForce(transform.forward * speed * Time.fixedDeltaTime);
    }

    //private void FixedUpdate()
    //{
    //    rb.AddForce(transform.forward * speed * Time.fixedDeltaTime, ForceMode.VelocityChange);
    //}

    private void OnCollisionEnter(Collision collision)
    {
        ITakeDamage damageTaker = collision.gameObject.GetComponent<ITakeDamage>();

        if(damageTaker != null)
        {
            damageTaker.TakeDamage(Damage);
            collision.gameObject.GetComponent<Rigidbody>().AddForce(transform.forward * 500, ForceMode.Impulse);
        }

        ParticleSystem explosion = Instantiate(explosionPrefab, collision.contacts[0].point, transform.rotation);
        explosion.transform.Rotate(Vector3.up, 180f);

        Destroy(gameObject);
    }
}
