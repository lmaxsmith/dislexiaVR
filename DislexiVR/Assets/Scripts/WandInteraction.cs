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
    public Material lineMat;


    /*
    public int particleAmount;
    public GameObject spawnPoint;
    public List<GameObject> pooledParticles;
    public static int particlePoolNum = 0; */

    private Interactable interactable;
    public SteamVR_Action_Boolean castMagic;

    private LineRenderer currLine;
    private int numClicks = 0;
    // Start is called before the first frame update
    void Start()
    {
        interactable = GetComponent<Interactable>();

    }

    // Update is called once per frame
    void Update()
    {
        if (interactable.attachedToHand)
        {
            if (castMagic.GetStateDown(SteamVR_Input_Sources.Any))
            {
                GameObject go = new GameObject();
                currLine = go.AddComponent<LineRenderer>();
                currLine.startWidth = 0.05f;
                currLine.endWidth = 0.05f;
                currLine.material = lineMat;
                currLine.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;

                numClicks = 0;
            }
            else if (castMagic.GetState(SteamVR_Input_Sources.Any))
            {
                GameObject magic = Instantiate(particleMagic, spawnPoint.transform.position, Quaternion.identity);
                magic.transform.localScale = new Vector3(0.1f,0.1f, 0.1f);
                currLine.SetVertexCount(numClicks + 1);
                currLine.SetPosition(numClicks, spawnPoint.transform.position);
                numClicks++;
            }
        }
    
           
  
    }

}
