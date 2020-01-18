using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class WandInteraction : MonoBehaviour
{
    // variables to identify particle prefab and spawnpoint
    public GameObject particleMagic;
    public GameObject spawnPoint;


    /*
    public int particleAmount;
    public GameObject spawnPoint;
    public List<GameObject> pooledParticles;
    public static int particlePoolNum = 0; */

    private Interactable interactable;
    public SteamVR_Action_Boolean castMagic;

    private LineRenderer currLine;
    // Start is called before the first frame update
    void Start()
    {
        interactable = GetComponent<Interactable>();

    }

    // Update is called once per frame
    void Update()
    {
        if (interactable.attachedToHand && castMagic.GetState(SteamVR_Input_Sources.Any))
        {
            GameObject magic = Instantiate(particleMagic, spawnPoint.transform.position, Quaternion.identity);
            magic.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);

            //GameObject go = new GameObject();
            //currLine = go.AddComponent<LineRenderer>();
        }
    }

}
