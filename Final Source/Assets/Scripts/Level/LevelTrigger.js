#pragma strict

import TouchScript;

private var finished:boolean = false;
private var lost:boolean = false;
private var gameLogicScript:GameLogic = null;
private var soundEngine:SoundEngineScript = null;
private var notFinished:boolean = false;
private var skin:GUIStyle = new GUIStyle();

public var toMenuWinTexture:Texture	= null;
private var winMenuRect:Rect;
public var winMenuX:float = 0.0f;
public var winMenuY:float = 0.0f;

public var toMenuLoseTexture:Texture = null;
private var loseMenuRect:Rect;
public var loseMenuX:float = 0.0f;
public var loseMenuY:float = 0.0f;

//scales for button positions
private var originalWidth 	:float = 1920.0f;
private var originalHeight	:float = 1080.0f;
private var scale			:Vector3 = Vector3.zero;


public function Awake()
{
	gameLogicScript = GameObject.Find("GameLogic").GetComponent(GameLogic) as GameLogic;
	if(GameObject.Find("TextureLoader") != null)
	{
		var textureLoader:TextureLoader = GameObject.Find("TextureLoader").GetComponent(TextureLoader) as TextureLoader;
		//get the textures from the texture loader
		toMenuWinTexture = textureLoader.getTexture("WIN - return to menu");
		toMenuLoseTexture = textureLoader.getTexture("LOSE - return to menu");
	}
	if(Application.loadedLevelName == "LevelLoaderScene")
	{
		soundEngine = GameObject.Find("SoundEngine").GetComponent(SoundEngineScript) as SoundEngineScript;
	}
}

public function OnEnable():void
{
	if(TouchManager.Instance != null)
	{
		TouchManager.Instance.TouchesBegan += touchBegan;
	}
}

public function OnDisable():void
{
	if(TouchManager.Instance != null)
	{
		TouchManager.Instance.TouchesBegan -= touchBegan;
	}
}

private function touchBegan(sender:Object, events:TouchEventArgs):void
{
	for each(var touchPoint in events.Touches)
	{
		var position:Vector2 = touchPoint.Position;
		position = Vector2(position.x, (position.y - Screen.height)*-1);
		
		isPressingButton(position);
	}
}

private function isPressingButton(inputXY:Vector2):void
{
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

function OnTriggerEnter(hit:Collider):void {
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

public function OnTriggerExit(object:Collider):void
{
	if(object.gameObject.name == "Player")
	{
		notFinished = false;
	}
}

function OnGUI()
{
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

private function scaleButtons():void
{
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

private function scaleRect(rect:Rect):Rect
{
	var newRect:Rect = new Rect(rect.x * scale.x, rect.y * scale.y, rect.width * scale.x, rect.height * scale.y);
	return newRect;
}

function setFinished(isFinished:boolean):void {
	finished = isFinished;
}

function getFinished():boolean {
	return finished;
}

function setLost(isLost:boolean):void {
	lost = isLost;
}

function getLost():boolean {
	return lost;
}
