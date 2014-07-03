// Converted from UnityScript to C# at http://www.M2H.nl/files/js_to_c.php - by Mike Hergaarden
// Do test the code! You usually need to change a few small bits.

using UnityEngine;
using System.Collections;
using TouchScript;

public class LevelTrigger : MonoBehaviour {

	private bool  finished = false;
	private bool  lost = false;
	private GameLogic gameLogicScript = null;
	private SoundEngineScript soundEngine = null;
	private bool  notFinished = false;
	private GUIStyle skin = new GUIStyle();
	
	public Texture toMenuWinTexture	= null;
	private Rect winMenuRect;
	public float winMenuX = 0.0f;
	public float winMenuY = 0.0f;
	
	public Texture toMenuLoseTexture = null;
	private Rect loseMenuRect;
	public float loseMenuX = 0.0f;
	public float loseMenuY = 0.0f;
	
	//scales for button positions
	private float originalWidth = 1920.0f;
	private float originalHeight = 1080.0f;
	private Vector3 scale = Vector3.zero;
	
	
	public void  Awake (){
		gameLogicScript = GameObject.Find("GameLogic").GetComponent<GameLogic>() as GameLogic;
		if(GameObject.Find("TextureLoader") != null)
		{
			TextureLoader textureLoader = GameObject.Find("TextureLoader").GetComponent<TextureLoader>() as TextureLoader;
			//get the textures from the texture loader
			toMenuWinTexture = textureLoader.getTexture("WIN - return to menu");
			toMenuLoseTexture = textureLoader.getTexture("LOSE - return to menu");
		}
		if(Application.loadedLevelName == "LevelLoaderScene")
		{
			soundEngine = GameObject.Find("SoundEngine").GetComponent<SoundEngineScript>() as SoundEngineScript;
		}
	}
	
	public void OnEnable (){
		if(TouchManager.Instance != null)
		{
			TouchManager.Instance.TouchesBegan += touchBegan;
		}
	}
	
	public void OnDisable (){
		if(TouchManager.Instance != null)
		{
			TouchManager.Instance.TouchesBegan -= touchBegan;
		}
	}
	
	private void touchBegan ( Object sender ,   TouchEventArgs events  ){
		foreach(var touchPoint in events.Touches)
		{
			Vector2 position = touchPoint.Position;
			position = Vector2(position.x, (position.y - Screen.height)*-1);
			
			isPressingButton(position);
		}
	}
	
	private void isPressingButton ( Vector2 inputXY  ){
		if(finished)
		{
			if (winMenuRect.Contains(inputXY))
			{
				Application.LoadLevel("Menu");
				soundEngine.changeMusic("Menu");
			}
		}
		
		if(lost)
		{
			if (loseMenuRect.Contains(inputXY))
			{
				Application.LoadLevel("Menu");
				soundEngine.changeMusic("Menu");
			}
		}
	}
	
	void OnTriggerEnter ( Collider hit  ){
		if(hit.gameObject.name == "Player")
		{
			if(gameLogicScript.checkWin() == true)
			{
				finished = true;
				gameLogicScript.stopTimer();	
			}
			else
			{
				notFinished = true;
			}
		}
	}
	
	public void OnTriggerExit ( Collider obj ){
		if(obj.gameObject.name == "Player")
		{
			notFinished = false;
		}
	}
	
	void  OnGUI (){
		if(finished)
		{
			gameLogicScript.stopBattery();
			//first scale the buttons before drawing them
			scaleButtons();
			
			//this is the texture of the button
			GUI.DrawTexture(winMenuRect, toMenuWinTexture);
		}	
		if(notFinished)
		{
			GUI.Label(new Rect(Screen.width / 2 - 150, Screen.height / 2, 300, 50), "Je hebt " + gameLogicScript.getCrystalsToComplete().ToString() + "nodig om het level te eindigen");
		}
		if(lost){
			gameLogicScript.stopBattery();
			//first scale the buttons before drawing them
			scaleButtons();
			
			//this is the texture of the button
			GUI.DrawTexture(loseMenuRect, toMenuLoseTexture);
		}	
		
	}
	
	private void scaleButtons (){
		//get the current scale by using the current screen size and the original screen size
		//original width / height is defined in a variable at top, we use an aspect ratio of 16:9 and original screen size of 1920x1080
		scale.x = Screen.width / originalWidth;		//X scale is the current width divided by the original width
		scale.y = Screen.height / originalHeight;	//Y scale is the current height divided by the original height
		
		//first put the rectangles back to its original size before scaling
		winMenuRect 			= new Rect(winMenuX			, winMenuY			, toMenuWinTexture.width			, toMenuWinTexture.height);
		loseMenuRect 			= new Rect(loseMenuX			, loseMenuY			, toMenuLoseTexture.width			, toMenuLoseTexture.height);
		
		//second scale the rectangles
		winMenuRect 			= scaleRect(winMenuRect);
		loseMenuRect 			= scaleRect(loseMenuRect);
	}
	
	private Rect scaleRect ( Rect rect  ){
		Rect newRect = new Rect(rect.x * scale.x, rect.y * scale.y, rect.width * scale.x, rect.height * scale.y);
		return newRect;
	}
	
	void setFinished ( bool isFinished  ){
		finished = isFinished;
	}
	
	bool getFinished (){
		return finished;
	}
	
	void setLost ( bool isLost  ){
		lost = isLost;
	}
	
	bool getLost (){
		return lost;
	}
	
}