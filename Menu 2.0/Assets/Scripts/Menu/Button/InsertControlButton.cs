using UnityEngine;
using System.Collections;

public class InsertControlButton : MonoBehaviour
{
    public bool debugMode = false;      //debug mode to see the hitbox
    public Texture2D debugTexture = null;   //debug texture to mark the rectangle area
    public string controlName = "";     //name of the control it corresponds to {MoveLeft, MoveRight, Jump, Flash, NormalShroom, BumpyShroom}
    public float xText = 0.0f;              //x,y position text
    public float yText = 0.0f;

    public float hitboxX = 0.0f;        //x of the hitbox
    public float hitboxY = 0.0f;        //y of the hitbox
    public float hitboxWidth = 0.0f;    //change hitbox size
    public float hitboxHeight = 0.0f;

    public float textBoxWidth = 100.0f;
    public float textBoxHeight = 100.0f;
    public int originalFontSize = 16;       //original font size
    private GUIStyle customFont = new GUIStyle();

    private Vector2 scale = new Vector2();  //scaling of the hitbox & text

    private Rect textRect;               //text rectangle
    private Rect hitboxRect;            //hitbox rectangle
    private Menu menu = null;           //menu script communication
    
    public float originalWidth = 1920.0f;
    public float originalHeight = 1080.0f;

    private bool changingKey = false;

    public int layerID = 1;

    public void Awake()
    {
        initializeControlButton();
    }

    private void initializeControlButton()
    {
        menu = GameObject.Find("Menu").GetComponent<Menu>();
        customFont = menu.getCustomFont;

        customFont.alignment = TextAnchor.MiddleRight;
    }

    //on touch began
    public bool isTouched(Vector2 input)
    {
        if (hitboxRect.Contains(input))
        {
            //code
            return true;
        }

        return false;
    }

    //on touch moved
    public bool isStillTouching(Vector2 input)
    {
        if (hitboxRect.Contains(input))
        {
            //code
            return true;
        }
        return false;
    }

    //on touch ended
    public bool isTouchReleased(Vector2 input)
    {
        if (hitboxRect.Contains(input))
        {
            //code
            if (!menu.isChangingKey && !changingKey)
            {
                menu.isChangingKey = true;
                changingKey = true;
            }
            return true;
        }
        return false;
    }

    //draw things
    public void OnGUI()
    {
        updateRectangles(); //update rectangles first then draw

        GUI.depth = layerID;

        customFont.fontSize = Mathf.RoundToInt(originalFontSize * scale.x);

        if (debugMode && debugTexture != null) GUI.DrawTexture(hitboxRect, debugTexture);

        if (!menu.isChangingKey && !changingKey)
        {
            GUI.Label(textRect, menu.getKeyCode(controlName), customFont);   //current key assigned
        }
        else if(menu.isChangingKey && !changingKey)
        {
            GUI.Label(textRect, menu.getKeyCode(controlName), customFont);   //this key is not the one changed
        }
        else if (menu.isChangingKey && changingKey)                         //this key is being changed
        {
            GUI.Label(textRect, "...", customFont);
        }

        if(menu.isChangingKey && changingKey)
        {
            Event e = Event.current;

            if(e.isKey && e.keyCode != KeyCode.None)
            {
                if (menu.checkValidKey(e.keyCode))
                {
                    menu.setKeyCode(controlName, e.keyCode);
                    menu.isChangingKey = false;
                    changingKey = false;
                }
                else
                {
                    menu.isChangingKey = false;
                    changingKey = false;
                }
            }
        }

        
    }

    private void updateRectangles()
    {
        scale = new Vector2(Screen.width / originalWidth, Screen.height / originalWidth);

        textRect = scaleRect(new Rect(xText, yText, textBoxWidth, textBoxHeight));
        hitboxRect = scaleRect(new Rect(hitboxX, hitboxY, hitboxWidth, hitboxHeight));
    }

    //scale a rectangle to screen size
    private Rect scaleRect(Rect rect)
    {
        return new Rect(rect.x * scale.x, rect.y * scale.y, rect.width * scale.x, rect.height * scale.y);
    }
}
