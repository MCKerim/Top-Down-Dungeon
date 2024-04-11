using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Collectable : MonoBehaviour
{
    [HideInInspector] public Transform playerTransform;
    private Rigidbody thisRb;

    private float startSpeed = 30;
    private float currentSpeed;
    private float followRange = 10;
    private float pickupRange = 1;

    [SerializeField] public ParticleSystem pickUpParticle;

    // Start is called before the first frame update
    void Start()
    {
        playerTransform = GameObject.FindObjectOfType<PlayerController>().transform;
        thisRb = gameObject.GetComponent<Rigidbody>();

        currentSpeed = startSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        if(IsPlayerNearBy(followRange))
        {
            Vector3 targetDirection = (playerTransform.position - transform.position).normalized;
            thisRb.MovePosition(transform.position + targetDirection * currentSpeed * Time.deltaTime);
            currentSpeed += 20 * Time.deltaTime;

            if(IsPlayerNearBy(pickupRange))
            {
                Collect();
            }
        }
        else
        {
            if(currentSpeed != startSpeed)
            {
                currentSpeed = startSpeed;
            }
        }
    }

    private bool IsPlayerNearBy(float distance)
    {
        return Vector3.Distance(transform.position, playerTransform.position) <= distance;
    }

    public abstract void Collect();

    public void InstantiateParticles()
    {
        if(pickUpParticle != null)
            Instantiate(pickUpParticle, transform.position, Quaternion.identity);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, followRange);
        Gizmos.color = new Color(0, 1, 0);

        Gizmos.DrawWireSphere(transform.position, pickupRange);
        Gizmos.color = new Color(1, 0, 0);
    }
}
