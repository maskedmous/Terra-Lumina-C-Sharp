using UnityEngine;
using System.Collections;
using System.IO;

///////////////////////////////////////////////////
//
//
//Written by: Kevin Mettes
//Usage:
//Create a scene, a GameObject, attach the script
//fill in the desired filePath
//fill in the desired fileType what it should search for
//fill in the next scene it should load
//add a loading screen texture
//
//
///////////////////////////////////////////////////



public class TextureLoader : MonoBehaviour
{
    //settings
    public string filePath = "/FolderName";	//filePath of the textures
    public string fileType = "*.png";			//* means all names possible  .png means files with the png extention only
    public string nextScene = "someScene";		//next scene
    public Texture2D loadingScreen = null;				//texture2D loadingscreen
    public int loadedLabelWidth = 100;				//width and height for the % loaded
    public int loadedLabelHeight = 100;
    public int fontSize = 18;				//font size for the loading %

    //private variables not to be touched
    private int fileCount = 0;				//the amount of files that are to be loaded
    private int loadedFiles = 0;				//the amount that is loaded at this point
    private ArrayList textureArray = new ArrayList();		//array where we put all textures in
    private GUIStyle guiStyle = new GUIStyle();	//style for the loading % in a different font
    private bool loaded = false;			//bool  to see if it is loaded or not
    private bool validPath = false;

    private float dotTimer = 1.0f;						//timer for going to the new dot
    public Texture2D dotTexture = null;					//texture for the dot
    public float dotX = 0.0f;							//position of the initial dot
    public float dotY = 0.0f;
    private bool firstDot = true;					//booleans to show which dot
    private bool secondDot = false;
    private bool thirdDot = false;

    private Vector2 scale = new Vector2();				//scale for texture scaling 16:9
    private float originalWidth = 1920.0f;					//original width and height of a 16:9 ratio
    private float originalHeight = 1080.0f;

    private bool invalidPath = false;
    //static variables not to be touched
    static bool LoaderExists = false;				//static to check if the loader exists already or not

    public void Awake()
    {
        //we don't want the texture loader to be destroyed
        DontDestroyOnLoad(this.gameObject);

        //make sure it doesn't exist already
        if (LoaderExists == false)
        {
            LoaderExists = true;				//putting the loader exists on true

            guiStyle.font = (Font)Resources.Load("Fonts/sofachrome rg", typeof(Font));		//loading the font

            Color textColor = guiStyle.normal.textColor;

            textColor.b = 197.0f / 255.0f;						//setting the color of the font
            textColor.g = 185.0f / 255.0f;
            textColor.r = 147.0f / 255.0f;
            guiStyle.normal.textColor = textColor;

            guiStyle.fontSize = fontSize;	//setting the size of the font

            //the filePath of the textures you want to load(use the root folder, it searches through all subfolders)
            filePath = Application.dataPath + filePath;
            validPath = checkValidFilePath(filePath);

            //check if the filePath is initialized properly
            if (filePath != Application.dataPath + "/FolderName" && nextScene != "someScene")	//don't change this its a check, fill in the public variables instead
            {
                Debug.Log("Loading Textures...");	//showing that it'll load textures
                StartCoroutine(fillTextureArray());					//calling the function to fill the array
            }
            else
            {
                Debug.LogError("You probably forgot to set either the foldername or nextscene");
            }
        }
        else
        {
            //destroy this because it exists already
            Destroy(this.gameObject);
        }
    }

    private string fixPath(string path)
    {
        if (path.Contains("ä")) path = path.Replace("ä", "%E4");
        if (path.Contains("ë")) path = path.Replace("ë", "%EB");
        if (path.Contains("ö")) path = path.Replace("ö", "%F6");
        if (path.Contains("ü")) path = path.Replace("ü", "%FC");
        if (path.Contains("ï")) path = path.Replace("ï", "%EF");
        if (path.Contains("#")) path = path.Replace("#", "%23");
        return path;
    }

    private bool checkValidFilePath(string path)
    {
        if (path.Contains("ä") || path.Contains("ë") || path.Contains("ö") || path.Contains("ü") || path.Contains("ï") || path.Contains("#")) return false;
        return true;
    }

    //checking the dot which dot to show first second or third
    private void checkDot()
    {
        dotTimer -= Time.deltaTime;

        if (dotTimer <= 0.0f)
        {
            dotTimer = 1.0f;

            if (firstDot)
            {
                firstDot = false;
                secondDot = true;
                return;
            }
            else if (secondDot)
            {
                secondDot = false;
                thirdDot = true;
                return;
            }
            else if (thirdDot)
            {
                thirdDot = false;
                firstDot = true;
                return;
            }
        }
    }

    public void OnGUI()
    {
        if (!loaded)
        {
            //scale every OnGUI call so there won't be isues resizing the window in a 16:9 aspect ratio
            scale = new Vector2(Screen.width / originalWidth, Screen.height / originalHeight);

            //draw the loading screen
            if (loadingScreen != null)
            {
                GUI.DrawTexture(new Rect(0.0f, 0.0f, Screen.width, Screen.height), loadingScreen);
            }

            //check which dot to show
            checkDot();
            //draw the dot on the screen
            if (firstDot)
            {
                GUI.DrawTexture(new Rect(dotX * scale.x, dotY * scale.y, dotTexture.width * scale.x, dotTexture.height * scale.y), dotTexture);
            }
            else if (secondDot)
            {
                GUI.DrawTexture(new Rect((dotX + 50) * scale.x, dotY * scale.y, dotTexture.width * scale.x, dotTexture.height * scale.y), dotTexture);
            }
            else if (thirdDot)
            {
                GUI.DrawTexture(new Rect((dotX + 100) * scale.x, dotY * scale.y, dotTexture.width * scale.x, dotTexture.height * scale.y), dotTexture);
            }
            //if it is still loading, update the progress
            if (isLoading())
            {
                GUI.Label(new Rect(Screen.width / 2, Screen.height / 2 + (Screen.height * 5 / 16), loadedLabelWidth, loadedLabelHeight), percentLoaded() + "%", guiStyle);
            }
        }

        if (invalidPath) GUI.Label(new Rect(0, 0, Screen.width, 200), "The filepath you installed the game is invalid!\nplease move it to Program Files or Program Files(x86)", guiStyle);
    }

    //Fills the arrays with the textures
    private IEnumerator fillTextureArray()
    {
        //getting all files at the desired filePath with the desired fileType and searching through all directories
        string[] fileInfo = Directory.GetFiles(filePath, fileType, SearchOption.AllDirectories);

        fileCount = fileInfo.Length;

        //checks if valid path if not fix it


        foreach (string file in fileInfo)
        {
            string path = file;

            if (!validPath) path = fixPath(file);               //it is detected that the path is not valid so try fixing it before using it in the WWW

            //download the file via WWW
            WWW wwwTexture = new WWW("file:///" + path);
            //wait for it to download
            yield return wwwTexture;
            if (!string.IsNullOrEmpty(wwwTexture.error)) invalidPath = true;
            Texture2D texture = wwwTexture.texture;
            //assign the name to the texture
            string textureName = Path.GetFileNameWithoutExtension(file);
            texture.name = textureName;
            //put the texture into the array
            textureArray.Add(texture);
            //loaded files + 1
            loadedFiles++;
        }
        loaded = true; 						// it is loaded up so loaded is true
        Debug.Log("Done loading textures");	//giving the msg that it is done loading
        if (!invalidPath) Application.LoadLevel(nextScene);	//its done loading so load the next scene
    }

    //returns bool  if it is still loading files
    public bool isLoading()
    {
        if (countedFiles() == currentLoaded())
        {
            return false;
        }

        return true;
    }

    //calculating the progress of loading files
    public string percentLoaded()
    {
        float part = currentLoaded();
        float total = countedFiles();
        float percentage = (part / total) * 100.0f;
        percentage = Mathf.Round(percentage);
        return percentage.ToString();
    }

    //returns the amount of files that are to be loaded
    public int countedFiles()
    {
        return fileCount;
    }

    //returns the amount of files that are loaded currently
    public int currentLoaded()
    {
        return loadedFiles;
    }

    //load additional textures into the array if the path is different
    public IEnumerator loadTextureInArray(string aFilePath, string textureName)
    {
        //searching for the specific texture
        string[] fileInfo = Directory.GetFiles(aFilePath, textureName + fileType, SearchOption.AllDirectories);
        //if the texture is found then the length would be 1 if not found it would be 0
        //so if it is not 0 continue
        if (fileInfo.Length != 0)
        {
            foreach (string file in fileInfo)
            {
                //if the texture exists already in the array then you have doubles, conflicting
                if (textureExistsAlready(textureName) == false)
                {
                    //download the texture
                    WWW wwwTexture = new WWW("file://" + file);
                    //wait for it to be done
                    yield return wwwTexture;
                    //add it to the array
                    Texture2D texture = wwwTexture.texture;
                    texture.name = textureName;
                    textureArray.Add(texture);
                }
                else
                {
                    //you tried to add a texture that was already added previously
                    Debug.LogError("You tried to add a texture but the texture was already in there. Texture: " + textureName);
                }
            }
        }
        else
        {
            Debug.LogError("You tried to use loadTexture but the texture was not found. Texture: " + textureName);
        }
    }

    //getter for textures
    public Texture2D getTexture(string textureName)
    {
        //go through the textureNameArray with the given textureName
        foreach (Texture2D checkTexture in textureArray)
        {
            //if the names match
            if (checkTexture.name == textureName)
            {
                //texture was found so return it
                return checkTexture;
            }
        }
        //texture hasn't been return yet which means it is not found
        Debug.LogError("Texture not found using getTexture: " + textureName);
        return null;
    }

    //checks if the texture is already in the textureArray
    private bool textureExistsAlready(string textureName)
    {
        foreach (Texture2D texture in textureArray)
        {
            //if this is true then it is already in the array
            if (texture.name == textureName)
            {
                //the texture is already in the array return true
                return true;
            }
        }
        //the texture does not exist in the array yet, return false
        return false;
    }
}