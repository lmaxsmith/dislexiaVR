using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bookMenu : MonoBehaviour
{
    
    public GameManager gameManager;
    public GameObject wand;
    public bool startGame = false;

    public GameObject traceGame;

    private void OnCollisionEnter(Collision collision)
    {
        StartGame();
    }

    [ContextMenu("Start Game")]
    public void StartGame()
    {
        gameManager.transitionScene();
    }
}
