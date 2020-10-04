using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LegPanel : MonoBehaviour
{
    [Header("Needed scripts")]
    public GameController gcScript;
    public CharacterBuilder cbScript;
    public CharacterLoad characterLoader;
    [Header("Text")]
    public Text ThighText;
    public Text CalfText;
    public Text FootText;
    // Start is called before the first frame update
    void Start() {
        gcScript = GameController.instance;
        cbScript = GameObject.FindObjectOfType<CharacterBuilder>();
        characterLoader = GameObject.FindObjectOfType<CharacterLoad>();
        UpdateNames();

    }
    void UpdateNames() {
        ThighText.text = characterLoader.Chests[gcScript.playerData.Legs[0]].name;
        CalfText.text = characterLoader.Waists[gcScript.playerData.Legs[1]].name;
        FootText.text = characterLoader.Waists[gcScript.playerData.Legs[2]].name;

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void setThigh(int value) {
        cbScript.setThigh(value);
        UpdateNames();
    }
    public void setCalf(int value) {
        cbScript.setCalf(value);
        UpdateNames();
    }
    public void setFoot(int value) {
        cbScript.setFoot(value);
        UpdateNames();
    }
    private void OnDestroy() {
        gcScript.SaveGame();
    }
}
