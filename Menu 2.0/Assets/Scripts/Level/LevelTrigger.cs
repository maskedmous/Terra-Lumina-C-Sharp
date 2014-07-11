using UnityEngine;
using System.Collections;
using TouchScript;

public class LevelTrigger : MonoBehaviour
{

    private bool finished = false;
    private bool lost = false;
    private GameLogic gameLogicScript = null;
    private SoundEngineScript soundEngine = null;
    private bool notFinished = false;
    //private GUIStyle skin = new GUIStyle();

    public Texture2D toMenuWinTexture = null;
    private Rect winMenuRect;

    public Texture2D toMenuLoseTexture = null;
    private Rect loseMenuRect;

    public Texture2D notEnoughCrystalsTexture = null;
    private Rect notEnoughCrystalsRect;
    public float notEnoughCrystalsX = 0.0f;
    public float notEnoughCrystalsY = 0.0f;

    //scales for button positions
    private float originalWidth = 1920.0f;
    private float originalHeight = 1080.0f;
    private Vector3 scale = Vector3.zero;


    public void Awake()
    {
        gameLogicScript = GameObject.Find("GameLogic").GetComponent<GameLogic>() as GameLogic;
        if (GameObject.Find("TextureLoader") != null)
        {
            TextureLoader textureLoader = GameObject.Find("TextureLoader").GetComponent<TextureLoader>() as TextureLoader;
            //get the textures from the texture loader
            toMenuWinTexture = textureLoader.getTexture("WIN - return to menu");
            toMenuLoseTexture = textureLoader.getTexture("LOSE - return to menu");
            notEnoughCrystalsTexture = textureLoader.getTexture("NotEnoughCrystals");
        }
        if (Application.loadedLevelName == "LevelLoaderScene")
        {
            soundEngine = GameObject.Find("SoundEngine").GetComponent<SoundEngineScript>() as SoundEngineScript;
        }
    }

    public void OnEnable()
    {
        if (TouchManager.Instance != null)
        {
            TouchManager.Instance.TouchesBegan += touchBegan;
        }
    }

    public void OnDisable()
    {
        if (TouchManager.Instance != null)
        {
            TouchManager.Instance.TouchesBegan -= touchBegan;
        }
    }

    private void touchBegan(object sender, TouchEventArgs events)
    {
        foreach (var touchPoint in events.Touches)
        {
            Vector2 position = touchPoint.Position;
            position = new Vector2(position.x, (position.y - Screen.height) * -1);

            isPressingButton(position);
        }
    }

    private void isPressingButton(Vector2 inputXY)
    {
        if (finished)
        {
            if (winMenuRect.Contains(inputXY))
            {
                loadMenu();
            }
        }

        if (lost)
        {
            if (loseMenuRect.Contains(inputXY))
            {
                loadMenu();
            }
        }
    }

    public void loadMenu()
    {
        Application.LoadLevel("Menu");
        soundEngine.changeMusic("Menu");
    }

    void OnTriggerEnter(Collider hit)
    {
        if (hit.gameObject.name == "Player")
        {
            if (gameLogicScript.checkWin() == true)
            {
                finished = true;
                gameLogicScript.stopTimer();
            }
            else
            {
                notFinished = true;
            }
        }
    }

    public void OnTriggerExit(Collider obj)
    {
        if (obj.gameObject.name == "Player")
        {
            notFinished = false;
        }
    }

    void OnGUI()
    {
        scaleButtons();

        if (finished)
        {
            gameLogicScript.stopBattery();
            //this is the texture of the button
            GUI.DrawTexture(winMenuRect, toMenuWinTexture);
        }
        if (notFinished)
        {
            GUI.DrawTexture(notEnoughCrystalsRect, notEnoughCrystalsTexture);
        }
        if (lost)
        {
            gameLogicScript.stopBattery();
            //this is the texture of the button
            GUI.DrawTexture(loseMenuRect, toMenuLoseTexture);
        }

    }

    private void scaleButtons()
    {
        //get the current scale by using the current screen size and the original screen size
        //original width / height is defined in a variable at top, we use an aspect ratio of 16:9 and original screen size of 1920x1080
        scale.x = Screen.width / originalWidth;		//X scale is the current width divided by the original width
        scale.y = Screen.height / originalHeight;	//Y scale is the current height divided by the original height

        //first put the rectangles back to its original size before scaling
        winMenuRect = new Rect(0.0f, 0.0f, toMenuWinTexture.width, toMenuWinTexture.height);
        loseMenuRect = new Rect(0.0f, 0.0f, toMenuLoseTexture.width, toMenuLoseTexture.height);
        notEnoughCrystalsRect = new Rect(notEnoughCrystalsX, notEnoughCrystalsY, notEnoughCrystalsTexture.width, notEnoughCrystalsTexture.height);
        //second scale the rectangles
        winMenuRect = scaleRect(winMenuRect);
        loseMenuRect = scaleRect(loseMenuRect);
        notEnoughCrystalsRect = scaleRect(notEnoughCrystalsRect);
    }

    private Rect scaleRect(Rect rect)
    {
        Rect newRect = new Rect(rect.x * scale.x, rect.y * scale.y, rect.width * scale.x, rect.height * scale.y);
        return newRect;
    }

    public void setFinished(bool isFinished)
    {
        finished = isFinished;
    }

    public bool getFinished()
    {
        return finished;
    }

    public void setLost(bool isLost)
    {
        lost = isLost;
    }

    public bool getLost()
    {
        return lost;
    }

}