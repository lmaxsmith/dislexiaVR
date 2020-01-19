using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bookMenu : MonoBehaviour
{
    public GameObject fader;
    float duration = 100.0f;
    float alpha = 0f;
    Renderer tempColor;
    public GameObject newPlayerPosition;
    public GameObject player;
    public GameManager gameManager;
    public GameObject wand;
    public bool startGame = false;

    private void OnCollisionEnter(Collision collision)
    {
        StartGame();
    }
    void StartGame()
    {
        tempColor = fader.GetComponent<Renderer>();
        gameManager.GameStarted = true;
        StartCoroutine(Fade());
        
    }
    private void Update()
    {
      if (startGame)
        {
            StartGame();
            startGame = false;

        }
    }
    IEnumerator FadeIn()
    {
        for (float f = 1.1f; f >=0 ; f -= 0.05f)
        {
            Color c = tempColor.material.color;
            c.a = f;
            tempColor.material.color = c;
            yield return new WaitForSeconds(0.05f);

        }
    }

    IEnumerator Fade( )
    {
        for (float f = 0.0f; f <= 1.1; f+= 0.05f)
        {
            Color c = tempColor.material.color;
            c.a = f;
            tempColor.material.color = c;
            yield return new WaitForSeconds(0.05f);

        }
        player.transform.position = newPlayerPosition.transform.position;
        player.transform.rotation = newPlayerPosition.transform.rotation;
        StartCoroutine(FadeIn());
    }
}
