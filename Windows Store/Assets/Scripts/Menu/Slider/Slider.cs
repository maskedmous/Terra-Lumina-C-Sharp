using UnityEngine;
using System.Collections;

public class Slider : MonoBehaviour
{
    //debug slider
    public bool debugMode = false;
    public Texture2D debugDot = null;
    private Rect debugDotLeftRect;
    private DebugDotGraphic debugDotLeftGraphic = null;
    private Rect debugDotRightRect;
    private DebugDotGraphic debugDotRightGraphic = null;

    //background
    public float xBackground = 0.0f;
    public float yBackground = 0.0f;
    public string sliderBackgroundName = "";
    private Texture2D sliderBackgroundTexture = null;
    private Rect sliderBackgroundRect;
    //button
    public float xButton = 0.0f;
    public float yButton = 0.0f;
    public string sliderButtonName = "";
    private Texture2D sliderButtonTexture = null;
    private Rect sliderButtonRect;

    private float minX = 0.0f;
    public float offsetMinX = 0.0f;
    private float maxX = 0.0f;
    public float offsetMaxX = 0.0f;

    public bool isEnabled = true;

    public float originalWidth = 1920.0f;
    public float originalHeight = 1080.0f;
    private Vector2 scale = new Vector2();

    public string sliderBehaviourName = "";
    private SliderBehaviour sliderBehaviour = null;
    public int layerID = 0;

    public void Awake()
    {
        initializeSlider();
    }

    private void initializeSlider()
    {
        TextureLoader textureLoader = null;

        if (textureLoader = GameObject.Find("TextureLoader").GetComponent<TextureLoader>())
        {
            if (sliderBackgroundName != "") sliderBackgroundTexture = textureLoader.getTexture(sliderBackgroundName);
            if (sliderButtonName != "") sliderButtonTexture = textureLoader.getTexture(sliderButtonName);
        }
        else
        {

        }

        if (sliderBackgroundTexture != null)
        {
            minX = xBackground + offsetMinX;
            maxX = xBackground + sliderBackgroundTexture.width - offsetMaxX;
        }

        //if the slider behaviour is not empty named
        if (sliderBehaviourName != "")
        {
            sliderBehaviour = (SliderBehaviour)this.gameObject.AddComponent(sliderBehaviourName) as SliderBehaviour;
            sliderBehaviour.initializeSlider(this);
        }
        else Debug.LogError("Forgot to put in slider behaviour name");

        //debug dot layer
        int debugLayer = layerID - 1;
        if (debugLayer == -1) debugLayer = 0;
        //left side
        debugDotLeftGraphic = this.gameObject.AddComponent<DebugDotGraphic>();
        debugDotLeftGraphic.initialize(debugDot, debugLayer);

        //right side
        debugDotRightGraphic = this.gameObject.AddComponent<DebugDotGraphic>();
        debugDotRightGraphic.initialize(debugDot, debugLayer);
    }

    public void setPosition(float position)
    {
        position = Mathf.Clamp(position, 0.0f, 1.0f);
        
        xButton = minRange + (position * (maxRange - minRange));
        if (xButton > maxRange) xButton = maxRange;
        else if (xButton < minRange) xButton = minRange;
    }

    public bool isTouched(Vector2 input)
    {
        if (sliderBackgroundRect.Contains(input))
        {
            if (sliderBehaviour != null) sliderBehaviour.executeSliderFunction(input);
            return true;
        }
        return false;
    }

    public bool isStillTouching(Vector2 input)
    {
        if (sliderBackgroundRect.Contains(input))
        {
            if (sliderBehaviour != null) sliderBehaviour.executeSliderFunction(input);
            return true;
        }
        return false;
    }

    public bool isTouchReleased(Vector2 input)
    {
        if (sliderBackgroundRect.Contains(input))
        {
            if (sliderBehaviour != null) sliderBehaviour.executeSliderFunction(input);
            return true;
        }
        return false;
    }

    public Vector2 sliderScale
    {
        get
        {
            return scale;
        }
        set
        {
            scale = value;
        }
    }

    public float sliderButtonX
    {
        get
        {
            return xButton;
        }
        set
        {
            xButton = value;
        }
    }

    public float sliderWidth
    {
        get
        {
            return sliderButtonTexture.width;
        }
    }

    public float percentageOfSlider()
    {
        float percentage = 0.0f;

        float percentScale = (maxRange - minRange) / 100;
        percentage = ((xButton - minRange) / percentScale) / 100;

        percentage = Mathf.Clamp(percentage, 0.0f, 1.0f);

        //return the number
        return percentage;
    }

    public void OnGUI()
    {
        if (isEnabled)
        {

            //check if they are both not null
            if (sliderBackgroundTexture != null && sliderButtonTexture != null)
            {
                //update the slider first before drawing (scaling)
                updateSlider();

                //set the layer
                GUI.depth = layerID;

                //draw the slider
                GUI.DrawTexture(sliderBackgroundRect, sliderBackgroundTexture);
                GUI.DrawTexture(sliderButtonRect, sliderButtonTexture);

                if (debugMode)
                {
                    GUI.DrawTexture(debugDotLeftRect, debugDot);
                    GUI.DrawTexture(debugDotRightRect, debugDot);
                }
            }
        }
    }

    private void updateSlider()
    {
        scale = new Vector2(Screen.width / originalWidth, Screen.height / originalHeight);

        minX = xBackground + offsetMinX;
        maxX = xBackground + sliderBackgroundTexture.width - offsetMaxX;

        sliderBackgroundRect = new Rect(xBackground * scale.x, yBackground * scale.y, sliderBackgroundTexture.width * scale.x, sliderBackgroundTexture.height * scale.y);
        sliderButtonRect = new Rect(xButton * scale.x, yButton * scale.y, sliderButtonTexture.width * scale.x, sliderButtonTexture.height * scale.y);

        if (debugDot != null && debugDotLeftGraphic != null)
        {
            //minimal side
            debugDotLeftRect = new Rect((minX - (debugDot.width / 2.0f)) * scale.x, sliderBackgroundRect.y + (sliderBackgroundRect.height / 2.0f) - (debugDot.height / 2.0f * scale.y), debugDot.width * scale.x, debugDot.height * scale.y);
            debugDotLeftGraphic.updateRect(debugDotLeftRect);

            //maximum side
            debugDotRightRect = new Rect((maxX - (debugDot.width / 2.0f)) * scale.x, sliderBackgroundRect.y + (sliderBackgroundRect.height / 2.0f) - (debugDot.height / 2.0f * scale.y), debugDot.width * scale.x, debugDot.height * scale.y);
            debugDotRightGraphic.updateRect(debugDotRightRect);
        }
        if (debugDotLeftGraphic.debug && !debugMode) debugDotLeftGraphic.debug = debugMode;
        else if (!debugDotLeftGraphic.debug && debugMode) debugDotLeftGraphic.debug = debugMode;
        if (debugDotRightGraphic.debug && !debugMode) debugDotRightGraphic.debug = debugMode;
        else if (!debugDotRightGraphic.debug && debugMode) debugDotRightGraphic.debug = debugMode;
    }

    public float minRange
    {
        get
        {
            return minX - (sliderButtonTexture.width / 2);
        }
    }

    public float maxRange
    {
        get
        {
            return maxX - (sliderButtonTexture.width / 2);
        }
    }
}
