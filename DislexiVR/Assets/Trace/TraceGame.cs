using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class TraceGame : MonoBehaviour
{

    public float generalProficiency = 0;
    public LetterConfiguration[] availableLetters;
    public Letter letter;
    public UnityEngine.UI.Text monitor;

    public wizardInGameManager wizardManager;

    public GameObject SpellCastObject;
    GameObject summonedObject;

    TraceSession currentSession;

    private UnityEngine.XR.InputDevice controller;

    // Start is called before the first frame update
    void Start()
    {
        AssignControllers();
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

    // Update is called once per frame
    void Update()
    {
        letter.transform.Translate(Vector3.forward * ControllerOffset().y * Time.deltaTime);
    }

    Vector2 ControllerOffset()
    {
        Vector2 axis;
        controller.TryGetFeatureValue(UnityEngine.XR.CommonUsages.primary2DAxis, out axis);

        return axis;
    }

    #region =================Session Region====================

    public void StartSession(int startingProficiency)
    {
        currentSession = new TraceSession();

        StartRound();
    }
    public void StartSession()
    {
        StartSession(0); //TODO
    }

    #endregion



    #region ============= Round region ============

    LetterRound currentRound;

    public void StartRound()
    {
        if (summonedObject)
        {
            Destroy(summonedObject);
        }

        wizardManager.playAudioClip(0);

        currentRound = new LetterRound();
        currentSession.letterRounds.Add(currentRound);

        System.Random random = new System.Random();
        int letterIndex = UnityEngine.Random.Range(0, availableLetters.Length - 1);


        letter.CleanUpBulbs();
        letter.ResetLetter();
        letter.LoadConfiguration(availableLetters[letterIndex]);

        currentRound.letter = letter.letterConfiguration.letter.Substring(0, 1);

        StartAttempt(true);

    }

    IEnumerator RoundCoroutine()//do i even need this?
    {
        while (true)
        {

            yield return new WaitForEndOfFrame();
        }
    }

    public void CastSpellAttempt()
    {
        Debug.Log("Expelliarmus!");
        wizardManager.playAudioClip(7);
        FindObjectOfType<voice_movement>().StartVoiceCommandListen();
        //SpellCastObject.SetActive(true);

        currentSession.markEndTime();

        StartCoroutine(sendSessionData(currentSession));
    }

    IEnumerator sendSessionData(TraceSession sess)
    {

        string jsonData = JsonUtility.ToJson(sess);
        Debug.Log(jsonData);
        string sessionTimestamp = sess.dateTime;
        string secretAuthKey = "PJX1mOVOPUuwPv7qIyPS0J4jSEsJF4hok0gpBi0b";
        string destUrl = $"https://rh2020-dyslexia-db.firebaseio.com/sessions/{sessionTimestamp}.json?auth={secretAuthKey}";

        using (UnityWebRequest www = UnityWebRequest.Put(destUrl, jsonData))
        {
            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
            }
            else
            {
                Debug.Log("Form upload complete!");
            }
        }
    }

    public void CastSpell()
    {
        summonedObject = Instantiate(letter.summonedObject);
        summonedObject.transform.parent = SpellCastObject.transform;
        summonedObject.transform.localPosition = Vector3.zero;
    }

    #endregion




    #region ================= Attempt Region ====================

    Attempt currentAttempt;

    public void StartAttempt(bool isFirstAttempt)
    {
        //cleanup line renderers
        GameObject[] castingLines = GameObject.FindGameObjectsWithTag("CastingTracer");

        Logger.IngameDebug($"Number of lines: {castingLines.Length}");
        foreach (GameObject castingLine in castingLines)
        {
            Logger.IngameDebug($"Destroying a line");
            Destroy(castingLine);
        }

        if (isFirstAttempt)
        {
            currentAttempt = new Attempt(1, 2);

        }
        else //not our first rodeo
        {
            int hintLevel = currentAttempt.hintLevel;
            int sizeLevel = currentAttempt.sizeLevel;



            if (currentAttempt.passed)
            {
                if (hintLevel <= 0)
                {
                    wizardManager.playAudioClip(6);
                    sizeLevel--;
                }
                else
                {
                    wizardManager.playAudioClip(3);
                    hintLevel--;
                }
            }
            else //dind't pass
            {
                if (hintLevel == 0)
                {
                    wizardManager.playAudioClip(4);
                    hintLevel++;
                }
                else if (sizeLevel < 2)
                {
                    wizardManager.playAudioClip(5);
                    sizeLevel++;
                }
                else
                {
                    wizardManager.playAudioClip(1);
                }

            }

            currentAttempt = new Attempt(hintLevel, sizeLevel);
            Debug.Log(string.Format("New attempt with hint level {0} and size level {1}", hintLevel, sizeLevel));

        }
        monitor.text = string.Format("Hint Level: {0},  Size Level: {1}", currentAttempt.hintLevel, currentAttempt.sizeLevel);


        currentRound.attempts.Add(currentAttempt);

        //setup letter
        letter.enabled = true;
        letter.ResetLetter();
        float letterScaleMultiplier = 1f;
        if (currentAttempt.sizeLevel == 2)
        {
            letterScaleMultiplier = 1;
        }
        else if (currentAttempt.sizeLevel == 1)
        {
            letterScaleMultiplier = .75f;
        }
        else
        {
            letterScaleMultiplier = .5f;
        }

        letter.transform.localScale = new Vector3(letterScaleMultiplier, letterScaleMultiplier, letterScaleMultiplier);
        Debug.Log("scaling letter to " + letterScaleMultiplier);
        if (currentAttempt.hintLevel == 1)
        {
            letter.letterTextMesh.enabled = true;
        }
        else
        {
            letter.letterTextMesh.enabled = false;
        }


        //StartCoroutine(AttemptCoroutine());
        FindObjectOfType<Wand>().StopCastingEvent.AddListener(CheckLetter);
    }

    public void CheckLetter()
    {
        letter.strokesUsed++;

        Logger.IngameDebug($"Bulbs: {letter.ignitedBulbs} of {letter.letterBulbs.Length}");

        if (letter.inBoundsTime < letter.inBoundsThreshold)
        {
            Logger.IngameDebug($"Fail: too little time in bounds ({letter.inBoundsTime} of {letter.inBoundsThreshold})");
            StopAttempt(false);
        }
        else if (letter.ignitedBulbs >= letter.letterBulbs.Length)
        {
            StopAttempt(true);
            //break;
        }
        else if (letter.strokesUsed > letter.strokesAllowed)
        {
            Logger.IngameDebug($"Fail: too many strokes ({letter.strokesUsed} of {letter.strokesAllowed})");
            StopAttempt(false);
        }


    }

    IEnumerator AttemptCoroutine() //replace with check result
    {
        while (true)
        {
            Debug.Log(letter.ignitedBulbs + " bulbs ignited out of " + letter.letterBulbs.Length);
            if (letter.ignitedBulbs >= letter.letterBulbs.Length)
            {
                StopAttempt(true);
                yield break;
            }

            yield return new WaitForEndOfFrame();
        }
    }

    public void StopAttempt(bool passed)
    {
        letter.enabled = false;
        FindObjectOfType<Wand>().StopCastingEvent.RemoveListener(CheckLetter);

        // decide result and next steps
        currentAttempt.score = (float)letter.ignitedBulbs / (float)letter.letterBulbs.Length;

        if (passed)
        {
            Debug.Log("Passed!");
            currentAttempt.passed = true;
            if (currentAttempt.sizeLevel == 0 && currentAttempt.hintLevel == 0)
            {
                CastSpellAttempt();
                return;
            }
        }
        else
        {
            currentAttempt.passed = false;
            Debug.Log("Faaaaaailed.");
        }

        StartCoroutine(AttemptPauseCoroutine());
    }

    IEnumerator AttemptPauseCoroutine()
    {
        yield return new WaitForSeconds(3);

        StartAttempt(false);

        yield break;
    }

    public void StepByResult()
    {

    }

    #endregion

}


#region ==================== classes region =========================

[Serializable]
public class TraceSession
{
    public string dateTime;
    public string endDateTime;
    public List<LetterRound> letterRounds;
    public int proficiency;

    public TraceSession()
    {
        System.DateTime epochStart = new System.DateTime(1970, 1, 1, 0, 0, 0, System.DateTimeKind.Utc);
        dateTime = Math.Round((System.DateTime.UtcNow - epochStart).TotalSeconds).ToString();

        letterRounds = new List<LetterRound>();
    }

    public void markEndTime()
    {
        System.DateTime epochStart = new System.DateTime(1970, 1, 1, 0, 0, 0, System.DateTimeKind.Utc);
        endDateTime = Math.Round((System.DateTime.UtcNow - epochStart).TotalSeconds).ToString();
    }
}

[Serializable]
public class LetterRound
{
    public string letter;

    public int highScore;
    public int lowScore;
    public int averageScore;

    public List<Attempt> attempts = new List<Attempt>();

    public LetterRound()
    {
        attempts = new List<Attempt>();
    }

    //TODO: add images?
}

[Serializable]
public class Attempt
{
    public int sizeLevel = 2;
    public int hintLevel = 1;

    public bool passed;
    public float score;

    public Attempt(int hint, int size)
    {
        sizeLevel = size;
        hintLevel = hint;
    }
}

#endregion
