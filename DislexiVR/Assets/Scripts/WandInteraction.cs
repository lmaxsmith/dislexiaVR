using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class WandInteraction : MonoBehaviour
{
    // variables to identify particle prefab and spawnpoint
    public GameObject particleMagic;
    public GameObject particleBlast;
    public AudioSource audioSource;
    public AudioClip pressClip;
    public AudioClip releaseClip;
    public AudioClip magicBlast;

    public GameObject spawnPoint;
    public Material lineMat;

  


    /*
    public int particleAmount;
    public GameObject spawnPoint;
    public List<GameObject> pooledParticles;
    public static int particlePoolNum = 0; */

    private Interactable interactable;
    public SteamVR_Action_Boolean castMagic;
    private GameManager gameManager;
    


    private LineRenderer currLine;
    private int numClicks = 0;
    // Start is called before the first frame update
    void Start()
    {

        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();


    }

    // Update is called once per frame
    void Update()
    {
        if (gameManager.GameStarted)
        {
            Wand wand = FindObjectOfType<Wand>();
            if (castMagic.GetStateDown(SteamVR_Input_Sources.RightHand))
            {
                GameObject go = new GameObject();
                currLine = go.AddComponent<LineRenderer>();
                currLine.startWidth = 0.05f;
                currLine.endWidth = 0.05f;
                currLine.material = lineMat;
                currLine.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
                audioSource.clip = pressClip;
                audioSource.Play();

                //talk to logan's wand
                
                wand.StartCasting();

                numClicks = 0;
            }
                else if (castMagic.GetLastStateUp(SteamVR_Input_Sources.Any))
            {
                wand.StopCasting();

                audioSource.clip = releaseClip;
                audioSource.Play();
            } 
            else if (castMagic.GetState(SteamVR_Input_Sources.RightHand))
            {
                GameObject magic = Instantiate(particleMagic, spawnPoint.transform.position, Quaternion.identity);
                magic.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
                currLine.SetVertexCount(numClicks + 1);
                currLine.SetPosition(numClicks, spawnPoint.transform.position);
                numClicks++;
            }
        }
        else
        {
            if (castMagic.GetStateDown(SteamVR_Input_Sources.Any))
            {
                audioSource.clip = magicBlast;
                audioSource.Play();
                GameObject magic = Instantiate(particleBlast, spawnPoint.transform.position, Quaternion.identity);
                magic.transform.localScale = new Vector3(0.05f, 0.05f, 0.05f);
                magic.GetComponent<Rigidbody>().AddForce(transform.up * 350);
                
            }

        } 
  
    }

}
