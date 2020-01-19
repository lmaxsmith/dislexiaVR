using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Letter Configuration", menuName = "ScriptableObjects/LetterConfiguration", order = 1)]
public class LetterConfiguration : ScriptableObject
{
    public string letter;
    public string wordRemainder;

    public int strokes = 1;

    public Vector3[] bulbPositions;
    public GameObject summonPrefab;

}
