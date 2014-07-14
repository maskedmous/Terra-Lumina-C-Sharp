using UnityEngine;
using System.Collections;

public class TextBox : MonoBehaviour
{
    public string textBoxName = ""; //for scripts
    public string text = "";
    public float xPosition = 0.0f;
    public float yPosition = 0.0f;
    public float width = 0.0f;
    public float height = 0.0f;
    public int originalFontSize = 16;
    public bool isEnabled = true;
    public int layerID = 1;

    private Vector2 scale = new Vector2();
    private Menu menu = null;
    private GUIStyle customFont = null;

    public void Awake()
    {
        menu = GameObject.Find("Menu").GetComponent<Menu>();
        customFont = menu.getCustomFont;
    }
	public void OnGUI()
    {
        if (isEnabled)
        {
            GUI.depth = layerID;
            //scale the font and label
            scale = new Vector2(Screen.width / 1920.0f, Screen.height / 1080.0f);
            customFont.fontSize = Mathf.RoundToInt(originalFontSize * scale.x);
            //draw the label
            GUI.Label(new Rect(xPosition * scale.x, yPosition * scale.y, width * scale.x, height * scale.x), text, customFont);
        }
    }
	public virtual void Update ()
	{
	    //update for if extending
	}

    public string textBoxText
    {
        get
        {
            return text;
        }
        set
        {
            text = value;
        }
    }
}
