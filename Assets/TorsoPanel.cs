using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TorsoPanel : MonoBehaviour
{
    [Header("Needed scripts")]
    public GameController gcScript;
    public CharacterBuilder cbScript;
    public CharacterLoad characterLoader;
    [Header("Text")]
    public Text ChestText;
    public Text WaistText;
    // Start is called before the first frame update
    void Start() {
        gcScript = GameController.instance;
        cbScript = GameObject.FindObjectOfType<CharacterBuilder>();
        characterLoader = GameObject.FindObjectOfType<CharacterLoad>();
        UpdateNames();

    }
    void UpdateNames() {
        ChestText.text = characterLoader.Chests[gcScript.playerData.Torso[0]].name;
        WaistText.text = characterLoader.Waists[gcScript.playerData.Torso[1]].name;

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void setChest(int value) {
        cbScript.setChest(value);
        UpdateNames();
    }
    public void setWaist(int value) {
        cbScript.setWaist(value);
        UpdateNames();
    }
    private void OnDestroy() {
        gcScript.SaveGame();
    }
}
