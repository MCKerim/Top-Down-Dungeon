using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastWeapon : Weapon
{
    public float range;
    public float damage;
    public float hitForce;

    private LineRenderer lineRenderer;
    public ParticleSystem shootParticle;
    public ParticleSystem impactParticle;

    private void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    public override void Attack()
    {
        if (canShoot)
        {
            StartCoroutine(ShotEffect());
            lineRenderer.SetPosition(0, transform.position);

            if(shootParticle != null)
            {
                Instantiate(shootParticle, transform.position, transform.rotation);
            }

            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, range))
            {
                lineRenderer.SetPosition(1, hit.point);

                ITakeDamage damageTaker = hit.transform.GetComponent<ITakeDamage>();
                if (damageTaker != null)
                {
                    damageTaker.TakeDamage(damage);
                    hit.rigidbody.AddForce(-hit.normal * hitForce, ForceMode.Impulse);
                }

                if (impactParticle != null)
                {
                    Instantiate(impactParticle, hit.point, Quaternion.FromToRotation(Vector3.forward, hit.normal));
                }
            }
            else
            {
                lineRenderer.SetPosition(1, transform.position + transform.TransformDirection(Vector3.forward) * range);
            }

            canShoot = false;
            timer = delay;
        }
    }

    private IEnumerator ShotEffect()
    {
        lineRenderer.enabled = true;
        yield return new WaitForSeconds(0.07f);
        lineRenderer.enabled = false;
    }
}