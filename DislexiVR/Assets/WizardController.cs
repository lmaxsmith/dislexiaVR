using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WizardController : MonoBehaviour
{
    
    public Animator anim;

    public AudioSource audioSource;
    public AudioClip wizardTalkingClip;
    public AudioClip staffPoundClip;
    public AudioClip tutorialClip;
    bool playedAnim = false;
    bool wizardTalking = false;
    bool tutorialClicked = false;
    public GameManager gameManager;
    // Start is called before the first frame update
    void Start()
    {
        Invoke("WizardTalkingIntro", 3f);
    }

    // Update is called once per frame
    void Update()
    {
        
        if (audioSource.isPlaying && wizardTalking)
        {
            anim.SetBool("Talking", true);
        } else
        {
            if (!playedAnim && wizardTalking)
            {
                anim.SetBool("Talking", false);
                Invoke("StaffPound", 0.5f);
               
                playedAnim = true;
            }    
        }
    }
    void StaffPound()
    {
        wizardTalking = false;
        anim.SetBool("Stomp", true);
        if (tutorialClicked)
        {
            gameManager.transitionScene();
        }

    
    }
    void WizardTalkingIntro()
    {
        audioSource.clip = wizardTalkingClip;
        anim.SetBool("Talking", true);
        audioSource.Play();
        wizardTalking = true;

    }
    void StopStaffAnimation()
    {
        
        anim.SetBool("Stomp", false);
    }
    void PlayPoundAudio()
    {
        audioSource.clip = staffPoundClip;
        audioSource.Play();
    }
    public void PlayTutorialClip()
    {
        wizardTalking = true;
        playedAnim = false;
        anim.SetBool("Talking", true);
        audioSource.clip = tutorialClip;
        audioSource.Play();
        tutorialClicked = true;
    }
}
