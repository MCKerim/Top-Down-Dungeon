using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LevelNode : MonoBehaviour
{
    [SerializeField] private string levelName;
    [SerializeField] private int requiredLevel;
    private bool unlocked;

    [SerializeField] private LevelNode levelParent;
    [SerializeField] private LevelNode[] levelChildren;

    private LevelSelect levelSelect;

    private SaveSystem saveSystem;

    [SerializeField] private float rotationSpeed = 15;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private GameObject arenaModell;

    private Color lockedColor = new Color(1.000f, 0.310f, 0.271f, 1.000f);

    private Animator animator;

    private void Start()
    {
        saveSystem = SaveSystem.current;
        saveSystem.LevelChanged += OnLevelChange;
        OnLevelChange();

        animator = GetComponent<Animator>();
        levelSelect = FindObjectOfType<LevelSelect>();
    }

    private void OnDestroy()
    {
        saveSystem.LevelChanged -= OnLevelChange;
    }

    private void OnLevelChange()
    {
        if (saveSystem.GetLevel() >= requiredLevel)
        {
            arenaModell.GetComponent<Renderer>().material.color = Color.white;
            nameText.SetText(levelName);
            nameText.color = Color.white;
            unlocked = true;
        }
        else
        {
            arenaModell.GetComponent<Renderer>().material.color = Color.black;
            nameText.SetText("Level " + requiredLevel + " required.");
            nameText.color = lockedColor;
            unlocked = false;
        }
    }

    private void FixedUpdate()
    {
        arenaModell.transform.Rotate(Vector3.up * rotationSpeed * Time.fixedDeltaTime);
    }

    public LevelNode[] GetChildren()
    {
        return levelChildren;
    }

    public LevelNode GetParent()
    {
        return levelParent;
    }

    public string GetLevelName()
    {
        return levelName;
    }

    private void OnMouseDown()
    {
        if(unlocked)
        {
            saveSystem.SetCurrentArenaLevel(requiredLevel);
            StartCoroutine(WaitForAnimation());
        }
    }

    IEnumerator WaitForAnimation()
    {
        animator.SetTrigger("scaleDown");
        yield return new WaitForSeconds(0.5f);
        levelSelect.ChangeGameObjects(this);
    }   
}
