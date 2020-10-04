using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeadPanel : MonoBehaviour
{
    [Header ("Needed scripts")]
    public GameController gcScript;
    public CharacterBuilder cbScript;
    public CharacterLoad characterLoader;
    [Header("Text")]
    public Text helmetText;
    public Text visorText;
    public Text mouthplateText;
    // Start is called before the first frame update
    void Start()
    {
        gcScript = GameController.instance;
        cbScript = GameObject.FindObjectOfType<CharacterBuilder>();
        characterLoader = GameObject.FindObjectOfType<CharacterLoad>();
        UpdateNames();
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void UpdateNames() {
        helmetText.text = characterLoader.Helmets[gcScript.playerData.Head[0]].name;
        if (characterLoader.Visors[gcScript.playerData.Head[1]]!=null) {
            visorText.text = characterLoader.Visors[gcScript.playerData.Head[1]].name;
        } else {
            visorText.text = "None";
        }
        if (characterLoader.Mouthplates[gcScript.playerData.Head[2]] != null) {
            mouthplateText.text = characterLoader.Mouthplates[gcScript.playerData.Head[2]].name;
        } else {
            mouthplateText.text = "None";
        }

    }
    public void setHelmet(int value) {
        cbScript.setHelmet(value);
        UpdateNames();
    }
    public void setVisor(int value) {
        cbScript.setVisor(value);
        UpdateNames();
    }
    public void setMouthPlate(int value) {
        cbScript.setMouthplate(value);
        UpdateNames();
    }
    private void OnDestroy() {
        gcScript.SaveGame();
    }
}
