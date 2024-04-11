using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class SaveSystem : MonoBehaviour
{
    [SerializeField] private List<GameObject> allCharakters;
    private List<string> unlockedCharakters;
    public event Action unlockedCharactersChanged;

    private string selectedCharackterName;
    public event Action SelectedCharacterChanged;
      
    private float volumeUsw;

    private int level;
    public event Action LevelChanged;
    private int currentArenaLevel;

    private int coins;
    public event Action CoinsChanged;

    private int wins;
    public event Action WinsChanged;
    private int losts;
    public event Action LostsChanged;

    public static SaveSystem current;
    private void Awake()
    {
        if (SaveSystem.current != null)
        {
            Destroy(gameObject);
        }
        else
        {
            current = this;
            unlockedCharakters = new List<string>();

            StatsData data = LoadStats();
            if (data != null)
            {
                unlockedCharakters = data.unlockedCharakters;
                selectedCharackterName = data.selectedCharackterName;
                level = data.level;
                coins = data.coins;
                wins = data.wins;
                losts = data.losts;
            }
            else
            {
                ResetStats();
                SaveStats();
            }
        }
    }

    public int GetLevel()
    {
        return level;
    }

    public int GetCoins()
    {
        return coins;
    }

    public int GetWins()
    {
        return wins;
    }

    public int GetLosts()
    {
        return losts;
    }

    public void SetLevel(int value)
    {
        level = value;
        LevelChanged?.Invoke();
    }

    public void SetCoins(int value)
    {
        coins = value;
        CoinsChanged?.Invoke();
    }

    public void SetWins(int value)
    {
        wins = value;
        WinsChanged?.Invoke();
    }

    public void SetLosts(int value)
    {
        losts = value;
        LostsChanged?.Invoke();
    }

    private void OnApplicationQuit()
    {
        SaveStats();
    }

    public void SetCurrentArenaLevel(int selectedLevel)
    {
        currentArenaLevel = selectedLevel;
    }

    public void playerWon()
    {
        if(level == currentArenaLevel)
        {
            SetLevel(level+1);
        }
        SetWins(wins+1);
    }

    public void playerLost()
    {
        SetLosts(losts+1);
    }

    public bool IsUnlocked(string targetCharacter)
    {
        return unlockedCharakters.Contains(targetCharacter);
    }

    public void Unlock(string targetCharacter)
    {
        if(!IsUnlocked(targetCharacter))
        { 
            unlockedCharakters.Add(targetCharacter);
        }
    }

    public void SelectCharacter(string targetCharacter)
    {
        if(IsUnlocked(targetCharacter))
        {
            selectedCharackterName = targetCharacter;
            SelectedCharacterChanged?.Invoke();
        }
    }

    public GameObject GetSelectedCharacter()
    {
        for(int i=0; i < allCharakters.Count; i++)
        {
            if(allCharakters[i].name == selectedCharackterName)
            {
                return allCharakters[i];
            }
        }
        return null;
    }

    public string GetSelectedCharacterName()
    {
        return selectedCharackterName;
    }

    public void ResetStats()
    {
        unlockedCharakters.Clear();
        Unlock(allCharakters[0].name);
        SelectCharacter(unlockedCharakters[0]);

        SetLevel(1);
        SetCoins(0);
        SetWins(0);
        SetLosts(0);
        Debug.Log("Stats Resetet.");
    }

    public void Cheat()
    {
        SetLevel(100);
        SetCoins(1000);
        Debug.Log("Stats Cheatet.");
    }

    public void SaveStats()
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/stats.MCKerimData";
        FileStream stream = new FileStream(path, FileMode.Create);

        StatsData data = new StatsData(unlockedCharakters, selectedCharackterName, level, coins, wins,losts);

        formatter.Serialize(stream, data);
        stream.Close();

        Debug.Log("Data Saved.");
    }

    public StatsData LoadStats()
    {
        string path = Application.persistentDataPath + "/stats.MCKerimData";
        if(File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            StatsData data = formatter.Deserialize(stream) as StatsData;
            stream.Close();

            return data;
        }
        else
        {
            Debug.Log("Save File not found in " + path);
            return null;
        }
    }
}

[System.Serializable]
public class StatsData
{
    public List<string> unlockedCharakters;
    public string selectedCharackterName;

    public int level;
    public int coins;

    public int wins;
    public int losts;

    public StatsData(List<string> _unlockedCharacters, string _selectedCharackterName, int _level, int _coins, int _wins, int _losts)
    {
        unlockedCharakters = _unlockedCharacters;
        selectedCharackterName = _selectedCharackterName;
        level = _level;
        coins = _coins;
        wins = _wins;
        losts = _losts;
    }
}
