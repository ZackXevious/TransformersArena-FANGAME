using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterLoad : MonoBehaviour {

    public GameObject gcObject;
    public GameController gcScript;

    [Header("Spawners")]
    //Spawners---------------------------------------------------
    //Legs
    public GameObject ThighL;
    public GameObject ThighR;
    public GameObject LegL;
    public GameObject LegR;
    public GameObject FootL;
    public GameObject FootR;


    //Arms
    public GameObject ShoulderL;
    public GameObject ShoulderR;
    public GameObject ArmL;
    public GameObject ArmR;
    public GameObject HandL;
    public GameObject HandR;

    //Torso
    public GameObject Head;
    public GameObject Chest;
    public GameObject Waist;

    [Space(10)]
    [Header("Containers")]
    //SpawnedObjectContainers------------------------------------
    //Head
    public GameObject HelmetMesh = null;
    public GameObject VisorMesh = null;
    public GameObject MouthPlateMesh = null;
    //Torso
    public GameObject ChestMesh = null;
    public GameObject WaistMesh = null;


    //Legs
    public GameObject thighMeshL = null;
    public GameObject thighMeshR = null;
    public GameObject legMeshL = null;
    public GameObject legMeshR = null;
    public GameObject footMeshL = null;
    public GameObject footMeshR = null;

    //Arms
    public GameObject shoulderMeshL = null;
    public GameObject shoulderMeshR = null;
    public GameObject armMeshL = null;
    public GameObject armMeshR = null;
    public GameObject handMeshL = null;
    public GameObject handMeshR = null;

    //Kibble
    public GameObject chestKibble;
    public GameObject backKibble;
    public GameObject shoulderKibbleL;
    public GameObject shoulderKibbleR;
    public GameObject armKibbleL;
    public GameObject armKibbleR;
    public GameObject calfKibbleL;
    public GameObject calfKibbleR;


    [Space(10)]
    [Header("Game Object Arrays")]
    //GAMEOBJECTARRAYS---------------------------------------------
    //HeadStuf
    public GameObject[] Helmets;
    public GameObject[] Visors;
    public GameObject[] Mouthplates;

    //Torso
    public GameObject[] Chests;
    public GameObject[] Waists;
    
    //Legs
    public GameObject[] Thighs;
    public GameObject[] Legs;
    public GameObject[] Feet;
    
    //Arms
    public GameObject[] Shoulders;
    public GameObject[] Arms;
    public GameObject[] Hands;

    
    [Space(10)]
    [Header("Colors")]
    //Colors
    public Material primary;
    public Material secondary;
    public Material reflective;
    public Material glow;

    Renderer rend;

    //Startup
    private void Start() {
        gcObject = GameObject.FindGameObjectWithTag("GameController");
        gcScript = gcObject.GetComponent<GameController>();

        this.refreshCharacter();
        
    }

    //Get necessary stuffs----------------------------
        //Colors
    public Color getPrimaryColor() {
        return primary.color;
    }
    public Color getSecondaryColor() {
        return secondary.color;
    }
    public Color getReflectiveColor() {
        return reflective.color;
    }
    public Color getGlowColor() {
        return glow.GetColor("_EmissionColor");
    }
        //Arm Objects
    public void HideArm(bool right) {
        if (right) {
            armMeshR.SetActive(false);
            handMeshR.SetActive(false);
        } else {
            armMeshL.SetActive(false);
            handMeshL.SetActive(false);
        }
    }
    public void ShowArms() {
        armMeshR.SetActive(true);
        handMeshR.SetActive(true);
        armMeshL.SetActive(true);
        handMeshL.SetActive(true);
    }

    //Get number of things-------------------------------
    //Head
    public int getNumHelmets() {
        return Helmets.Length;
    }
    public int getNumVisors() {
        return Visors.Length;
    }
    public int getNumMouthplates() {
        return Mouthplates.Length;
    }
    //Legs
    public int getNumThighs() {
        return Thighs.Length;
    }
    public int getNumLegs() {
        return Legs.Length;
    }
    public int getNumFeet() {
        return Feet.Length;
    }
    
        //Arms
    public int getNumShoulders() {
        return Shoulders.Length;
    }
    public int getNumArms() {
        return Arms.Length;
    }
    public int getNumHands() {
        return Hands.Length;
    }
    
        //Torso
    
    public int getNumChests() {
        return Chests.Length;
    }
    public int getNumWaists() {
        return this.Waists.Length;
    }

    //Getting names of things-------------------------------------

        //Torso
    public string getNameOfHead(int index) {
        return Helmets[index].name;
    }
    public string getNameOfChest(int index) {
        return Chests[index].name;
    }
    public string getNameOfWaist(int index) {
        return Waists[index].name;
    }
        //Arms
    public string getNameOfArm(int index) {
        return Arms[index].name;
    }
    public string getNameOfShoulder(int index) {
        return Shoulders[index].name;
    }
        //Legs
    public string getNameOfThigh(int index) {
        return Thighs[index].name;
    }
    public string getNameOfCalf(int index) {
        return Legs[index].name;
    }
    public string getNameOfFoot(int index) {
        return Feet[index].name;
    }

    //Spawners-----------------------------------------------------------------------------------------------------------------
    public void setPrimaryColor(Color primColor) {

        //rend.material.shader = Shader.Find("Primary Test");
        primary.color=(primColor);
    }
    public void setSecondaryColor(Color secColor) {
        secondary.color = secColor;
    }
    public void setReflectiveColor(Color reflColor) {
        reflective.color= reflColor;
    }
    public void setGlowColor(Color glowColor) {
        glow.color = glowColor;
        glow.SetColor(Shader.PropertyToID("_EmissionColor"), glowColor);
    }
    public void refreshColors() {
        setPrimaryColor(gcScript.getPrimaryColor());
        setSecondaryColor(gcScript.getSecondaryColor());
        setReflectiveColor(gcScript.getReflectiveColor());
        setGlowColor(gcScript.getGlowColor());
    }
    //spawn the head
    public void SpawnHelmet() {
        if (HelmetMesh != null) {
            Destroy(HelmetMesh);
        }
        HelmetMesh = Instantiate(Helmets[gcScript.getHelmetModel()], Head.transform);

    }
    public void SpawnVisor() {
        if (VisorMesh != null) {
            Destroy(VisorMesh);
        }
        if (Visors[gcScript.getVisorModel()] != null) {
            VisorMesh = Instantiate(Visors[gcScript.getVisorModel()], Head.transform);
        }

    }
    public void SpawnMouthPiece() {
        if (MouthPlateMesh != null) {
            Destroy(MouthPlateMesh);
        }
        if (Mouthplates[gcScript.getMouthPlateModel()]!=null) {
            MouthPlateMesh = Instantiate(Mouthplates[gcScript.getMouthPlateModel()], Head.transform);
        }
        

    }

    //spawn the chest
    public void SpawnChest() {
        if (gcScript.altModes[gcScript.playerData.vehicleMode].chest != null) {
            if (ChestMesh != null) {
                Destroy(ChestMesh);
            }
            ChestMesh = Instantiate(gcScript.altModes[gcScript.playerData.vehicleMode].chest, Chest.transform);
        } else {
            if (ChestMesh != null) {
                Destroy(ChestMesh);
            }
            ChestMesh = Instantiate(Chests[gcScript.getChestModel()], Chest.transform);
        }
        
    }
    public void SpawnWaist() {
        if (WaistMesh != null) {
            Destroy(WaistMesh);
        }
        WaistMesh = Instantiate(Waists[gcScript.getWaistModel()], Waist.transform);
    }

    //Spawn the thighs
    public void SpawnThighs() {
        if (thighMeshR != null) {
            Destroy(thighMeshR);
        }
        thighMeshR = Instantiate(Thighs[gcScript.getThighModel()], ThighR.transform);
        if (thighMeshL != null) {
            Destroy(thighMeshL);
        }
        thighMeshL = Instantiate(Thighs[gcScript.getThighModel()], ThighL.transform);
        thighMeshL.transform.localScale = new Vector3(thighMeshL.transform.localScale.x , thighMeshL.transform.localScale.y , thighMeshL.transform.localScale.z * -1);
    }
    //Spawn the legs
    public void SpawnLegs() {
        if (gcScript.altModes[gcScript.playerData.vehicleMode].legs != null) {
            if (legMeshR != null) {
                Destroy(legMeshR);
            }
            legMeshR = Instantiate(gcScript.altModes[gcScript.playerData.vehicleMode].legs, LegR.transform);
            if (legMeshL != null) {
                Destroy(legMeshL);
            }
            legMeshL = Instantiate(Legs[gcScript.getLegModel()], LegL.transform);
            legMeshL.transform.localScale = new Vector3(legMeshL.transform.localScale.x, legMeshL.transform.localScale.y, legMeshL.transform.localScale.z * -1);
        } else {
            if (legMeshR != null) {
                Destroy(legMeshR);
            }
            legMeshR = Instantiate(Legs[gcScript.getLegModel()], LegR.transform);
            if (legMeshL != null) {
                Destroy(legMeshL);
            }
            legMeshL = Instantiate(Legs[gcScript.getLegModel()], LegL.transform);
            legMeshL.transform.localScale = new Vector3(legMeshL.transform.localScale.x, legMeshL.transform.localScale.y, legMeshL.transform.localScale.z * -1);
        }
        
    }
    //Spawn the feet
    public void SpawnFeet() {
        if (gcScript.altModes[gcScript.playerData.vehicleMode].feet!=null) {
            if (footMeshR != null) {
                Destroy(footMeshR);
            }
            footMeshR = Instantiate(gcScript.altModes[gcScript.playerData.vehicleMode].feet, FootR.transform);
            if (footMeshL != null) {
                Destroy(footMeshL);
            }
            footMeshL = Instantiate(gcScript.altModes[gcScript.playerData.vehicleMode].feet, FootL.transform);
        } else {
            if (footMeshR != null) {
                Destroy(footMeshR);
            }
            footMeshR = Instantiate(Feet[gcScript.getFootModel()], FootR.transform);
            if (footMeshL != null) {
                Destroy(footMeshL);
            }
            footMeshL = Instantiate(Feet[gcScript.getFootModel()], FootL.transform);
        }
        
    }



    //spawn the arms
    public void SpawnShoulders() {
        if (shoulderMeshR != null) {
            Destroy(shoulderMeshR);
        }
        if (gcScript.altModes[gcScript.playerData.vehicleMode].shoulder != null) {
            shoulderMeshR = Instantiate(gcScript.altModes[gcScript.playerData.vehicleMode].shoulder, ShoulderR.transform);
        } else {
            shoulderMeshR = Instantiate(Shoulders[gcScript.getShoulderModel()], ShoulderR.transform);
        }
            
        if (shoulderMeshL != null) {
            Destroy(shoulderMeshL);
        }
        if (gcScript.altModes[gcScript.playerData.vehicleMode].shoulder != null) {
            shoulderMeshL = Instantiate(gcScript.altModes[gcScript.playerData.vehicleMode].shoulder, ShoulderL.transform);
        } else {
            shoulderMeshL = Instantiate(Shoulders[gcScript.getShoulderModel()], ShoulderL.transform);
        }
        shoulderMeshL.transform.localScale = new Vector3(shoulderMeshL.transform.localScale.x, shoulderMeshL.transform.localScale.y, shoulderMeshL.transform.localScale.z * -1);
    }
    public void SpawnArms() {
        //Arms themselves
        if (armMeshR != null) {
            Destroy(armMeshR);
        }
        armMeshR = Instantiate(Arms[gcScript.getArmModel()], ArmR.transform);
        if (armMeshL != null) {
            Destroy(armMeshL);
        }
        armMeshL = Instantiate(Arms[gcScript.getArmModel()], ArmL.transform);
        armMeshL.transform.localScale = new Vector3(armMeshL.transform.localScale.x , armMeshL.transform.localScale.y , armMeshL.transform.localScale.z * -1);
        //Kibble
        if (gcScript.altModes[gcScript.playerData.vehicleMode].armKibble!=null) {
            if (armKibbleR != null) {
                Destroy(armKibbleR);
            }
            armKibbleR = Instantiate(gcScript.altModes[gcScript.playerData.vehicleMode].armKibble, ArmR.transform);
            if (armKibbleL != null) {
                Destroy(armKibbleL);
            }
            armKibbleL = Instantiate(gcScript.altModes[gcScript.playerData.vehicleMode].armKibble, ArmL.transform);
            armKibbleL.transform.localScale = new Vector3(armMeshL.transform.localScale.x, armMeshL.transform.localScale.y, armMeshL.transform.localScale.z * -1);
        }
        
    }
    public void SpawnHands() {
        if (handMeshR != null) {
            Destroy(handMeshR);
        }
        handMeshR = Instantiate(Hands[0], HandR.transform);
        if (handMeshL != null) {
            Destroy(handMeshL);
        }
        handMeshL = Instantiate(Hands[0], HandL.transform);
        handMeshL.transform.localScale = new Vector3(handMeshL.transform.localScale.x, handMeshL.transform.localScale.y*-1, handMeshL.transform.localScale.z);
    }

    

    public void refreshCharacter() {
        //SetColors
        refreshColors();

        //Spawn Legs
        SpawnThighs();
        SpawnLegs();
        SpawnFeet();

        //Spawn Arms
        SpawnShoulders();
        SpawnArms();
        SpawnHands();

        //Spawn Torso
        
        SpawnWaist();
        SpawnChest();

        SpawnHelmet();
        SpawnVisor();
        SpawnMouthPiece();
    }

}
