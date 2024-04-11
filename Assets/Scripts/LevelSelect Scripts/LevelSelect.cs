using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class LevelSelect : MonoBehaviour
{
    [SerializeField] private LevelNode root;
    private LevelNode currentLevelNode;

    private Queue<GameObject> activeGameObjects;

    [SerializeField] private Transform[] nodePositions;

    private SaveSystem saveSystem;

    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private LeanTweenType levelTextAlpha;
    [SerializeField] private TextMeshProUGUI playerLevelText;

    // Start is called before the first frame update
    void Start()
    {
        saveSystem = SaveSystem.current;
        saveSystem.LevelChanged += OnLevelChange;
        OnLevelChange();

        activeGameObjects = new Queue<GameObject>();
        ChangeGameObjects(root);
    }

    private void OnDestroy()
    {
        saveSystem.LevelChanged -= OnLevelChange;
    }

    private void OnLevelChange()
    {
        playerLevelText.SetText("Lv: " + saveSystem.GetLevel());
    }

    public void ChangeGameObjects(LevelNode newLevelNode)
    {
        if(newLevelNode.GetChildren().Length > 0)
        {
            DestroyActiveGameObjects();
            currentLevelNode = newLevelNode;

            LeanTween.alphaCanvas(levelText.GetComponent<CanvasGroup>(), 0, 0.5f).setEase(levelTextAlpha).setOnComplete(ChangeLevelText);

            InstantiateGameObjects(currentLevelNode.GetChildren());
        }
        else
        {
            LoadScene(newLevelNode.GetLevelName());
        }
    }

    private void ChangeLevelText()
    {
        levelText.SetText(currentLevelNode.GetLevelName());
        LeanTween.alphaCanvas(levelText.GetComponent<CanvasGroup>(), 1, 1);
    }

    public void ChangeToPreviousGameObjects()
    {
        if(currentLevelNode.GetParent() != null)
        {
            ChangeGameObjects(currentLevelNode.GetParent());
        }
    }

    private void InstantiateGameObjects(LevelNode[] newGameObjects)
    {
        for (int i = 0; i < newGameObjects.Length; i++)
        {
            activeGameObjects.Enqueue(Instantiate(newGameObjects[i].gameObject, nodePositions[i].transform.position, transform.rotation));
        }
    }

    private void DestroyActiveGameObjects()
    {
        while (activeGameObjects.Count > 0)
        {
            Destroy(activeGameObjects.Dequeue());
        }
    }

    private void LoadScene(string scene)
    {
        SceneManager.LoadScene(scene);
    }
}
