using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public bool GameStarted = false;
    public GameObject fader;
    public GameObject traceGame;
    public GameObject newPlayerPosition;
    public GameObject player;

    Renderer tempColor;

    public void transitionScene()
    {
        tempColor = fader.GetComponent<Renderer>();
        GameStarted = true;
        StartCoroutine(FadeOut());
        traceGame.SetActive(true);
    }
    IEnumerator FadeIn()
    {
        for (float f = 1.1f; f >= 0; f -= 0.05f)
        {
            Color c = tempColor.material.color;
            c.a = f;
            tempColor.material.color = c;
            yield return new WaitForSeconds(0.05f);

        }
    }
    IEnumerator FadeOut()
    {
        for (float f = 0.0f; f <= 1.1; f += 0.05f)
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
