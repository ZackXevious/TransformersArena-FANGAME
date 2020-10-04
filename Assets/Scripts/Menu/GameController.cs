using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Rendering.PostProcessing;
using System.IO;
using UnityEngine.SceneManagement;
using UnityEngine.Video;
using System.Runtime.Serialization.Formatters.Binary;

public class GameController : MonoBehaviour {

    //SaveData PlayerData;
    public static GameController instance;
    public int SaveGameIndex;
    public PlayerStats playerData;

    public VehicleScript[] altModes;

    [Header("AudioStuffs")]
    public AudioMixer audiomixer;
    [Header("PostProcessingStuffs")]
    public PostProcessProfile PostProcessingProfile;
    public bool BoolBloom;
    public bool BoolMotionBlur;
    public bool BoolDepthofField;
    [Header("Game Options")]
    public float CameraSensativityX;
    public float CameraSensativityY;
    public bool CameraInversion;
    public bool ShowFPS;
    public bool ShowClock;
    
    [Header("GameInfo")]
    private string[] StageNames = { "TEST"};
    public string[] SceneNames = { "MakeShiftCity"};
    private string[] StageDescriptions = {
        "Welcome to Makeshift city! This is a testing area to get familiar with life here on Earth."
    };

    public ArrayList unlockedText;
    public int[] StageType = { 0, 0, 1, 1, 0, 0, 2, 0 };

    [Header("Stuff to pull from")]
    public Sprite[] SecretIcons;

    [Header("MinuteByMinuteStuff")]
    public int stageNumber; 



    private Color[] pallete = {
        new Color(0,0,0),       //Black
        new Color(.25f,.25f,.25f),       //LightGrey
        new Color(.5f,.5f,.5f), //MidGrey
        new Color(.75f,.75f,.75f),       //DarkGrey
        new Color(1,1,1),    //White
        new Color(1,0,0),       //Red
        new Color(1,.25f,0),    //RedOrange
        new Color(1,.5f,0),     //Orange
        new Color(1,.75f,0),    //YellowOrange
        new Color(1,1,0),       //Yellow
        new Color(.5f,1,0),     //YellowGreen
        new Color(0,1,0),       //Green
        new Color(0,.5f,0),     //Dark Green
        new Color(0,1,.5f),     //Greenish blue
        new Color(0,1,1),       //Cyan
        new Color(0,.5f,1),
        new Color(0,0,1),       //Blue
        new Color(0,0,.5f),     //DarkBlue
        new Color(.5f,0,1),     //Magenta
        new Color(1,0,1)       //Magenta
    };

    

	// Use this for initialization
	void Awake () {
        if (instance!=null) {
            Destroy(this.gameObject);
        } else {
            instance = this;
        }
        GameObject.DontDestroyOnLoad(this.gameObject);
        playerData = LoadGame();
        if (playerData==null) {
            playerData = new PlayerStats();
        }
        
    }
    private void Start() {
        unlockedText = new ArrayList();
        Debug.Log(PlayerPrefs.GetFloat("MasterVolume"));
        audiomixer.SetFloat("MasterVolume", PlayerPrefs.GetFloat("MasterVolume", 0));
        audiomixer.SetFloat("MusicVolume", PlayerPrefs.GetFloat("MusicVolume", 0));
        audiomixer.SetFloat("SFXVolume", PlayerPrefs.GetFloat("SFXVolume", 0));

        ShowFPS = PlayerPrefs.GetInt("ShowFPS",0) > 0;
        BoolBloom = PlayerPrefs.GetInt("Bloom",0)>0;
        BoolMotionBlur = PlayerPrefs.GetInt("MotionBlur", 0) > 0;
        BoolDepthofField = PlayerPrefs.GetInt("DOF") > 0;

    }


    private void FixedUpdate() {
        Bloom bloomSettings;
        MotionBlur blurSettings;
        DepthOfField depthSettings;
        PostProcessingProfile.TryGetSettings<Bloom>(out bloomSettings);
        PostProcessingProfile.TryGetSettings<MotionBlur>(out blurSettings);
        PostProcessingProfile.TryGetSettings<DepthOfField>(out depthSettings);
        bloomSettings.enabled.value = BoolBloom;
        blurSettings.enabled.value = BoolMotionBlur;
        depthSettings.enabled.value = BoolDepthofField;
        if (GameObject.FindObjectOfType<StageControllerScript>()!=null) {
            depthSettings.enabled.value = BoolDepthofField;
        } else {
            depthSettings.enabled.value = false;
        }
        
    }

    void OnGUI() {
        int w = Screen.width, h = Screen.height;

        GUIStyle style = new GUIStyle();

        if (ShowFPS) {
            Rect rect = new Rect(0, 0, w, h - (h * 3 / 100));
            style.alignment = TextAnchor.LowerRight;
            style.fontSize = h * 3 / 100;
            style.normal.textColor = new Color(1.0f, 1.0f, 1.0f, 1.0f);
            float msec = Time.deltaTime * 1000.0f;
            float fps = 1.0f / Time.deltaTime;
            string text = string.Format("{0:0.0} ms {1:0.} fps", msec, fps);
            GUI.Label(rect, text, style);
        }
        
    }
    //Pallet related stuff to make it look nice
    public Color getPalletIndex(int index) {
        return pallete[(index + pallete.Length) % pallete.Length];
    }
    public string getStageName(int index){
        if (index < StageNames.Length && index>=0){
            return this.StageNames[index];
        }
        else{
            return "Stage description not found. Out of bounds exception.";
        } 
    }
    public string getStageDescription(int index) {
        if (index < StageNames.Length && index >= 0) {
            return this.StageDescriptions[index];
        } else {
            return "Stage description not found. Out of bounds exception.";
        }
    }

    //Customizability-!-!-!
    public void setPrimaryColor(int primColor) {
        playerData.Colors[0] = (primColor + pallete.Length) % pallete.Length;
    }
    public void setSecondaryColor(int secColor) {
        playerData.Colors[1] = (secColor + pallete.Length) % pallete.Length;
    }
    public void setReflectiveColor(int reflectColor) {
        playerData.Colors[2] = (reflectColor + pallete.Length) % pallete.Length;
    }
    public void setGlowColor(int glowColor) {
        playerData.Colors[3] = (glowColor + pallete.Length) % pallete.Length;
    }


    //Headstuff
    public void setHelmetModel(int currHead) {
        int desiredvalue = ((playerData.Head[0] + currHead) + GameObject.FindObjectOfType<CharacterLoad>().getNumHelmets()) % GameObject.FindObjectOfType<CharacterLoad>().getNumHelmets();
        playerData.Head[0] = desiredvalue;
    }
    public void setVisorModel(int VisorModel) {
        int desiredvalue = ((playerData.Head[1] + VisorModel) + GameObject.FindObjectOfType<CharacterLoad>().getNumVisors()) % GameObject.FindObjectOfType<CharacterLoad>().getNumVisors();
        playerData.Head[1] = desiredvalue;
    }
    public void setMouthPlateModel(int MouthModel) {
        int desiredvalue = ((playerData.Head[2] + MouthModel) + GameObject.FindObjectOfType<CharacterLoad>().getNumMouthplates()) % GameObject.FindObjectOfType<CharacterLoad>().getNumMouthplates();
        playerData.Head[2] = desiredvalue;
    }

    //Torso
    public void setChestModel(int currChest) {
        int desiredvalue = ((playerData.Torso[0] + currChest) + GameObject.FindObjectOfType<CharacterLoad>().getNumChests()) % GameObject.FindObjectOfType<CharacterLoad>().getNumChests();
        playerData.Torso[0] = desiredvalue;
    }

    public void setWaistModel(int currWaist) {
        int desiredvalue = ((playerData.Torso[1] + currWaist) + GameObject.FindObjectOfType<CharacterLoad>().getNumWaists()) % GameObject.FindObjectOfType<CharacterLoad>().getNumWaists();
        playerData.Torso[1] = desiredvalue;
    }

    //Arms
    public void setShoulderModel(int currShoulder) {
        int desiredvalue = ((playerData.Arms[0] + currShoulder) + GameObject.FindObjectOfType<CharacterLoad>().getNumShoulders()) % GameObject.FindObjectOfType<CharacterLoad>().getNumShoulders();
        playerData.Arms[0] = desiredvalue;
    }
    public void setArmModel(int currArm) {
        int desiredvalue = ((playerData.Arms[1] + currArm) + GameObject.FindObjectOfType<CharacterLoad>().getNumArms()) % GameObject.FindObjectOfType<CharacterLoad>().getNumArms();
        playerData.Arms[1] = desiredvalue;
    }

    
    //Legs
    public void setThighModel(int currThigh) {
        int desiredvalue= ((playerData.Legs[0]+currThigh) + GameObject.FindObjectOfType<CharacterLoad>().getNumThighs()) % GameObject.FindObjectOfType<CharacterLoad>().getNumThighs();
        playerData.Legs[0] = desiredvalue;     
    }
    public void setLegModel(int currLeg) {
        int desiredvalue = ((playerData.Legs[1] + currLeg) + GameObject.FindObjectOfType<CharacterLoad>().getNumLegs()) % GameObject.FindObjectOfType<CharacterLoad>().getNumLegs();
        playerData.Legs[1] = desiredvalue;
    }
    public void setFootModel(int currFoot) {
        int desiredvalue = ((playerData.Legs[2] + currFoot) + GameObject.FindObjectOfType<CharacterLoad>().getNumFeet()) % GameObject.FindObjectOfType<CharacterLoad>().getNumFeet();
        playerData.Legs[2] = desiredvalue;
    }



    //Customizers
    //Colors
    public int getPrimaryColorIndex() {
        return playerData.Colors[0];
    }
    public int getSecondaryIndex() {
        return playerData.Colors[1];
    }
    public int getReflectiveIndex() {
        return playerData.Colors[2];
    }
    public int getGlowIndex() {
        return playerData.Colors[3];
    }
    public Color getPrimaryColor() {
        return this.pallete[playerData.Colors[0]];
        
    }
    public Color getSecondaryColor() {
        return this.pallete[playerData.Colors[1]];
    }
    public Color getReflectiveColor() {
        return this.pallete[playerData.Colors[2]];
    }
    public Color getGlowColor() {
        return this.pallete[playerData.Colors[3]];
    }

    //Torso
    
    public int getHelmetModel() {
        return playerData.Head[0];
    }
    public int getVisorModel() {
        return playerData.Head[1];
    }
    public int getMouthPlateModel() {
        return playerData.Head[2];
    }
    public int getChestModel() {
        return playerData.Torso[0];
    }
    public int getWaistModel() {
        return playerData.Torso[1];
    }
    //Arms
    public int getShoulderModel() {
        return playerData.Arms[0];
    }
    public int getArmModel() {
        return playerData.Arms[1];
    }
    //Legs
    public int getThighModel() {
        return playerData.Legs[0];
    }
    public int getLegModel() {
        return playerData.Legs[1];
    }
    public int getFootModel() {
        return playerData.Legs[2];
    }
    
    public void SaveGame() {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/SaveGame.makeshift";
        FileStream stream = new FileStream(path,FileMode.Create);

        PlayerStats data = playerData;
        formatter.Serialize(stream, data);
        stream.Close();
    }
    public PlayerStats LoadGame() {
        string path = Application.persistentDataPath + "/SaveGame.makeshift";
        try {
            if (File.Exists(path)) {
                BinaryFormatter formatter = new BinaryFormatter();
                FileStream stream = new FileStream(path, FileMode.Open);
                PlayerStats data = formatter.Deserialize(stream) as PlayerStats;
                stream.Close();
                Debug.Log("Loaded from "+path);
                return data;

            } else {
                return null;
            }
        } catch (InvalidDataException f) {
            return null;
        }
        
        
        
    }
    public void newGame() {
        this.playerData = new PlayerStats();
        string path = Application.persistentDataPath + "/SaveGame.makeshift";
        if (File.Exists(path)) {
            File.Delete(path);
        }
        
    }
    public void goToNextScene() {
        SceneManager.LoadScene("LoadingScreen");
    }
    public void setStageToGoTo(int value) {
        this.stageNumber = value;
    }
    

    

}
