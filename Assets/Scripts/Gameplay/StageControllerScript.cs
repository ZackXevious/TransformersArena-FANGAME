using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class StageControllerScript : MonoBehaviour {


    [Header("Hud stuff")]
    public GameObject HUD;
    public HudScript HudScript;
    public GameObject PauseMenu;
    public GameObject optionsPanel;
    public GameObject weaponSelectPanel;
    public GameObject playerHud;

    

    [Header("Stage Starting Related stuff")]
    //StageStartingRelatedStuffs
    public PlayerMovement playermove;
    //public PlayerAttack shotSpawn;
    public float screenfadeTimer;
    public float screenMaxFadeDelay=2;
    public int PlayerHealth;
    public bool displayHud=true;
    public Sprite[] WeaponIcons;
    public Color[] WeaponColors;
    public GameController gcScript;

    public WeaponClass[] weapons=new WeaponClass[5];


    

    //Gameplay stuff
    [Header("Gameplay stuff")]
    public GameObject currCheckpoint;
    public GameObject playerObject;
    public GameObject trailObject;
    public GameObject currTrailObject;
    public GameObject SpawnExplosion;
    public PlayerAttack paScript;
    public GameObject Camrig;
    public int currWeapon;
    public GameObject deathSpawn;


    public Boolean isRobot;
    public GameObject vehicle;
    public GameObject[] vehicleModes;
    public float transformDuration;
    public float transformTimer;
    

    [Header("Stage Ending stuff")]
    public GameObject[] stageGoals;
    public bool stageOver;
    //public GameObject VictoryScreen;
    public int currentGoalTargetsLeft;


    // Use this for initialization
    void Start () {
        isRobot = true;
        this.currentGoalTargetsLeft = 0;
        for (int x = 0; x < stageGoals.Length; x++) {
            if (stageGoals[x] != null) {
                this.currentGoalTargetsLeft += 1;
            }
        }
        gcScript = GameController.instance;

        Camrig = GameObject.FindGameObjectWithTag("PlayerCameraRig");
        playerObject = GameObject.FindGameObjectWithTag("Player");
        paScript = GameObject.FindObjectOfType<PlayerAttack>();

        weapons[0] = new WeaponClass("Buster", 0, 0, true);
        weapons[1] = new WeaponClass("Shotgun", 40, 0, true);
        weapons[2] = new WeaponClass("Assault", 100, .15f, true);
        weapons[3] = new WeaponClass("Sniper", 20, 0f, true);
        weapons[4] = new WeaponClass("Rockets", 20, 0, true);

        if (checkIfGoalsMet()&&stageGoals.Length>0) {
            stageGoals[0] = GameObject.FindGameObjectWithTag("Goal");
        }

        if (HudScript == null) {
            HudScript = HUD.GetComponent<HudScript>();
        }
            
        
        for (int x=0;  x<weapons.Length; x++) {
            if (weapons[x]!=null) {
                weapons[x].setIcon(this.WeaponIcons[x]);
                weapons[x].setColor(this.WeaponColors[x]);
            } else {

            }
            
        }


        //Set game ready
        Time.timeScale = 0;
        Cursor.lockState=CursorLockMode.Locked;
        Cursor.visible = false;
        Time.timeScale = 1;
        //VictoryScreen.SetActive(false);
        currTrailObject = Instantiate(trailObject, 
            new Vector3(
                playerObject.transform.position.x,
                playerObject.transform.position.y+3, 
                playerObject.transform.position.z), playerObject.transform.rotation);
        screenfadeTimer = screenMaxFadeDelay;

        //SetStageGoals
        this.currCheckpoint = this.gameObject;
        /**/
        playerObject.transform.position = new Vector3(currCheckpoint.transform.position.x, currCheckpoint.transform.position.y + 1, currCheckpoint.transform.position.z);
        Camrig.transform.position = new Vector3(currCheckpoint.transform.position.x, currCheckpoint.transform.position.y + 2, currCheckpoint.transform.position.z);
        Camrig.transform.rotation = currCheckpoint.transform.rotation;

    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetButtonDown("Pause")&&!stageOver) {
            this.Pause();
        }
        if (this.screenfadeTimer<=0 && displayHud) {
            playerHud.SetActive(true);
        } else {
            playerHud.SetActive(false);
        }
    }
    private void FixedUpdate() {
        if (Input.GetButtonDown("Transform")&&transformTimer<=0) {
            if (isRobot) {
                vehicle=Instantiate(GameController.instance.altModes[0].gameObject,playerObject.transform.position,playerObject.transform.rotation);
                vehicle.GetComponent<Rigidbody>().velocity = playerObject.GetComponent<CharacterController>().velocity;
                playerObject.SetActive(false);
                isRobot = false;
                Debug.Log("Transform to car");
            } else {
                playerObject.transform.rotation = Quaternion.Euler(0.0f, vehicle.transform.rotation.eulerAngles.y, 0.0f);
                playerObject.transform.position = vehicle.transform.position;
                playerObject.SetActive(true);
                //playerObject.GetComponent<CharacterController>().attachedRigidbody.velocity = vehicle.GetComponent<Rigidbody>().velocity;
                Destroy(vehicle);
                isRobot = true;
                Debug.Log("Transform to robot");
            }
            transformTimer = transformDuration;
        }
        if (transformTimer>0) {
            transformTimer -= Time.deltaTime;
        }
        
        this.currentGoalTargetsLeft = 0;
        for (int x = 0; x < stageGoals.Length; x++) {
            if (stageGoals[x] != null) {
                this.currentGoalTargetsLeft += 1;
            }
        }
        if (playerHud.activeSelf) {
            HudScript.UpdateUI();
        } else {
            HudScript.updateFade();
        }



        //Countdown stuff


        //Fade related stuff--------------------------------------------------------------
        if (this.screenfadeTimer > 0) {
            isRobot = true;
            this.screenfadeTimer -= Time.deltaTime;
            paScript.enabled = false;
            playerObject.SetActive(false);
            playerObject.GetComponent<PlayerMovement>().enabled = false ;
            Camrig.GetComponent<CameraControls>().enabled=false;
            playerHud.SetActive(false);
        } else {
            this.screenfadeTimer = 0;
            paScript.enabled = true;
            if (isRobot) {
                playerObject.SetActive(true);
                playerObject.GetComponent<PlayerMovement>().enabled = true;
            }
            
            Camrig.GetComponent<CameraControls>().enabled = true;
            if (currTrailObject!=null) {
                Instantiate(SpawnExplosion, currCheckpoint.transform.position, currCheckpoint.transform.rotation);
                Destroy(currTrailObject);
                this.GetComponent<PlayerSounds>().playSpawnSound();
            }
            
            //GameObject.FindObjectOfType<musicController>().setPitch(1);

        }
        //RESPAWN STUFF------------------------------------------------------------------
        if (PlayerHealth <= 0) {
            respawnPlayer();
            //GameObject.FindObjectOfType<musicController>().setPitch(.5f);
        }
        stageOver = false;
        /*if (stageOver) {
            this.EndStage();
        }*/
        

    }


    public WeaponClass getWeapon(int index) {
        return weapons[index];
    }
    //Methods for stuff!!
    public void Pause() {
        if (Time.timeScale != 0) {
            //Pause the game
            Time.timeScale = 0;
            PauseMenu.SetActive(true);
            this.playPauseSound();
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        } else {
            //Unpause the game
            Time.timeScale = 1;
            PauseMenu.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    //Modifiers}
    public void addPlayerHealth(int value) {
        if (value<0 && !stageOver) {
            PlayerHealth += value;
            if (PlayerHealth>=1) {
                GameObject.FindObjectOfType<PlayerSounds>().playHurtSound();
            }
            
        } else {
            PlayerHealth = Mathf.Clamp(PlayerHealth + value, 0, 20);
            GameObject.FindObjectOfType<PlayerSounds>().playHealSound();
        }
    }
    public int addWeaponAmmo(int weaponnumber, int value) {
        if (GameController.instance.playerData.weaponsUnlocked[weaponnumber]) {
            int weaponammo=Mathf.Clamp(this.getWeapon(weaponnumber).getCurrAmmo() + value,0,this.getWeapon(weaponnumber).getMaxAmmo());
            this.getWeapon(weaponnumber).setCurrAmmo(weaponammo);
            GameObject.FindObjectOfType<PlayerSounds>().playAmmoSound();
            return 0;
        } else {
            return 1;
        }
    }
    public void setCurrentWeapon(int value) {
        this.currWeapon = value;
    }
    public void setCheckpoint(GameObject other) {
        if (currCheckpoint!=other) {
            currCheckpoint = other;
        }
    }

    public bool unlockWeapon(int number) {
        if (GameController.instance.playerData.weaponsUnlocked[number]!=true) {
            GameController.instance.playerData.weaponsUnlocked[number] = true;
            return true;
        } else if(this.getWeapon(number).getCurrAmmo()<(this.getWeapon(number).getMaxAmmo()*.6)){
            this.addWeaponAmmo(number,999);
            return true;
        } else {
            return false;
        }
        
    }
    public void respawnPlayer() {
        
        if (this.screenfadeTimer <= 0) {
            this.screenfadeTimer = 2 * this.screenMaxFadeDelay;
            ParticleSystem ps = Instantiate(deathSpawn, playerObject.transform.position, playerObject.transform.rotation).GetComponent<ParticleSystem>();
            var main = ps.main;
            main.startColor = GameController.instance.getPrimaryColor();
            GameObject.FindObjectOfType<PlayerSounds>().playDeathSound();
        }
        if ((this.screenfadeTimer < this.screenMaxFadeDelay) && PlayerHealth <= 0) {
            Debug.Log("regen");
            PlayerHealth = 20;
            playerObject.SetActive(true);
            playerObject.GetComponent<PlayerMovement>().enabled = true;
            playerObject.GetComponent<PlayerMovement>().resetMovement();
            playerObject.GetComponent<PlayerMovement>().enabled = false;
            playerObject.SetActive(false);
            Debug.Log("Player location "+ playerObject.transform.position);
            Debug.Log("Repositioning player back to spawn");
            playerObject.transform.position = new Vector3(currCheckpoint.transform.position.x, currCheckpoint.transform.position.y + 1, currCheckpoint.transform.position.z);
            playerObject.transform.rotation = currCheckpoint.transform.rotation;
            Debug.Log("Player location " + playerObject.transform.position);
            Camrig.transform.position = new Vector3(currCheckpoint.transform.position.x, currCheckpoint.transform.position.y + 2, currCheckpoint.transform.position.z);
            Camrig.transform.rotation = currCheckpoint.transform.rotation;
            currTrailObject =Instantiate(trailObject, currCheckpoint.transform.position, playerObject.transform.rotation);


        }
    }

    public bool checkIfGoalsMet() {
        for (int x=0; x<stageGoals.Length;x++) {
            if (stageGoals[x]!=null) {
                return false;
            }
        }
        return true;
    }

    public void EndStage() {
        Destroy(GameObject.FindGameObjectWithTag("Music"));
        if (this.screenfadeTimer <= 0) {
            this.screenfadeTimer = 2 * this.screenMaxFadeDelay;
            ParticleSystem ps = Instantiate(deathSpawn, playerObject.transform.position, playerObject.transform.rotation).GetComponent<ParticleSystem>();
            
            var main = ps.main;
            main.startColor = GameController.instance.getPrimaryColor();
            GameObject.FindObjectOfType<PlayerSounds>().playLeaveSound();
        } else if(this.screenfadeTimer < this.screenMaxFadeDelay) {
            Time.timeScale = 0;
            PauseMenu.SetActive(false);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            displayHud = false;
            GameController.instance.SaveGame();
            /*if (VictoryScreen == null) {
                SceneManager.LoadScene("PlayerMenu");
            } else {
                VictoryScreen.SetActive(true);
            }*/
        }

        
        
    }

    //Getters
    public int getPlayerHealth() {
        return this.PlayerHealth;
    }
    public float getFadeTimer() {
        return this.screenfadeTimer;
    }
    public float getMaxFadeDelay() {
        return this.screenMaxFadeDelay;
    }
    public int getCurrentWeapon() {
        return this.currWeapon;
    }
    public WeaponClass getWeaponNumber(int num) {
        return this.weapons[num];
    }

    public void playPauseSound() {
        GameObject.FindObjectOfType<menusounds>().playConfirmSound();
    }
    public void GoToPlayerMenu() {
        GameController.instance.setStageToGoTo(-1);
        GameController.instance.goToNextScene();
    }
}
