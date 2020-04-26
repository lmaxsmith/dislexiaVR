using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Valve.VR;
using Valve.VR.InteractionSystem;

public class WandInteraction : MonoBehaviour
{
    #region fields
    // variables to identify particle prefab and spawnpoint
    public GameObject particleMagic;
    public GameObject particleBlast;
    public AudioSource audioSource;
    public AudioClip pressClip;
    public AudioClip releaseClip;
    public AudioClip magicBlast;

    public GameObject spawnPoint;
    public Material lineMat;

    private Interactable interactable;
    public SteamVR_Action_Boolean castMagic;
    private GameManager gameManager;

    private LineRenderer currLine;
    private int numClicks = 0;

    delegate void UpdateHandler();
    UpdateHandler updateHandler;
    #endregion

    private UnityEngine.XR.InputDevice controller;
    private bool controllerWasDown;
    private bool controllerIsDown;

    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        audioSource = GameObject.Find("RightHand Controller").GetComponent<AudioSource>();
        updateHandler = QuestUpdate;

        AssignControllers();

        Debug.Log("A START LOG");
    }

    private void AssignControllers()
    {
        var devices = new List<UnityEngine.XR.InputDevice>();
        UnityEngine.XR.InputDevices.GetDevicesAtXRNode(UnityEngine.XR.XRNode.RightHand, devices);

        if (devices.Count == 1)
        {
            controller = devices[0];
            Debug.Log(string.Format("Device name '{0}' with role '{1}'", controller.name, controller.role.ToString()));
        }
        else if (devices.Count > 1)
        {
            Debug.Log("Found more than one left hand!");
        }
    }

    private void UpdateControllerButtonPress()
    {
        controllerWasDown = controllerIsDown;
        controller.TryGetFeatureValue(UnityEngine.XR.CommonUsages.triggerButton, out controllerIsDown);
    }

    private bool PrimaryButtonIsPressed()
    {
        return controllerIsDown && !controllerWasDown;
    }

    private bool PrimaryButtonIsHeld()
    {
        return controllerIsDown;
    }

    private Vector3 ControllerForward()
    {
        Quaternion deviceOrientation;
        controller.TryGetFeatureValue(UnityEngine.XR.CommonUsages.deviceRotation, out deviceOrientation);
        // Debug.Log($"Orientation: {deviceOrientation.eulerAngles.ToString()}");

        GameObject rig = GameObject.Find("XR Rig");

        Debug.Log($"Rig rotation: {rig.transform.eulerAngles.ToString()}");
        return rig.transform.rotation * deviceOrientation * Vector3.forward;
    }

    void Update()
    {
        UpdateControllerButtonPress();

        // float x = controller.transform.position.x;
        // float y = controller.transform.position.y;
        // float z = controller.transform.position.z;

        // Debug.Log($"Controller Transform: {x} {y} {z}");

        updateHandler();
    }

    void SteamUpdate()
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

                //audioSource.clip = releaseClip;
                //audioSource.Play();
            }
            else if (castMagic.GetState(SteamVR_Input_Sources.RightHand))
            {
                GameObject magic = Instantiate(particleMagic, spawnPoint.transform.position, Quaternion.identity);
                magic.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
                if (currLine == null)
                {
                    return;
                }
                currLine.positionCount = numClicks + 1;
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

    void QuestUpdate()
    {
        if (gameManager.GameStarted)
        {
            Wand wand = FindObjectOfType<Wand>();
            if (false)//castMagic.GetStateDown(SteamVR_Input_Sources.RightHand))
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
            else if (false)//castMagic.GetLastStateUp(SteamVR_Input_Sources.Any))
            {
                wand.StopCasting();

                //audioSource.clip = releaseClip;
                //audioSource.Play();
            }
            else if (false)//castMagic.GetState(SteamVR_Input_Sources.RightHand))
            {
                GameObject magic = Instantiate(particleMagic, spawnPoint.transform.position, Quaternion.identity);
                magic.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
                if (currLine == null)
                {
                    return;
                }
                currLine.positionCount = numClicks + 1;
                currLine.SetPosition(numClicks, spawnPoint.transform.position);
                numClicks++;
            }
        }
        else
        {
            if (PrimaryButtonIsPressed())//castMagic.GetStateDown(SteamVR_Input_Sources.Any))
            {
                audioSource.clip = magicBlast;
                audioSource.Play();
                GameObject magic = Instantiate(particleBlast, spawnPoint.transform.position, Quaternion.identity);
                magic.transform.localScale = new Vector3(0.05f, 0.05f, 0.05f);
                magic.GetComponent<Rigidbody>().AddForce(ControllerForward() * 350);
            }
        }
    }
}
