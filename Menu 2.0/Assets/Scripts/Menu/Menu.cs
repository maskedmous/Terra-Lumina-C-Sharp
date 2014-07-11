﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using TouchScript;
using System.IO;
using System.Xml;

public class Menu : MonoBehaviour
{
    public bool heimBuild = false;
    public bool debugTouch = false;     //debug the touch

    private GameObject currentState = null;
    public List<GameObject> states = new List<GameObject>();
    List<Button> buttonList = new List<Button>();
    List<Slider> sliderList = new List<Slider>();

    private BloomAndLensFlares bloomScript = null;

    //sound variables
    private SoundEngineScript soundEngine = null;

    private GUIStyle customFont = new GUIStyle();   //custom font we're using

    //difficulty
    private string difficulty = "Easy"; //the difficulty set by the player

    //levels
    private string levelFilename = "";
    private ArrayList levels = new ArrayList();
    private ArrayList xmlLevels = new ArrayList();
    private ArrayList levelIDs = new ArrayList();
    private string levelsXmlFilePath = "";
    private int startLevelCount = 1;

    private Animator roverAnim = null;   //call animations of the rover

    //different inputs
    private int inputType = 0;  //tuio + mouse standard
    private List<KeyCode> keyboardSettings = new List<KeyCode>(); //keyboard settings in an array

    private KeyCode moveLeftKey = KeyCode.A;    //standard keyboard input
    private KeyCode moveRightKey = KeyCode.D;
    private KeyCode jumpKey = KeyCode.Space;
    private KeyCode normalShroomKey = KeyCode.N;
    private KeyCode bumpyShroomKey = KeyCode.B;
    private KeyCode flashKey = KeyCode.LeftControl;

    TouchScript.InputSources.MouseInput mouseInput = null;  //all sorts of input
    TouchScript.InputSources.TuioInput tuioInput = null;
    TouchScript.InputSources.Win7TouchInput win7Input = null;
    TouchScript.InputSources.Win8TouchInput win8Input = null;

    public void Awake()
    {
        DontDestroyOnLoad(this.gameObject);

        initializeScripts();    //load scripts for communication
        initalizeInput();   //initialize the input from the inputtype (heim = TUIO)

        //load the levels that are in the xml
        levelsXmlFilePath = Application.dataPath + "/LevelsXML/";
        //fill the xml Level array
        fillXmlLevelArray();
        //fill the level array
        fillLevelArray();

        switchMenuState("MainMenu");    //handles initialize buttons
    }

    public void switchMenuState(string stateName)
    {
        foreach (GameObject aState in states)
        {
            if (aState.name == stateName)
            {
                //set the current state
                //set the previous state to inactive
                if (currentState != null) currentState.SetActive(false);
                //set the current state
                currentState = aState;
                //set it on active
                currentState.SetActive(true);
                //initialize the buttons of this state
                initializeButtons();
                //initialize the sliders of this state
                initializeSliders();
            }
        }
    }

    public void setRoverAnimation(string stateName, bool value)
    {
        roverAnim.SetBool(stateName, value);
    }

    private void initializeButtons()
    {
        //clear button list first
        buttonList.Clear();
        //get the current buttons and store them into a temp variable
        Button[] buttons = currentState.GetComponents<Button>();
        //go through the list of buttons and add them to the List<Button>
        for (int i = 0; i < buttons.Length; ++i)
        {
            buttonList.Add(buttons[i]);
        }
    }
    private void initializeSliders()
    {
        //clear slider list first
        sliderList.Clear();
        //get the current sliders and store them into a temp variable
        Slider[] sliders = currentState.GetComponents<Slider>();

        for (int i = 0; i < sliders.Length; ++i)
        {
            sliderList.Add(sliders[i]);
        }
    }
    private void initializeScripts()
    {
        roverAnim = GameObject.Find("RoverAnimMenu").GetComponent<Animator>();
        bloomScript = Camera.main.GetComponent<BloomAndLensFlares>();
        soundEngine = GameObject.Find("SoundEngine").GetComponent<SoundEngineScript>();

        GameObject inputSources = GameObject.Find("TouchScript").transform.FindChild("Inputs").gameObject;

        mouseInput = inputSources.GetComponent<TouchScript.InputSources.MouseInput>();
        tuioInput = inputSources.GetComponent<TouchScript.InputSources.TuioInput>();
        win7Input = inputSources.GetComponent<TouchScript.InputSources.Win7TouchInput>();
        win8Input = inputSources.GetComponent<TouchScript.InputSources.Win8TouchInput>();


        //setting font
        customFont.font = (Font)Resources.Load("Fonts/sofachrome rg") as Font;

        //setting color
        Color fontColor = customFont.normal.textColor;

        fontColor.b = 197.0f / 255.0f;
        fontColor.g = 185.0f / 255.0f;
        fontColor.r = 147.0f / 255.0f;

        customFont.normal.textColor = fontColor;
    }

    private void initalizeInput()
    {
        //tuio + mouse
        if (inputType == 0)
        {
            mouseInput.enabled = true;
            tuioInput.enabled = true;
            win7Input.enabled = false;
            win8Input.enabled = false;
        }
        //keyboard + mouse
        else if (inputType == 1)
        {
            mouseInput.enabled = true;
            tuioInput.enabled = true;
            win7Input.enabled = false;
            win8Input.enabled = false;
        }
        //win 7 touch
        else if (inputType == 2)
        {
            mouseInput.enabled = false;
            tuioInput.enabled = true;
            win7Input.enabled = true;
            win8Input.enabled = false;
        }
        //win 8 touch
        else if (inputType == 3)
        {
            mouseInput.enabled = false;
            tuioInput.enabled = true;
            win7Input.enabled = false;
            win8Input.enabled = true;
        }
        //xbox controller + mouse
        else if (inputType == 4)
        {
            mouseInput.enabled = true;
            tuioInput.enabled = true;
            win7Input.enabled = false;
            win8Input.enabled = false;
        }
    }

    public void changeVolume(float volume)
    {
        volume = Mathf.Clamp(volume, 0.0f, 1.0f);
        soundEngine.changeVolume(volume);
    }

    public int getInputType()
    {
        return inputType;
    }

    //sofachrome
    private string getSelectedControllerType()
    {
        if (inputType == 0) return "TUIO Input";
        else if (inputType == 1) return "Keyboard & Mouse";
        else if (inputType == 2) return "Win 7 Touch";
        else if (inputType == 3) return "Win 8 Touch";
        else if (inputType == 4) return "Xbox Controller";

        return "";
    }

    public List<KeyCode> getKeyboardSettings()
    {
        return keyboardSettings;
    }

    public void OnEnable()
    {
        if (TouchManager.Instance != null)
        {
            TouchManager.Instance.TouchesBegan += touchBegan;
            TouchManager.Instance.TouchesMoved += touchMoved;
            TouchManager.Instance.TouchesEnded += touchEnded;
        }
    }

    public void OnDisable()
    {
        if (TouchManager.Instance != null)
        {
            TouchManager.Instance.TouchesBegan -= touchBegan;
            TouchManager.Instance.TouchesMoved -= touchMoved;
            TouchManager.Instance.TouchesEnded -= touchEnded;
        }
    }

    private Vector2 invertY(Vector2 vector)
    {
        return new Vector2(vector.x, (vector.y - Screen.height) * -1);
    }
    //if a touch event is made
    private void touchBegan(object sender, TouchEventArgs touchEvent)
    {
        foreach (var point in touchEvent.Touches)
        {
            Vector2 touchPoint = point.Position;
            touchPoint = invertY(touchPoint);

            foreach (Button aButton in buttonList)
            {
                aButton.isTouched(touchPoint);
            }
            if (sliderList.Count != 0)
            {
                foreach (Slider aSlider in sliderList)
                {
                    aSlider.isTouched(touchPoint);
                }
            }
        }
    }
    //if a touch event is moved
    private void touchMoved(object sender, TouchEventArgs touchEvent)
    {
        foreach (var point in touchEvent.Touches)
        {
            Vector2 touchPoint = point.Position;
            touchPoint = invertY(touchPoint);

            foreach (Button aButton in buttonList)
            {
                aButton.isStillTouching(touchPoint);
            }
            if (sliderList.Count != 0)
            {
                foreach (Slider aSlider in sliderList)
                {
                    aSlider.isStillTouching(touchPoint);
                }
            }
        }
    }

    //if a touch has ended
    private void touchEnded(object sender, TouchEventArgs touchEvent)
    {
        foreach (var point in touchEvent.Touches)
        {
            Vector2 touchPoint = point.Position;
            touchPoint = invertY(touchPoint);
            if (buttonList.Count != 0)
            {
                foreach (Button aButton in buttonList)
                {
                    if (aButton.isTouchReleased(touchPoint)) return;
                }
            }
            if (sliderList.Count != 0)
            {
                foreach (Slider aSlider in sliderList)
                {
                    aSlider.isTouchReleased(touchPoint);
                }
            }
        }
    }

    //debugging
    private string isMouseInputEnabled()
    {
        if (mouseInput.enabled) return "Mouse Input is Enabled";

        return "Mouse input is disabled";
    }

    private string isWin7InputEnabled()
    {
        if (win7Input.enabled) return "Win 7 Input is Enabled";

        return "Win 7 Input is Disabled";
    }

    private string isWin8InputEnabled()
    {
        if (win8Input.enabled) return "Win 8 Input is Enabled";

        return "Win 8 Input is Disabled";
    }

    private string numberOfTouches()
    {
        return "Number of Touches: " + TouchManager.Instance.ActiveTouches.Count.ToString();
    }

    public void OnGUI()
    {
        //debug the touch so we are able to see which input type is used and if it is enabled or not
        //showing how many number of touches there are as well
        if (debugTouch) GUI.Label(new Rect(0, 0, Screen.width, 200), "InputType: " + getInputType() + "\n" + isMouseInputEnabled() + "\n" + isWin7InputEnabled() + "\n" + isWin8InputEnabled() + "\n" + numberOfTouches());
    }

    private bool checkValidKey(KeyCode aKey)
    {
        //check if the key matches an other key
        //if so revert it back immediately
        if (aKey == moveLeftKey) return false;
        else if (aKey == moveRightKey) return false;
        else if (aKey == jumpKey) return false;
        else if (aKey == flashKey) return false;
        else if (aKey == normalShroomKey) return false;
        else if (aKey == bumpyShroomKey) return false;

        return true;
    }

    public bool bloom
    {
        get
        {
            return bloomScript.enabled;
        }
        set
        {
            bloomScript.enabled = value;
        }
    }

    public SoundEngineScript sound
    {
        get
        {
            return soundEngine;
        }
    }

    private void fillXmlLevelArray()
    {
        //look for all levels and fill the array	
        string[] fileInfo = Directory.GetFiles(levelsXmlFilePath, "*.xml", SearchOption.AllDirectories);

        foreach (var file in fileInfo)
        {
            xmlLevels.Add(Path.GetFileName(file));
        }
        if (xmlLevels.Count == 0)
        {
            Debug.LogError("Xml level files not loaded for some reason, check the LevelsXML folder");
        }
        else
        {
            Debug.Log("Xml level files counted: " + xmlLevels.Count);
        }
    }

    private void fillLevelArray()
    {
        if (xmlLevels.Count == 0)
        {
            Debug.LogError("No xml levels found in the array can't fill level array");
            return;
        }

        int levelErrors = 0;

        foreach (var _level in xmlLevels)
        {
            //level xml name
            string aLevel = _level as string;
            //get level ID from xml
            int levelID = getLevelID(aLevel);
            //if levelID is valid
            if (levelID != -1)
            {
                //check if new level ID
                bool newLevelID = checkForNewLevelID(levelID);
                //level difficulty setting
                string levelDifficulty = getDifficultySetting(aLevel);
                //create new level
                if (newLevelID == true)
                {
                    //new level ID add to levelIDs
                    levelIDs.Add(levelID);
                    //creating a new level
                    Level newLevel = new Level();
                    //setting the ID
                    newLevel.setLevelID(levelID);
                    //setting the filename for the level difficulty
                    newLevel.setLevelDifficultyXmlFile(aLevel, levelDifficulty);
                    levels.Add(newLevel);
                }
                else
                {
                    //is already seen before
                    //look through the array and add the difficulty setting
                    Level oldLevel = getLevelByID(levelID);
                    oldLevel.setLevelDifficultyXmlFile(aLevel, levelDifficulty);
                }
            }
            else
            {
                levelErrors++;
            }
        }

        sortByLevelID();

        if (levelErrors > 0)
        {
            Debug.LogError("Level errors occured: " + levelErrors);
        }
    }

    private void sortByLevelID()
    {
        levelIDs.Sort();
    }

    private int getLevelID(string levelFilename)
    {
        XmlDocument xmlDocument = new XmlDocument();
        //read the xml
        xmlDocument.Load(levelsXmlFilePath + levelFilename);

        XmlNode masterNode = xmlDocument.DocumentElement;
        XmlNodeList masterNodeChildren = masterNode.ChildNodes;

        //get the level ID
        foreach (var _nodes in masterNodeChildren)
        {
            XmlNode nodes = _nodes as XmlNode;

            if (nodes.Name == "LevelID")
            {
                return int.Parse(nodes.InnerText);
            }
        }

        Debug.LogError("LevelID NOT FOUND! re-create xml from level editor: " + levelFilename);
        return -1;
    }

    private Level getLevelByID(int levelID)
    {
        foreach (var aLevel in levels)
        {
            Level levelScript = aLevel as Level;

            if (levelScript.getLevelID() == levelID)
            {
                return levelScript;
            }
        }

        Debug.LogError("Level by ID not found? This shouldn't be possible! LevelID: " + levelID);
        return null;
    }

    private bool checkForNewLevelID(int aLevelID)
    {
        int levelID = -1;

        foreach (var level in levels)
        {
            Level levelScript = level as Level;
            levelID = levelScript.getLevelID();
            if (levelID == aLevelID)
            {
                //level ID is already in the list return false
                return false;
            }
        }
        //this level ID is not in the list yet so this is a new level return true
        return true;
    }

    private string getDifficultySetting(string levelFilename)
    {
        XmlDocument xmlDocument = new XmlDocument();
        //read the xml
        xmlDocument.Load(levelsXmlFilePath + levelFilename);

        XmlNode masterNode = xmlDocument.DocumentElement;
        XmlNodeList masterNodeChildren = masterNode.ChildNodes;

        //get the level ID
        foreach (var _nodes in masterNodeChildren)
        {
            XmlNode nodes = _nodes as XmlNode;

            if (nodes.Name == "Difficulty")
            {
                return nodes.InnerText;
            }
        }

        Debug.LogError("Difficulty NOT FOUND! re-create xml from level editor: " + levelFilename);
        return "";
    }


    public void createSettings()
    {
        Debug.Log("Creating Settings...");
        //create keys and values with defaults
        PlayerPrefs.SetFloat("Volume", 0.9031847f);
        PlayerPrefs.SetInt("Bloom", 1);
        PlayerPrefs.SetInt("InputType", 0);

        //save default keyboard keys
        PlayerPrefs.SetString("MoveLeft", KeyCode.A.ToString());
        PlayerPrefs.SetString("MoveRight", KeyCode.D.ToString());
        PlayerPrefs.SetString("Jump", KeyCode.Space.ToString());
        PlayerPrefs.SetString("Flash", KeyCode.LeftControl.ToString());
        PlayerPrefs.SetString("NormalShroom", KeyCode.N.ToString());
        PlayerPrefs.SetString("BumpyShroom", KeyCode.B.ToString());
        //set the created settings to "true", there is no SetBool so int = 1 for true int = 0 for false
        PlayerPrefs.SetInt("CreatedSettings", 1);

        PlayerPrefs.Save();
        Debug.Log("Saving Created Settings");
    }

    public void saveSettings()
    {
        Debug.Log("Saving Settings...");

        PlayerPrefs.SetFloat("Volume", soundEngine.getVolume());
        if (bloom) PlayerPrefs.SetInt("Bloom", 1);
        else PlayerPrefs.SetInt("Bloom", 0);
        PlayerPrefs.SetInt("InputType", inputType);

        PlayerPrefs.SetString("MoveLeft", moveLeftKey.ToString());
        PlayerPrefs.SetString("MoveRight", moveRightKey.ToString());
        PlayerPrefs.SetString("Jump", jumpKey.ToString());
        PlayerPrefs.SetString("Flash", flashKey.ToString());
        PlayerPrefs.SetString("NormalShroom", normalShroomKey.ToString());
        PlayerPrefs.SetString("BumpyShroom", bumpyShroomKey.ToString());

        PlayerPrefs.Save();
        Debug.Log("Saved Settings");
    }

    public void loadSettings()
    {
        if (PlayerPrefs.HasKey("CreatedSettings"))
        {
            soundEngine.changeVolume(PlayerPrefs.GetFloat("Volume"));
            if (PlayerPrefs.GetInt("Bloom") == 1) bloom = true;
            else bloom = false;

            //set input type
            inputType = PlayerPrefs.GetInt("InputType");
            //load  to keyboard Settings array
            getKeyboardSettingsFromPreferences();
            //apply the settings that are loaded
            applyKeyboardSettingsToKeys();

            Debug.Log("loaded settings");
        }
        else createSettings();
    }

    //check if there are any keyboard changes, if so return true
    public bool hasKeyboardChanges()
    {
        bool moveLeftChanged = true;
        bool moveRightChanged = true;
        bool jumpChanged = true;
        bool flashChanged = true;
        bool normalShroomChanged = true;
        bool bumpyShroomChanged = true;

        getKeyboardSettingsFromPreferences(); //first reload keyboardSettings array with the last input

        //check if the current keys put in has changed
        if (moveLeftKey == keyboardSettings[0]) moveLeftChanged = false;
        if (moveRightKey == keyboardSettings[1]) moveRightChanged = false;
        if (jumpKey == keyboardSettings[2]) jumpChanged = false;
        if (flashKey == keyboardSettings[3]) flashChanged = false;
        if (normalShroomKey == keyboardSettings[4]) normalShroomChanged = false;
        if (bumpyShroomKey == keyboardSettings[5]) bumpyShroomChanged = false;

        if (moveLeftChanged || moveRightChanged || jumpChanged || flashChanged || normalShroomChanged || bumpyShroomChanged)
        {
            return true;
        }

        return false;
    }
    //load the keys into the array
    public void getKeyboardSettingsFromPreferences()
    {
        keyboardSettings.Clear();

        keyboardSettings.Add((KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("MoveLeft")));
        keyboardSettings.Add((KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("MoveRight")));
        keyboardSettings.Add((KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Jump")));
        keyboardSettings.Add((KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Flash")));
        keyboardSettings.Add((KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("NormalShroom")));
        keyboardSettings.Add((KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("BumpyShroom")));
    }

    //when loading the settings it should apply them
    public void applyKeyboardSettingsToKeys()
    {
        moveLeftKey = keyboardSettings[0];
        moveRightKey = keyboardSettings[1];
        jumpKey = keyboardSettings[2];
        flashKey = keyboardSettings[3];
        normalShroomKey = keyboardSettings[4];
        bumpyShroomKey = keyboardSettings[5];
    }

    //private void resetSettings()
    //{
    //    //delete em
    //    PlayerPrefs.DeleteAll();
    //    //create em
    //    createSettings();
    //    //load em
    //    loadSettings();
    //}

    //private void deleteSettings()
    //{
    //    PlayerPrefs.DeleteAll();
    //}

    public bool isHeimBuild
    {
        get
        {
            return heimBuild;
        }
        set
        {
            heimBuild = value;
        }
    }

    public string difficultySetting
    {
        get
        {
            return difficulty;
        }
        set
        {
            difficulty = value;
        }
    }

    private void setLevelFileNameByInt(int level)
    {
        //array position of the Level
        int arrayint = level - 1;
        int levelID = (int)levelIDs[arrayint];
        //get the Level
        Level aLevel = getLevelByID(levelID);
        //get the file that has to be loaded
        levelFilename = aLevel.getLevelXmlByDifficulty(difficulty);
        //log what is loaded
        Debug.Log(levelFilename);
        Debug.Log("Level ID: " + aLevel.getLevelID());
        Debug.Log("Difficulty: " + difficulty);
    }

    public void loadHeimLevel()
    {
        int levelID = (int)levelIDs[0];    //get first level ID
        Level aLevel = getLevelByID(levelID); //get the level
        levelFilename = aLevel.getLevelXmlByDifficulty(difficulty); //get the file from the given difficulty

        //some debugs to show which level is loaded up at which difficulty
        Debug.Log(levelFilename);
        Debug.Log("Level ID: " + aLevel.getLevelID());
        Debug.Log("Difficulty: " + difficulty);

        //set the loading screen
        StartCoroutine(loadLevel());
    }

    private IEnumerator loadLevel()
    {
        //set the state on loading level
        //next scene with the loader
        Application.LoadLevel("LevelLoaderScene");
        //first wait for the next scene to loader		  		
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();       //double end of frame because the scene was not loaded yet with 1 end of frame

        //if they input type is keyboard -> push it to the player to update
        if (inputType == 1)
        {
            PlayerInputScript playerInput = GameObject.Find("Player").GetComponent<PlayerInputScript>();
            playerInput.setKeyboardSettings(keyboardSettings);
        }

        GameObject levelLoaderObject = GameObject.Find("LevelLoader");
        //get the levelloader script
        if (levelLoaderObject != null)
        {
            //get the level loader
            XmlToScene levelLoader = (XmlToScene)levelLoaderObject.GetComponent(typeof(XmlToScene));
            //set the level string
            levelLoader.setLevel(levelFilename);
            //load the level
            levelLoader.loadLevel();
            //destroy this gameobject as we don't need the main menu in the game
            Destroy(this.gameObject);
        }
        else Debug.LogError("levelLoader is null");
    }
}