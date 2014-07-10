using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using TouchScript;
using System.IO;
using System.Xml;

public class MenuScript : MonoBehaviour
{
    enum menuState { mainMenu, levelSelectionMenu, difficultyMenu, optionsMenu, setKeyboardControls, acceptSettings, creditsMenu, loadingLevel }

    private menuState currentMenuState = menuState.mainMenu;

    public bool heimBuild = false;      //switch between heim build and windows build
    public bool exitButtonEnabled = false;
    public bool debugTouch = false;     //debug the touch

    private Texture2D background = null;
    private Texture2D loadingScreen = null;
    private Texture2D level1 = null;
    private Texture2D backToMenuButton = null;
    private Texture2D creditsScreen = null;

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

    //Menu animations
    private float menuButtonXStart;
    private float menuButtonXSettings;
    private float menuButtonXCredits;
    private float menuButtonXExit;
    public int animMultiplier = 1800;
    private bool startAnimFinished = false;
    private bool settingsAnimFinished = false;
    private bool creditsAnimFinished = false;
    private bool clickedStart = false;
    private bool clickedSettings = false;
    private bool clickedCredits = false;
    private bool clickedQuit = false;
    private bool leaveMenuAnim = false;
    private bool menuAnim = true;

    //scales for button positions
    private float originalWidth = 1920.0f;
    private float originalHeight = 1080.0f;
    private Vector3 scale = Vector3.zero;

    //button positions
    public Texture2D startButtonTexture = null;
    public Texture2D startButtonPressedTexture = null;
    private Texture2D currentStartTexture = null;
    private Rect startButtonRect;
    public float startButtonX = 0.0f;
    public float startButtonY = 300.0f;

    public Texture2D settingsButtonTexture = null;
    public Texture2D settingsButtonPressedTexture = null;
    private Texture2D currentSettingsTexture = null;
    private Rect settingsButtonRect;
    public float settingsButtonX = 0.0f;
    public float settingsButtonY = 469.0f;

    public Texture2D creditsButtonTexture = null;
    public Texture2D creditsButtonPressedTexture = null;
    private Texture2D currentCreditsTexture = null;
    private Rect creditsButtonRect;
    public float creditsButtonX = 0.0f;
    public float creditsButtonY = 599.0f;

    public Texture2D exitButtonTexture = null;
    public Texture2D exitButtonPressedTexture = null;
    private Texture2D currentExitTexture = null;
    private Rect exitButtonRect;
    public float exitButtonX = 0.0f;
    public float exitButtonY = 730.0f;

    private Rect backToMenuButtonRect;
    public float backToMenuButtonX = -25.0f;
    public float backToMenuButtonY = 870.0f;

    private Rect creditsScreenRect;
    public float creditsScreenX = 0.0f;
    public float creditsScreenY = 0.0f;

    // settings vars
    public Texture2D soundSliderTexture = null;
    public Texture2D soundSliderThumbTexture = null;
    private Rect soundSliderRect;
    public float soundSliderX = 600.0f;
    public float soundSliderY = 360.0f;
    private Rect soundSliderThumbRect;
    public float soundSliderThumbX = 1350.0f;
    public float soundSliderThumbY = 338.0f;

    //blooming checkbox
    private BloomAndLensFlares bloomScript = null;
    public Texture2D bloomCheckBoxTexture = null;
    public Texture2D bloomCheckBoxActiveTexture = null;
    public Texture2D bloomCheckBoxInactiveTexture = null;
    private Rect bloomCheckBoxRect;
    public float bloomCheckBoxX = 525.0f;
    public float bloomCheckBoxY = 490.0f;

    public GUISkin sliderSkin;
    public Texture2D optionsScreenTexture = null;
    private Rect optionsScreenRect;
    public float optionsScreenX = 0.0f;
    public float optionsScreenY = 0.0f;

    public Texture2D arrowLeftTexture = null;
    private Rect arrowLeftRect;
    public float arrowLeftX = 520.0f;
    public float arrowLeftY = 700.0f;

    public int controllerTypeFontSize = 26;
    private Rect controllerTypeTextRect;
    public float controllerTypeTextX = 710.0f;
    public float controllerTypeTextY = 725.0f;
    public float controllerTypeTextWidth = 200.0f;
    public float controllerTypeTextHeight = 100.0f;

    public Texture2D arrowRightTexture = null;
    private Rect arrowRightRect;
    public float arrowRightX = 1200.0f;
    public float arrowRightY = 700.0f;

    public Texture2D customizeKeyboardTexture = null;
    private Rect customizeKeyboardRect;
    public float customizeKeyboardX = 680.0f;
    public float customizeKeyboardY = 800.0f;

    private bool changeKey = false;
    private bool insertMoveLeft = false;
    private bool insertMoveRight = false;
    private bool insertJump = false;
    private bool insertFlash = false;
    private bool insertShootNormal = false;
    private bool insertShootBumpy = false;

    private KeyCode inputKey = KeyCode.None;
    public Texture2D insertTextureBackground = null;

    //move Left
    private Rect moveLeftCustomizeRect;
    public float moveLeftCustomizeX = 0.0f;
    public float moveLeftCustomizeY = 0.0f;

    private Rect insertMoveLeftCustomizeRect;
    public float insertMoveLeftCustomizeX = 0.0f;
    public float insertMoveLeftCustomizeY = 0.0f;

    //move Right
    private Rect moveRightCustomizeRect;
    public float moveRightCustomizeX = 0.0f;
    public float moveRightCustomizeY = 0.0f;

    private Rect insertMoveRightCustomizeRect;
    public float insertMoveRightCustomizeX = 0.0f;
    public float insertMoveRightCustomizeY = 0.0f;
    //jump
    private Rect jumpCustomizeRect;
    public float jumpCustomizeX = 0.0f;
    public float jumpCustomizeY = 0.0f;

    private Rect insertJumpCustomizeRect;
    public float insertJumpCustomizeX = 0.0f;
    public float insertJumpCustomizeY = 0.0f;
    //flash
    private Rect flashCustomizeRect;
    public float flashCustomizeX = 0.0f;
    public float flashCustomizeY = 0.0f;

    private Rect insertFlashCustomizeRect;
    public float insertFlashCustomizeX = 0.0f;
    public float insertFlashCustomizeY = 0.0f;

    //Shoot Normal
    private Rect shootNormalCustomizeRect;
    public float shootNormalCustomizeX = 0.0f;
    public float shootNormalCustomizeY = 0.0f;

    private Rect insertShootNormalCustomizeRect;
    public float insertShootNormalCustomizeX = 0.0f;
    public float insertShootNormalCustomizeY = 0.0f;
    //shoot bumpy
    private Rect shootBumpyCustomizeRect;
    public float shootBumpyCustomizeX = 0.0f;
    public float shootBumpyCustomizeY = 0.0f;

    private Rect insertShootBumpyCustomizeRect;
    public float insertShootBumpyCustomizeX = 0.0f;
    public float insertShootBumpyCustomizeY = 0.0f;



    public Texture2D applySettingsTexture = null;
    private Rect applySettingsRect;
    public float applySettingsX = 1400.0f;
    public float applySettingsY = 900.0f;

    public int questionSettingsSize = 26;
    private Rect questionSettingsRect;
    public float questionSettingsX = 30.0f;
    public float questionSettingsY = 490.0f;
    public float questionSettingsWidth = 200.0f;
    public float questionSettingsHeight = 100.0f;

    public Texture2D acceptSettingsTexture = null;
    private Rect acceptSettingsRect;
    public float acceptSettingsX = 140.0f;
    public float acceptSettingsY = 600.0f;

    public Texture2D revertSettingsTexture = null;
    private Rect revertSettingsRect;
    public float revertSettingsX = 430.0f;
    public float revertSettingsY = 600.0f;
    private float revertTimer = 10.0f;
    private bool revertTimerEnabled = false;
    private bool changedSettings = false;

    private SoundEngineScript soundEngine = null;
    private bool touchEnabled = false;

    //sound slider
    private float min = 0.0f;
    private float max = 0.0f;
    private float calculationLength = 0.0f;
    private float calculation = 0.0f;

    //level buttons

    //easy
    public Texture2D easyButtonTexture = null;
    public Texture2D easyButtonTexturePressed = null;
    private Rect easyButtonRect;
    public float easyButtonX = 300.0f;
    public float easyButtonY = 330.0f;

    //medium
    public Texture2D mediumButtonTexture = null;
    public Texture2D mediumButtonTexturePressed = null;
    private Rect mediumButtonRect;
    public float mediumButtonX = 300.0f;
    public float mediumButtonY = 500.0f;

    //hard
    public Texture2D hardButtonTexture = null;
    public Texture2D hardButtonTexturePressed = null;
    private Rect hardButtonRect;
    public float hardButtonX = 300.0f;
    public float hardButtonY = 670.0f;

    public float levelButtonSpaceX = 265.0f;
    public float levelButtonSpaceY = 0.0f;
    public float levelButtonX = 500.0f;
    public float levelButtonY = 300.0f;

    private Animator roverAnim = null;   //call animations of the rover


    //different inputs

    private int inputType = 0;  //tuio + mouse standard
    private List<KeyCode> keyboardSettings = new List<KeyCode>();

    private KeyCode moveLeftKey = KeyCode.A;
    private KeyCode moveRightKey = KeyCode.D;
    private KeyCode jumpKey = KeyCode.Space;
    private KeyCode normalShroomKey = KeyCode.N;
    private KeyCode bumpyShroomKey = KeyCode.B;
    private KeyCode flashKey = KeyCode.LeftControl;


    TouchScript.InputSources.MouseInput mouseInput = null;
    TouchScript.InputSources.TuioInput tuioInput = null;
    TouchScript.InputSources.Win7TouchInput win7Input = null;
    TouchScript.InputSources.Win8TouchInput win8Input = null;

    //private float combinationCooldown = 0.0f; //debugging

    public void Awake()
    {
        DontDestroyOnLoad(this.gameObject);

        initializeScripts();    //load scripts for communication
        initializeTextures();   //load textures for the buttons

        if (!heimBuild)
        {
            loadSettings(); //load the settings set by the player
        }
        else deleteSettings();

        initializeSound();  //initialize the sound
        initalizeInput();   //initialize the input from the inputtype (heim = TUIO)
        startMenuAnim();    //start the animation of the menu

        //load the levels that are in the xml
        levelsXmlFilePath = Application.dataPath + "/LevelsXML/";
        //fill the xml Level array
        fillXmlLevelArray();
        //fill the level array
        fillLevelArray();
    }

    public void Update()
    {
        //non-heim features
        if (!heimBuild)
        {
            //if the revert timer is enabled
            if (revertTimerEnabled)
            {
                //count down in seconds
                revertTimer -= Time.deltaTime;
                //if the countdown is finished
                if (revertTimer <= 0.0f)
                {
                    loadSettings(); //reload the settings
                    initalizeInput();   //re-initialize the input from the reloaded settings
                    currentMenuState = menuState.optionsMenu;   //go back to the options menu
                    revertTimerEnabled = false;                 //set the revert  timer to false
                }
            }
            //if the player is trying to change keys in the setkeyboardcontrols
            if (currentMenuState == menuState.setKeyboardControls && changeKey)
            {
                //if the input is filled in set it to the correct key
                if (inputKey != KeyCode.None)
                {
                    Debug.Log("changing key");  //notify that you're changing the key

                    //check which one should be changed
                    if (insertMoveLeft) moveLeftKey = inputKey;
                    else if (insertMoveRight) moveRightKey = inputKey;
                    else if (insertJump) jumpKey = inputKey;
                    else if (insertFlash) flashKey = inputKey;
                    else if (insertShootNormal) normalShroomKey = inputKey;
                    else if (insertShootBumpy) bumpyShroomKey = inputKey;

                    //once its done changing reset the insertKeys so it won't constantly ask for a key
                    //and make sure that the player isn't able to insert multiple keys at the same time
                    resetInsertKey();
                }
            }
        }

        //
        //Debugging section for debugging keys
        //

        //combinationCooldown -= Time.deltaTime;

        //if (combinationCooldown <= 0.0f)
        //{
        //    combinationCooldown = 0.0f;

        //    if (Input.GetKey(KeyCode.LeftAlt) && Input.GetKey(KeyCode.F1))
        //    {
        //        if (debugTouch) debugTouch = false;
        //        else debugTouch = true;

        //        combinationCooldown = 1.0f;
        //    }
        //    else if (Input.GetKey(KeyCode.LeftAlt) && Input.GetKey(KeyCode.F2))
        //    {
        //        if (heimBuild) heimBuild = false;
        //        else heimBuild = true;

        //        combinationCooldown = 1.0f;
        //    }
        //}
    }

    private void resetInsertKey()
    {
        inputKey = KeyCode.None;
        changeKey = false;
        insertMoveLeft = false;
        insertMoveRight = false;
        insertJump = false;
        insertFlash = false;
        insertShootNormal = false;
        insertShootBumpy = false;
    }

    private void initializeScripts()
    {
        roverAnim = GameObject.Find("RoverAnimMenu").GetComponent<Animator>();
        soundEngine = GameObject.Find("SoundEngine").GetComponent<SoundEngineScript>() as SoundEngineScript;
        bloomScript = Camera.main.GetComponent<BloomAndLensFlares>();

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

    private void initializeSound()
    {
        min = soundSliderX + 27;
        max = soundSliderX + soundSliderTexture.width - 20;
        calculationLength = max - min;
        soundSliderThumbX = (soundEngine.getVolume() * calculationLength) + min;
        calculateSound();
    }

    private void initializeTextures()
    {
        //getting the texture loader
        TextureLoader textureLoader = GameObject.Find("TextureLoader").GetComponent<TextureLoader>() as TextureLoader;
        //get the textures from the texture loader
        startButtonTexture = textureLoader.getTexture("startbuttonNormal");
        exitButtonTexture = textureLoader.getTexture("quitbuttonNormal");
        creditsButtonTexture = textureLoader.getTexture("creditsbuttonNormal");
        settingsButtonTexture = textureLoader.getTexture("settingsbuttonNormal");

        startButtonPressedTexture = textureLoader.getTexture("startbuttonPressed");
        exitButtonPressedTexture = textureLoader.getTexture("quitbuttonPressed");
        creditsButtonPressedTexture = textureLoader.getTexture("creditsbuttonPressed");
        settingsButtonPressedTexture = textureLoader.getTexture("settingsbuttonPressed");
        background = textureLoader.getTexture("Background");
        loadingScreen = textureLoader.getTexture("Loading");
        level1 = textureLoader.getTexture("Level 1 Idle");
        backToMenuButton = textureLoader.getTexture("Terug");

        soundSliderTexture = textureLoader.getTexture("sliderBackground");
        soundSliderThumbTexture = textureLoader.getTexture("sliderThumb");
        bloomCheckBoxInactiveTexture = textureLoader.getTexture("uncheckedBox");
        bloomCheckBoxActiveTexture = textureLoader.getTexture("checkedBox");

        arrowLeftTexture = textureLoader.getTexture("leftArrow");
        arrowRightTexture = textureLoader.getTexture("rightArrow");
        applySettingsTexture = textureLoader.getTexture("applySettings");
        acceptSettingsTexture = textureLoader.getTexture("acceptSettings");
        revertSettingsTexture = textureLoader.getTexture("revertSettings");
        customizeKeyboardTexture = textureLoader.getTexture("CustomizeKeyboard");
        insertTextureBackground = textureLoader.getTexture("InsertBackground");

        creditsScreen = textureLoader.getTexture("Credits Screen");
        optionsScreenTexture = textureLoader.getTexture("Options Screen");

        easyButtonTexture = textureLoader.getTexture("Easy Idle");
        mediumButtonTexture = textureLoader.getTexture("Medium Idle");
        hardButtonTexture = textureLoader.getTexture("Hard Idle");

        easyButtonTexturePressed = textureLoader.getTexture("Easy Pressed");
        mediumButtonTexturePressed = textureLoader.getTexture("Medium Pressed");
        hardButtonTexturePressed = textureLoader.getTexture("Hard Pressed");

        currentStartTexture = startButtonTexture;
        currentSettingsTexture = settingsButtonTexture;
        currentCreditsTexture = creditsButtonTexture;
        currentExitTexture = exitButtonTexture;

        bloomCheckBoxTexture = bloomCheckBoxActiveTexture;

        menuButtonXStart = startButtonX;
        menuButtonXSettings = settingsButtonX;
        menuButtonXCredits = creditsButtonX;
        menuButtonXExit = exitButtonX;
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
            TouchManager.Instance.TouchesEnded += touchEnded;
            TouchManager.Instance.TouchesBegan += touchBegan;
            TouchManager.Instance.TouchesMoved += touchMoved;
        }
    }

    public void OnDisable()
    {
        if (TouchManager.Instance != null)
        {
            TouchManager.Instance.TouchesEnded -= touchEnded;
            TouchManager.Instance.TouchesBegan -= touchBegan;
            TouchManager.Instance.TouchesMoved -= touchMoved;
        }
    }

    private void calculateSound()
    {
        calculation = (soundSliderThumbX - min) / calculationLength;
        soundEngine.changeVolume(Mathf.Clamp(calculation, 0.0f, 1.0f));
    }

    private Vector2 invertY(Vector2 vector)
    {
        return new Vector2(vector.x, (vector.y - Screen.height) * -1);
    }

    //if a touch event is moved
    private void touchMoved(object sender, TouchEventArgs events)
    {
        foreach (var touchPoint in events.Touches)
        {
            if (currentMenuState == menuState.optionsMenu)
            {
                Vector2 position = touchPoint.Position;
                position = invertY(position);
                updateSlider(position);
            }
        }
    }
    //if a touch event is made
    private void touchBegan(object sender, TouchEventArgs events)
    {
        foreach (var touchPoint in events.Touches)
        {
            if (currentMenuState == menuState.optionsMenu)
            {
                Vector2 position = touchPoint.Position;
                position = invertY(position);
                updateSlider(position);
            }
        }
    }
    //if a touch has ended
    private void touchEnded(object sender, TouchEventArgs events)
    {
        foreach (var touchPoint in events.Touches)
        {
            Vector2 position = touchPoint.Position;
            position = invertY(position);
            if (isReleasingButton(position)) return;
        }
    }
    //update the slider
    private void updateSlider(Vector2 position)
    {
        //scaled rect.contains position
        if (soundSliderRect.Contains(position))
        {
            //sliderEnabled = true;
            soundSliderThumbX = (position.x / scale.x) - (soundSliderThumbTexture.width / 2);
            if (soundSliderThumbX + (soundSliderThumbTexture.width / 2) < min) soundSliderThumbX = min - (soundSliderThumbTexture.width / 2);
            else if (soundSliderThumbX + (soundSliderThumbTexture.width / 2) > max) soundSliderThumbX = max - (soundSliderThumbTexture.width / 2);
            calculateSound();
            changedSettings = true;
        }
    }

    private bool isReleasingButton(Vector2 inputXY)
    {
        //only if the touch is enabled it should read touches
        if (touchEnabled)
        {
            switch (currentMenuState)
            {
                case (menuState.mainMenu):
                    if (startButtonRect.Contains(inputXY))
                    {
                        leaveMenuAnim = clickedStart = true;
                        roverAnim.SetBool("levelBool", true);
                        return true;
                    }
                    else if (settingsButtonRect.Contains(inputXY))
                    {
                        roverAnim.SetBool("settingsBool", true);
                        leaveMenuAnim = clickedSettings = true;
                        return true;
                    }
                    else if (creditsButtonRect.Contains(inputXY))
                    {
                        roverAnim.SetBool("creditsBool", true);
                        leaveMenuAnim = clickedCredits = true;
                        return true;
                    }
                    if (exitButtonRect.Contains(inputXY) && !heimBuild)
                    {
                        leaveMenuAnim = clickedQuit = true;
                        roverAnim.SetBool("exitBool", true);
                        return true;
                    }
                    break;

                case (menuState.difficultyMenu):
                    if (easyButtonRect.Contains(inputXY))
                    {
                        difficulty = "Easy";
                        if (heimBuild)
                        {
                            loadHeimLevel();
                        }
                        else
                        {
                            currentMenuState = menuState.levelSelectionMenu;
                        }
                        return true;
                    }
                    else if (mediumButtonRect.Contains(inputXY))
                    {
                        difficulty = "Medium";
                        if (heimBuild)
                        {
                            loadHeimLevel();
                        }
                        else
                        {
                            currentMenuState = menuState.levelSelectionMenu;
                        }
                        return true;
                    }
                    else if (hardButtonRect.Contains(inputXY))
                    {
                        difficulty = "Hard";
                        if (heimBuild)
                        {
                            loadHeimLevel();
                        }
                        else
                        {
                            currentMenuState = menuState.levelSelectionMenu;
                        }

                        return true;
                    }
                    else if (backToMenuButtonRect.Contains(inputXY))
                    {
                        startMenuAnim();
                        currentMenuState = menuState.mainMenu;
                        touchEnabled = false;
                        roverAnim.SetBool("levelBool", false);
                        return true;
                    }
                    break;

                //non-heim only
                case (menuState.levelSelectionMenu):
                    int levelCount = startLevelCount;   //begin at level 1
                    int spaceCountX = 0;                //space multiplyer between buttons
                    int spaceCountY = 0;

                    for (int i = startLevelCount; i < startLevelCount + 6; ++i) //draw maximum of 6 levels
                    {
                        if (i <= levelIDs.Count)
                        {
                            if (scaleRect(new Rect(levelButtonX + (levelButtonSpaceX * spaceCountX), levelButtonY + (levelButtonSpaceY * spaceCountY), level1.width, level1.height)).Contains(inputXY))
                            {
                                touchEnabled = false;
                                setLevelFileNameByInt(i);
                                StartCoroutine(loadLevel());
                                return true;
                            }

                            spaceCountX++;
                            levelCount++;

                            if (levelCount == 3)
                            {
                                spaceCountY++;
                                spaceCountX = 1;
                            }
                        }

                        //back button
                        if (backToMenuButtonRect.Contains(inputXY))
                        {
                            currentMenuState = menuState.difficultyMenu;
                            return true;
                        }
                    }
                    break;

                case (menuState.optionsMenu):
                    if (backToMenuButtonRect.Contains(inputXY))
                    {
                        startMenuAnim();
                        currentMenuState = menuState.mainMenu;
                        touchEnabled = false;
                        roverAnim.SetBool("settingsBool", false);
                        changedSettings = false;
                        if (!heimBuild) loadSettings();
                        return true;
                    }

                    if (bloomCheckBoxRect.Contains(inputXY))
                    {
                        if (isBloomEnabled())
                        {
                            disableBloom();
                        }
                        else
                        {
                            enableBloom();
                        }
                        changedSettings = true;
                        return true;
                    }

                    if (!heimBuild)
                    {
                        if (arrowLeftRect.Contains(inputXY))
                        {
                            inputType--;
                            if (inputType == -1) inputType = 4;
                            changedSettings = true;
                            return true;
                        }
                        if (arrowRightRect.Contains(inputXY))
                        {
                            if (inputType++ == 4) inputType = 0;
                            changedSettings = true;
                            return true;
                        }
                        if (applySettingsRect.Contains(inputXY) && changedSettings)
                        {
                            initalizeInput();
                            revertTimer = 10.0f;
                            revertTimerEnabled = true;
                            currentMenuState = menuState.acceptSettings;
                            return true;
                        }
                        if (inputType == 1)//if using keyboard can customize controls
                        {
                            if (customizeKeyboardRect.Contains(inputXY))
                            {
                                getKeyboardSettings();                              //load the keyboard settings in
                                currentMenuState = menuState.setKeyboardControls;
                                return true;
                            }
                        }
                    }

                    break;
                case (menuState.setKeyboardControls):
                    //if the player clicks on the customize key rectangle, it should take the next input as keycode
                    //it should only execute this if the player is not already changing a key

                    //move Left
                    if (insertMoveLeftCustomizeRect.Contains(inputXY) && !changeKey)
                    {
                        Debug.Log("change move left key");
                        changeKey = true;
                        insertMoveLeft = true;
                        return true;
                    }
                    //move Right
                    else if (insertMoveRightCustomizeRect.Contains(inputXY) && !changeKey)
                    {
                        changeKey = true;
                        insertMoveRight = true;
                        return true;
                    }
                    //Jump
                    else if (insertJumpCustomizeRect.Contains(inputXY) && !changeKey)
                    {
                        changeKey = true;
                        insertJump = true;
                        return true;
                    }
                    //Flash
                    else if (insertFlashCustomizeRect.Contains(inputXY) && !changeKey)
                    {
                        changeKey = true;
                        insertFlash = true;
                        return true;
                    }
                    //Normal Shroom
                    else if (insertShootNormalCustomizeRect.Contains(inputXY) && !changeKey)
                    {
                        changeKey = true;
                        insertShootNormal = true;
                        return true;
                    }
                    //Bumpy Shroom
                    else if (insertShootNormalCustomizeRect.Contains(inputXY) && !changeKey)
                    {
                        changeKey = true;
                        insertShootBumpy = true;
                        return true;
                    }

                    if (backToMenuButtonRect.Contains(inputXY))
                    {
                        if (hasKeyboardChanges()) changedSettings = true;
                        currentMenuState = menuState.optionsMenu;
                        return true;
                    }
                    break;
                case (menuState.acceptSettings):
                    if (acceptSettingsRect.Contains(inputXY))
                    {
                        saveSettings(); // save the settings
                        getKeyboardSettingsFromPreferences(); //apply the preferences
                        changedSettings = false;        //you're not changing settings anymore
                        revertTimerEnabled = false;     //revert timer is disabled cause you accepted the changes
                        revertTimer = 10.0f;            //timer is set back to 10.0f
                        currentMenuState = menuState.optionsMenu;  //go back to options menu
                        return true;
                    }
                    if (revertSettingsRect.Contains(inputXY))
                    {
                        loadSettings();
                        changedSettings = false;
                        currentMenuState = menuState.optionsMenu;
                        return true;
                    }
                    break;
                case (menuState.creditsMenu):
                    if (backToMenuButtonRect.Contains(inputXY))
                    {
                        startMenuAnim();
                        currentMenuState = menuState.mainMenu;
                        touchEnabled = false;
                        roverAnim.SetBool("creditsBool", false);
                        return true;
                    }
                    break;
            }
        }

        return false;
    }

    private bool isBloomEnabled()
    {
        if (bloomScript.enabled) return true;

        return false;
    }

    private void disableBloom()
    {
        bloomScript.enabled = false;
    }

    private void enableBloom()
    {
        bloomScript.enabled = true;
    }

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

        //background texture
        GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), background);

        //first scale the buttons before drawing them
        scaleButtons();

        switch (currentMenuState)
        {
            case (menuState.mainMenu):
                //Animations of the menu
                if (menuAnim)
                {
                    currentStartTexture = startButtonTexture;
                    currentSettingsTexture = settingsButtonTexture;
                    currentCreditsTexture = creditsButtonTexture;
                    currentExitTexture = exitButtonTexture;

                    if (startButtonX <= menuButtonXStart)
                    {
                        startButtonX += Time.deltaTime * animMultiplier;
                        if (startButtonX >= menuButtonXStart)
                        {
                            startAnimFinished = true;
                            startButtonX = menuButtonXStart;
                        }
                    }
                    if (startAnimFinished)
                    {
                        if (settingsButtonX <= menuButtonXSettings)
                        {
                            settingsButtonX += Time.deltaTime * animMultiplier;
                            if (settingsButtonX >= menuButtonXSettings)
                            {
                                settingsAnimFinished = true;
                                settingsButtonX = menuButtonXSettings;
                            }
                        }
                    }
                    if (settingsAnimFinished)
                    {
                        if (creditsButtonX <= menuButtonXCredits)
                        {
                            creditsButtonX += Time.deltaTime * animMultiplier;
                            if (creditsButtonX >= menuButtonXCredits)
                            {
                                creditsAnimFinished = true;
                                creditsButtonX = menuButtonXCredits;
                            }
                        }
                    }
                    if (creditsAnimFinished)
                    {
                        if (exitButtonX <= menuButtonXExit)
                        {
                            exitButtonX += Time.deltaTime * animMultiplier;
                            if (exitButtonX >= menuButtonXExit)
                            {
                                exitButtonX = menuButtonXExit;
                                menuAnim = false;
                                touchEnabled = true;
                                roverAnim.SetBool("levelBool", false);
                                roverAnim.SetBool("settingsBool", false);
                                roverAnim.SetBool("creditsBool", false);
                            }
                        }
                    }
                }

                if (leaveMenuAnim)
                {
                    if (clickedStart)
                    {
                        currentStartTexture = startButtonPressedTexture;
                        if (settingsButtonX > settingsButtonTexture.width * -1) settingsButtonX -= Time.deltaTime * animMultiplier;
                        if (creditsButtonX > creditsButtonTexture.width * -1) creditsButtonX -= Time.deltaTime * animMultiplier;
                        if (exitButtonX > exitButtonTexture.width * -1) exitButtonX -= Time.deltaTime * animMultiplier;

                        if (exitButtonX <= exitButtonTexture.width * -1)
                        {
                            startButtonX -= Time.deltaTime * animMultiplier;
                            if (startButtonX <= startButtonTexture.width * -1)
                            {
                                clickedStart = false;
                                leaveMenuAnim = false;
                                currentMenuState = menuState.difficultyMenu;
                            }
                        }
                    }
                    if (clickedSettings)
                    {
                        currentSettingsTexture = settingsButtonPressedTexture;
                        if (startButtonX > startButtonTexture.width * -1) startButtonX -= Time.deltaTime * animMultiplier;
                        if (creditsButtonX > creditsButtonTexture.width * -1) creditsButtonX -= Time.deltaTime * animMultiplier;
                        if (exitButtonX > exitButtonTexture.width * -1) exitButtonX -= Time.deltaTime * animMultiplier;

                        if (exitButtonX <= exitButtonTexture.width * -1)
                        {
                            settingsButtonX -= Time.deltaTime * animMultiplier;
                            if (settingsButtonX <= settingsButtonTexture.width * -1)
                            {
                                clickedSettings = false;
                                leaveMenuAnim = false;
                                if (!heimBuild) loadSettings(); //load the current Settings
                                currentMenuState = menuState.optionsMenu;
                            }
                        }
                    }
                    if (clickedCredits)
                    {
                        currentCreditsTexture = creditsButtonPressedTexture;
                        if (settingsButtonX > settingsButtonTexture.width * -1) settingsButtonX -= Time.deltaTime * animMultiplier;
                        if (startButtonX > startButtonTexture.width * -1) startButtonX -= Time.deltaTime * animMultiplier;
                        if (exitButtonX > exitButtonTexture.width * -1) exitButtonX -= Time.deltaTime * animMultiplier;

                        if (exitButtonX <= exitButtonTexture.width * -1)
                        {
                            creditsButtonX -= Time.deltaTime * animMultiplier;
                            if (creditsButtonX <= creditsButtonTexture.width * -1)
                            {
                                clickedCredits = false;
                                leaveMenuAnim = false;
                                currentMenuState = menuState.creditsMenu;
                            }
                        }
                    }
                    if (clickedQuit)
                    {
                        currentExitTexture = exitButtonPressedTexture;
                        if (settingsButtonX > settingsButtonTexture.width * -1) settingsButtonX -= Time.deltaTime * animMultiplier;
                        if (creditsButtonX > creditsButtonTexture.width * -1) creditsButtonX -= Time.deltaTime * animMultiplier;
                        if (startButtonX > startButtonTexture.width * -1) startButtonX -= Time.deltaTime * animMultiplier;

                        if (startButtonX <= startButtonTexture.width * -1)
                        {
                            exitButtonX -= Time.deltaTime * animMultiplier;
                            if (exitButtonX <= exitButtonTexture.width * -1)
                            {
                                clickedQuit = false;
                                leaveMenuAnim = false;
                                Application.Quit();
                            }
                        }
                    }
                }
                //end of animations of the menu

                //start button
                GUI.DrawTexture(startButtonRect, currentStartTexture);

                //settings button
                GUI.DrawTexture(settingsButtonRect, currentSettingsTexture);

                //credits button
                GUI.DrawTexture(creditsButtonRect, currentCreditsTexture);

                //exit button
                if (!heimBuild || exitButtonEnabled) GUI.DrawTexture(exitButtonRect, currentExitTexture);

                break;
            case (menuState.difficultyMenu):
                GUI.DrawTexture(easyButtonRect, easyButtonTexture);
                GUI.DrawTexture(mediumButtonRect, mediumButtonTexture);
                GUI.DrawTexture(hardButtonRect, hardButtonTexture);
                GUI.DrawTexture(backToMenuButtonRect, backToMenuButton);
                break;

            case (menuState.levelSelectionMenu):
                int levelCount = startLevelCount;//level count which level are we starting with? starts at 1, but changable with buttons to support more levels
                int spaceCountX = 0; //no space needed the first time but the second time there needs to be space so buttons don't overlap on the same spot
                int spaceCountY = 0;

                for (int i = startLevelCount; i < startLevelCount + 6; ++i)
                {
                    //if the i smaller than the levelID count else it may not draw
                    if (i <= levelIDs.Count)
                    {
                        //draw the level button
                        GUI.DrawTexture(scaleRect(new Rect(levelButtonX + (levelButtonSpaceX * spaceCountX), levelButtonY + (levelButtonSpaceY * spaceCountY), level1.width, level1.height)), level1, ScaleMode.StretchToFill);

                        //add 1 space in X for the next button
                        spaceCountX++;
                        //we are having the next level button so levelCount ++
                        levelCount++;

                        //once we draw 3 levels we up the Y so it will pick a new line, reset the spaceCountX to 1
                        if (levelCount == 3)
                        {
                            spaceCountY++;
                            spaceCountX = 1;
                        }
                    }
                }

                //back button
                if (startLevelCount < 6)
                {
                    GUI.DrawTexture(backToMenuButtonRect, backToMenuButton);
                }
                break;

            case (menuState.optionsMenu):
                //flip the bloom texture
                if (isBloomEnabled())
                {
                    bloomCheckBoxTexture = bloomCheckBoxActiveTexture;
                }
                else bloomCheckBoxTexture = bloomCheckBoxInactiveTexture;

                GUI.DrawTexture(soundSliderRect, soundSliderTexture);
                GUI.DrawTexture(soundSliderThumbRect, soundSliderThumbTexture);
                GUI.DrawTexture(optionsScreenRect, optionsScreenTexture);
                GUI.DrawTexture(bloomCheckBoxRect, bloomCheckBoxTexture);

                //only non-heim builds should have these options available
                if (!heimBuild)
                {
                    GUI.DrawTexture(arrowLeftRect, arrowLeftTexture);

                    //scale the font 16
                    customFont.fontSize = Mathf.RoundToInt(scale.x * controllerTypeFontSize);


                    GUI.Label(controllerTypeTextRect, getSelectedControllerType(), customFont);
                    GUI.DrawTexture(arrowRightRect, arrowRightTexture);

                    if (inputType == 1) GUI.DrawTexture(customizeKeyboardRect, customizeKeyboardTexture);
                    if (changedSettings) GUI.DrawTexture(applySettingsRect, applySettingsTexture);
                }

                //back button
                GUI.DrawTexture(backToMenuButtonRect, backToMenuButton);

                break;
            case (menuState.setKeyboardControls):

                //backgrounds for inserting key
                GUI.DrawTexture(insertMoveLeftCustomizeRect, insertTextureBackground);
                GUI.DrawTexture(insertMoveRightCustomizeRect, insertTextureBackground);
                GUI.DrawTexture(insertJumpCustomizeRect, insertTextureBackground);
                GUI.DrawTexture(insertFlashCustomizeRect, insertTextureBackground);
                GUI.DrawTexture(insertShootNormalCustomizeRect, insertTextureBackground);
                GUI.DrawTexture(insertShootBumpyCustomizeRect, insertTextureBackground);

                //Draw things
                customFont.fontSize = Mathf.RoundToInt(scale.x * 26); //font

                //move Left
                if (insertMoveLeft && changeKey) GUI.Label(moveLeftCustomizeRect, "Move Left:    ...", customFont);
                else GUI.Label(moveLeftCustomizeRect, "Move Left:    " + moveLeftKey.ToString(), customFont);
                //move Right
                if (insertMoveRight && changeKey) GUI.Label(moveRightCustomizeRect, "Move Right:    ...", customFont);
                else GUI.Label(moveRightCustomizeRect, "Move Right:    " + moveRightKey.ToString(), customFont);
                //jump
                if (insertJump && changeKey) GUI.Label(jumpCustomizeRect, "Jump:    ...", customFont);
                else GUI.Label(jumpCustomizeRect, "Jump:    " + jumpKey.ToString(), customFont);
                //flash
                if (insertFlash && changeKey) GUI.Label(flashCustomizeRect, "Flash:    ...", customFont);
                else GUI.Label(flashCustomizeRect, "Flash:    " + flashKey.ToString(), customFont);
                //Shoot Normal
                if (insertShootNormal && changeKey) GUI.Label(shootNormalCustomizeRect, "Normal Shroom:    ...", customFont);
                else GUI.Label(shootNormalCustomizeRect, "Normal Shroom:    " + normalShroomKey.ToString(), customFont);
                //shoot bumpy
                if (insertShootBumpy && changeKey) GUI.Label(shootBumpyCustomizeRect, "Bumpy Shroom:    ...", customFont);
                else GUI.Label(shootBumpyCustomizeRect, "Bumpy Shroom:    " + bumpyShroomKey.ToString(), customFont);



                //only execute when changing keys
                if (changeKey)
                {
                    //read out the next keyboard input event
                    Event e = Event.current;
                    //if the key is a keyboard key and it not being null
                    if (e.isKey && e != null)
                    {
                        //if the keycode isn't NONE or a key up key (only key downs)
                        if (e.keyCode != KeyCode.None && e.type != EventType.keyUp)
                        {
                            //check if it is a valid key because we don't want duplicates do we?
                            if (checkValidKey(e.keyCode))
                            {
                                Debug.Log("Inserted new keycode: " + e.keyCode.ToString());
                                //insert the new keycode into the inputKey
                                inputKey = e.keyCode;
                            }
                            //the key is invalid
                            else
                            {
                                resetInsertKey();                       //reset insert key so it won't ask for a key
                                getKeyboardSettingsFromPreferences(); //reload the keyboard settings becuase it was invalid
                            }
                        }
                    }
                }

                //back button
                GUI.DrawTexture(backToMenuButtonRect, backToMenuButton);

                break;
            case (menuState.acceptSettings):
                customFont.fontSize = Mathf.RoundToInt(scale.x * questionSettingsSize);
                GUI.Label(questionSettingsRect, "Are you sure you want these changes? " + Mathf.RoundToInt(revertTimer).ToString(), customFont);
                GUI.DrawTexture(acceptSettingsRect, acceptSettingsTexture);
                GUI.DrawTexture(revertSettingsRect, revertSettingsTexture);
                break;
            case (menuState.creditsMenu):
                GUI.DrawTexture(creditsScreenRect, creditsScreen);
                GUI.DrawTexture(backToMenuButtonRect, backToMenuButton);
                break;
        }
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

    private void scaleButtons()
    {
        //get the current scale by using the current screen size and the original screen size
        //original width / height is defined in a variable at top, we use an aspect ratio of 16:9 and original screen size of 1920x1080
        scale.x = Screen.width / originalWidth;		//X scale is the current width divided by the original width
        scale.y = Screen.height / originalHeight;	//Y scale is the current height divided by the original height
        switch (currentMenuState)
        {
            case (menuState.mainMenu):
                startButtonRect = new Rect(startButtonX, startButtonY, startButtonTexture.width, startButtonTexture.height);
                settingsButtonRect = new Rect(settingsButtonX, settingsButtonY, settingsButtonTexture.width, settingsButtonTexture.height);
                creditsButtonRect = new Rect(creditsButtonX, creditsButtonY, creditsButtonTexture.width, creditsButtonTexture.height);
                exitButtonRect = new Rect(exitButtonX, exitButtonY, exitButtonTexture.width, exitButtonTexture.height);

                //second scale the rectangles
                startButtonRect = scaleRect(startButtonRect);
                settingsButtonRect = scaleRect(settingsButtonRect);
                creditsButtonRect = scaleRect(creditsButtonRect);
                exitButtonRect = scaleRect(exitButtonRect);
                break;

            case (menuState.difficultyMenu):
                easyButtonRect = new Rect(easyButtonX, easyButtonY, easyButtonTexture.width, easyButtonTexture.height);
                easyButtonRect = scaleRect(easyButtonRect);

                mediumButtonRect = new Rect(mediumButtonX, mediumButtonY, mediumButtonTexture.width, mediumButtonTexture.height);
                mediumButtonRect = scaleRect(mediumButtonRect);

                hardButtonRect = new Rect(hardButtonX, hardButtonY, hardButtonTexture.width, hardButtonTexture.height);
                hardButtonRect = scaleRect(hardButtonRect);

                backToMenuButtonRect = new Rect(backToMenuButtonX, backToMenuButtonY, backToMenuButton.width, backToMenuButton.height);
                backToMenuButtonRect = scaleRect(backToMenuButtonRect);
                break;

            case (menuState.levelSelectionMenu):
                backToMenuButtonRect = new Rect(backToMenuButtonX, backToMenuButtonY, backToMenuButton.width, backToMenuButton.height);
                backToMenuButtonRect = scaleRect(backToMenuButtonRect);
                break;

            case (menuState.optionsMenu):
                soundSliderRect = new Rect(soundSliderX, soundSliderY, soundSliderTexture.width, soundSliderTexture.height);
                soundSliderThumbRect = new Rect(soundSliderThumbX, soundSliderThumbY, soundSliderThumbTexture.width, soundSliderThumbTexture.height);
                bloomCheckBoxRect = new Rect(bloomCheckBoxX, bloomCheckBoxY, bloomCheckBoxTexture.width, bloomCheckBoxTexture.height);
                optionsScreenRect = new Rect(optionsScreenX, optionsScreenY, optionsScreenTexture.width, optionsScreenTexture.height);
                backToMenuButtonRect = new Rect(backToMenuButtonX, backToMenuButtonY, backToMenuButton.width, backToMenuButton.height);

                soundSliderRect = scaleRect(soundSliderRect);
                soundSliderThumbRect = scaleRect(soundSliderThumbRect);
                bloomCheckBoxRect = scaleRect(bloomCheckBoxRect);
                optionsScreenRect = scaleRect(optionsScreenRect);
                backToMenuButtonRect = scaleRect(backToMenuButtonRect);

                if (!heimBuild)
                {
                    arrowLeftRect = new Rect(arrowLeftX, arrowLeftY, arrowLeftTexture.width, arrowLeftTexture.height);
                    controllerTypeTextRect = new Rect(controllerTypeTextX, controllerTypeTextY, controllerTypeTextWidth, controllerTypeTextHeight);
                    arrowRightRect = new Rect(arrowRightX, arrowRightY, arrowRightTexture.width, arrowRightTexture.height);

                    arrowLeftRect = scaleRect(arrowLeftRect);
                    controllerTypeTextRect = scaleRect(controllerTypeTextRect);
                    arrowRightRect = scaleRect(arrowRightRect);

                    if (inputType == 1)
                    {
                        customizeKeyboardRect = new Rect(customizeKeyboardX, customizeKeyboardY, customizeKeyboardTexture.width, customizeKeyboardTexture.height);
                        customizeKeyboardRect = scaleRect(customizeKeyboardRect);
                    }

                    if (changedSettings)
                    {
                        applySettingsRect = new Rect(applySettingsX, applySettingsY, applySettingsTexture.width, applySettingsTexture.height);
                        applySettingsRect = scaleRect(applySettingsRect);
                    }

                }

                break;
            case (menuState.acceptSettings):
                acceptSettingsRect = new Rect(acceptSettingsX, acceptSettingsY, acceptSettingsTexture.width, acceptSettingsTexture.height);
                acceptSettingsRect = scaleRect(acceptSettingsRect);

                revertSettingsRect = new Rect(revertSettingsX, revertSettingsY, revertSettingsTexture.width, revertSettingsTexture.height);
                revertSettingsRect = scaleRect(revertSettingsRect);

                questionSettingsRect = new Rect(questionSettingsX, questionSettingsY, questionSettingsWidth, questionSettingsHeight);
                questionSettingsRect = scaleRect(questionSettingsRect);

                break;
            case (menuState.setKeyboardControls):
                moveLeftCustomizeRect = new Rect(moveLeftCustomizeX, moveLeftCustomizeY, 200, 100);
                moveRightCustomizeRect = new Rect(moveRightCustomizeX, moveRightCustomizeY, 200, 100);
                jumpCustomizeRect = new Rect(jumpCustomizeX, jumpCustomizeY, 200, 100);
                flashCustomizeRect = new Rect(flashCustomizeX, flashCustomizeY, 200, 100);
                shootNormalCustomizeRect = new Rect(shootNormalCustomizeX, shootNormalCustomizeY, 200, 100);
                shootBumpyCustomizeRect = new Rect(shootBumpyCustomizeX, shootBumpyCustomizeY, 200, 100);

                moveLeftCustomizeRect = scaleRect(moveLeftCustomizeRect);
                moveRightCustomizeRect = scaleRect(moveRightCustomizeRect);
                jumpCustomizeRect = scaleRect(jumpCustomizeRect);
                flashCustomizeRect = scaleRect(flashCustomizeRect);
                shootNormalCustomizeRect = scaleRect(shootNormalCustomizeRect);
                shootBumpyCustomizeRect = scaleRect(shootBumpyCustomizeRect);


                insertMoveLeftCustomizeRect = new Rect(insertMoveLeftCustomizeX, insertMoveLeftCustomizeY, insertTextureBackground.width, insertTextureBackground.height);
                insertMoveRightCustomizeRect = new Rect(insertMoveRightCustomizeX, insertMoveRightCustomizeY, insertTextureBackground.width, insertTextureBackground.height);
                insertJumpCustomizeRect = new Rect(insertJumpCustomizeX, insertJumpCustomizeY, insertTextureBackground.width, insertTextureBackground.height);
                insertFlashCustomizeRect = new Rect(insertFlashCustomizeX, insertFlashCustomizeY, insertTextureBackground.width, insertTextureBackground.height);
                insertShootNormalCustomizeRect = new Rect(insertShootNormalCustomizeX, insertShootNormalCustomizeY, insertTextureBackground.width, insertTextureBackground.height);
                insertShootBumpyCustomizeRect = new Rect(insertShootBumpyCustomizeX, insertShootBumpyCustomizeY, insertTextureBackground.width, insertTextureBackground.height);

                insertMoveLeftCustomizeRect = scaleRect(insertMoveLeftCustomizeRect);
                insertMoveRightCustomizeRect = scaleRect(insertMoveRightCustomizeRect);
                insertJumpCustomizeRect = scaleRect(insertJumpCustomizeRect);
                insertFlashCustomizeRect = scaleRect(insertFlashCustomizeRect);
                insertShootNormalCustomizeRect = scaleRect(insertShootNormalCustomizeRect);
                insertShootBumpyCustomizeRect = scaleRect(insertShootBumpyCustomizeRect);


                backToMenuButtonRect = new Rect(backToMenuButtonX, backToMenuButtonY, backToMenuButton.width, backToMenuButton.height);
                backToMenuButtonRect = scaleRect(backToMenuButtonRect);
                break;

            case (menuState.creditsMenu):
                creditsScreenRect = new Rect(creditsScreenX, creditsScreenY, creditsScreen.width, creditsScreen.height);
                backToMenuButtonRect = new Rect(backToMenuButtonX, backToMenuButtonY, backToMenuButton.width, backToMenuButton.height);

                creditsScreenRect = scaleRect(creditsScreenRect);
                backToMenuButtonRect = scaleRect(backToMenuButtonRect);
                break;
        }
    }

    private Rect scaleRect(Rect rect)
    {
        Rect newRect = new Rect(rect.x * scale.x, rect.y * scale.y, rect.width * scale.x, rect.height * scale.y);
        return newRect;
    }

    private void startMenuAnim()
    {
        startButtonX = startButtonTexture.width * -1;
        settingsButtonX = settingsButtonTexture.width * -1;
        creditsButtonX = creditsButtonTexture.width * -1;
        exitButtonX = exitButtonTexture.width * -1;
        menuAnim = true;
        startAnimFinished = settingsAnimFinished = creditsAnimFinished = false;
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

    private void createSettings()
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

    private void saveSettings()
    {
        Debug.Log("Saving Settings...");

        PlayerPrefs.SetFloat("Volume", soundEngine.getVolume());
        if (isBloomEnabled()) PlayerPrefs.SetInt("Bloom", 1);
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

    private void loadSettings()
    {
        if (PlayerPrefs.HasKey("CreatedSettings"))
        {
            soundEngine.changeVolume(PlayerPrefs.GetFloat("Volume"));
            if (PlayerPrefs.GetInt("Bloom") == 1) enableBloom();
            else disableBloom();

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
    private bool hasKeyboardChanges()
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
    private void getKeyboardSettingsFromPreferences()
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
    private void applyKeyboardSettingsToKeys()
    {
        moveLeftKey = keyboardSettings[0];
        moveRightKey = keyboardSettings[1];
        jumpKey = keyboardSettings[2];
        flashKey = keyboardSettings[3];
        normalShroomKey = keyboardSettings[4];
        bumpyShroomKey = keyboardSettings[5];
    }

    private void resetSettings()
    {
        //delete em
        PlayerPrefs.DeleteAll();
        //create em
        createSettings();
        //load em
        loadSettings();
    }

    private void deleteSettings()
    {
        PlayerPrefs.DeleteAll();
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

    private void loadHeimLevel()
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
        currentMenuState = menuState.loadingLevel;
        //put the background to loading level
        background = loadingScreen;

        //is bloom enabled?
        bool bloom = isBloomEnabled();

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
            //set the bloom if heim build, heim build doesn't have playerprefs else it'll be loaded automatically
            if (heimBuild)
            {
                BloomAndLensFlares bloomScript = Camera.main.GetComponent<BloomAndLensFlares>();
                bloomScript.enabled = bloom;
            }
            //play the music according to the difficulty
            soundEngine.changeMusic(difficulty);
            //destroy this gameobject as we don't need the main menu in the game
            Destroy(this.gameObject);
        }
        else Debug.LogError("levelLoader is null");
    }
}