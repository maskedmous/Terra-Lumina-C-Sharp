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

    public bool heimBuild = false;
    public bool debugTouch = false;

    private Texture2D background = null;
    private Texture2D loadingScreen = null;
    private Texture2D level1 = null;
    private Texture2D backToMenuButton = null;
    private Texture2D creditsScreen = null;

    private GUIStyle customFont = new GUIStyle();

    //difficulty
    private string difficulty = "Easy";

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
    private float min;
    private float max;
    private float calculationLength;
    private float calculation;

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

    private Animator anim = null;


    //different inputs

    private int inputType = 0;  //tuio + mouse standard
    private List<KeyCode> keyboardSettings = new List<KeyCode>();

    private KeyCode moveLeftKey = KeyCode.A;
    private KeyCode moveRightKey = KeyCode.D;
    private KeyCode jumpKey = KeyCode.Space;
    private KeyCode normalShroomKey = KeyCode.N;
    private KeyCode bumpyShroomKey = KeyCode.B;
    private KeyCode flashKey = KeyCode.LeftControl;
    private KeyCode escapeKey = KeyCode.Escape;


    TouchScript.InputSources.MouseInput mouseInput = null;
    TouchScript.InputSources.TuioInput tuioInput = null;
    TouchScript.InputSources.Win7TouchInput win7Input = null;
    TouchScript.InputSources.Win8TouchInput win8Input = null;

    public void Awake()
    {
        DontDestroyOnLoad(this.gameObject);

        initializeScripts();
        initializeTextures();

        if (!heimBuild)
        {
            loadSettings();
        }

        initializeSound();
        initalizeInput();
        startMenuAnim();

        levelsXmlFilePath = Application.dataPath + "/LevelsXML/";
        fillXmlLevelArray();
        fillLevelArray();
    }

    public void Update()
    {
        if (revertTimerEnabled)
        {
            revertTimer -= Time.deltaTime;

            if (revertTimer <= 0.0f)
            {
                loadSettings(); //load previous settings
                initalizeInput();
                currentMenuState = menuState.optionsMenu;
                revertTimerEnabled = false;
            }
        }
    }

    private void initializeScripts()
    {
        anim = GameObject.Find("RoverAnimMenu").GetComponent<Animator>();
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

    private void touchMoved(object sender, TouchEventArgs events)
    {
        foreach (var touchPoint in events.Touches)
        {
            if (currentMenuState == menuState.optionsMenu)
            {
                Vector2 position = touchPoint.Position;
                position = invertY(position);
                updatePressedMenuTextures(position);
                updateSlider(position);
            }
        }
    }

    private void touchBegan(object sender, TouchEventArgs events)
    {
        foreach (var touchPoint in events.Touches)
        {
            if (currentMenuState == menuState.optionsMenu)
            {
                Vector2 position = touchPoint.Position;
                position = invertY(position);
                updatePressedMenuTextures(position);
                updateSlider(position);
            }
        }
    }

    private void updatePressedMenuTextures(Vector2 inputXY)
    {
        switch (currentMenuState)
        {
            case (menuState.mainMenu):

                break;
            case (menuState.difficultyMenu):

                break;
            case (menuState.levelSelectionMenu):

                break;
            case (menuState.creditsMenu):

                break;
        }
    }

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

    private void touchEnded(object sender, TouchEventArgs events)
    {
        foreach (var touchPoint in events.Touches)
        {
            Vector2 position = touchPoint.Position;
            position = invertY(position);
            if (isReleasingButton(position)) return;
        }
    }

    private bool isReleasingButton(Vector2 inputXY)
    {
        if (touchEnabled)
        {
            switch (currentMenuState)
            {
                case (menuState.mainMenu):
                    if (startButtonRect.Contains(inputXY))
                    {
                        leaveMenuAnim = clickedStart = true;
                        anim.SetBool("levelBool", true);
                        return true;
                    }
                    else if (settingsButtonRect.Contains(inputXY))
                    {
                        anim.SetBool("settingsBool", true);
                        leaveMenuAnim = clickedSettings = true;
                        return true;
                    }
                    else if (creditsButtonRect.Contains(inputXY))
                    {
                        anim.SetBool("creditsBool", true);
                        leaveMenuAnim = clickedCredits = true;
                        return true;
                    }
                    if (exitButtonRect.Contains(inputXY) && !heimBuild)
                    {
                        leaveMenuAnim = clickedQuit = true;
                        anim.SetBool("exitBool", true);
                        return true;
                    }
                    break;

                case (menuState.difficultyMenu):
                    if (easyButtonRect.Contains(inputXY))
                    {
                        difficulty = "Easy";
                        currentMenuState = menuState.levelSelectionMenu;
                        return true;
                    }
                    else if (mediumButtonRect.Contains(inputXY))
                    {
                        difficulty = "Medium";
                        currentMenuState = menuState.levelSelectionMenu;
                        return true;
                    }
                    else if (hardButtonRect.Contains(inputXY))
                    {
                        difficulty = "Hard";
                        currentMenuState = menuState.levelSelectionMenu;
                        return true;
                    }
                    else if (backToMenuButtonRect.Contains(inputXY))
                    {
                        startMenuAnim();
                        currentMenuState = menuState.mainMenu;
                        touchEnabled = false;
                        anim.SetBool("levelBool", false);
                        return true;
                    }
                    break;

                case (menuState.levelSelectionMenu):

                    //show all levels (max 6? per screen)
                    int levelCount = startLevelCount;
                    int spaceCountX = 0;
                    int spaceCountY = 0;
                    float levelButtonXSize = Screen.width / 9;
                    float levelButtonYSize = Screen.height / 5;

                    for (int i = startLevelCount; i < startLevelCount + 6; ++i)
                    {
                        if (i <= levelIDs.Count)
                        {
                            if (scaleRect(new Rect(levelButtonX + (levelButtonSpaceX * spaceCountX), levelButtonY + (levelButtonSpaceY * spaceCountY), level1.width, level1.height)).Contains(inputXY))
                            {
                                touchEnabled = false;
                                setLevelFileNameByInt(i);
                                currentMenuState = menuState.loadingLevel;
                                background = loadingScreen;
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

                        //next page button (if applicable)
                        if (startLevelCount + 5 < levels.Count)
                        {
                            //there are more levels available
                            if (new Rect(Screen.width - levelButtonXSize, levelButtonYSize * 2, levelButtonXSize, levelButtonYSize).Contains(inputXY))
                            {
                                startLevelCount += 6;
                                return true;
                            }
                        }
                        //previous page button (if applicable)
                        if (startLevelCount > 6)
                        {
                            if (new Rect(0, levelButtonYSize * 2, levelButtonXSize, levelButtonYSize).Contains(inputXY))
                            {
                                startLevelCount -= 6;
                                return true;
                            }
                        }
                        //back button

                        if (startLevelCount < 6)
                        {
                            //GUI.DrawTexture(new Rect(0, levelButtonYSize * 2, levelButtonXSize, levelButtonYSize), backToMenuButton, ScaleMode.StretchToFill);
                            if (backToMenuButtonRect.Contains(inputXY))
                            {
                                currentMenuState = menuState.difficultyMenu;
                                return true;
                            }
                        }
                    }
                    break;

                case (menuState.optionsMenu):
                    if (backToMenuButtonRect.Contains(inputXY))
                    {
                        startMenuAnim();
                        currentMenuState = menuState.mainMenu;
                        touchEnabled = false;
                        anim.SetBool("settingsBool", false);
                        changedSettings = false;
                        loadSettings();
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
                                currentMenuState = menuState.setKeyboardControls;
                                return true;
                            }
                        }
                    }

                    break;
                case (menuState.setKeyboardControls):
                    //the whole table thing
                    //
                    //
                    //

                    if (backToMenuButtonRect.Contains(inputXY))
                    {
                        changedSettings = true;
                        currentMenuState = menuState.optionsMenu;
                        return true;
                    }
                    break;
                case (menuState.acceptSettings):
                    if (acceptSettingsRect.Contains(inputXY))
                    {
                        saveSettings();
                        changedSettings = false;
                        startMenuAnim();
                        currentMenuState = menuState.mainMenu;
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
                        anim.SetBool("creditsBool", false);
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
        if (debugTouch) GUI.Label(new Rect(0, 0, Screen.width, 200), "InputType: " + getInputType() + "\n" + isMouseInputEnabled() + "\n" + isWin7InputEnabled() + "\n" + isWin8InputEnabled() + "\n" + numberOfTouches());
        //background texture
        GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), background);

        //first scale the buttons before drawing them
        scaleButtons();

        switch (currentMenuState)
        {
            case (menuState.mainMenu):

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
                                anim.SetBool("levelBool", false);
                                anim.SetBool("settingsBool", false);
                                anim.SetBool("creditsBool", false);
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
                                if (!heimBuild) loadSettings();
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
                //start button
                GUI.DrawTexture(startButtonRect, currentStartTexture);

                //settings button
                GUI.DrawTexture(settingsButtonRect, currentSettingsTexture);

                //credits button
                GUI.DrawTexture(creditsButtonRect, currentCreditsTexture);

                //exit button
                if (!heimBuild) GUI.DrawTexture(exitButtonRect, currentExitTexture);

                break;
            case (menuState.difficultyMenu):
                GUI.DrawTexture(easyButtonRect, easyButtonTexture);
                GUI.DrawTexture(mediumButtonRect, mediumButtonTexture);
                GUI.DrawTexture(hardButtonRect, hardButtonTexture);
                GUI.DrawTexture(backToMenuButtonRect, backToMenuButton);
                break;

            case (menuState.levelSelectionMenu):
                //show all levels (max 6? per screen)
                int levelCount = startLevelCount;
                int spaceCountX = 0;
                int spaceCountY = 0;

                for (int i = startLevelCount; i < startLevelCount + 6; ++i)
                {
                    if (i <= levelIDs.Count)
                    {
                        GUI.DrawTexture(scaleRect(new Rect(levelButtonX + (levelButtonSpaceX * spaceCountX), levelButtonY + (levelButtonSpaceY * spaceCountY), level1.width, level1.height)), level1, ScaleMode.StretchToFill);

                        spaceCountX++;
                        levelCount++;

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
                if (isBloomEnabled())
                {
                    bloomCheckBoxTexture = bloomCheckBoxActiveTexture;
                }
                else bloomCheckBoxTexture = bloomCheckBoxInactiveTexture;

                GUI.DrawTexture(soundSliderRect, soundSliderTexture);
                GUI.DrawTexture(soundSliderThumbRect, soundSliderThumbTexture);
                GUI.DrawTexture(optionsScreenRect, optionsScreenTexture);
                GUI.DrawTexture(bloomCheckBoxRect, bloomCheckBoxTexture);

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

                //Draw things
                string inputString = "";
                Event e = Event.current;
                if (e.isKey)
                {
                    if (e.keyCode == KeyCode.None || e.type == EventType.keyUp) return;
                    inputString = e.keyCode.ToString();
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

    private void createSettings()
    {
        Debug.Log("Creating Settings...");
        //create keys and values with defaults
        PlayerPrefs.SetFloat("Volume", 0.9031847f);
        PlayerPrefs.SetInt("Bloom", 1);
        PlayerPrefs.SetInt("InputType", 0);
        PlayerPrefs.SetString("MoveLeft", KeyCode.A.ToString());
        PlayerPrefs.SetString("MoveRight", KeyCode.D.ToString());
        PlayerPrefs.SetString("Jump", KeyCode.Space.ToString());
        PlayerPrefs.SetString("NormalShroom", KeyCode.N.ToString());
        PlayerPrefs.SetString("BumpyShroom", KeyCode.B.ToString());
        PlayerPrefs.SetString("Flash", KeyCode.LeftControl.ToString());
        PlayerPrefs.SetString("Escape", KeyCode.Escape.ToString());
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
        PlayerPrefs.SetString("NormalShroom", normalShroomKey.ToString());
        PlayerPrefs.SetString("BumpyShroom", bumpyShroomKey.ToString());
        PlayerPrefs.SetString("Flash", flashKey.ToString());
        PlayerPrefs.SetString("Escape", escapeKey.ToString());

        PlayerPrefs.Save();
        Debug.Log("Saved Settings");
    }

    private void loadSettings()
    {
        if (PlayerPrefs.HasKey("CreatedSettings"))
        {
            Debug.Log("Loading Settings...");
            soundEngine.changeVolume(PlayerPrefs.GetFloat("Volume"));
            if (PlayerPrefs.GetInt("Bloom") == 1) enableBloom();
            else disableBloom();

            //set input type
            inputType = PlayerPrefs.GetInt("InputType");
            keyboardSettings.Clear();
            //load keyboard Settings
            keyboardSettings.Add((KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("MoveLeft")));
            keyboardSettings.Add((KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("MoveRight")));
            keyboardSettings.Add((KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Jump")));
            keyboardSettings.Add((KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("NormalShroom")));
            keyboardSettings.Add((KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("BumpyShroom")));
            keyboardSettings.Add((KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Flash")));
            keyboardSettings.Add((KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Escape")));

            Debug.Log("Settings Loaded");
        }
        else createSettings();
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

    private IEnumerator loadLevel()
    {
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
            XmlToScene levelLoader = (XmlToScene)levelLoaderObject.GetComponent(typeof(XmlToScene));
            //set the level string
            levelLoader.setLevel(levelFilename);
            //load the level
            levelLoader.loadLevel();
            //play the music according to the difficulty
            soundEngine.changeMusic(difficulty);
            //destroy this gameobject as we don't need the main menu in the game
            Destroy(this.gameObject);
        }
        else Debug.LogError("levelLoader is null");
    }
}