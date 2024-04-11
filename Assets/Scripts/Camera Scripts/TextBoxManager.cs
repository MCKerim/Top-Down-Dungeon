using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class TextBoxManager : MonoBehaviour
{
    [SerializeField] private GameObject textBox;
    [SerializeField] private TextMeshProUGUI textGui;
    [SerializeField] private TextMeshProUGUI speakerText;

    [SerializeField] private LeanTweenType textBoxScaleIn;
    [SerializeField] private LeanTweenType textBoxScaleOut;

    [SerializeField] private List<TextObject> textObjects;

    private string currentText = "";
    private int currentTextId = 0;

    private float delay = 0.05f;
    private float timer;
    private bool isPrinting;

    public event Action TextBoxOpened;
    public event Action TextBoxClosed;

    private void Start()
    {
        textBox.SetActive(false);
        textGui.SetText("");
        speakerText.SetText("");
    }

    private void Update()
    {
        if(timer <= 0 && isPrinting)
        {
            PrintText();
        }
        else if(isPrinting)
        {
            timer -= Time.deltaTime;
        }
    }

    private void PrintText()
    {
        if (textGui.text.Length < currentText.Length)
        {
            textGui.text += currentText[textGui.text.Length];
            timer = delay;
        }
        else
        {
            isPrinting = false;
        }
    }

    public void SkipButtonClicked()
    {
        if (isPrinting)
        {
            textGui.SetText(currentText);
            isPrinting = false;
        }
        else
        {
            ShowNextText();
        }
    }

    private void ShowNextText()
    {
        currentTextId++;

        if(currentTextId > textObjects.Count)
        {
            CloseTextBox();
            return;
        }

        speakerText.SetText(textObjects[currentTextId - 1].speaker);
        textGui.SetText("");
        currentText = textObjects[currentTextId-1].text;
        timer = delay;
        isPrinting = true;
    }

    public void OpenTextBox()
    {
        if(!textBox.activeSelf)
        {
            TextBoxOpened?.Invoke();
            textBox.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
            textBox.gameObject.SetActive(true);
            LeanTween.scale(textBox, new Vector3(1, 1, 1), 1).setEase(textBoxScaleIn).setOnComplete(ShowNextText);
        }
    }

    public void CloseTextBox()
    {
        if(textBox.activeSelf && !isPrinting)
        {
            LeanTween.scale(textBox, new Vector3(0.1f, 0.1f, 0.1f), 1).setEase(textBoxScaleOut).setOnComplete(DeactivateTextBox);
        }
    }

    private void DeactivateTextBox()
    {
        textBox.SetActive(false);
        textGui.SetText("");
        speakerText.SetText("");
        TextBoxClosed?.Invoke();
    }
}

[System.Serializable]
public class TextObject
{
    public string speaker;
    public string text;
}
