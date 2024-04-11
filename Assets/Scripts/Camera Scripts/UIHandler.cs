using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIHandler : MonoBehaviour
{
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private GameObject controllsPanel;
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private GameObject winPanel;

    [SerializeField] private TextMeshProUGUI waveDisplayText;
    [SerializeField] private GameObject waveDisplayPanel;
    [SerializeField] private LeanTweenType waveDisplayTextAlpha;
    [SerializeField] private LeanTweenType waveDisplayTextScale;

    public void ShowPausePanel(bool show)
    {
        pausePanel.SetActive(show);
        if (show)
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
        }
    }

    public void ShowGameOverPanel(bool show)
    { 
        gameOverPanel.SetActive(show);

        if(show)
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
        }
    }

    public void ShowWinPanel(bool show)
    {
        winPanel.SetActive(show);

        if (show)
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
        }
    }

    public void ShowContollsPanel(bool show)
    {
        pausePanel.SetActive(!show);
        controllsPanel.SetActive(show);
    }

    public void StartMenü()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("3D Menü 1");
    }

    public void RestartGame()
    {
        Time.timeScale = 1;
        string currentScene = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(currentScene);
    }

    public void ShowWaveDisplayText(int nextWave, int waveMax)
    {
        waveDisplayPanel.gameObject.SetActive(true);
        waveDisplayText.SetText("Wave " + nextWave + " - " + waveMax);
        LeanTween.alphaCanvas(waveDisplayPanel.GetComponent<CanvasGroup>(), 1, 2).setEase(waveDisplayTextAlpha);
        LeanTween.scale(waveDisplayPanel, new Vector3(1, 1, 1), 2).setEase(waveDisplayTextScale).setOnComplete(HideWaveDisplayText);
    }

    private void HideWaveDisplayText()
    {
        LeanTween.alphaCanvas(waveDisplayPanel.GetComponent<CanvasGroup>(), 0, 1).setEase(waveDisplayTextAlpha).setDelay(1f).setOnComplete(DeactivateWaveDisplayText);
    }

    private void DeactivateWaveDisplayText()
    {
        waveDisplayPanel.gameObject.SetActive(false);
        waveDisplayPanel.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
    }
}
