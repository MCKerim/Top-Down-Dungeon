using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StatsPanel : MonoBehaviour
{
    private SaveSystem saveSystem;

    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private TextMeshProUGUI coinsText;
    [SerializeField] private TextMeshProUGUI winsText;
    [SerializeField] private TextMeshProUGUI lostsText;

    [SerializeField] private GameObject warningPanel;

    [SerializeField] private TMP_InputField cheatCodeInput;
    private string cheatCode = "KerimIstCool";

    private void Start()
    {
        saveSystem = SaveSystem.current;
        saveSystem.LevelChanged += OnLevelChange;
        saveSystem.CoinsChanged += OnCoinsChange;
        saveSystem.WinsChanged += OnWinsChange;
        saveSystem.LostsChanged += OnLostsChange;

        OnLevelChange();
        OnCoinsChange();
        OnWinsChange();
        OnLostsChange();
    }

    private void OnDestroy()
    {
        saveSystem.LevelChanged -= OnLevelChange;
        saveSystem.CoinsChanged -= OnCoinsChange;
        saveSystem.WinsChanged -= OnWinsChange;
        saveSystem.LostsChanged -= OnLostsChange;
    }

    private void OnLevelChange()
    {
        levelText.SetText("Level " + saveSystem.GetLevel());
    }

    private void OnCoinsChange()
    {
        coinsText.SetText("Coins: " + saveSystem.GetCoins());
    }

    private void OnWinsChange()
    {
        winsText.SetText("Wins: " + saveSystem.GetWins());
    }

    private void OnLostsChange()
    {
        lostsText.SetText("Losts: " + saveSystem.GetLosts());
    }

    public void ShowWarning()
    {
        warningPanel.SetActive(true);
    }

    public void CloseWarning()
    {
        warningPanel.SetActive(false);
    }

    public void ResetStats()
    {
        CloseWarning();
        saveSystem.ResetStats();
    }

    public void CheatCode()
    {
        if(cheatCode == cheatCodeInput.text)
        {
            cheatCodeInput.SetTextWithoutNotify("CheatCode Active.");
            saveSystem.Cheat();
        }
        else
        {
            cheatCodeInput.SetTextWithoutNotify("Wrong CheatCode.");
        }
    }
}
