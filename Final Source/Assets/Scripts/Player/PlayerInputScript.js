#pragma strict

import TouchScript;	//we're making use of a touch library so import it

private var player:GameObject = null;					//the player itself
private var playerController:PlayerController = null;	//the controller
private var soundEngine:SoundEngineScript = null;		//sound engine for sounds
private var gameLogic:GameLogic;						//the Game Logic

private var inactiveTimer				:float = 60.0;	//if the player is inactive for 60 seconds go back to the menu

private var shootTimer:float = 0.0f;				//cooldown for shooting set after shooting
private var chargingNormalShot:boolean = false;
private var chargingBumpyShot:boolean = false;

private var endLevelTriggerObject:GameObject 	= null;
private var endLevelTriggerScript:LevelTrigger 	= null;

private var jumpButtonEnabled			:boolean = true;	//enables / disables controls
private var flashButtonEnabled			:boolean = true;
private var normalShroomButtonEnabled	:boolean = true;
private var bumpyShroomButtonEnabled	:boolean = true;
private var movementLeftEnabled			:boolean = true;
private var movementRightEnabled		:boolean = true;

private var blinkingCounter				:float 	 = 0.75f;	//0.75 sec there 0.75 sec not (blinking!)
private var blinkingJumpButton			:boolean = false;	//which button should blink
private var blinkingFlashButton			:boolean = false;
private var blinkingNormalShroomButton	:boolean = false;
private var blinkingBumpyShroomButton	:boolean = false;

//button positions
private var currentJumpButtonTexture	:Texture2D 	= null;	//texture of the button
public 	var jumpButtonTexture			:Texture2D 	= null;	//normal texture
public  var jumpButtonInactiveTexture 	:Texture2D 	= null;	//inactive texture
public	var jumpButtonActiveTexture		:Texture2D	= null;	//when pressed texture
private var jumpButtonRect				:Rect;
public  var jumpButtonX					:float = 0.0f;		//position of the button
public 	var jumpButtonY					:float = 780.0f;


private var currentFlashButtonTexture	:Texture2D = null;
public  var flashButtonTexture			:Texture2D = null;
public  var flashButtonInactiveTexture	:Texture2D = null;
public	var flashButtonActiveTexture	:Texture2D = null;
private var flashButtonRect				:Rect;
public  var flashButtonX				:float = 0.0f;
public  var flashButtonY				:float = 900.0f;


private var currentNormalShroomButtonTexture	:Texture2D = null;
public 	var normalShroomButtonTexture			:Texture2D = null;
public  var normalShroomButtonInactiveTexture	:Texture2D = null;
public	var normalShroomButtonActiveTexture		:Texture2D = null;
private var normalShroomButtonRect				:Rect;
public 	var normalShroomButtonX					:float = 1600.0f;
public 	var normalShroomButtonY					:float = 900.0f;


private var currentBumpyShroomButtonTexture :Texture2D = null;
public 	var bumpyShroomButtonTexture		:Texture2D = null;
public 	var bumpyShroomButtonInactiveTexture:Texture2D = null;
public	var bumpyShroomButtonActiveTexture	:Texture2D = null;
private var bumpyShroomButtonRect			:Rect;
public 	var bumpyShroomButtonX				:float = 1600.0f;
public 	var bumpyShroomButtonY				:float = 720.0f;

private var currentEscapeButtonTexture		:Texture2D = null;
public var escapeButtonTexture				:Texture2D = null;
public var escapeButtonActiveTexture		:Texture2D = null;
private var escapeButtonRect				:Rect;
public var escapeButtonX					:float = 1650.0f;
public var escapeButtonY					:float = 20.0f;
private var escapePressed					:boolean = false;

public var confirmationTrueTexture			:Texture2D = null;
public var confirmationFalseTexture			:Texture2D = null;
public var confirmationTruePressedTexture	:Texture2D = null;
public var confirmationFalsePressedTexture	:Texture2D = null;
public var confirmationScreenTexture		:Texture2D = null;
private var confirmationScreenRect			:Rect;
public var confirmationScreenX				:float = 0.0f;
public var confirmationScreenY				:float = 0.0f;
public var confirmationButtonY				:float = 700.0f;
public var confirmationTrueX				:float = 288.0f;
public var confirmationFalseX				:float = 1090.0f;   
private var confirmationTrueRect			:Rect;
private var confirmationFalseRect			:Rect;

//scale for buttons
private var originalWidth 	:float = 1920;
private var originalHeight	:float = 1080;
private var scale			:Vector3;

public function Awake():void
{
	playerController = this.gameObject.GetComponent("PlayerController") as PlayerController;
	gameLogic = GameObject.Find("GameLogic").GetComponent(GameLogic) as GameLogic;
	
	if(Application.loadedLevelName == "LevelLoaderScene")
	{
		soundEngine = GameObject.Find("SoundEngine").GetComponent(SoundEngineScript);
	}
	
	//initialize the button textures
	currentJumpButtonTexture			= jumpButtonTexture;
	currentFlashButtonTexture			= flashButtonTexture;
	currentNormalShroomButtonTexture 	= normalShroomButtonTexture;
	currentBumpyShroomButtonTexture		= bumpyShroomButtonTexture;
	currentEscapeButtonTexture			= escapeButtonTexture;
}

//when the player is enabled it should add a touchBegan event to the touch manager
public function OnEnable():void
{
	if(TouchManager.Instance != null)
	{
		TouchManager.Instance.TouchesBegan += touchBegan;	//giving the function pointer to the touch manager
	}
}

public function OnDisable():void
{
	if(TouchManager.Instance != null)
	{
		TouchManager.Instance.TouchesBegan -= touchBegan;	//removing the function pointer
	}
}

public function Update ():void
{
	//fix to get the endlevel trigger as it might not have been loaded yet when the player is initialized
	if(endLevelTriggerObject == null)
	{
		if(GameObject.Find("EndLevelTrigger") != null)
		{
			endLevelTriggerObject = GameObject.Find("EndLevelTrigger") as GameObject;
			endLevelTriggerScript = endLevelTriggerObject.GetComponent(LevelTrigger) as LevelTrigger;
		}
	}
	//if the game is not finished or lost yet, check for input
	else if (!endLevelTriggerScript.getFinished() && !endLevelTriggerScript.getLost())
	{
		checkReleasingButton();
		if (chargingNormalShot || chargingBumpyShot) playerController.chargeShot();
		checkAmmo();
		readTouch();
		playerController.brake();
	}
	//else if the game is lost or finished stop the moment and control of the player
	else if(endLevelTriggerScript.getFinished() || endLevelTriggerScript.getLost())
	{
		playerController.stopMovement();
		playerController.stopControl();
	}
	
	if (shootTimer > 0.0f) shootTimer -= Time.deltaTime;
	
	
	//check if the player is inactive for 60, if so return to menu
	if(TouchManager.Instance.ActiveTouches.Count == 0)
	{
		inactiveTimer -= Time.deltaTime;
		if(inactiveTimer <= 0.0f)
		{
			Application.LoadLevel("Menu");
			soundEngine.changeMusic("Menu");
		}
	}
	else
	{
		inactiveTimer = 60.0f;
	}	
}
//check the ammo
private function checkAmmo()
{
	if(!gameLogic.getInfiniteAmmo())
	{
		if(gameLogic.getCurrentNormalSeeds() == 0) setNormalShroomButtonEnabled(false);
		else setNormalShroomButtonEnabled(true);
		
		if(gameLogic.getCurrentBumpySeeds() == 0) setBumpyShroomButtonEnabled(false);
		else setBumpyShroomButtonEnabled(true);
	}
}
//on press event handler
private function touchBegan(sender:Object, events:TouchEventArgs):void
{
	if(endLevelTriggerObject != null)
	{
		if (!endLevelTriggerScript.getFinished() && !endLevelTriggerScript.getLost())
		{
			for each(var touchPoint in events.Touches)
			{
				var position:Vector2 = touchPoint.Position;
				position = new Vector2(position.x, (position.y - Screen.height)*-1);	//position of the press is Y inverted
				
				isPressingButton(position);	//check if the player is pressing a button if so activate it
			}
		}
	}
}

//checks whether the button is being pressed or not
private function checkReleasingButton():void
{
	var jumpingButtonTouched		:boolean = false;
	var flashButtonTouched			:boolean = false;
	var normalShroomButtonTouched	:boolean = false;
	var bumpyShroomButtonTouched	:boolean = false;
	var escapeButtonTouched			:boolean = false;
	
	for each(var touchPoint in TouchManager.Instance.ActiveTouches)
	{
		var inputXY:Vector2 = touchPoint.Position;
		inputXY = new Vector2(inputXY.x, (inputXY.y - Screen.height)*-1);
		
			if(jumpButtonRect.Contains(inputXY))
			{
				jumpingButtonTouched = true;
			}
			else if (flashButtonRect.Contains(inputXY))
			{
				flashButtonTouched = true;
			}
			else if(normalShroomButtonRect.Contains(inputXY))
			{	
				normalShroomButtonTouched = true;			
			}
			else if(bumpyShroomButtonRect.Contains(inputXY))
			{
				bumpyShroomButtonTouched = true;
			}
			else if(escapeButtonRect.Contains(inputXY))
			{
				escapeButtonTouched = true;
			}
	}
	
		//none of the buttons are touched so reset their textures
		if(jumpButtonEnabled && !blinkingJumpButton && !jumpingButtonTouched)
		{
			currentJumpButtonTexture = jumpButtonTexture;
		}
		
		if (flashButtonEnabled && !blinkingFlashButton && !flashButtonTouched)
		{
			currentFlashButtonTexture = flashButtonTexture;
		}
		
		if(normalShroomButtonEnabled && !blinkingNormalShroomButton && !normalShroomButtonTouched)
		{
			currentNormalShroomButtonTexture = normalShroomButtonTexture;
		}
		//extra check to see if the player should shoot a mushroom
		if(normalShroomButtonEnabled && chargingNormalShot && !normalShroomButtonTouched)
		{
			if(blinkingNormalShroomButton) blinkingNormalShroomButton = false;
			playerController.shoot(0);
			chargingNormalShot = false;
			shootTimer = 2.0f;
		}
		if(bumpyShroomButtonEnabled && !blinkingBumpyShroomButton && !bumpyShroomButtonTouched)
		{
			currentBumpyShroomButtonTexture = bumpyShroomButtonTexture;
		}
		//extra check to see if the player should shoot a mushroom
		if(bumpyShroomButtonEnabled && chargingBumpyShot && !bumpyShroomButtonTouched)
		{
			if(blinkingBumpyShroomButton) blinkingBumpyShroomButton = false;
			playerController.shoot(1);
			chargingBumpyShot = false;
			shootTimer = 2.0f;
		}
		if(!escapeButtonTouched)
		{
			currentEscapeButtonTexture = escapeButtonTexture;
		}
}

//if the player is pressing a button
private function isPressingButton(inputXY:Vector2):void
{
	if(jumpButtonEnabled)
	{
		if(jumpButtonRect.Contains(inputXY))
		{
			currentJumpButtonTexture = jumpButtonActiveTexture;
			if(blinkingJumpButton) blinkingJumpButton = false;
			playerController.jump();
			chargingNormalShot = false;
			chargingBumpyShot = false;
			playerController.resetShot();
			return;
		}
	}
	
	if (flashButtonEnabled)
	{
		if (flashButtonRect.Contains(inputXY))
		{
			currentFlashButtonTexture = flashButtonActiveTexture;
			if(blinkingFlashButton) blinkingFlashButton = false;
			playerController.flash();
			playerController.resetShot();
			return;
		}
	}
	
	if(normalShroomButtonEnabled)
	{
		if(normalShroomButtonRect.Contains(inputXY))
		{	
			currentNormalShroomButtonTexture = normalShroomButtonActiveTexture;
			if (shootTimer <= 0.0f && !chargingNormalShot) chargingNormalShot = true;
			return;			
		}
	}
	
	if(bumpyShroomButtonEnabled)
	{
		if(bumpyShroomButtonRect.Contains(inputXY))
		{
			currentBumpyShroomButtonTexture = bumpyShroomButtonActiveTexture;
			if (shootTimer <= 0.0f && !chargingBumpyShot) chargingBumpyShot = true;
			return;
		}
	}
	if(escapeButtonRect.Contains(inputXY))
	{
		escapePressed = true;
		currentEscapeButtonTexture = escapeButtonActiveTexture;
		return;
	}
	if(escapePressed)
	{
		if(confirmationTrueRect.Contains(inputXY))
		{
			Application.LoadLevel("Menu");
			soundEngine.changeMusic("Menu");
			return;
		}	
		if(confirmationFalseRect.Contains(inputXY))
		{
			escapePressed = false;
			return;
		}
	}
}
//check if the player is touching a button
private function isTouchingButton(inputXY:Vector2):boolean
{	
	if(jumpButtonRect.Contains(inputXY))
	{
		return true;
	}
	if(flashButtonRect.Contains(inputXY))
	{
		return true;
	}
	if(normalShroomButtonRect.Contains(inputXY))
	{			
		return true;
	}
	if(bumpyShroomButtonRect.Contains(inputXY))
	{
		return true;
	}
	if(escapeButtonRect.Contains(inputXY))
	{
		return true;
	}
	return false;
}

public function OnGUI():void
{
	if(endLevelTriggerObject != null)
	{
		if (!endLevelTriggerScript.getFinished() && !endLevelTriggerScript.getLost())
		{
			//first scale the buttons before drawing them
			scaleButtons();
			checkBlinkingButtons();
			
			//if the jump button is enabled it will be drawn
			if(jumpButtonEnabled)
			{
				//this is the texture of the button
				GUI.DrawTexture(jumpButtonRect, currentJumpButtonTexture);
			}
			else
			{
				//this is the texture of the inactive button
				GUI.DrawTexture(jumpButtonRect, jumpButtonInactiveTexture);
			}
			if (flashButtonEnabled)
			{
				GUI.DrawTexture(flashButtonRect, currentFlashButtonTexture);
			}
			else {
				GUI.DrawTexture(flashButtonRect, flashButtonInactiveTexture);
			}
			
			if(normalShroomButtonEnabled)
			{
				GUI.DrawTexture(normalShroomButtonRect, currentNormalShroomButtonTexture);
			}
			else
			{
				GUI.DrawTexture(normalShroomButtonRect, normalShroomButtonInactiveTexture);
			}
			
			if(bumpyShroomButtonEnabled)
			{
				GUI.DrawTexture(bumpyShroomButtonRect, currentBumpyShroomButtonTexture);
			}
			else
			{
				GUI.DrawTexture(bumpyShroomButtonRect, bumpyShroomButtonInactiveTexture);
			}
			
			if(!escapePressed && escapeButtonTexture != null)
			{
				GUI.DrawTexture(escapeButtonRect, currentEscapeButtonTexture);
			}
			
			if(escapePressed && confirmationScreenTexture != null)
			{
				GUI.DrawTexture(confirmationScreenRect, confirmationScreenTexture);
				GUI.DrawTexture(confirmationTrueRect, confirmationTrueTexture);
				GUI.DrawTexture(confirmationFalseRect, confirmationFalseTexture);
			}
		}
	}
}
//check for blinking buttons, if so blink em!
private function checkBlinkingButtons():void
{
	//first check if either one of the buttons should blink
	if(blinkingJumpButton || blinkingFlashButton || blinkingNormalShroomButton || blinkingBumpyShroomButton)
	{
		//decrease the blinking counter
		blinkingCounter -= Time.deltaTime;
		//if it is 0 execute the switch
		if(blinkingCounter <= 0.0f)
		{
			blinkingCounter = 0.75f;
			
			if(blinkingJumpButton)
			{
				if(currentJumpButtonTexture == jumpButtonTexture) currentJumpButtonTexture = jumpButtonInactiveTexture;
				else currentJumpButtonTexture = jumpButtonTexture;
			}
			else if(blinkingFlashButton)
			{
				if(currentFlashButtonTexture == flashButtonTexture) currentFlashButtonTexture = flashButtonInactiveTexture;
				else currentFlashButtonTexture = flashButtonTexture;
			}
			else if(blinkingNormalShroomButton)
			{
				if(currentNormalShroomButtonTexture == normalShroomButtonTexture) currentNormalShroomButtonTexture = normalShroomButtonInactiveTexture;
				else currentNormalShroomButtonTexture = normalShroomButtonTexture;
			}
			else if(blinkingBumpyShroomButton)
			{
				if(currentBumpyShroomButtonTexture == bumpyShroomButtonTexture) currentBumpyShroomButtonTexture = bumpyShroomButtonInactiveTexture;
				else currentBumpyShroomButtonTexture = bumpyShroomButtonTexture;
			}
		}
	}
}
//scale the buttons to the right size according to the screen size with the 16:9 aspect ratio
private function scaleButtons():void
{
	//get the current scale by using the current screen size and the original screen size
	//original width / height is defined in a variable at top, we use an aspect ratio of 16:9 and original screen size of 1920x1080
	scale.x = Screen.width / originalWidth;		//X scale is the current width divided by the original width
	scale.y = Screen.height / originalHeight;	//Y scale is the current height divided by the original height
	
	//first put the rectangles back to its original size before scaling
	jumpButtonRect 			= new Rect(jumpButtonX			, jumpButtonY			, jumpButtonTexture.width			, jumpButtonTexture.height);
	flashButtonRect			= new Rect(flashButtonX			, flashButtonY			, flashButtonTexture.width			, flashButtonTexture.height);
	normalShroomButtonRect	= new Rect(normalShroomButtonX	, normalShroomButtonY	, normalShroomButtonTexture.width	, normalShroomButtonTexture.height);
	bumpyShroomButtonRect  	= new Rect(bumpyShroomButtonX	, bumpyShroomButtonY	, bumpyShroomButtonTexture.width	, bumpyShroomButtonTexture.height);
	
	//second scale the rectangles
	jumpButtonRect 			= scaleRect(jumpButtonRect);
	flashButtonRect			= scaleRect(flashButtonRect);
	normalShroomButtonRect	= scaleRect(normalShroomButtonRect);
	bumpyShroomButtonRect  	= scaleRect(bumpyShroomButtonRect);
	
	
	if(escapeButtonTexture != null)
	{
		escapeButtonRect 		= new Rect(escapeButtonX, escapeButtonY, escapeButtonTexture.width, escapeButtonTexture.height);
		escapeButtonRect 		= scaleRect(escapeButtonRect);
	}
	if(escapePressed && confirmationScreenTexture != null)
	{
		confirmationScreenRect 		= new Rect(confirmationScreenX, confirmationScreenY, confirmationScreenTexture.width, confirmationScreenTexture.height);
		confirmationTrueRect 		= new Rect(confirmationTrueX, confirmationButtonY, confirmationTrueTexture.width, confirmationTrueTexture.height);
		confirmationFalseRect 		= new Rect(confirmationFalseX, confirmationButtonY, confirmationFalseTexture.width, confirmationFalseTexture.height);
		
		confirmationScreenRect 		= scaleRect(confirmationScreenRect);
		confirmationTrueRect 		= scaleRect(confirmationTrueRect);
		confirmationFalseRect 		= scaleRect(confirmationFalseRect);
	}
}
//scaling the rectangles
private function scaleRect(rect:Rect):Rect
{
	var newRect:Rect = new Rect(rect.x * scale.x, rect.y * scale.y, rect.width * scale.x, rect.height * scale.y);
	return newRect;
}

//read all touch points and check if the player should move
function readTouch()
{
	for each(var touch in TouchManager.Instance.ActiveTouches)
	{	
		var position:Vector2 = touch.Position;
		//sendRay(position);
		
		position = Vector2(position.x, (position.y - Screen.height)*-1);
		
		if(!isTouchingButton(position))
		{
			if (chargingNormalShot || chargingBumpyShot)
			{
				chargingNormalShot = false;
				chargingBumpyShot = false;
				playerController.resetShot();
			}
			if (position.x > Screen.width / 2)
			{
				if(movementRightEnabled) playerController.move(position.x);
				return;
			}
			if (position.x < Screen.width / 2)
			{
				if(movementLeftEnabled) playerController.move(position.x);
				return;
			}
		}
	}
}

/*
	Setters
*/
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
	normalShroomButtonEnabled = value;
}

public function setBumpyShroomButtonEnabled(value:boolean):void
{
	bumpyShroomButtonEnabled = value;
}

public function setBlinkingJumpButton(value:boolean):void
{
	//if(!value) currentJumpButtonTexture = jumpButtonTexture;
	blinkingJumpButton = value;
}

public function setBlinkingFlashButton(value:boolean):void
{
	//if(!value) currentFlashButtonTexture = flashButtonTexture;
	blinkingFlashButton = value;
}

public function setBlinkingNormalShroomButton(value:boolean):void
{
	//if(!value) currentNormalShroomButtonTexture = normalShroomButtonTexture;
	blinkingNormalShroomButton = value;
}

public function setBlinkingBumpyShroomButton(value:boolean):void
{
	//if(!value) currentBumpyShroomButtonTexture = bumpyShroomButtonTexture;
	blinkingBumpyShroomButton = value;
}