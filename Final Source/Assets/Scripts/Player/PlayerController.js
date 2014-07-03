#pragma strict

import System.Collections.Generic;

private var soundEngine:SoundEngineScript = null;
private var debugInfo:String = "";

private var lastDirection:String = "Right";
private var maxSpeed:float = 7.5f;
private var accelerationSpeed:float = 13.0f;

private var lineRenderer:LineRenderer = null;
private var y0:float = 0.0f;
private var x1:float = 0.0f;
private var x2:float = 0.0f;
private var x3:float = 0.0f;
private var x4:float = 0.0f;
private var x5:float = 0.0f;
private var x6:float = 0.0f;
private var x7:float = 0.0f;
private var x8:float = 2.3f;
private var increasing:boolean = true;
private var g:float = 0.0f;
private var tanAngle:float = 0.75f;
private var cosAngle:float = 0.6f;
private var v:float  = 0.0f;
private var vx:float = 0.0f;
private var vy:float = 0.0f;

private var isJumping:boolean = false;
private var initializeJumping:boolean = false;
private var jumpForce:float = 7.5f;
private var wheels:List.<GameObject> = new List.<GameObject>();

//shroom seed shooting
private var isShooting:boolean = false;								//is it shooting at the moment?
private var shrooms:List.<GameObject> = new List.<GameObject>();	//list of shrooms to shoot (should be 2)
private var crystals:List.<GameObject> = new List.<GameObject>();	//the crystals you collect

private var gameLogic:GameLogic = null;
private var particleScript:PlayerParticleScript	= null;

public var flashlight:GameObject;
private var flashBool:boolean = false;
private var counter:float = 0.0f;

private var control:boolean = true;

private var anim:Animator = null;

private var currentMaterial:Material = null;
private var currentRoverTexture			:Texture2D = null;
private var highBatteryRoverTexture		:Texture2D = null;
private var yellowBatteryRoverTexture	:Texture2D = null;
private var redBatteryRoverTexture		:Texture2D = null;

public function Awake():void
{
	gameLogic = GameObject.Find("GameLogic").GetComponent("GameLogic") as GameLogic;
	
	particleScript = this.gameObject.GetComponent("PlayerParticleScript") as PlayerParticleScript;
	
	anim = GetComponent(Animator);
	
	if(GameObject.Find("TextureLoader") != null)
	{
		var textureLoader:TextureLoader = GameObject.Find("TextureLoader").GetComponent(TextureLoader);
		
		highBatteryRoverTexture 	= textureLoader.getTexture("FinalTexture");
		yellowBatteryRoverTexture 	= textureLoader.getTexture("FinalTextureYellowWarning");
		redBatteryRoverTexture 		= textureLoader.getTexture("FinalTextureRedWarning");
		
		var roverMesh:GameObject = this.gameObject.transform.FindChild("RoverBodyMesh").gameObject;
		currentMaterial = roverMesh.renderer.material;
		currentRoverTexture = currentMaterial.GetTexture(0) as Texture2D;
	}
	
	if(Application.loadedLevelName == "LevelLoaderScene")
	{
		soundEngine = GameObject.Find("SoundEngine").GetComponent(SoundEngineScript) as SoundEngineScript;
	}
	
	shrooms.Add(Resources.Load("Prefabs/NormalShroom") as GameObject);
	shrooms.Add(Resources.Load("Prefabs/BumpyShroom") as GameObject);
}

public function Start():void
{
	lineRenderer = this.gameObject.GetComponent(LineRenderer);
	lineRenderer.enabled = false;
	g = -Physics.gravity.y;
	
	rigidbody.centerOfMass = new Vector3(-0.2f, -0.25f, 0.0f);
	
	wheels.Add(this.gameObject.transform.FindChild("Wheel1").gameObject);
	wheels.Add(this.gameObject.transform.FindChild("Wheel2").gameObject);
	wheels.Add(this.gameObject.transform.FindChild("Wheel3").gameObject);
	wheels.Add(this.gameObject.transform.FindChild("Wheel4").gameObject);
}

public function Update():void
{
	checkBatteryStatus();
	if(control)
	{
		checkIfJumping();
		
		if(flashBool == true) {
			flash();
			counter += Time.deltaTime;
			if(counter >= 2.0f) {
				flashlight.SetActive(false);
				flashBool = false;
				counter = 0.0f;
			}
		}
	}
	if(isJumping) {
		anim.SetBool("inAir", true);
	}
	if(!isJumping) {
		anim.SetBool("inAir", false);
	}
}

public function stopMovement():void
{
	this.gameObject.rigidbody.velocity.x = 0;
	anim.SetBool("isMoving", false);
}

public function stopControl():void
{
	control = false;
}

private function checkIfJumping():void
{
	var hitFound:boolean = false;
	var hitDown:RaycastHit;
	var layerMask:int = 1 << 8;
	layerMask = ~layerMask;
	
	for (var i:uint = 0; i < wheels.Count; ++i) {
		if (Physics.Raycast(wheels[i].transform.position, Vector3.down, hitDown, 0.5f, layerMask)) {
			hitFound = true;
			break;
		}
	}
	
	if (hitFound) {
		if (isJumping) particleScript.playParticle("landDust");
		isJumping = false;
		this.gameObject.rigidbody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY| RigidbodyConstraints.FreezePositionZ;
	}
	else {
		isJumping = true;
	}
	
	//making sure rover does not rotate beyond 45 degrees from original rotation
	var cRot:float = this.gameObject.transform.rotation.eulerAngles.z;
	if (cRot > 45.0f && cRot < 180.0f) this.gameObject.transform.rotation.eulerAngles.z = 45.0f;
	if (cRot > 180.0f && cRot < 315.0f) this.gameObject.transform.rotation.eulerAngles.z = 315.0f;
}

public function move(mousePos:float):void
{
	if(soundEngine != null)
	{
		if(soundEngine.getDrive() == false)
		{
			soundEngine.playSoundEffect("roverStart");
			soundEngine.setDrive(true);
		}
		soundEngine.playSoundEffect("roverDrive");
	}
	if (mousePos > Screen.width / 2) moveRight();
	if (mousePos < Screen.width / 2) moveLeft();
	anim.SetBool("isMoving", true);
}

private function moveLeft():void
{	
	if (this.gameObject.rigidbody.velocity.x > -maxSpeed)
	{
		this.gameObject.rigidbody.velocity.x -= accelerationSpeed * Time.deltaTime;
		if(this.gameObject.rigidbody.velocity.x < -maxSpeed) this.gameObject.rigidbody.velocity.x = -maxSpeed;
	}
	if (this.getDirection() == "Right")
	{
		this.gameObject.transform.rotation.eulerAngles.y = 180.0f;
		this.gameObject.transform.rotation.eulerAngles.z *= -1;
		this.gameObject.transform.rotation.eulerAngles.x *= -1;
	}
	setDirection("Left");
}

private function moveRight():void
{
	if (this.gameObject.rigidbody.velocity.x < maxSpeed)
	{
		this.gameObject.rigidbody.velocity.x += accelerationSpeed * Time.deltaTime;
		if(this.gameObject.rigidbody.velocity.x > maxSpeed) this.gameObject.rigidbody.velocity.x = maxSpeed;
	}
	if (this.getDirection() == "Left")
	{
		this.gameObject.transform.rotation.eulerAngles.y = 0.0f;
		this.gameObject.transform.rotation.eulerAngles.z *= -1;
		this.gameObject.transform.rotation.eulerAngles.x *= -1;
	}
	setDirection("Right");
}

public function brake():void
{
	if (!isJumping)
	{
		var vx:float = this.gameObject.rigidbody.velocity.x;
		if (Mathf.Abs(vx) > 0.01f) particleScript.playParticle("driveDust", Mathf.Abs(vx));
		if (vx > 0.10f) this.gameObject.rigidbody.velocity.x -= 5 * Time.deltaTime;
		else if (vx < -0.10f) this.gameObject.rigidbody.velocity.x += 5 * Time.deltaTime;
		else if (vx > -0.10f && vx < 0.10f)
		{
			if(vx != 0.0f)
			{
			 	this.gameObject.rigidbody.velocity.x = 0.0f;
			 	anim.SetBool("isMoving", false);
			 	if(soundEngine != null)
			 	{
				 	if(soundEngine.getDrive() == true)
				 	{
						soundEngine.playSoundEffect("roverStop");
						soundEngine.setDrive(false);
					}
				}
			}
		}
	}
	else particleScript.playParticle("driveDust", 0.0f);
}

public function jump():void
{
	if (!isJumping)
	{
		this.gameObject.rigidbody.velocity.y = jumpForce;
		isJumping = true;
		this.gameObject.rigidbody.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionZ;
		gameLogic.decreaseBatteryBy(3.0f);
		
		if(soundEngine != null)
		{
			soundEngine.playSoundEffect("jump");
		}
		if (particleScript != null)
		{
			particleScript.playParticle("jumpDust");
			particleScript.playParticle("engineJump");
		}
	}
}

public function chargeShot():void
{
	lineRenderer.enabled = true;
	
	if(soundEngine != null)
	{
		soundEngine.playSoundEffect("aim");
		soundEngine.setAim(true);
	}
	
	y0 = this.gameObject.transform.position.y;
	
	if (increasing)
	{
		vx += 3.0f * Time.deltaTime;
		vy += 2.25f * Time.deltaTime;
	}
	else
	{
		vx -= 3.0f * Time.deltaTime;
		vy -= 2.25f * Time.deltaTime;
	}
	v = vx * vx + vy * vy;

	//formula for max dist: d = (v*v*sin(2*angle)) / gravity
	//replaced sin(2*angle), which is .96, with a bigger number due to the trajectory not starting on the ground.
	if (vx < 3.0f)
	{
		increasing = true;
		vx = 3.0f;
		vy = 2.25f;
	}
	if (vx > 8.8f)
	{
		increasing = false;
	}
	
	x8 = v * 1.15 / 9.81;
	x1 = x8 / 8.0f;
	x2 = 2.0f * x8 / 8.0f;
	x3 = 3.0f * x8 / 8.0f;
	x4 = 4.0f * x8 / 8.0f;
	x5 = 5.0f * x8 / 8.0f;
	x6 = 6.0f * x8 / 8.0f;
	x7 = 7.0f * x8 / 8.0f;
	
	//trajectory function: Y = Y0 + tan(angle) * X - (gravity*x*x)/(2*v*v*cos(angle)*cos(angle))
	var y1:float = (y0 + x1 * 0.75) - (9.81 * x1 * x1) / (1.28 * v) + 1;
	var y2:float = (y0 + x2 * 0.75) - (9.81 * x2 * x2) / (1.28 * v) + 1;
	var y3:float = (y0 + x3 * 0.75) - (9.81 * x3 * x3) / (1.28 * v) + 1;
	var y4:float = (y0 + x4 * 0.75) - (9.81 * x4 * x4) / (1.28 * v) + 1;
	var y5:float = (y0 + x5 * 0.75) - (9.81 * x5 * x5) / (1.28 * v) + 1;
	var y6:float = (y0 + x6 * 0.75) - (9.81 * x6 * x6) / (1.28 * v) + 1;
	var y7:float = (y0 + x7 * 0.75) - (9.81 * x7 * x7) / (1.28 * v) + 1;
	var y8:float = (y0 + x8 * 0.75) - (9.81 * x8 * x8) / (1.28 * v) + 1;
	//0 = y0 + x8 * tanAngle - (g * x8 * x8) / (2 * (v * v * cosAngle * cosAngle))
	
	if (getDirection() == "Left")
	{
		x1 = -x1;
		x2 = -x2;
		x3 = -x3;
		x4 = -x4;
		x5 = -x5;
		x6 = -x6;
		x7 = -x7;
		x8 = -x8;
		
		lineRenderer.SetPosition(0, new Vector3(this.gameObject.transform.position.x - 2, y0 + 1, this.gameObject.transform.position.z));
		lineRenderer.SetPosition(1, new Vector3(this.gameObject.transform.position.x - 2 + x1, y1, this.gameObject.transform.position.z));
		lineRenderer.SetPosition(2, new Vector3(this.gameObject.transform.position.x - 2 + x2, y2, this.gameObject.transform.position.z));
		lineRenderer.SetPosition(3, new Vector3(this.gameObject.transform.position.x - 2 + x3, y3, this.gameObject.transform.position.z));
		lineRenderer.SetPosition(4, new Vector3(this.gameObject.transform.position.x - 2 + x4, y4, this.gameObject.transform.position.z));
		lineRenderer.SetPosition(5, new Vector3(this.gameObject.transform.position.x - 2 + x5, y5, this.gameObject.transform.position.z));
		lineRenderer.SetPosition(6, new Vector3(this.gameObject.transform.position.x - 2 + x6, y6, this.gameObject.transform.position.z));
		lineRenderer.SetPosition(7, new Vector3(this.gameObject.transform.position.x - 2 + x7, y7, this.gameObject.transform.position.z));
		lineRenderer.SetPosition(8, new Vector3(this.gameObject.transform.position.x - 2 + x8, y8, this.gameObject.transform.position.z));
	}
	else
	{	
		lineRenderer.SetPosition(0, new Vector3(this.gameObject.transform.position.x + 2, y0 + 1, this.gameObject.transform.position.z));
		lineRenderer.SetPosition(1, new Vector3(this.gameObject.transform.position.x + 2 + x1, y1, this.gameObject.transform.position.z));
		lineRenderer.SetPosition(2, new Vector3(this.gameObject.transform.position.x + 2 + x2, y2, this.gameObject.transform.position.z));
		lineRenderer.SetPosition(3, new Vector3(this.gameObject.transform.position.x + 2 + x3, y3, this.gameObject.transform.position.z));
		lineRenderer.SetPosition(4, new Vector3(this.gameObject.transform.position.x + 2 + x4, y4, this.gameObject.transform.position.z));
		lineRenderer.SetPosition(5, new Vector3(this.gameObject.transform.position.x + 2 + x5, y5, this.gameObject.transform.position.z));
		lineRenderer.SetPosition(6, new Vector3(this.gameObject.transform.position.x + 2 + x6, y6, this.gameObject.transform.position.z));
		lineRenderer.SetPosition(7, new Vector3(this.gameObject.transform.position.x + 2 + x7, y7, this.gameObject.transform.position.z));
		lineRenderer.SetPosition(8, new Vector3(this.gameObject.transform.position.x + 2 + x8, y8, this.gameObject.transform.position.z));
	}
}

public function resetShot():void
{	
	vx = 0.0f;
	vy = 0.0f;
	x8 = 2.3f;
	lineRenderer.enabled = false;
}

public function shoot(shroomType:int):IEnumerator
{
	if(soundEngine != null) soundEngine.setAim(false);
	
	if(!gameLogic.getInfiniteAmmo()) 
	{
		if(shroomType == 0) gameLogic.decreaseNormalSeeds();
		else if(shroomType == 1) gameLogic.decreaseBumpySeeds();
	}
		
	//create new seed
	var newSeed:GameObject = null;
	if (getDirection() == "Right")
	{
		newSeed = Instantiate(Resources.Load("Prefabs/Seed", GameObject));
		newSeed.transform.position = this.gameObject.transform.position + new Vector3(2, 1, 0);
	}
	else
	{
		newSeed = Instantiate(Resources.Load("Prefabs/Seed", GameObject));
		newSeed.transform.position = this.gameObject.transform.position - new Vector3(2, -1, 0);
	}
	newSeed.gameObject.name = "Seed";
	newSeed.gameObject.transform.parent = GameObject.Find("SeedContainer").gameObject.transform;
	newSeed.gameObject.GetComponent(SeedBehaviour).setShroomType(shrooms[shroomType]);
	
	if(soundEngine != null)
	{
		soundEngine.playSoundEffect("shoot");
	}
	
	if (getDirection() == "Left") vx = -vx;
	newSeed.rigidbody.velocity = new Vector3(vx, vy, 0.0f);
	
	yield WaitForSeconds(1.5f);	//wait for 1.5 sec to reset the shot, removing the arc
	resetShot();
}

public function flash():void
{
	var hit:RaycastHit;
	var direction:Vector3;
	var layerMask:int = 1 << 8;
	layerMask = ~layerMask;
	if (getDirection() == "Right") direction = new Vector3(1.0f, 0.0f, 0.0f);
	else if (getDirection() == "Left") direction = new Vector3(-1.0f, 0.0f, 0.0f);
	if (Physics.Raycast(this.gameObject.transform.position, direction, hit, 20.0f, layerMask))
	{
		if (hit.collider.gameObject.name == "Slug") hit.collider.gameObject.GetComponent(SlugScript).toFleeState();
		if (hit.collider.gameObject.name == "Web") hit.collider.gameObject.SetActive(false);
	}
	if(flashBool == false)
	{	
		flashlight.SetActive(true);
		flashBool = true;
		gameLogic.decreaseBatteryBy(5.0f);
		if(soundEngine != null)
		{
			soundEngine.playSoundEffect("flash");
		}
	}
}

public function addCrystal(crystal:GameObject):void
{
	crystals.Add(crystal);
}

private function setDirection(direction:String):void
{
	if(lastDirection != direction)
	{
		lastDirection = direction;
	}
}

private function getDirection():String
{
	return lastDirection;
}

public function bounceShroomY():void
{
	this.gameObject.rigidbody.velocity.x = 0.0f;
	this.gameObject.rigidbody.velocity.y = 15.0f;
}

public function bounceShroomX():void
{
	var velocity:float = this.gameObject.rigidbody.velocity.x;
	if(Mathf.Abs(velocity) < 0.4)
	{
		if(velocity < 0.0f) velocity = -3.0f;
		else if(velocity > 0.0f) velocity = 3.0f;
	}
	
	velocity *= -4.0f;
	if(velocity > 10.0f)
	{
		velocity = 10.0f;
	}
	else if(velocity < -10.0f)
	{
		velocity = -10.0f;
	}
	this.gameObject.rigidbody.velocity.x = velocity;
}

//check the battery status and change the texture if needed
private function checkBatteryStatus():void
{
	var maxBattery			:float = gameLogic.getBatteryCapacity();
	var currentBatteryPower	:float = gameLogic.getBattery();
	
	var amountOfBatteryBars:int = currentBatteryPower / (maxBattery / 10);
	
	if(amountOfBatteryBars >= 6)
	{
		if(currentRoverTexture != highBatteryRoverTexture)
		{
			currentRoverTexture = highBatteryRoverTexture;
			currentMaterial.SetTexture(0, highBatteryRoverTexture);
		}
	}
	else if(amountOfBatteryBars >= 4 && amountOfBatteryBars <= 5)
	{		
		if(currentRoverTexture != yellowBatteryRoverTexture)
		{
			currentRoverTexture = yellowBatteryRoverTexture;
			currentMaterial.SetTexture(0, yellowBatteryRoverTexture);
		}
	}
		
	else if(amountOfBatteryBars <= 3)
	{
		if(currentRoverTexture != redBatteryRoverTexture)
		{
			currentRoverTexture = redBatteryRoverTexture;
			currentMaterial.SetTexture(0, redBatteryRoverTexture);
		}
	}
}