using UnityEngine;
using System.Collections;

public class TutorialTriggerScript : MonoBehaviour
{
    //gamelogic
    private GameLogic gameLogic = null;

    private PlayerInputScript playerInput = null;	//player input to change controls
    private SoundEngineScript soundEngine = null;	//sound engine to play sounds
    private float timePassed = 0.0f;				//count the time has passed during player visit

    //movement and button availability
    public bool movementLeftEnabled = true;	//default movement is true
    public bool movementRightEnabled = true;
    public bool jumpButtonEnabled = false;	//other controls that are false will be disabled upon entering
    public bool flashButtonEnabled = false;
    public bool shootNormalShroomButtonEnabled = false;
    public bool shootBumpyShroomButtonEnabled = false;

    //alpha GameObject
    public GameObject alphaObject = null;						//an object that is transparent

    private float blinkingTime = 4.0f;							//blinking time of the buttons
    //tutorial kind
    public bool jumpButtonTutorial = false;		//kinds of tutorials that will have special functions
    public bool normalShroomButtonTutorial = false;
    public bool flashButtonTutorial = false;
    public bool bumpyShroomButtonTutorial = false;
    //special tutorials with barriers (block objects)
    public bool lightTutorial = false;
    public bool slugTutorial = false;
    public bool crystalTutorial = false;

    //slug tutorial
    public GameObject slugObject = null;	//the slug for the slug tutorial

    //block object
    public GameObject blockObject = null;	//the barrier that will break down for some tutorials

    //scaling of the textures
    private Vector2 scale;
    private float originalWidth = 1920.0f;
    private float originalHeight = 1080.0f;
    //texture for the tutorial to show
    private bool showTutorialTextures = false;

    private bool touchInput = true;
    private bool keyboardInput = false;
    private bool xboxInput = false;

    //
    //Touch Textures
    //
    //tutorial texture A
    private Rect tutorialTextureARect;				//position and texture placed by the creative
    public Texture2D tutorialTextureA = null;
    public float xPositionTexA = 0.0f;
    public float yPositionTexA = 0.0f;
    public float timerTexA = -1.0f;	//timer of the texture that it'll go away
    //tutorial texture B
    private Rect tutorialTextureBRect;
    public Texture2D tutorialTextureB = null;
    public float xPositionTexB = 0;
    public float yPositionTexB = 0;
    public float timerTexB = -1;

    //
    //Keyboard Textures
    //
    private Rect keyboardTutorialTextureARect;				//position and texture placed by the creative
    public Texture2D keyboardTutorialTextureA = null;
    public float xPositionKeyboardTexA = 0.0f;
    public float yPositionKeyboardTexA = 0.0f;
    public float timerKeyboardTexA = -1.0f;	//timer of the texture that it'll go away
    //tutorial texture B
    private Rect keyboardTutorialTextureBRect;
    public Texture2D keyboardTutorialTextureB = null;
    public float xPositionKeyboardTexB = 0;
    public float yPositionKeyboardTexB = 0;
    public float timerKeyboardTexB = -1;

    //
    //Xbox Textures
    //
    private Rect xboxTutorialTextureARect;				//position and texture placed by the creative
    public Texture2D xboxTutorialTextureA = null;
    public float xPositionXboxTexA = 0.0f;
    public float yPositionXboxTexA = 0.0f;
    public float timerXboxTexA = -1.0f;	//timer of the texture that it'll go away

    //tutorial texture B
    private Rect xboxTutorialTextureBRect;
    public Texture2D xboxTutorialTextureB = null;
    public float xPositionXboxTexB = 0;
    public float yPositionXboxTexB = 0;
    public float timerXboxTexB = -1;


    //destroy this tutorial object on exit
    public bool destroyOnExit = false;
    public bool destroyOnCompletion = false;

    private bool cameraMoving = false;

    public void Awake()
    {
        playerInput = GameObject.Find("Player").GetComponent<PlayerInputScript>() as PlayerInputScript;
        gameLogic = GameObject.Find("GameLogic").GetComponent<GameLogic>() as GameLogic;

        if (Application.loadedLevelName == "LevelLoaderScene")
        {
            soundEngine = GameObject.Find("SoundEngine").GetComponent<SoundEngineScript>() as SoundEngineScript;
        }

        //get the input type!
        //
        //insert code
        //
    }

    public void Start()
    {
        //left is normally always enabled
        if (!movementLeftEnabled)
        {
            cameraMoving = true;								//camera should move true
            this.gameObject.AddComponent("CameraStartScript");	//add the component required for the intro
        }
    }

    //scaling the textures to show
    private void scaleTextures()
    {
        //get the current scale by using the current screen size and the original screen size
        //original width / height is defined in a variable at top, we use an aspect ratio of 16:9 and original screen size of 1920x1080
        scale.x = Screen.width / originalWidth;		//X scale is the current width divided by the original width
        scale.y = Screen.height / originalHeight;	//Y scale is the current height divided by the original height

        if (touchInput)
        {
            if (tutorialTextureA != null)
            {
                tutorialTextureARect = new Rect(xPositionTexA, yPositionTexA, tutorialTextureA.width, tutorialTextureA.height);
                tutorialTextureARect = scaleRect(tutorialTextureARect);
            }
            if (tutorialTextureB != null)
            {
                tutorialTextureBRect = new Rect(xPositionTexB, yPositionTexB, tutorialTextureB.width, tutorialTextureB.height);
                tutorialTextureBRect = scaleRect(tutorialTextureBRect);
            }
        }
        else if (keyboardInput)
        {
            if (keyboardTutorialTextureA != null)
            {
                keyboardTutorialTextureARect = new Rect(xPositionKeyboardTexA, yPositionKeyboardTexB, keyboardTutorialTextureA.width, keyboardTutorialTextureA.height);
                keyboardTutorialTextureARect = scaleRect(keyboardTutorialTextureARect);
            }
            if (keyboardTutorialTextureB != null)
            {
                keyboardTutorialTextureBRect = new Rect(xPositionKeyboardTexB, yPositionKeyboardTexB, keyboardTutorialTextureB.width, keyboardTutorialTextureB.height);
                keyboardTutorialTextureBRect = scaleRect(keyboardTutorialTextureBRect);
            }
        }
        else if (xboxInput)
        {
            if (xboxTutorialTextureA != null)
            {
                xboxTutorialTextureARect = new Rect(xPositionXboxTexA, yPositionXboxTexA, xboxTutorialTextureA.width, xboxTutorialTextureA.height);
                xboxTutorialTextureARect = scaleRect(xboxTutorialTextureARect);
            }
            if (xboxTutorialTextureB != null)
            {
                xboxTutorialTextureBRect = new Rect(xPositionXboxTexB, yPositionXboxTexB, xboxTutorialTextureB.width, xboxTutorialTextureB.height);
                xboxTutorialTextureBRect = scaleRect(xboxTutorialTextureBRect);
            }
        }
    }
    //scaling the rectangle
    private Rect scaleRect(Rect rect)
    {
        Rect newRect = new Rect(rect.x * scale.x, rect.y * scale.y, rect.width * scale.x, rect.height * scale.y);
        return newRect;
    }
    //show the textures on screen
    public void OnGUI()
    {
        if (showTutorialTextures && !cameraMoving && !gameLogic.isPaused())
        {
            scaleTextures();

            if (touchInput)
            {
                if (tutorialTextureA != null)
                {
                    GUI.DrawTexture(tutorialTextureARect, tutorialTextureA);
                }
                if (tutorialTextureB != null)
                {
                    GUI.DrawTexture(tutorialTextureBRect, tutorialTextureB);
                }
            }
            else if (keyboardInput)
            {
                if (keyboardTutorialTextureA != null)
                {
                    GUI.DrawTexture(keyboardTutorialTextureARect, keyboardTutorialTextureA);
                }
                if (keyboardTutorialTextureB != null)
                {
                    GUI.DrawTexture(keyboardTutorialTextureBRect, keyboardTutorialTextureB);
                }
            }
            else if (xboxInput)
            {
                if (xboxTutorialTextureA != null)
                {
                    GUI.DrawTexture(xboxTutorialTextureARect, xboxTutorialTextureA);
                }
                if (xboxTutorialTextureB != null)
                {
                    GUI.DrawTexture(xboxTutorialTextureBRect, xboxTutorialTextureB);
                }
            }
        }
    }

    //enables or disables controls called by onTriggerEnter
    private void changeControls()
    {
        if (!movementLeftEnabled) playerInput.setMovementLeftEnabled(false);
        else playerInput.setMovementLeftEnabled(true);

        if (!movementRightEnabled) playerInput.setMovementRightEnabled(false);
        else playerInput.setMovementRightEnabled(true);

        if (!jumpButtonEnabled) playerInput.setJumpButtonEnabled(false);
        else playerInput.setJumpButtonEnabled(true);

        if (!flashButtonEnabled) playerInput.setFlashButtonEnabled(false);
        else playerInput.setFlashButtonEnabled(true);

        if (!shootNormalShroomButtonEnabled) playerInput.setNormalShroomButtonEnabled(false);
        else playerInput.setNormalShroomButtonEnabled(true);

        if (!shootBumpyShroomButtonEnabled) playerInput.setBumpyShroomButtonEnabled(false);
        else playerInput.setBumpyShroomButtonEnabled(true);
    }

    //blink the buttons
    private void blinkButtons()
    {
        //xbox input has clickable buttons
        if (touchInput)
        {
            if (jumpButtonTutorial) playerInput.setBlinkingJumpButton(true);
            else if (normalShroomButtonTutorial) playerInput.setBlinkingNormalShroomButton(true);
            else if (flashButtonTutorial) playerInput.setBlinkingFlashButton(true);
            else if (bumpyShroomButtonTutorial) playerInput.setBlinkingBumpyShroomButton(true);
        }
    }

    //reset the blinking buttons
    private void resetBlinkingButtons()
    {
        if (touchInput)
        {
            if (jumpButtonTutorial) playerInput.setBlinkingJumpButton(false);
            else if (normalShroomButtonTutorial) playerInput.setBlinkingNormalShroomButton(false);
            else if (flashButtonTutorial) playerInput.setBlinkingFlashButton(false);
            else if (bumpyShroomButtonTutorial) playerInput.setBlinkingBumpyShroomButton(false);
        }
    }

    private IEnumerator playAnimation()
    {
        if (blockObject != null)
        {
            Animation animation = blockObject.GetComponent<Animation>();
            animation.Play();
            if (soundEngine != null)
            {
                soundEngine.playSoundEffect("rock");
            }
            ParticleSystem particleSystem = blockObject.transform.FindChild("rockDust").GetComponent<ParticleSystem>();
            particleSystem.Play();

            yield return new WaitForSeconds(animation.GetClip("Take 001").length);
            Destroy(blockObject);

            if (destroyOnCompletion)
            {
                Destroy(this.gameObject);
            }
        }
    }

    //on trigger enter it should change controls and maybe blink buttons if it is a tutorial
    public void OnTriggerEnter(Collider collider)
    {
        //change the controls upon entering the trigger box
        if (collider.gameObject.name == "Player")
        {
            changeControls();
            blinkButtons();
            if (isUsingTextures())
            {
                showTutorialTextures = true;
            }
        }
    }

    private bool isUsingTextures()
    {
        if (tutorialTextureA != null || tutorialTextureB != null) return true;
        if (keyboardTutorialTextureA != null || keyboardTutorialTextureB != null) return true;
        if (xboxTutorialTextureA != null || xboxTutorialTextureB != null) return true;

        return false;
    }
    //while the player stays inside the tutorial box
    public void OnTriggerStay(Collider collider)
    {
        if (collider.gameObject.name == "Player")
        {
            if (!cameraMoving) timePassed += Time.deltaTime;

            if (touchInput)
            {
                if (timePassed > timerTexA)
                {
                    tutorialTextureA = null;
                }
                if (timePassed > timerTexB)
                {
                    tutorialTextureB = null;
                }
                if (timePassed > blinkingTime)
                {
                    resetBlinkingButtons();
                }
            }
            else if (keyboardInput)
            {
                if (timePassed > timerKeyboardTexA)
                {
                    keyboardTutorialTextureA = null;
                }
                if (timePassed > timerKeyboardTexB)
                {
                    keyboardTutorialTextureB = null;
                }
            }
            else if (xboxInput)
            {
                if (timePassed > timerXboxTexA)
                {
                    xboxTutorialTextureA = null;
                }
                if (timePassed > timerXboxTexB)
                {
                    xboxTutorialTextureB = null;
                }
            }

            if (lightTutorial)
            {
                //if the tutorial is complete, play the animation and put it on false
                if (gameLogic.getBattery() == gameLogic.getBatteryCapacity())
                {
                    StartCoroutine(playAnimation());				//play the animation of the barrier
                    lightTutorial = false;			//completed objective
                }
            }
            else if (crystalTutorial)
            {
                if (gameLogic.getCrystalsSampleCount() > 0)
                {
                    StartCoroutine(playAnimation());
                    crystalTutorial = false;
                }
            }
            else if (slugTutorial)
            {
                SlugScript slugScript = (SlugScript)slugObject.GetComponent(typeof(SlugScript));
                if (slugScript.isWaitState())
                {
                    StartCoroutine(playAnimation());
                    slugTutorial = false;
                }
            }
        }
    }
    //when the player leaves the trigger box
    public void OnTriggerExit(Collider collider)
    {
        if (collider.name == "Player")
        {
            showTutorialTextures = false;	//stop showing textures

            resetBlinkingButtons();			//stop blinking buttons

            if (destroyOnExit)
            {
                Destroy(this.gameObject);	//if it is destroy on exit then destroy the object
            }
        }
    }

    //
    //setters
    //
    public void setAlphaObject(GameObject alphaObj)
    {
        alphaObject = alphaObj;
    }

    public void setJumpButtonTutorial(bool value)
    {
        jumpButtonTutorial = value;
    }

    public void setNormalShroomButtonTutorial(bool value)
    {
        normalShroomButtonTutorial = value;
    }

    public void setFlashButtonTutorial(bool value)
    {
        flashButtonTutorial = value;
    }

    public void setBumpyShroomButtonTutorial(bool value)
    {
        bumpyShroomButtonTutorial = value;
    }


    public void setLightTutorial(bool value)
    {
        lightTutorial = value;
    }

    public void setSlugTutorial(bool value)
    {
        slugTutorial = value;
    }

    public void setCrystalTutorial(bool value)
    {
        crystalTutorial = value;
    }

    public void setSlugObject(GameObject slugObj)
    {
        slugObject = slugObj;
    }

    public void setBlockObject(GameObject blockObj)
    {
        blockObject = blockObj;
    }

    public void setMovementLeftEnabled(bool value)
    {
        movementLeftEnabled = value;
    }
    public void setMovementRightEnabled(bool value)
    {
        movementRightEnabled = value;
    }

    public void setJumpButtonEnabled(bool value)
    {
        jumpButtonEnabled = value;
    }
    public void setFlashButtonEnabled(bool value)
    {
        flashButtonEnabled = value;
    }
    public void setNormalShroomButtonEnabled(bool value)
    {
        shootNormalShroomButtonEnabled = value;
    }
    public void setBumpyShroomButtonEnabled(bool value)
    {
        shootBumpyShroomButtonEnabled = value;
    }

    //
    //touch
    //
    public void setTutorialTextureA(Texture2D tex)
    {
        tutorialTextureA = tex;
    }
    public void setXPositionTexA(float value)
    {
        xPositionTexA = value;
    }
    public void setYPositionTexA(float value)
    {
        yPositionTexA = value;
    }
    public void setTimerTexA(float value)
    {
        timerTexA = value;
    }

    public void setTutorialTextureB(Texture2D tex)
    {
        tutorialTextureB = tex;
    }

    public void setXPositionTexB(float value)
    {
        xPositionTexB = value;
    }
    public void setYPositionTexB(float value)
    {
        yPositionTexB = value;
    }

    public void setTimerTexB(float value)
    {
        timerTexB = value;
    }

    //
    //keyboard
    //
    public void setKeyboardTutorialTextureA(Texture2D tex)
    {
        keyboardTutorialTextureA = tex;
    }
    public void setXPositionKeyboardTexA(float value)
    {
        xPositionKeyboardTexA = value;
    }
    public void setYPositionKeyboardTexA(float value)
    {
        yPositionKeyboardTexA = value;
    }
    public void setTimerKeyboardTexA(float value)
    {
        timerKeyboardTexA = value;
    }

    public void setKeyboardTutorialTextureB(Texture2D tex)
    {
        keyboardTutorialTextureB = tex;
    }

    public void setXPositionKeyboardTexB(float value)
    {
        xPositionKeyboardTexB = value;
    }
    public void setYPositionKeyboardTexB(float value)
    {
        yPositionKeyboardTexB = value;
    }

    public void setTimerKeyboardTexB(float value)
    {
        timerKeyboardTexB = value;
    }
    //
    //xbox
    //
    public void setXboxTutorialTextureA(Texture2D tex)
    {
        xboxTutorialTextureA = tex;
    }
    public void setXPositionXboxTexA(float value)
    {
        xPositionXboxTexA = value;
    }
    public void setYPositionXboxTexA(float value)
    {
        yPositionXboxTexA = value;
    }
    public void setTimerXboxTexA(float value)
    {
        timerXboxTexA = value;
    }

    public void setXboxTutorialTextureB(Texture2D tex)
    {
        xboxTutorialTextureB = tex;
    }

    public void setXPositionXboxTexB(float value)
    {
        xPositionXboxTexB = value;
    }
    public void setYPositionXboxTexB(float value)
    {
        yPositionXboxTexB = value;
    }

    public void setTimerXboxTexB(float value)
    {
        timerXboxTexB = value;
    }
    //
    public void setDestroyOnExit(bool value)
    {
        destroyOnExit = value;
    }

    public void setDestroyOnCompletion(bool value)
    {
        destroyOnCompletion = value;
    }

    //
    //getters
    //
    public GameObject getAlphaObject()
    {
        if (alphaObject != null)
        {
            return alphaObject;
        }

        return null;
    }

    public bool getJumpButtonTutorial()
    {
        return jumpButtonTutorial;
    }

    public bool getNormalShroomButtonTutorial()
    {
        return normalShroomButtonTutorial;
    }

    public bool getFlashButtonTutorial()
    {
        return flashButtonTutorial;
    }

    public bool getBumpyShroomButtonTutorial()
    {
        return bumpyShroomButtonTutorial;
    }

    public bool getLightTutorial()
    {
        return lightTutorial;
    }

    public bool getSlugTutorial()
    {
        return slugTutorial;
    }

    public bool getCrystalTutorial()
    {
        return crystalTutorial;
    }

    public GameObject getSlugObject()
    {
        return slugObject;
    }

    public GameObject getBlockObject()
    {
        return blockObject;
    }

    public bool getMovementLeftEnabled()
    {
        return movementLeftEnabled;
    }
    public bool getMovementRightEnabled()
    {
        return movementRightEnabled;
    }
    public bool getJumpButtonEnabled()
    {
        return jumpButtonEnabled;
    }
    public bool getFlashButtonEnabled()
    {
        return flashButtonEnabled;
    }
    public bool getNormalShroomButtonEnabled()
    {
        return shootNormalShroomButtonEnabled;
    }
    public bool getBumpyShroomButtonEnabled()
    {
        return shootBumpyShroomButtonEnabled;
    }

    //
    //Touch
    //
    public Texture2D getTutorialTextureA()
    {
        return tutorialTextureA;
    }

    public float getXPositionTexA()
    {
        return xPositionTexA;
    }

    public float getYPositionTexA()
    {
        return yPositionTexA;
    }

    public float getTimerTexA()
    {
        return timerTexA;
    }

    public Texture2D getTutorialTextureB()
    {
        return tutorialTextureB;
    }

    public float getXPositionTexB()
    {
        return xPositionTexB;
    }

    public float getYPositionTexB()
    {
        return yPositionTexB;
    }

    public float getTimerTexB()
    {
        return timerTexB;
    }

    //
    //keyboard
    //
    public Texture2D getKeyboardTutorialTextureA()
    {
        return keyboardTutorialTextureA;
    }
    public float getXPositionKeyboardTexA()
    {
        return xPositionKeyboardTexA;
    }
    public float getYPositionKeyboardTexA()
    {
        return yPositionKeyboardTexA;
    }
    public float getTimerKeyboardTexA()
    {
        return timerKeyboardTexA;
    }

    public Texture2D getKeyboardTutorialTextureB()
    {
        return keyboardTutorialTextureB;
    }

    public float getXPositionKeyboardTexB()
    {
        return xPositionKeyboardTexB;
    }
    public float getYPositionKeyboardTexB()
    {
        return yPositionKeyboardTexB;
    }

    public float getTimerKeyboardTexB()
    {
        return timerKeyboardTexB;
    }
    //
    //xbox
    //
    public Texture2D getXboxTutorialTextureA()
    {
        return xboxTutorialTextureA;
    }
    public float getXPositionXboxTexA()
    {
        return xPositionXboxTexA;
    }
    public float getYPositionXboxTexA()
    {
        return yPositionXboxTexA;
    }
    public float getTimerXboxTexA()
    {
        return timerXboxTexA;
    }

    public Texture2D getXboxTutorialTextureB()
    {
        return xboxTutorialTextureB;
    }

    public float getXPositionXboxTexB()
    {
        return xPositionXboxTexB;
    }
    public float getYPositionXboxTexB()
    {
        return yPositionXboxTexB;
    }

    public float getTimerXboxTexB()
    {
        return timerXboxTexB;
    }
    //

    public bool getDestroyOnExit()
    {
        return destroyOnExit;
    }

    public bool getDestroyOnCompletion()
    {
        return destroyOnCompletion;
    }

    public void setCameraMoving(bool value)
    {
        cameraMoving = value;
    }
}