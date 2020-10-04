using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerStats 
{
    [Header("General Stuff")]
    public string PlayerName;
    public int[] Colors;

    public int[] Head;
    public int[] Torso;
    public int[] Legs;
    public int[] Arms;

    public int vehicleMode;
    public int wheelChoice;

    [Header("Unlocks")]
    public bool[] weaponsUnlocked;

    [Header("StageData")]
    public bool[] stagesCompleted;
    public bool[] stagesUnlocked;
    public bool[,] stageSecrets;

    public PlayerStats() {
        //general player data
        PlayerName = "Rookie";
        vehicleMode = 0;
        wheelChoice = 0;
        Colors = new int[4] { 0, 2, 4, 2 };
        Head = new int[3] { 0, 0, 0};
        Torso = new int[2] { 0, 0 };
        Legs = new int[3] { 0, 0 ,0};
        Arms = new int[2] { 0, 0};
        weaponsUnlocked = new bool[5] { true, false, false, false, false };
        stagesUnlocked = new bool[8] { true, false, false, false, false, false, false, false };
        stagesCompleted = new bool[8] { false, false, false, false, false, false,false,false };
        stageSecrets = new bool[4,3] { { false, false, false }, { false, false, false } , { false, false, false } , { false, false, false } };
    }
}
