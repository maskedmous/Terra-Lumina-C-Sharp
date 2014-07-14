using UnityEngine;
using System.Collections;

public class GraphicTexture : MonoBehaviour
{
    public string graphicTextureName = "";  //name it for use in scripts
    public float x = 0.0f;              //x position of the graphic
    public float y = 0.0f;              //y position

    public string textureName = "";     //name of the texture that will be loaded
    private Texture2D texture = null;   //texture itself
    private Rect textureRect;

    private Vector2 scale = new Vector2();
    public float originalWidth = 1920.0f; //original designed width
    public float originalHeight = 1080.0f; //original designed height
    public bool isEnabled = true;
    public int layerID = 3; //third layer


    public void Awake()
    {
        initializeTexture();
    }

    private void initializeTexture()
    {
        TextureLoader textureLoader = null;

        if (textureLoader = GameObject.Find("TextureLoader").GetComponent<TextureLoader>())
        {
            if (textureName != "") texture = textureLoader.getTexture(textureName);
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
                
            if (texture != null) GUI.DrawTexture(textureRect, texture);
        }
    }

    private void updateTexture()
    {
        if (texture != null)
        {
            scale = new Vector2(Screen.width / originalWidth, Screen.height / originalHeight);
            if (texture != null) textureRect = new Rect(x * scale.x, y * scale.y, texture.width * scale.x, texture.height * scale.y);
        }
    }

    public string nameOfGraphicTexture
    {
        get
        {
            return graphicTextureName;
        }
    }
}
