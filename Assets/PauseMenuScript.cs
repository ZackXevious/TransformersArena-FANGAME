using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenuScript : MonoBehaviour
{
    public GameObject StageInfoPanel;
    
    public Text StageName;
    public Text StageDescriptionText;
    public Text CurrentGoalText;
    

    StageControllerScript scScript;

    // Start is called before the first frame update
    void Start()
    {
        
    }
    private void OnEnable() {
        scScript = GameObject.FindObjectOfType<StageControllerScript>();
        if (scScript != null) {
            StageInfoPanel.SetActive(true);
            StageName.text = GameController.instance.getStageName(GameController.instance.stageNumber);
            StageDescriptionText.text = "" + GameController.instance.getStageDescription(GameController.instance.stageNumber);
            if (GameController.instance.StageType[GameController.instance.stageNumber]==0) {
                CurrentGoalText.text = "Get to the end of the stage!";
            }else if (GameController.instance.StageType[GameController.instance.stageNumber] == 1) {
                CurrentGoalText.text = "Destroy the Generators! "+scScript.currentGoalTargetsLeft+" Left!";
            } else {
                CurrentGoalText.text = "Defeat Prototype V4!";
            }
            
        } else {
            StageInfoPanel.SetActive(false);
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
