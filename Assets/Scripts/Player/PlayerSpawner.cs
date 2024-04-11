using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    private SaveSystem savesystem;

    private void Awake()
    {
        savesystem = SaveSystem.current;
        SpawnPlayer();
    }

    public void SpawnPlayer()
    {
        Instantiate(savesystem.GetSelectedCharacter(), transform.position, transform.rotation);
    }
}
