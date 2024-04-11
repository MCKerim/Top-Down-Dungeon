using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerController : MonoBehaviour, ITakeDamage
{
    private float movementSpeed = 10f;
    private Rigidbody playerRb;

    private float maxHealth = 200f;
    private float currentHealth;

    private int coins;

    [SerializeField] private float dashSpeed;
    private float dashCharger = 5f;
    private float dashCost = 20f;
    private float maxDash = 100f;
    private float currentDash;

    private UIHandler uiHandler;

    [SerializeField] private WeaponHolder weapons;

    private SaveSystem saveSystem;

    private CameraController cameraController;

    [SerializeField] private Animator animator;

    void Start()
    {
        saveSystem = SaveSystem.current;

        cameraController = FindObjectOfType<CameraController>();
        uiHandler = FindObjectOfType<UIHandler>();

        playerRb = GetComponent<Rigidbody>();

        currentHealth = maxHealth;
        cameraController.SetMaxValueHealthBar(maxHealth);

        currentDash = maxDash;
        cameraController.SetMaxValueDashBar(maxDash);

        cameraController.SetTextWeaponDisplay(weapons.GetWeaponName());
    }

    private void Update()
    {
        WeaponInput();

        if (Input.GetKeyDown(KeyCode.Space) && (currentDash - dashCost) >= 0)
        {
            Dash();
        }

        UpdateDashValue();
    }

    void FixedUpdate()
    {
        Movement();
        Rotation();
    }

    private void Movement()
    {
        //With Forces:
        //playerRb.AddForce(Vector3.forward * movementSpeed * Time.fixedDeltaTime * Input.GetAxis("Vertical"), ForceMode.Force);
        //playerRb.AddForce(Vector3.right * movementSpeed * Time.fixedDeltaTime * Input.GetAxis("Horizontal"), ForceMode.Force);

        //Without Force:
        Vector3 movementDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        playerRb.MovePosition(playerRb.position + movementDirection * movementSpeed * Time.fixedDeltaTime);

        if(animator != null)
        {
            float yRotation = transform.eulerAngles.y;

            if (yRotation <= 45 || yRotation >= 315)
            {
                Debug.Log("normal");
                //Ganz normal, geradeaus ist geradeaus
                animator.SetFloat("ForwardSpeed", movementDirection.x);
                animator.SetFloat("RightSpeed", movementDirection.z);
            }
            else if (yRotation >= 135 && yRotation <= 225)
            {
                Debug.Log("umgekehrt");
                //umgekehrt, geradeaus ist rückwärts
                animator.SetFloat("ForwardSpeed", -movementDirection.x);
                animator.SetFloat("RightSpeed", -movementDirection.z);
            }
            else if (yRotation > 45 && yRotation < 135)
            {
                Debug.Log("geradeaus ist rechts");
                // geradeaus ist rechts
                animator.SetFloat("ForwardSpeed", -movementDirection.z);
                animator.SetFloat("RightSpeed", -movementDirection.x);
            }
            else if (yRotation > 225 && yRotation < 315)
            {
                Debug.Log("geradeaus ist links");
                //geradeaus ist links
                animator.SetFloat("ForwardSpeed", movementDirection.z);
                animator.SetFloat("RightSpeed", movementDirection.x);
            }

            animator.SetFloat("ForwardSpeed", movementDirection.x);
            animator.SetFloat("RightSpeed", movementDirection.z);
        }
    }

    private void Rotation()
    {
        Vector2 mousePos = Input.mousePosition;
        Vector2 screenCenter = new Vector2(Screen.width / 2, Screen.height / 2);

        Vector3 mouseDirection2D = (mousePos - screenCenter).normalized;
        Vector3 mouseDirection3D = new Vector3(mouseDirection2D.x, 0, mouseDirection2D.y);

        Vector3 targetPoint = transform.position + mouseDirection3D;

        transform.LookAt(targetPoint);
    }

    private void Dash()
    {
        currentDash -= dashCost;
        cameraController.SetValueDashBar(currentDash);

        Vector3 dashDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        if(dashDirection.magnitude != 0)
        {
            playerRb.AddForce(dashDirection * dashSpeed, ForceMode.VelocityChange);
        }
        else
        {
            playerRb.AddForce(transform.forward * dashSpeed, ForceMode.VelocityChange);
        }
    }

    private void UpdateDashValue()
    {
        if ((currentDash + dashCharger * Time.deltaTime) <= maxDash)
        {
            currentDash += dashCharger * Time.deltaTime;
            cameraController.SetValueDashBar(currentDash);
        }
    }
    
    private void WeaponInput()
    {
        //Shooting
        if (Input.GetMouseButton(0))
        {
            weapons.Attack();
        }

        //Change Weapon
        if (Input.GetAxis("Mouse ScrollWheel") > 0f)
        {
            weapons.ChangeWeaponUp(true);
            cameraController.SetTextWeaponDisplay(weapons.GetWeaponName());
        }
        else if (Input.GetAxis("Mouse ScrollWheel") < 0f)
        {
            weapons.ChangeWeaponUp(false);
            cameraController.SetTextWeaponDisplay(weapons.GetWeaponName());
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            weapons.ChangeWeaponTo(1);
            cameraController.SetTextWeaponDisplay(weapons.GetWeaponName());
        }
    }

    public void TakeDamage(float damage)
    {
        if(damage >= 20)
        {
            StartCoroutine(cameraController.ScreenShake(0.2f, 0.3f));
        }

        currentHealth -= damage;
        cameraController.SetValueHealthBar(currentHealth);

        if (currentHealth <= 0)
        {
            GameOver();
        }
    }

    public float GetMaxHealth()
    {
        return maxHealth;
    }

    public void GameOver()
    {
        uiHandler.ShowGameOverPanel(true);
        saveSystem.playerLost();
    }

    public void Win()
    {
        int currentCoins = saveSystem.GetCoins() + coins;
        saveSystem.SetCoins(currentCoins);

        uiHandler.ShowWinPanel(true);
        saveSystem.playerWon();
    }

    public void AddCoins(int number)
    {
        coins += number;
        cameraController.SetTextCoinsText(coins.ToString());
    }

    public void AddHealth(int number)
    {
        if(currentHealth+number > maxHealth)
        {
            currentHealth = maxHealth;
        }
        else
        {
            currentHealth += number;
        }
        cameraController.SetValueHealthBar(currentHealth);
    }
}
