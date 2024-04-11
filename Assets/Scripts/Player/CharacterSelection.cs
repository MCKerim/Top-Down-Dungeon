using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CharacterSelection : MonoBehaviour
{
    [SerializeField] private List<GameObject> characters;
    private GameObject currentCharacter;
    private CharacterScript currentCharacterScript;
    private int number = 0;

    [SerializeField] private float rotationSpeed;

    private SaveSystem saveSystem;

    [SerializeField] private TextMeshProUGUI characterText;
    [SerializeField] private TextMeshProUGUI priceText;
    [SerializeField] private TextMeshProUGUI descriptionText;
    [SerializeField] private TextMeshProUGUI coinsText;

    [SerializeField] private Color defalutColor;
    [SerializeField] private Color selectedColor;
    [SerializeField] private Color tooExpensiveColor;

    [SerializeField] private Transform characterSpawnPosition;

    private void Start()
    {
        saveSystem = SaveSystem.current;
        saveSystem.CoinsChanged += OnCoinsChange;
        OnCoinsChange();
        saveSystem.SelectedCharacterChanged += OnSelectedCharacterChange;
        
        InstantiateCharacter(SearchForCharacter(saveSystem.GetSelectedCharacterName()));
    }

    private void OnDestroy()
    {
        saveSystem.SelectedCharacterChanged -= OnSelectedCharacterChange;
        saveSystem.SelectedCharacterChanged -= OnCoinsChange;
    }

    private void FixedUpdate()
    {
        currentCharacter.transform.Rotate(Vector3.up * rotationSpeed * Time.fixedDeltaTime);
    }

    private void OnCoinsChange()
    {
        coinsText.SetText("Coins: " + saveSystem.GetCoins());
    }

    private void OnSelectedCharacterChange()
    {
        InstantiateCharacter(number);
    }

    private int SearchForCharacter(string targetCharacter)
    {
        for (int i=0;  i < characters.Count;  i++)
        {
            if(characters[i].GetComponent<CharacterScript>().characterName == targetCharacter)
            {
                return i;
            }
        }
        return 0;
    }

    public void ShowNextCharacter()
    {
        if(number+1 < characters.Count)
        {
            number++;
        }
        else
        {
            number = 0;
        }

        InstantiateCharacter(number);
    }

    public void ShowPreviousCharacter()
    {
        if (number - 1 >= 0)
        {
            number--;
        }
        else
        {
            number = characters.Count-1;
        }

        InstantiateCharacter(number);
    }

    private void InstantiateCharacter(int index)
    {
        if(currentCharacter != null)
        {
            Destroy(currentCharacter);
        }

        currentCharacter = Instantiate(characters[index], characterSpawnPosition.position, characterSpawnPosition.rotation);
        currentCharacter.transform.parent = gameObject.transform;

        currentCharacterScript = currentCharacter.GetComponent<CharacterScript>();

        characterText.SetText(currentCharacterScript.characterName);

        if(saveSystem.GetSelectedCharacterName() == currentCharacterScript.characterName)
        {
            priceText.SetText("Selected");
            priceText.color = selectedColor;
        }
        else if(saveSystem.IsUnlocked(currentCharacterScript.characterName))
        {
            priceText.SetText("Unlocked");
            priceText.color = defalutColor;
        }
        else
        {
            priceText.SetText("Cost: " + currentCharacterScript.prize);
            priceText.color = defalutColor;
        }

        descriptionText.SetText(currentCharacterScript.characterDescription);
    }

    private void OnMouseDown()
    {
        if (saveSystem.IsUnlocked(currentCharacterScript.characterName))
        {
            saveSystem.SelectCharacter(currentCharacterScript.characterName);

            priceText.SetText("Selected");
            priceText.color = selectedColor;
        }
        else if(saveSystem.GetCoins() >= currentCharacterScript.prize)
        {
            saveSystem.SetCoins(saveSystem.GetCoins() - currentCharacterScript.prize);
            saveSystem.Unlock(currentCharacterScript.characterName);

            saveSystem.SelectCharacter(currentCharacterScript.characterName);

            priceText.SetText("Unlocked");
            priceText.color = selectedColor;
        }
        else
        {
            //priceText.SetText("You dont have enough Coins.");
            priceText.color = tooExpensiveColor;
        }
    }
}
