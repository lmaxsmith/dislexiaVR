using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class wizardInGameManager : MonoBehaviour
{
    public AudioClip[] clips;
    public Animator anim;
    public AudioSource audioSource;


    // Update is called once per frame
    void Update()
    {
        if (audioSource.isPlaying)
        {
            anim.SetBool("Talking", true);
        } else
        {
            anim.SetBool("Talking", false);
        }
    }
    public void playAudioClip(int i)
    {
        audioSource.clip = clips[i];
        audioSource.Play();
    }
}
