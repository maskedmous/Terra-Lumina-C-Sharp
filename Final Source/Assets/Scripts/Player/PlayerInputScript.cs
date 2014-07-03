// Converted from UnityScript to C# at http://www.M2H.nl/files/js_to_c.php - by Mike Hergaarden
// Do test the code! You usually need to change a few small bits.

using UnityEngine;
using System.Collections;
using TouchScript;	//we're making use of a touch library so import it

public class PlayerInputScript : MonoBehaviour {

	private GameObject player = null;					//the player itself
	private PlayerController playerController = null;	//the controller
	private SoundEngineScript soundEngine = null;		//sound engine for sounds
	private GameLogic gameLogic;						//the Game Logic
	
	private float inactiveTimer = 60.0f;	//if the player is inactive for 60 seconds go back to the menu
	
	private float shootTimer = 0.0f;				//cooldown for shooting set after shooting
	private bool  chargingNormalShot = false;
	private bool  chargingBumpyShot = false;
	
	private GameObject endLevelTriggerObject 	= null;
	private LevelTrigger endLevelTriggerScript 	= null;
	
	private bool  jumpButtonEnabled = true;	//enables / disables controls
	private bool  flashButtonEnabled = true;
	private bool  normalShroomButtonEnabled = true;
	private bool  bumpyShroomButtonEnabled = true;
	private bool  movementLeftEnabled = true;
	private bool  movementRightEnabled = true;
	
	private float blinkingCounter 	 = 0.75f;	//0.75f sec there 0.75f sec not (blinking!)
	private bool  blinkingJumpButton = false;	//which button should blink
	private bool  blinkingFlashButton = false;
	private bool  blinkingNormalShroomButton = false;
	private bool  blinkingBumpyShroomButton = false;
	
	//button positions
	private Texture2D currentJumpButtonTexture 	= null;	//texture of the button
	public 	Texture2D jumpButtonTexture 	= null;	//normal texture
	public  Texture2D jumpButtonInactiveTexture 	= null;	//inactive texture
	public	Texture2D jumpButtonActiveTexture	= null;	//when pressed texture
	private Rect jumpButtonRect;
	public  float jumpButtonX = 0.0f;		//position of the button
	public 	float jumpButtonY = 780.0f;
	
	
	private Texture2D currentFlashButtonTexture = null;
	public  Texture2D flashButtonTexture = null;
	public  Texture2D flashButtonInactiveTexture = null;
	public	Texture2D flashButtonActiveTexture = null;
	private Rect flashButtonRect;
	public  float flashButtonX = 0.0f;
	public  float flashButtonY = 900.0f;
	
	
	private Texture2D currentNormalShroomButtonTexture = null;
	public 	Texture2D normalShroomButtonTexture = null;
	public  Texture2D normalShroomButtonInactiveTexture = null;
	public	Texture2D normalShroomButtonActiveTexture = null;
	private Rect normalShroomButtonRect;
	public 	float normalShroomButtonX = 1600.0f;
	public 	float normalShroomButtonY = 900.0f;
	
	
	private Texture2D currentBumpyShroomButtonTexture = null;
	public 	Texture2D bumpyShroomButtonTexture = null;
	public 	Texture2D bumpyShroomButtonInactiveTexture = null;
	public	Texture2D bumpyShroomButtonActiveTexture = null;
	private Rect bumpyShroomButtonRect;
	public 	float bumpyShroomButtonX = 1600.0f;
	public 	float bumpyShroomButtonY = 720.0f;
	
	private Texture2D currentEscapeButtonTexture = null;
	public Texture2D escapeButtonTexture = null;
	public Texture2D escapeButtonActiveTexture = null;
	private Rect escapeButtonRect;
	public float escapeButtonX = 1650.0f;
	public float escapeButtonY = 20.0f;
	private bool  escapePressed = false;
	
	public Texture2D confirmationTrueTexture = null;
	public Texture2D confirmationFalseTexture = null;
	public Texture2D confirmationTruePressedTexture = null;
	public Texture2D confirmationFalsePressedTexture = null;
	public Texture2D confirmationScreenTexture = null;
	private Rect confirmationScreenRect;
	public float confirmationScreenX = 0.0f;
	public float confirmationScreenY = 0.0f;
	public float confirmationButtonY = 700.0f;
	public float confirmationTrueX = 288.0f;
	public float confirmationFalseX = 1090.0f;   
	private Rect confirmationTrueRect;
	private Rect confirmationFalseRect;
	
	//scale for buttons
	private float originalWidth = 1920;
	private float originalHeight = 1080;
	private Vector3 scale;
	
	public void Awake (){
		playerController = this.gameObject.GetComponent("PlayerController") as PlayerController;
		gameLogic = GameObject.Find("GameLogic").GetComponent("GameLogic") as GameLogic;
		
		if(Application.loadedLevelName == "LevelLoaderScene")
		{
			soundEngine = GameObject.Find("SoundEngine").GetComponent("SoundEngineScript") as SoundEngineScript;
		}
		
		//initialize the button textures
		currentJumpButtonTexture			= jumpButtonTexture;
		currentFlashButtonTexture			= flashButtonTexture;
		currentNormalShroomButtonTexture 	= normalShroomButtonTexture;
		currentBumpyShroomButtonTexture		= bumpyShroomButtonTexture;
		currentEscapeButtonTexture			= escapeButtonTexture;
	}
	
	//when the player is enabled it should add a touchBegan event to the touch manager
	public void OnEnable (){
		if(TouchManager.Instance != null)
		{
			TouchManager.Instance.TouchesBegan += touchBegan;	//giving the function pointer to the touch manager
		}
	}
	
	public void OnDisable (){
		if(TouchManager.Instance != null)
		{
			TouchManager.Instance.TouchesBegan -= touchBegan;	//removing the function pointer
		}
	}
	
	public void Update (){
		//fix to get the endlevel trigger as it might not have been loaded yet when the player is initialized
		if(endLevelTriggerObject == null)
		{
			if(GameObject.Find("EndLevelTrigger") != null)
			{
				endLevelTriggerObject = GameObject.Find("EndLevelTrigger") as GameObject;
				endLevelTriggerScript = endLevelTriggerObject.GetComponent("LevelTrigger") as LevelTrigger;
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
	private void  checkAmmo (){
		if(!gameLogic.getInfiniteAmmo())
		{
			if(gameLogic.getCurrentNormalSeeds() == 0) setNormalShroomButtonEnabled(false);
			else setNormalShroomButtonEnabled(true);
			
			if(gameLogic.getCurrentBumpySeeds() == 0) setBumpyShroomButtonEnabled(false);
			else setBumpyShroomButtonEnabled(true);
		}
	}
	//on press event handler
	private void touchBegan ( object sender ,   TouchEventArgs events  ){
		if(endLevelTriggerObject != null)
		{
			if (!endLevelTriggerScript.getFinished() && !endLevelTriggerScript.getLost())
			{
				foreach(var touchPoint in events.Touches)
				{
					Vector2 position = touchPoint.Position;
					position = new Vector2(position.x, (position.y - Screen.height)*-1);	//position of the press is Y inverted
					
					isPressingButton(position);	//check if the player is pressing a button if so activate it
				}
			}
		}
	}
	
	//checks whether the button is being pressed or not
	private void checkReleasingButton (){
		bool  jumpingButtonTouched = false;
		bool  flashButtonTouched = false;
		bool  normalShroomButtonTouched = false;
		bool  bumpyShroomButtonTouched = false;
		bool  escapeButtonTouched = false;
		
		foreach(var touchPoint in TouchManager.Instance.ActiveTouches)
		{
			Vector2 inputXY = touchPoint.Position;
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
	private void isPressingButton ( Vector2 inputXY  ){
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
	private bool isTouchingButton ( Vector2 inputXY  ){	
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
	
	public void OnGUI (){
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
	private void checkBlinkingButtons (){
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
	private void scaleButtons (){
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
	private Rect scaleRect ( Rect rect  ){
		Rect newRect = new Rect(rect.x * scale.x, rect.y * scale.y, rect.width * scale.x, rect.height * scale.y);
		return newRect;
	}
	
	//read all touch points and check if the player should move
	void  readTouch (){
		foreach(var touch in TouchManager.Instance.ActiveTouches)
		{	
			Vector2 position = touch.Position;
			//sendRay(position);
			
			position = new Vector2(position.x, (position.y - Screen.height)*-1);
			
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
	public void setMovementLeftEnabled ( bool value  ){
		movementLeftEnabled = value;
	}
	
	public void setMovementRightEnabled ( bool value  ){
		movementRightEnabled = value;
	}
	
	public void setJumpButtonEnabled ( bool value  ){
		jumpButtonEnabled = value;
	}
	
	public void setFlashButtonEnabled ( bool value  ){
		flashButtonEnabled = value;
	}
	
	public void setNormalShroomButtonEnabled ( bool value  ){
		normalShroomButtonEnabled = value;
	}
	
	public void setBumpyShroomButtonEnabled ( bool value  ){
		bumpyShroomButtonEnabled = value;
	}
	
	public void setBlinkingJumpButton ( bool value  ){
		//if(!value) currentJumpButtonTexture = jumpButtonTexture;
		blinkingJumpButton = value;
	}
	
	public void setBlinkingFlashButton ( bool value  ){
		//if(!value) currentFlashButtonTexture = flashButtonTexture;
		blinkingFlashButton = value;
	}
	
	public void setBlinkingNormalShroomButton ( bool value  ){
		//if(!value) currentNormalShroomButtonTexture = normalShroomButtonTexture;
		blinkingNormalShroomButton = value;
	}
	
	public void setBlinkingBumpyShroomButton ( bool value  ){
		//if(!value) currentBumpyShroomButtonTexture = bumpyShroomButtonTexture;
		blinkingBumpyShroomButton = value;
	}
}