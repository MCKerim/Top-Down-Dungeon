using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsCanvas : MonoBehaviour
{
    [SerializeField] private GameObject[] panels;
    private GameObject currentPanel;
    private string previousPanel;

    // Start is called before the first frame update
    void Start()
    {
        ChangePanel(panels[0].name);
    }

    public void ChangePanel(string targetPanel)
    {
        if(currentPanel != null)
        {
            currentPanel.SetActive(false);
        }

        int i = 0;
        while(panels[i].name != targetPanel)
        {
            i++;
        }

        if(currentPanel != null)
        {
            previousPanel = currentPanel.name;
        }

        currentPanel = panels[i];
        currentPanel.SetActive(true);
    }

    public void ChangeToPrevious()
    {
        if(previousPanel != null && currentPanel != panels[0])
        {
            ChangePanel(previousPanel);
        }
    }
}
