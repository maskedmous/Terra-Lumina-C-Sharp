using UnityEngine;
using System.Collections;

public class Button : MonoBehaviour
{
    public string buttonName = "";  //button name
    public float winX = 0.0f;      //position
    public float winY = 0.0f;
    private Texture2D currentButtonTexture = null;

    //windows texture
    public string idleWinTextureName = "";
    private Texture2D idleWinTexture = null;
    public string pressedWinTextureName = "";  //name of the texture
    private Texture2D pressedWinTexture = null;

    //heim texture
    public float heimX = 0.0f;
    public float heimY = 0.0f;
    public string idleHeimTextureName = "";
    private Texture2D idleHeimTexture = null;
    public string pressedHeimTextureName = "";
    private Texture2D pressedHeimTexture = null;

    private Rect buttonRect = new Rect(0.0f, 0.0f, 0.0f, 0.0f);  //rect

    public bool checkBox = false;
    public bool enabledBox = true;
    public float checkboxX = 0.0f;
    public float checkboxY = 0.0f;

    public string checkedBoxName = "";
    private Texture2D checkedBoxTexture = null;
    public string uncheckedBoxName = "";
    private Texture2D uncheckedBoxTexture = null;

    public bool switchGraphic = false;
    public string switchGraphicName = "";
    private SwitchGraphicTexture switchGraphicTexture = null;

    private Vector2 scale = new Vector2();      //scale
    public bool isEnabled = true;               //if the button is enabled

    public float originalWidth = 1920.0f;   //designed height and width for scaling
    public float originalHeight = 1080.0f;
    public string buttonBehaviourName = ""; //name of the script
    public bool heimButton = true;
    private Menu menu = null;
    private ButtonBehaviour buttonBehaviour = null; //behaviour tied to the button
    public int layerID = 1; //second layer

    //menu animations
    private Vector2 oldWinPosition = new Vector2();
    private Vector2 oldHeimPosition = new Vector2();

    public void Awake()
    {
        menu = GameObject.Find("Menu").GetComponent<Menu>();
        initializeButton();
    }

    public void resetButton()
    {
        winX = oldWinPosition.x;
        winY = oldWinPosition.y;

        heimX = oldHeimPosition.x;
        heimY = oldHeimPosition.y;
    }

    public void initializeButton()
    {
       oldWinPosition = new Vector2(winX, winY);
       oldHeimPosition = new Vector2(heimX, heimY);

        TextureLoader textureLoader = null;
        //use the texture loader to get the texture
        if (textureLoader = GameObject.Find("TextureLoader").GetComponent<TextureLoader>())
        {
            if (!checkBox)
            {
                if (idleWinTextureName != "" && pressedWinTextureName != "")
                {
                    idleWinTexture = textureLoader.getTexture(idleWinTextureName);
                    pressedWinTexture = textureLoader.getTexture(pressedWinTextureName);
                }
                if (idleHeimTextureName != "" && pressedWinTextureName != "")
                {
                    idleHeimTexture = textureLoader.getTexture(idleHeimTextureName);
                    pressedHeimTexture = textureLoader.getTexture(pressedHeimTextureName);
                }
            }
            else
            {
                checkedBoxTexture = textureLoader.getTexture(checkedBoxName);
                uncheckedBoxTexture = textureLoader.getTexture(uncheckedBoxName);
            }

        }
        //it can't find the texture loader so try using Resources.Load
        else
        {
            Debug.Log("Trying to load from resources: ");
            //try to get them from the resources
            if (!checkBox)
            {
                idleWinTexture = Resources.Load("Textures/" + idleWinTextureName) as Texture2D;
                pressedWinTexture = Resources.Load("Textures/" + pressedWinTextureName) as Texture2D;
            }
            else
            {
                checkedBoxTexture = Resources.Load("Textures/" + checkedBoxName) as Texture2D;
                uncheckedBoxTexture = Resources.Load("Textures/" + uncheckedBoxName) as Texture2D;
            }
        }

        if (!checkBox && !heimButton) currentButtonTexture = idleWinTexture;               //windows button english
        else if (!checkBox && heimButton && menu.isHeimBuild) currentButtonTexture = idleHeimTexture;       //not a checkbox but heim button in dutch
        else if (!checkBox && heimButton && !menu.isHeimBuild) currentButtonTexture = idleWinTexture;       //not a check box, part of heim
        else if (!checkBox && !heimButton && !menu.isHeimBuild) currentButtonTexture = idleWinTexture;      //not a check box windows only
        else if (checkBox && enabledBox) currentButtonTexture = checkedBoxTexture;      //checkbox so only checked / unchecked
        else if (checkBox && !enabledBox) currentButtonTexture = uncheckedBoxTexture;   //unchecked box texture

        if (buttonBehaviourName != "")
        {
            buttonBehaviour = (ButtonBehaviour)this.gameObject.AddComponent(buttonBehaviourName) as ButtonBehaviour;
            buttonBehaviour.initialize(this);
        }

        if(switchGraphic)
        {
            SwitchGraphicTexture[] switchGraphicsList = this.GetComponents<SwitchGraphicTexture>();

            foreach(SwitchGraphicTexture aSwitchGraphic in switchGraphicsList)
            {
                if(aSwitchGraphic.nameOfSwitchGraphic == switchGraphicName)
                {
                    switchGraphicTexture = aSwitchGraphic;
                }
            }
        }
    }

    public void increaseIterator()
    {
        if(switchGraphicTexture != null)
        {
            switchGraphicTexture.increaseIteration();
        }
    }

    public void decreaseIterator()
    {
        if (switchGraphicTexture != null)
        {
            switchGraphicTexture.decreaseIteration();
        }
    }

    public void OnGUI()
    {
        //if the button is enabled
        if (isEnabled)
        {
            //update the button first before drawing
            updateButton();
            GUI.depth = layerID;
            //draw the button

            if (menu.isHeimBuild && !heimButton)
            {
                //don't draw because this is the heim build and its not a heim button
            }
            //heim build
            else if (menu.isHeimBuild && heimButton)
            {

                //draw the heim texture
                if (idleHeimTexture != null || checkedBoxTexture != null && uncheckedBoxTexture != null)
                {
                    GUI.DrawTexture(buttonRect, currentButtonTexture);
                }
            }
            else
            {
                //it is not a heim build so draw windows textures
                if (idleWinTexture != null || checkedBoxTexture != null && uncheckedBoxTexture != null)
                {
                    GUI.DrawTexture(buttonRect, currentButtonTexture);
                }
            }
        }
    }

    //update the button so it scales with the current screen
    public void updateButton()
    {
        //get the current scale
        scale = new Vector2(Screen.width / originalWidth, Screen.height / originalHeight);

        if (!menu.isHeimBuild && idleWinTexture != null && pressedWinTexture != null)
        {
            //scale the button
            buttonRect = scaleButton(new Rect(winX, winY, idleWinTexture.width, idleWinTexture.height));
        }
        else if (menu.isHeimBuild && idleHeimTexture != null && pressedHeimTexture != null)
        {
            buttonRect = scaleButton(new Rect(heimX, heimY, idleHeimTexture.width, idleHeimTexture.height));
        }
        else if (checkBox && uncheckedBoxTexture != null && checkedBoxTexture != null)
        {
            buttonRect = scaleButton(new Rect(checkboxX, checkboxY, checkedBoxTexture.width, checkedBoxTexture.height));
        }
    }

    private Rect scaleButton(Rect rect)
    {
        return new Rect(rect.x * scale.x, rect.y * scale.y, rect.width * scale.x, rect.height * scale.y);
    }

    //check if the button is being touched, if so use the pressed texture
    public bool isTouched(Vector2 input)
    {
        //if the input is within the rectangle then it is being touched
        if (buttonRect.Contains(input))
        {
            //if it is not a checkbox then they're normal button textures
            //they're being touched so put the pressed texture on
            if (!checkBox)
            {
                if (!menu.isHeimBuild) currentButtonTexture = pressedWinTexture;
                else currentButtonTexture = pressedHeimTexture;
            }
            //it is a checkbox so on press behaviour
            else if (buttonBehaviour != null && checkBox)
            {
                //execute the checkbox function
                buttonBehaviour.executeButton();
            }
            return true;
        }
        return false;
    }

    public void switchCheckBox(bool value)
    {
        //if the box should be off uncheck it
        if (!value)
        {
            currentButtonTexture = uncheckedBoxTexture;
            enabledBox = false;
        }
        //if the box should be on check it
        else if (value)
        {
            currentButtonTexture = checkedBoxTexture;
            enabledBox = true;
        }
    }
    //if it is still touching the button after movement
    public bool isStillTouching(Vector2 input)
    {
        if (buttonRect.Contains(input))
        {
            if (!menu.isHeimBuild && !checkBox && currentButtonTexture == idleWinTexture) currentButtonTexture = pressedWinTexture;
            else if (menu.isHeimBuild && !checkBox && currentButtonTexture == idleHeimTexture) currentButtonTexture = pressedHeimTexture;
            return true;
        }
        if (!checkBox)
        {
            //if the button was touched before and isn't touched anymore, reset the texture
            if (!menu.isHeimBuild && currentButtonTexture == pressedWinTexture) currentButtonTexture = idleWinTexture;
            else if (menu.isHeimBuild && currentButtonTexture == pressedHeimTexture) currentButtonTexture = idleHeimTexture;
        }
        //button is not being pressed anymore
        return false;
    }

    //on release activate the button
    public bool isTouchReleased(Vector2 input)
    {
        if (buttonRect.Contains(input))
        {
            //activate the button
            if (buttonBehaviour != null && !checkBox)
            {
                buttonBehaviour.executeButton();
            }

            if (!checkBox)
            {
                if (!menu.isHeimBuild) currentButtonTexture = idleWinTexture;
                else if (menu.isHeimBuild) currentButtonTexture = idleHeimTexture;
            }
            //return the true
            return true;
        }
        return false;
    }

    public string nameOfButton
    {
        get
        {
            return buttonName;
        }
    }

    public float widthOfHeimTexture
    {
        get
        {
            return idleHeimTexture.width;
        }
    }
    public float widthOfWinTexture
    {
        get
        {
            return idleWinTexture.width;
        }
    }


    public float xPositionHeimButton
    {
        get
        {
            return oldHeimPosition.x;
        }
    }

    public float xPositionWinButton
    {
        get
        {
            return oldWinPosition.x;
        }
    }
}