using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WizardController : MonoBehaviour
{
    public Animator anim;

    public AudioSource audioSource;
    public AudioClip wizardTalkingClip;
    public AudioClip staffPoundClip;
    bool playedAnim = false;
    bool wizardTalking = false;
    // Start is called before the first frame update
    void Start()
    {
        Invoke("WizardTalkingIntro", 1f);
    }

    // Update is called once per frame
    void Update()
    {
        if (audioSource.isPlaying)
        {
            
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
        anim.SetBool("Stomp", true);

    
    }
    void WizardTalkingIntro()
    {
        audioSource.clip = wizardTalkingClip;
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
}
