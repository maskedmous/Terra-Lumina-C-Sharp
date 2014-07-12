using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SwitchGraphicTexture : MonoBehaviour
{
    public string switchGaphicName = "";
    public float x = 0.0f;              //x position of the graphic
    public float y = 0.0f;              //y position

    public string[] textureNames = null;     //name of the texture that will be loaded
    private Texture2D currentTexture = null;
    private List<Texture2D> textures = new List<Texture2D>();   //texture itself

    private int iterateThrough = 0;

    private Rect textureRect;

    private Vector2 scale = new Vector2();
    public float originalWidth = 1920.0f; //original designed width
    public float originalHeight = 1080.0f; //original designed height

    public bool isEnabled = true;

    private Menu menu = null;
    public int layerID = 3; //second layer

    public void Awake()
    {
        menu = GameObject.Find("Menu").GetComponent<Menu>();
        initializeTexture();
    }

    private void initializeTexture()
    {
        TextureLoader textureLoader = null;

        if (textureLoader = GameObject.Find("TextureLoader").GetComponent<TextureLoader>())
        {
            foreach (string nameOfTexture in textureNames)
            {
                Texture2D aTexture = textureLoader.getTexture(nameOfTexture);
                if (aTexture != null) textures.Add(aTexture);
            }
        }
    }
    
    public void increaseIteration()
    {
        iterateThrough++;
        if (iterateThrough >= textures.Count) iterateThrough = 0;
    }

    public void decreaseIteration()
    {
        iterateThrough--;
        if (iterateThrough <= -1) iterateThrough = textures.Count - 1;
    }

    public void Update()
    {
        if (currentTexture != textures[iterateThrough]) currentTexture = textures[iterateThrough];
    }

    public void OnGUI()
    {
        if (isEnabled)
        {
            updateTexture();
            GUI.depth = layerID;

            if (!menu.isHeimBuild && currentTexture != null)
            {
                GUI.DrawTexture(textureRect, currentTexture);
            }
        }
    }

    private void updateTexture()
    {
        if (currentTexture != null)
        {
            scale = new Vector2(Screen.width / originalWidth, Screen.height / originalHeight);
            textureRect = new Rect(x * scale.x, y * scale.y, currentTexture.width * scale.x, currentTexture.height * scale.y);
        }
    }

    public string nameOfSwitchGraphic
    {
        get
        {
            return switchGaphicName;
        }
    }
}
