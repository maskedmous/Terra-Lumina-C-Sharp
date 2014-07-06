using UnityEngine;
using System.Collections;

using TouchScript;
using System.IO;
using System.Xml;

public class MenuScript : MonoBehaviour
{
    enum menuState { mainMenu, levelSelectionMenu, difficultyMenu, optionsMenu, creditsMenu, loadingLevel }

    private menuState currentMenuState = menuState.mainMenu;

    private Texture2D background = null;
    private Texture2D loadingScreen = null;
    private Texture2D level1 = null;
    private Texture2D backToMenuButton = null;

    private Texture2D creditsScreen = null;

    private string difficulty = "Easy";

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

    public bool exitButtonAvailable = false;
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
    //public Texture2D bloomCheckBoxTexture = null;
    //public Texture2D bloomCheckBoxActiveTexture = null;
    //public Texture2D bloomCheckBoxInactiveTexture = null;
    //private Rect bloomCheckBoxRect;
    //public float bloomCheckBoxX = 0.0f;
    //public float bloomCheckBoxY = 0.0f;
    public GUISkin sliderSkin;
    public Texture2D optionsScreenTexture = null;
    private Rect optionsScreenRect;
    public float optionsScreenX = 0.0f;
    public float optionsScreenY = 0.0f;

    //scales for button positions
    private float originalWidth = 1920.0f;
    private float originalHeight = 1080.0f;
    private Vector3 scale = Vector3.zero;

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

    public void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
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
        level1 = textureLoader.getTexture("Level1");
        backToMenuButton = textureLoader.getTexture("Terug");
        soundSliderTexture = textureLoader.getTexture("sliderBackground");
        soundSliderThumbTexture = textureLoader.getTexture("sliderThumb");
        creditsScreen = textureLoader.getTexture("Credits Screen");
        optionsScreenTexture = textureLoader.getTexture("Options Screen");
        easyButtonTexture = textureLoader.getTexture("Easy");
        mediumButtonTexture = textureLoader.getTexture("Medium");
        hardButtonTexture = textureLoader.getTexture("Hard");

        currentStartTexture = startButtonTexture;
        currentSettingsTexture = settingsButtonTexture;
        currentCreditsTexture = creditsButtonTexture;
        currentExitTexture = exitButtonTexture;

        menuButtonXStart = startButtonX;
        menuButtonXSettings = settingsButtonX;
        menuButtonXCredits = creditsButtonX;
        menuButtonXExit = exitButtonX;
        startMenuAnim();

        soundEngine = GameObject.Find("SoundEngine").GetComponent<SoundEngineScript>() as SoundEngineScript;

        anim = GameObject.Find("RoverAnimMenu").GetComponent<Animator>();

        min = soundSliderX + 27;
        max = soundSliderX + soundSliderTexture.width - 20;
        calculationLength = max - min;
        soundSliderThumbX = (soundEngine.getVolume() * calculationLength) + min;
        calculateSound();

        if (startButtonTexture == null || exitButtonTexture == null || settingsButtonTexture == null || background == null || loadingScreen == null)
        {
            Debug.LogError("one of the textures loaded is null");
        }

        levelsXmlFilePath = Application.dataPath + "/LevelsXML/";
        fillXmlLevelArray();
        fillLevelArray();
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

    private void touchMoved(object sender, TouchEventArgs events)
    {
        foreach (var touchPoint in events.Touches)
        {
            if (currentMenuState == menuState.optionsMenu)
            {
                Vector2 position = touchPoint.Position;
                position = new Vector2(position.x, (position.y - Screen.height) * -1);

                updateSlider(position);
            }
        }
    }

    private void touchBegan(object sender, TouchEventArgs events)
    {
        foreach (var touchPoint in events.Touches)
        {
            //print(touchPoint.Position.x);
            if (currentMenuState == menuState.optionsMenu)
            {
                Vector2 position = touchPoint.Position;
                position = new Vector2(position.x, (position.y - Screen.height) * -1);

                updateSlider(position);
            }
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
        }
    }

    private void touchEnded(object sender, TouchEventArgs events)
    {
        foreach (var touchPoint in events.Touches)
        {
            Vector2 position = touchPoint.Position;
            position = new Vector2(position.x, (position.y - Screen.height) * -1);
            isReleasingButton(position);
        }
    }

    private void isReleasingButton(Vector2 inputXY)
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
                    }
                    else if (settingsButtonRect.Contains(inputXY))
                    {
                        anim.SetBool("settingsBool", true);
                        leaveMenuAnim = clickedSettings = true;
                    }
                    else if (creditsButtonRect.Contains(inputXY))
                    {
                        anim.SetBool("creditsBool", true);
                        leaveMenuAnim = clickedCredits = true;
                    }
                    if (exitButtonRect.Contains(inputXY) && exitButtonAvailable)
                    {
                        leaveMenuAnim = clickedQuit = true;
                        anim.SetBool("exitBool", true);
                    }
                    break;

                case (menuState.difficultyMenu):
                    if (easyButtonRect.Contains(inputXY))
                    {
                        difficulty = "Easy";
                        currentMenuState = menuState.levelSelectionMenu;
                    }
                    else if (mediumButtonRect.Contains(inputXY))
                    {
                        difficulty = "Medium";
                        currentMenuState = menuState.levelSelectionMenu;
                    }
                    else if (hardButtonRect.Contains(inputXY))
                    {
                        difficulty = "Hard";
                        currentMenuState = menuState.levelSelectionMenu;
                    }
                    else if(backToMenuButtonRect.Contains(inputXY))
                    {
                        startMenuAnim();
                        currentMenuState = menuState.mainMenu;
                        touchEnabled = false;
                        anim.SetBool("levelBool", false);
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
                            }
                        }
                        //previous page button (if applicable)
                        if (startLevelCount > 6)
                        {
                            if (new Rect(0, levelButtonYSize * 2, levelButtonXSize, levelButtonYSize).Contains(inputXY))
                            {
                                startLevelCount -= 6;
                            }
                        }
                        //back button

                        if (startLevelCount < 6)
                        {
                            //GUI.DrawTexture(new Rect(0, levelButtonYSize * 2, levelButtonXSize, levelButtonYSize), backToMenuButton, ScaleMode.StretchToFill);
                            if (backToMenuButtonRect.Contains(inputXY))
                            {
                                currentMenuState = menuState.difficultyMenu;   
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
                    }

                    //if (bloomCheckBoxRect.Contains(inputXY))
                    //{
                    //  if(bloomCheckBoxTexture == bloomCheckBoxActiveTexture)
                    //  {
                    //      bloomCheckBoxTexture = bloomCheckBoxInactiveTexture;
                    //      disableBloom();
                    //  }
                    //  else
                    //  { 
                    //      bloomCheckBoxTexture = bloomCheckBoxActiveTexture;
                    //      enableBloom();
                    //  }
                    //  
                    //}
                    break;

                case (menuState.creditsMenu):
                    if (backToMenuButtonRect.Contains(inputXY))
                    {
                        startMenuAnim();
                        currentMenuState = menuState.mainMenu;
                        touchEnabled = false;
                        anim.SetBool("creditsBool", false);
                    }
                    break;
            }
        }
    }

    public void OnGUI()
    {
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
                if (exitButtonAvailable) GUI.DrawTexture(exitButtonRect, currentExitTexture);

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
                GUI.DrawTexture(soundSliderRect, soundSliderTexture);
                GUI.DrawTexture(soundSliderThumbRect, soundSliderThumbTexture);
                GUI.DrawTexture(optionsScreenRect, optionsScreenTexture);
                //GUI.DrawTexture(bloomCheckBoxRect, bloomCheckBoxTexture);

                //back button
                GUI.DrawTexture(backToMenuButtonRect, backToMenuButton);

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
                break;

            case (menuState.levelSelectionMenu):
                backToMenuButtonRect = new Rect(backToMenuButtonX, backToMenuButtonY, backToMenuButton.width, backToMenuButton.height);
                backToMenuButtonRect = scaleRect(backToMenuButtonRect);
                break;

            case (menuState.optionsMenu):
                soundSliderRect = new Rect(soundSliderX, soundSliderY, soundSliderTexture.width, soundSliderTexture.height);
                soundSliderThumbRect = new Rect(soundSliderThumbX, soundSliderThumbY, soundSliderThumbTexture.width, soundSliderThumbTexture.height);
                //bloomCheckBoxRect = new Rect(bloomCheckBoxXm bloomCheckBoxY,bloomCheckBoxTexture.width, bloomCheckBoxTexture.height);
                optionsScreenRect = new Rect(optionsScreenX, optionsScreenY, optionsScreenTexture.width, optionsScreenTexture.height);
                backToMenuButtonRect = new Rect(backToMenuButtonX, backToMenuButtonY, backToMenuButton.width, backToMenuButton.height);

                soundSliderRect = scaleRect(soundSliderRect);
                soundSliderThumbRect = scaleRect(soundSliderThumbRect);
                //bloomCheckBoxRect = scaleRect(bloomCheckBoxRect);
                optionsScreenRect = scaleRect(optionsScreenRect);
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

    private IEnumerator loadLevel()
    {
        //next scene with the loader
        Application.LoadLevel("LevelLoaderScene");
        //first wait for the next scene to loader		  		
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();       //double end of frame because the scene was not loaded yet with 1 end of frame

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