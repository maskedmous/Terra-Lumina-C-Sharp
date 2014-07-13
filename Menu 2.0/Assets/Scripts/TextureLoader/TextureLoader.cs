using UnityEngine;
using System.Collections;
using System.IO;

///////////////////////////////////////////////////
//
//
//Written by: Kevin Mettes
//Usage:
//Create a scene, a GameObject, attach the script
//fill in the desired folder path
//fill in the desired fileType what it should search for
//fill in the next scene it should load
//add a loading screen texture
//
//
//Edited from standard for Terra Lumina Project
///////////////////////////////////////////////////



public class TextureLoader : MonoBehaviour
{
    //standard settings
    public string filePath = "/FolderName";	    //filePath of the textures
    public string fileType = "*.png";			//* means all names possible  .png means files with the png extention only
    public string nextScene = "someScene";		//next scene
    public Texture2D loadingScreen = null;		//texture2D loadingscreen
    public int loadedLabelWidth = 100;			//width and height for the % loaded
    public int loadedLabelHeight = 100;
    public int fontSize = 18;				    //font size for the loading %

    //private variables not to be touched
    private int fileCount = 0;				            //the amount of files that are to be loaded
    private int loadedFiles = 0;				        //the amount that is loaded at this point
    private ArrayList textureArray = new ArrayList();	//array where we put all textures in
    private GUIStyle guiStyle = new GUIStyle();	        //style for the loading % in a different font
    private bool loaded = false;			            //bool  to see if it is loaded or not
    private bool validPath = false;                     //seperate bools for valid / invalid path
    private bool invalidPath = false;

    private float dotTimer = 1.0f;						//timer for going to the new dot
    public Texture2D dotTexture = null;					//texture for the dot
    public float dotX = 0.0f;							//position of the initial dot
    public float dotY = 0.0f;
    private bool firstDot = true;					    //booleans to show which dot at loading screen
    private bool secondDot = false;
    private bool thirdDot = false;

    private Vector2 scale = new Vector2();				//scale for texture scaling 16:9
    private float originalWidth = 1920.0f;				//original width and height of a 16:9 ratio
    private float originalHeight = 1080.0f;

    private int screenshotCount = 0;

    
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
            //check if it is a valid path though
            validPath = checkValidFilePath(filePath);

            //check if the filePath is initialized properly
            if (filePath != Application.dataPath + "/FolderName" && nextScene != "someScene")	//don't change this its a check, fill in the public variables instead
            {
                Debug.Log("Loading Textures...");	//showing that it'll load textures
                StartCoroutine(fillTextureArray());	//calling the function to fill the array
            }
            else
            {
                Debug.LogError("You probably forgot to set either the foldername or nextscene");    //error if something goes wrong, failsaves
            }
        }
        else
        {
            //destroy this because it exists already, failsave because this scene should not be loaded multiple times
            Destroy(this.gameObject);
        }
    }

    public void Update()
    {
        //take a screenshot
        if (Input.GetKeyDown(KeyCode.F9))
        {
            string fileName = "Screenshot" + screenshotCount + ".png";
            Application.CaptureScreenshot(fileName);
            screenshotCount++;
        }
    }

    //fixes the pathing with some characters that WWW does not understand.
    //URL encoding did not work so doing this manually which will not exclude every possibility but still the major ones
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
    //check if it has a valid path without the WWW-nonunderstandable characters
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

            if (!validPath) path = fixPath(file);  //it is detected that the path is not valid so try fixing it before using it in the WWW else it'll error

            //download the file via WWW
            WWW wwwTexture = new WWW("file:///" + path);
            //wait for it to download
            yield return wwwTexture;
            if (!string.IsNullOrEmpty(wwwTexture.error)) invalidPath = true;    //if it is still invalid after fixing set invalidPath to true this means the game has to be moved to a valid path like Program Files
            //get the texture data out of the WWW download
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