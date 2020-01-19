using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cauldrenScript : MonoBehaviour
{
    
    public WizardController wiz;

    private void OnCollisionEnter(Collision collision)
    {
        wiz = GameObject.Find("Mage").GetComponent<WizardController>();
        wiz.PlayTutorialClip();
        Destroy(collision.gameObject);
    }
}
