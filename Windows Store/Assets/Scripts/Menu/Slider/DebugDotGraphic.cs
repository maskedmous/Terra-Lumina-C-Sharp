using UnityEngine;
using System.Collections;

public class DebugDotGraphic : MonoBehaviour
{
    private Texture2D debugTexture = null;
    private Rect debugRect = new Rect(0.0f, 0.0f, 0.0f, 0.0f);
    private int layerID = 0;
    private bool isEnabled = false;

	public void initialize(Texture2D debugTex, int layer)
    {
        debugTexture = debugTex;
        layerID = layer;
    }

    public void updateRect(Rect aRect)
    {
        debugRect = aRect;
    }
    
    public bool debug
    {
        set
        {
            isEnabled = value;
        }
        get
        {
            return isEnabled;
        }
    }

    public void OnGUI()
    {
        if (isEnabled)
        {
            GUI.depth = layerID;
            if (debugTexture != null)
            {
                GUI.DrawTexture(debugRect, debugTexture);
            }
        }
    }
}
