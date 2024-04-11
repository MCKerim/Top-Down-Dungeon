using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyController : MonoBehaviour, ITakeDamage
{
    [SerializeField] private float health = 100f;
    [SerializeField] private IndicatorBar healthbar;
    private bool healthBarVisible;

    private Rigidbody EnemyRb;

    [SerializeField] private Weapon weapon;
    [SerializeField] private float maxAttackRange;

    [SerializeField] private float maxWalkRange;
    [SerializeField] private float minWalkRange;
    [SerializeField] private float movementSpeed;
    [SerializeField] private float rotationSpeed;

    private Transform player;
    private Vector3 currentTargetPos;

    [SerializeField] private Collectable[] collectables;
    [SerializeField] private int amount;

    private Vector3 targetDirection;
    private Quaternion targetRotation;

    // Start is called before the first frame update
    void Start()
    {
        EnemyRb = gameObject.GetComponent<Rigidbody>();
        player = GameObject.FindObjectOfType<PlayerController>().transform;
        healthbar.SetMaxValue(health);
    }

    private void FixedUpdate()
    {
        RotateToTarget(player.position);

        if(Vector3.Distance(transform.position, player.position) <= maxWalkRange && Vector3.Distance(transform.position, player.position) >= minWalkRange)
        {
            MoveToTarget(player.position);
            
        }

        if(Vector3.Distance(transform.position, player.position) <= maxAttackRange)
        {
            weapon.Attack();
        }
    }

    private void RotateToTarget(Vector3 targetPos)
    {
        targetDirection = new Vector3(targetPos.x, transform.position.y, targetPos.z) - transform.position;
        targetRotation = Quaternion.LookRotation(targetDirection, Vector3.up);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.fixedDeltaTime * rotationSpeed);
    }

    private void MoveToTarget(Vector3 targetPosition)
    {
        //Vector3 movementDirection = (targetPosition - transform.position).normalized;
        //EnemyRb.MovePosition(EnemyRb.position + movementDirection * movementSpeed * Time.fixedDeltaTime);
        EnemyRb.MovePosition(EnemyRb.position + transform.forward * movementSpeed * Time.fixedDeltaTime);
    }

    public void TakeDamage(float damage)
    {
        if(!healthBarVisible)
        {
            healthbar.gameObject.SetActive(true);
            healthBarVisible = true;
        }

        health -= damage;
        healthbar.SetValue(health);

        if(health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        DropItems();
        Destroy(gameObject);
    }

    private void DropItems()
    {
        for(int i=0; i < amount; i++)
        {
            //int randonNumber = Random.Range(0, collectables.Length);
            int randonNumber = 0;
            Instantiate(collectables[randonNumber], transform.position, transform.rotation);
        }
    }
}
