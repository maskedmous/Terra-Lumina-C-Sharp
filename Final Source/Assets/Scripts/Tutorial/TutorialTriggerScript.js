#pragma strict

//gamelogic
private var gameLogic:GameLogic = null;

private var playerInput:PlayerInputScript 	= null;	//player input to change controls
private var soundEngine:SoundEngineScript 	= null;	//sound engine to play sounds
private var timePassed:float	= 0.0f;				//count the time has passed during player visit

//movement and button availability
public var movementLeftEnabled				:boolean = true;	//default movement is true
public var movementRightEnabled				:boolean = true;
public var jumpButtonEnabled				:boolean = false;	//other controls that are false will be disabled upon entering
public var flashButtonEnabled				:boolean = false;
public var shootNormalShroomButtonEnabled	:boolean = false;
public var shootBumpyShroomButtonEnabled	:boolean = false;

//alpha GameObject
public var alphaObject:GameObject = null;						//an object that is transparent

private var blinkingTime:float = 4.0f;							//blinking time of the buttons
//tutorial kind
public var jumpButtonTutorial			:boolean = false;		//kinds of tutorials that will have special functions
public var normalShroomButtonTutorial	:boolean = false;
public var flashButtonTutorial			:boolean = false;
public var bumpyShroomButtonTutorial	:boolean = false;
//special tutorials with barriers (block objects)
public var lightTutorial				:boolean 	= false;
public var slugTutorial					:boolean 	= false;
public var crystalTutorial				:boolean 	= false;

//slug tutorial
public var slugObject:GameObject = null;	//the slug for the slug tutorial

//block object
public var blockObject:GameObject = null;	//the barrier that will break down for some tutorials

//scaling of the textures
private var scale:Vector2;
private var originalWidth	:float = 1920.0f;
private var originalHeight	:float = 1080.0f;
//texture for the tutorial to show
private var showTutorialTextures:boolean = false;
//tutorial texture A
private var tutorialTextureARect:Rect;				//position and texture placed by the creative
public var tutorialTextureA	:Texture2D 	= null;
public var xPositionTexA	:float 		= 0.0f;
public var yPositionTexA	:float 		= 0.0f;
public var timerTexA		:float		= -1.0f;	//timer of the texture that it'll go away
//tutorial texture B
private var tutorialTextureBRect:Rect;
public var tutorialTextureB	:Texture2D 	= null;
public var xPositionTexB	:float 		= 0;
public var yPositionTexB	:float 		= 0;
public var timerTexB		:float		= -1;

//destroy this tutorial object on exit
public var destroyOnExit:boolean = false;
public var destroyOnCompletion:boolean = false;

private var cameraMoving:boolean = false;

public function Awake():void
{
	playerInput = GameObject.Find("Player").GetComponent(PlayerInputScript) as PlayerInputScript;
	gameLogic = GameObject.Find("GameLogic").GetComponent(GameLogic) as GameLogic;
	
	if(Application.loadedLevelName == "LevelLoaderScene")
	{
		soundEngine = GameObject.Find("SoundEngine").GetComponent(SoundEngineScript) as SoundEngineScript;
	}
}

public function Start ():void
{
	//left is normally always enabled
	if (!movementLeftEnabled)
	{
		cameraMoving = true;								//camera should move true
		this.gameObject.AddComponent("CameraStartScript");	//add the component required for the intro
	}
}

//scaling the textures to show
private function scaleTextures():void
{
	//get the current scale by using the current screen size and the original screen size
	//original width / height is defined in a variable at top, we use an aspect ratio of 16:9 and original screen size of 1920x1080
	scale.x = Screen.width / originalWidth;		//X scale is the current width divided by the original width
	scale.y = Screen.height / originalHeight;	//Y scale is the current height divided by the original height
	
	if(tutorialTextureA != null)
	{
		tutorialTextureARect = new Rect(xPositionTexA, yPositionTexA, tutorialTextureA.width, tutorialTextureA.height);
		tutorialTextureARect = scaleRect(tutorialTextureARect);
	}
	if(tutorialTextureB != null)
	{
		tutorialTextureBRect = new Rect(xPositionTexB, yPositionTexB, tutorialTextureB.width, tutorialTextureB.height);
		tutorialTextureBRect = scaleRect(tutorialTextureBRect);
	}
}
//scaling the rectangle
private function scaleRect(rect:Rect):Rect
{
	var newRect:Rect = new Rect(rect.x * scale.x, rect.y * scale.y, rect.width * scale.x, rect.height * scale.y);
	return newRect;
}
//show the textures on screen
public function OnGUI():void
{
	if(showTutorialTextures && !cameraMoving)
	{
		scaleTextures();
		
		if(tutorialTextureA != null)
		{
			GUI.DrawTexture(tutorialTextureARect, tutorialTextureA);
		}
		if(tutorialTextureB != null)
		{
			GUI.DrawTexture(tutorialTextureBRect, tutorialTextureB);
		}
	}
}

//enables or disables controls called by onTriggerEnter
private function changeControls():void
{
	if(!movementLeftEnabled) playerInput.setMovementLeftEnabled(false);
	else 					 playerInput.setMovementLeftEnabled(true);
	
	if(!movementRightEnabled) playerInput.setMovementRightEnabled(false);
	else 					  playerInput.setMovementRightEnabled(true);
	
	if(!jumpButtonEnabled) playerInput.setJumpButtonEnabled(false);
	else 				   playerInput.setJumpButtonEnabled(true);
	
	if(!flashButtonEnabled) playerInput.setFlashButtonEnabled(false);
	else					playerInput.setFlashButtonEnabled(true);
	
	if(!shootNormalShroomButtonEnabled) playerInput.setNormalShroomButtonEnabled(false);
	else 								playerInput.setNormalShroomButtonEnabled(true);
	
	if(!shootBumpyShroomButtonEnabled) playerInput.setBumpyShroomButtonEnabled(false);
	else 							   playerInput.setBumpyShroomButtonEnabled(true);
}

//blink the buttons
private function blinkButtons():void
{
	if(jumpButtonTutorial) playerInput.setBlinkingJumpButton(true);
	else if(normalShroomButtonTutorial) playerInput.setBlinkingNormalShroomButton(true);
	else if(flashButtonTutorial) playerInput.setBlinkingFlashButton(true);
	else if(bumpyShroomButtonTutorial) playerInput.setBlinkingBumpyShroomButton(true);
}

//reset the blinking buttons
private function resetBlinkingButtons():void
{
	if(jumpButtonTutorial) playerInput.setBlinkingJumpButton(false);
	else if(normalShroomButtonTutorial) playerInput.setBlinkingNormalShroomButton(false);
	else if(flashButtonTutorial) playerInput.setBlinkingFlashButton(false);
	else if(bumpyShroomButtonTutorial) playerInput.setBlinkingBumpyShroomButton(false);
}

private function playAnimation():IEnumerator
{
	if(blockObject != null)
	{
		var animation:Animation = blockObject.GetComponent(Animation);
		animation.Play();
		if(soundEngine != null)
		{
			soundEngine.playSoundEffect("rock");
		}
		var particleSystem:ParticleSystem = blockObject.transform.FindChild("rockDust").GetComponent(ParticleSystem);
		particleSystem.Play();
		
		yield WaitForSeconds(animation.GetClip("Take 001").length);
		Destroy(blockObject);
		
		if(destroyOnCompletion)
		{
			Destroy(this.gameObject);
		}
	}
}

//on trigger enter it should change controls and maybe blink buttons if it is a tutorial
public function OnTriggerEnter(collider:Collider):void
{
	//change the controls upon entering the trigger box
	if(collider.gameObject.name == "Player")
	{
		changeControls();
		blinkButtons();
		if(tutorialTextureA != null || tutorialTextureB != null)
		{
			showTutorialTextures = true;
		}
	}
}
//while the player stays inside the tutorial box
public function OnTriggerStay (collider:Collider):void
{
	if(collider.gameObject.name == "Player")
	{
		if (!cameraMoving) timePassed += Time.deltaTime;
		if(timePassed > timerTexA)
		{
			tutorialTextureA = null;
		}
		if(timePassed > timerTexB)
		{
			tutorialTextureB = null;
		}
		if(timePassed > blinkingTime)
		{
			resetBlinkingButtons();
		}
		
		if(lightTutorial)
		{
			//if the tutorial is complete, play the animation and put it on false
			if(gameLogic.getBattery() == gameLogic.getBatteryCapacity())
			{
				playAnimation();				//play the animation of the barrier
				lightTutorial = false;			//completed objective
			}
		}
		else if(crystalTutorial)
		{
			if(gameLogic.getCrystalsSampleCount() > 0)
			{
				playAnimation();
				crystalTutorial = false;
			}
		}
		else if(slugTutorial)
		{
			if(slugObject.GetComponent(SlugScript).isWaitState())
			{
				playAnimation();
				slugTutorial = false;
			}
		}
	}
}
//when the player leaves the trigger box
public function OnTriggerExit (collider:Collider):void
{
	if(collider.name == "Player")
	{
		showTutorialTextures = false;	//stop showing textures
		
		resetBlinkingButtons();			//stop blinking buttons
		
		if(destroyOnExit)
		{
			Destroy(this.gameObject);	//if it is destroy on exit then destroy the object
		}
	}
}

//
//setters
//

public function setAlphaObject(alphaObj:GameObject):void
{
	alphaObject = alphaObj;
}

public function setJumpButtonTutorial(value:boolean):void
{
	jumpButtonTutorial = value;
}

public function setNormalShroomButtonTutorial(value:boolean):void
{
	normalShroomButtonTutorial = value;
}

public function setFlashButtonTutorial(value:boolean):void
{
	flashButtonTutorial = value;
}

public function setBumpyShroomButtonTutorial(value:boolean):void
{
	bumpyShroomButtonTutorial = value;
}


public function setLightTutorial(value:boolean):void
{
	lightTutorial = value;
}

public function setSlugTutorial(value:boolean):void
{
	slugTutorial = value;
}

public function setCrystalTutorial(value:boolean):void
{
	crystalTutorial = value;
}

public function setSlugObject(slugObj:GameObject):void
{
	slugObject = slugObj;
}

public function setBlockObject(blockObj:GameObject):void
{
	blockObject = blockObj;
}

public function setMovementLeftEnabled(value:boolean):void
{
	movementLeftEnabled = value;
}
public function setMovementRightEnabled(value:boolean):void
{
	movementRightEnabled = value;
}

public function setJumpButtonEnabled(value:boolean):void
{
	jumpButtonEnabled = value;
}
public function setFlashButtonEnabled(value:boolean):void
{
	flashButtonEnabled = value;
}
public function setNormalShroomButtonEnabled(value:boolean):void
{
	shootNormalShroomButtonEnabled = value;
}
public function setBumpyShroomButtonEnabled(value:boolean):void
{
	shootBumpyShroomButtonEnabled = value;
}

public function setTutorialTextureA(tex:Texture2D):void
{
	tutorialTextureA = tex;
}
public function setXPositionTexA(value:float):void
{
	xPositionTexA = value;
}
public function setYPositionTexA(value:float):void
{
	yPositionTexA = value;
}
public function setTimerTexA(value:float):void
{
	timerTexA = value;
}

public function setTutorialTextureB(tex:Texture2D):void
{
	tutorialTextureB = tex;
}

public function setXPositionTexB(value:float):void
{
	xPositionTexB = value;
}
public function setYPositionTexB(value:float):void
{
	yPositionTexB = value;
}

public function setTimerTexB(value:float):void
{
	timerTexB = value;
}

public function setDestroyOnExit(value:boolean):void
{
	destroyOnExit = value;
}

public function setDestroyOnCompletion(value:boolean):void
{
	destroyOnCompletion = value;
}

//
//getters
//
public function getAlphaObject():GameObject
{
	if(alphaObject != null)
	{
		return alphaObject;
	}
	
	return null;
}

public function getJumpButtonTutorial():boolean
{
	return jumpButtonTutorial;
}

public function getNormalShroomButtonTutorial():boolean
{
	return normalShroomButtonTutorial;
}

public function getFlashButtonTutorial():boolean
{
	return flashButtonTutorial;
}

public function getBumpyShroomButtonTutorial():boolean
{
	return bumpyShroomButtonTutorial;
}

public function getLightTutorial():boolean
{
	return lightTutorial;
}

public function getSlugTutorial():boolean
{
	return slugTutorial;
}

public function getCrystalTutorial():boolean
{
	return crystalTutorial;
}

public function getSlugObject():GameObject
{
	return slugObject;
}

public function getBlockObject():GameObject
{
	return blockObject;
}

public function getMovementLeftEnabled():boolean
{
	return movementLeftEnabled;
}
public function getMovementRightEnabled():boolean
{
	return movementRightEnabled;
}
public function getJumpButtonEnabled():boolean
{
	return jumpButtonEnabled;
}
public function getFlashButtonEnabled():boolean
{
	return flashButtonEnabled;
}
public function getNormalShroomButtonEnabled():boolean
{
	return shootNormalShroomButtonEnabled;
}
public function getBumpyShroomButtonEnabled():boolean
{
	return shootBumpyShroomButtonEnabled;
}

public function getTutorialTextureA():String
{
	if(tutorialTextureA != null)
	{
		return tutorialTextureA.name;
	}
	
	return "";
}

public function getXPositionTexA():float
{
	return xPositionTexA;
}

public function getYPositionTexA():float
{
	return yPositionTexA;
}

public function getTimerTexA():float
{
	return timerTexA;
}

public function getTutorialTextureB():String
{
	if(tutorialTextureB != null)
	{
		return tutorialTextureB.name;
	}
	return "";
}

public function getXPositionTexB():float
{
	return xPositionTexB;
}

public function getYPositionTexB():float
{
	return yPositionTexB;
}

public function getTimerTexB():float
{
	return timerTexB;
}

public function getDestroyOnExit():boolean
{
	return destroyOnExit;
}

public function getDestroyOnCompletion():boolean
{
	return destroyOnCompletion;
}

public function setCameraMoving(value:boolean)
{
	cameraMoving = value;
}