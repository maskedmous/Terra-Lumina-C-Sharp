using UnityEngine;
using System.Collections;

public class GraphicTexture : MonoBehaviour
{
    public float x = 0.0f;              //x position of the graphic
    public float y = 0.0f;              //y position

    public string textureName = "";     //name of the texture that will be loaded
    private Texture2D texture = null;   //texture itself
    private Rect textureRect;

    public string heimTextureName = ""; //heim texture name
    private Texture2D heimTexture = null;   //texture might be different from the normal build
    private Rect heimTextureRect;

    private Vector2 scale = new Vector2();
    public float originalWidth = 1920.0f; //original designed width
    public float originalHeight = 1080.0f; //original designed height
    public bool isEnabled = true;
    public bool heim = false;
    private Menu menu = null;
    public int layerID = 3; //third layer


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
            if (textureName != "") texture = textureLoader.getTexture(textureName);
            if (heimTextureName != "")
            {
                heimTexture = textureLoader.getTexture(heimTextureName);
            }
        }
        else
        {
            texture = Resources.Load("Textures/" + textureName) as Texture2D;
        }
    }

    public void OnGUI()
    {
        if (isEnabled)
        {
            updateTexture();
            GUI.depth = layerID;

            //if it is a heim build and heim texture it should draw
            if (menu.isHeimBuild && !heim)
            {
                //don't draw
            }
            else if (menu.isHeimBuild && heim)
            {
                //heim texture
                if (heimTexture != null) GUI.DrawTexture(heimTextureRect, heimTexture);
            }
            else
            {
                //draw everything because it is not a heim build
                if (texture != null) GUI.DrawTexture(textureRect, texture);
            }
        }
    }

    private void updateTexture()
    {
        if (texture != null || heimTexture != null)
        {
            scale = new Vector2(Screen.width / originalWidth, Screen.height / originalHeight);
            if (texture != null) textureRect = new Rect(x * scale.x, y * scale.y, texture.width * scale.x, texture.height * scale.y);
            if (heimTexture != null) heimTextureRect = new Rect(x * scale.x, y * scale.y, heimTexture.width * scale.x, heimTexture.height * scale.y);
        }
    }
}
