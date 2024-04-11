using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CameraController : MonoBehaviour
{
    private GameObject player;
    private Transform playerTransform;
    private Vector3 offset;

    [SerializeField] private float smoothSpeed = 5f;

    [SerializeField] private IndicatorBar healthBar;
    [SerializeField] private TextMeshProUGUI coinsText;
    [SerializeField] private IndicatorBar dashBar;
    [SerializeField] private TextMeshProUGUI weaponDisplay;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerTransform = player.transform;
        offset = transform.position - playerTransform.position;
    }

    void FixedUpdate()
    {
        Vector3 desiredPosition = playerTransform.position + offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
        transform.position = smoothedPosition;
    }

    public IEnumerator ScreenShake(float magnitude, float duration)
    {
        while(duration > 0)
        {
            float x = Random.Range(-1f, 1f) * magnitude;
            float z = Random.Range(-1f, 1f) * magnitude;

            transform.localPosition = new Vector3(transform.localPosition.x + x, transform.localPosition.y, transform.localPosition.z + z);

            duration -= Time.deltaTime;

            if(Time.timeScale > 0)
            {
                duration -= Time.deltaTime;
            }
            else
            {
                duration -= 0.01f;
            }
            yield return null;
        }
    }

    public void SetMaxValueHealthBar(float value)
    {
        healthBar.SetMaxValue(value);
    }

    public void SetMaxValueDashBar(float value)
    {
        dashBar.SetMaxValue(value);
    }

    public void SetValueHealthBar(float value)
    {
        healthBar.SetValue(value);
    }

    public void SetValueDashBar(float value)
    {
        dashBar.SetValue(value);
    }

    public void SetTextWeaponDisplay(string text)
    {
        weaponDisplay.SetText(text);
    }

    public void SetTextCoinsText(string text)
    {
        coinsText.SetText(text);
    }
}