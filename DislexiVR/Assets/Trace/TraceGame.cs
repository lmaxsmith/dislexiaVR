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

    public GameObject SpellCastObject;
    GameObject summonedObject;

    TraceSession currentSession;

    // Start is called before the first frame update
    void Start()
    {
        StartSession();
    }

    // Update is called once per frame
    void Update()
    {

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

        currentRound = new LetterRound();
        currentSession.letterRounds.Add(currentRound);

        System.Random random = new System.Random();
        int letterIndex = Random.Range(0, availableLetters.Length - 1);


        letter.CleanUpBulbs();
        letter.ResetLetter();
        letter.LoadConfiguration(availableLetters[letterIndex]);

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

        FindObjectOfType<voice_movement>().StartVoiceCommandListen();
        //SpellCastObject.SetActive(true);

        currentSession.markEndTime();

        StartCoroutine(sendSessionData(currentSession));
    }

    IEnumerator sendSessionData(TraceSession sess)
    {

        string jsonData = JsonUtility.ToJson(sess);

        string sessionTimestamp = sess.dateTime;
        string secretAuthKey = "REPLACE_ME";
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
        LineRenderer[] lines = FindObjectsOfType<LineRenderer>();
        for (int i = 0; i < lines.Length; i++)
        {
            DestroyImmediate(lines[i].gameObject);
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
                    sizeLevel--;
                }
                else
                {
                    hintLevel--;
                }
            }
            else //dind't pass
            {
                if (hintLevel == 0)
                {
                    hintLevel++;
                }
                else if (sizeLevel < 2)
                {
                    sizeLevel++;
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
        if (letter.inBoundsTime < letter.inBoundsThreshold)
        {
            StopAttempt(false);
        }
        else if (letter.ignitedBulbs >= letter.letterBulbs.Length)
        {
            StopAttempt(true);
            //break;
        }
        else if (letter.strokesUsed >= letter.strokesAllowed)
        {
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
        dateTime = (System.DateTime.UtcNow - epochStart).TotalSeconds.ToString();

        letterRounds = new List<LetterRound>();
    }

    public void markEndTime()
    {
        System.DateTime epochStart = new System.DateTime(1970, 1, 1, 0, 0, 0, System.DateTimeKind.Utc);
        endDateTime = (System.DateTime.UtcNow - epochStart).TotalSeconds.ToString();

    }
}

[Serializable]
public class LetterRound
{
    public char letter;

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
