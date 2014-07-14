using UnityEngine;
using System.Collections;

public class Button : MonoBehaviour
{
    public string buttonName = "";  //button name
    public float xPosition = 0.0f;      //position
    public float yPosition = 0.0f;
    private Texture2D currentButtonTexture = null;

    //windows texture
    public string idleTextureName = "";
    private Texture2D idleTexture = null;
    public string pressedTextureName = "";  //name of the texture
    private Texture2D pressedTexture = null;

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
    public bool isDisabled = false;             //disables the execute button function

    public float originalWidth = 1920.0f;   //designed height and width for scaling
    public float originalHeight = 1080.0f;
    public string buttonBehaviourName = ""; //name of the script
    private ButtonBehaviour buttonBehaviour = null; //behaviour tied to the button

    public string textBoxName = "";
    private TextBox textBox = null;

    public int layerID = 1; //second layer
    //menu animations
    private Vector2 oldPosition = new Vector2();

    public void Awake()
    {
        initializeButton();
    }

    public void resetButton()
    {
        xPosition = oldPosition.x;
        yPosition = oldPosition.y;
    }

    public void initializeButton()
    {
        oldPosition = new Vector2(xPosition, yPosition);

        TextureLoader textureLoader = null;
        //use the texture loader to get the texture
        if (textureLoader = GameObject.Find("TextureLoader").GetComponent<TextureLoader>())
        {
            if (!checkBox)
            {
                if (idleTextureName != "" && pressedTextureName != "")
                {
                    idleTexture = textureLoader.getTexture(idleTextureName);
                    pressedTexture = textureLoader.getTexture(pressedTextureName);
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
                idleTexture = Resources.Load("Textures/" + idleTextureName) as Texture2D;
                pressedTexture = Resources.Load("Textures/" + pressedTextureName) as Texture2D;
            }
            else
            {
                checkedBoxTexture = Resources.Load("Textures/" + checkedBoxName) as Texture2D;
                uncheckedBoxTexture = Resources.Load("Textures/" + uncheckedBoxName) as Texture2D;
            }
        }

        if (!checkBox) currentButtonTexture = idleTexture;                           //windows button english
        else if (checkBox && enabledBox) currentButtonTexture = checkedBoxTexture;      //checkbox so only checked / unchecked
        else if (checkBox && !enabledBox) currentButtonTexture = uncheckedBoxTexture;   //unchecked box texture

        if (buttonBehaviourName != "")
        {
            buttonBehaviour = (ButtonBehaviour)this.gameObject.AddComponent(buttonBehaviourName) as ButtonBehaviour;
            buttonBehaviour.initialize(this);
        }

        if (textBoxName != "")
        {
            TextBox[] textboxes = this.GetComponents<TextBox>();
            foreach (TextBox aTextBox in textboxes)
            {
                if (aTextBox.textBoxName == textBoxName)
                {
                    textBox = aTextBox;
                }
            }
        }

        if (switchGraphic)
        {
            SwitchGraphicTexture[] switchGraphicsList = this.GetComponents<SwitchGraphicTexture>();

            foreach (SwitchGraphicTexture aSwitchGraphic in switchGraphicsList)
            {
                if (aSwitchGraphic.nameOfSwitchGraphic == switchGraphicName)
                {
                    switchGraphicTexture = aSwitchGraphic;
                }
            }
        }
    }

    public void increaseIterator()
    {
        if (switchGraphicTexture != null)
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

    public int iterator
    {
        get
        {
            if (switchGraphicTexture != null)
            {
                return switchGraphicTexture.iteration;
            }
            return 1;
        }
        set
        {
            if (switchGraphicTexture != null)
            {
                switchGraphicTexture.iteration = value;
            }
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
            if (idleTexture != null || checkedBoxTexture != null && uncheckedBoxTexture != null)
            {
                GUI.DrawTexture(buttonRect, currentButtonTexture);
            }
        }
    }

    //update the button so it scales with the current screen
    public void updateButton()
    {
        //get the current scale
        scale = new Vector2(Screen.width / originalWidth, Screen.height / originalHeight);

        if (idleTexture != null && pressedTexture != null)
        {
            //scale the button
            buttonRect = scaleButton(new Rect(xPosition, yPosition, idleTexture.width, idleTexture.height));
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
        if (isEnabled && !isDisabled)
        {
            //if the input is within the rectangle then it is being touched
            if (buttonRect.Contains(input))
            {
                //if it is not a checkbox then they're normal button textures
                //they're being touched so put the pressed texture on
                if (!checkBox)
                {
                    currentButtonTexture = pressedTexture;
                }
                //it is a checkbox so on press behaviour
                else if (buttonBehaviour != null && checkBox)
                {
                    //execute the checkbox function
                    buttonBehaviour.executeButton();
                }
                return true;
            }
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

    public bool isEnabledBox
    {
        get
        {
            return enabledBox;
        }
    }

    //if it is still touching the button after movement
    public bool isStillTouching(Vector2 input)
    {
        if (isEnabled && !isDisabled)
        {
            if (buttonRect.Contains(input))
            {
                if (!checkBox && currentButtonTexture == idleTexture) currentButtonTexture = pressedTexture;
                return true;
            }
            if (!checkBox)
            {
                //if the button was touched before and isn't touched anymore, reset the texture
                if (currentButtonTexture == pressedTexture) currentButtonTexture = idleTexture;
            }
        }
        //button is not being pressed anymore
        return false;
    }

    //on release activate the button
    public bool isTouchReleased(Vector2 input)
    {
        if (isEnabled && !isDisabled)
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
                    currentButtonTexture = idleTexture;
                }
                //return the true
                return true;
            }
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

    public float widthOfTexture
    {
        get
        {
            return idleTexture.width;
        }
    }

    public float xPositionButton
    {
        get
        {
            return oldPosition.x;
        }
    }

    public bool disabled
    {
        get
        {
            return isDisabled;
        }
        set
        {
            isDisabled = value;
        }
    }

    public TextBox getTextbox
    {
        get
        {
            return textBox;
        }
    }
}