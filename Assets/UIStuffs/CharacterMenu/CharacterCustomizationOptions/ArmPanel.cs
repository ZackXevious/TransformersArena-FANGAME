using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ArmPanel : MonoBehaviour
{
    [Header("Important stuff")]
    public GameController gcScript;
    public CharacterBuilder cbScript;
    public CharacterLoad characterLoader;
    [Header("TextLabels")]
    public Text ShoulderText;
    public Text ArmText;
    public Text handText;
    // Start is called before the first frame update
    void Start()
    {
        gcScript = GameController.instance;
        cbScript = GameObject.FindObjectOfType<CharacterBuilder>();
        characterLoader = GameObject.FindObjectOfType<CharacterLoad>();
        UpdateNames();
    }
    void UpdateNames() {
        ShoulderText.text = characterLoader.Shoulders[gcScript.playerData.Arms[0]].name;
        ArmText.text = characterLoader.Arms[gcScript.playerData.Arms[1]].name;

    }
    // Update is called once per frame
    void Update()
    {
        
    }
    public void setShoulders(int value) {
        cbScript.setShoulders(value);
        UpdateNames();
    }
    public void setForearms(int value) {
        cbScript.setForearms(value);
        UpdateNames();
    }
    private void OnDestroy() {
        gcScript.SaveGame();
    }
}
