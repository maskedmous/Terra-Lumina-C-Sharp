#pragma strict

import System.Collections.Generic;

private var cam:Camera;
private var speed:float = 150.0f;
private var timer:float = 1.5f;
private var startTimer:float = 2.0f;

private var crystals:GameObject[];
private var crystalPositions:List.<Vector3> = new List.<Vector3>();

private var currentCrystal:int = 0;
private var startPos:Vector3 = Vector3.zero;
private var targetPos:Vector3 = Vector3.zero;

private var gameLogic:GameLogic = null;

private var tutorialTexture:Texture2D = null;
private var tutorialRect:Rect = new Rect(0.0f, 0.0f, 0.0f, 0.0f);
public var textureX:float = 500.0f;
public var textureY:float = 500.0f;

private var cameraScript:CameraScript = null;

public function Awake():void
{
	cameraScript = Camera.main.GetComponent("CameraScript") as CameraScript;
	cameraScript.setMove(false);
}

public function Start ():void
{
	cam = Camera.main;
	cam.transform.position.x = GameObject.Find("EndLevelTrigger").transform.position.x;
	
	crystals = GameObject.FindGameObjectsWithTag("Pickup");
	for (var i:int = 0; i < crystals.length; ++i)
	{
		crystalPositions.Add(crystals[i].transform.position);
	}
	crystalPositions.RemoveAt(0);
	crystalPositions = sort(crystalPositions);
	
	startPos = Camera.main.gameObject.transform.position;
	targetPos = crystalPositions[0];
	
	gameLogic = GameObject.Find("GameLogic").GetComponent(GameLogic);
	gameLogic.stopBattery();
	
	tutorialTexture = Resources.Load("Textures/ScoreExplanation", Texture2D) as Texture2D;
	//tutorialRect = new Rect(Screen.width / 2 + tutorialTexture.width / 2, Screen.height / 2 - tutorialTexture.height / 2, tutorialTexture.width, tutorialTexture.height);
	//zonder scale proberen?
}

public function Update ():void
{
	if (startTimer > 0) startTimer -= Time.deltaTime;
	else moveCamera();
	if (cam.transform.position.x < -19.0f)
	{
		speed = 0.0f;
		cam.transform.position.x = -19.082f;
		startGame();
	}
	if (Input.GetKey(KeyCode.S) && Input.GetKey(KeyCode.K))
	{
		speed = 0.0f;
		cam.transform.position.x = -19.082f;
		startGame();
	}
}

public function OnGUI():void
{
	if(tutorialTexture != null) GUI.DrawTexture(scaleRect(new Rect(textureX, textureY, tutorialTexture.width, tutorialTexture.height)), tutorialTexture);
}

private function scaleRect(rect:Rect):Rect
{
	var scale:Vector2 = new Vector2(Screen.width / 1920.0f, Screen.height / 1080.0f);
	return new Rect(rect.x * scale.x, rect.y * scale.y, rect.width * scale.x, rect.height * scale.y);
}

private function moveCamera():void
{
	var camPos:Vector3 = Camera.main.gameObject.transform.position;
	Camera.main.gameObject.transform.position = Vector3.MoveTowards(camPos, targetPos, speed * Time.deltaTime);
	Camera.main.gameObject.transform.position.z = 6.0f;

	if (Mathf.Abs(camPos.x - targetPos.x) < 0.2f) {
		timer -= Time.deltaTime;
		speed = 0.0f;
		if (timer < 0) {
			timer = 1.5f;
			currentCrystal++;
			startPos = targetPos;
			targetPos = crystalPositions[currentCrystal];
			speed = 140.0f;
		}
	}
}

private function sort(list:List.<Vector3>):List.<Vector3>
{
	var newList:List.<Vector3> = new List.<Vector3>();
	var highest:float = int.MinValue;
	var index:int = 0;
	var length:int = list.Count;
	
	for (var l:int = length; l > 0; --l) {
		for (var i:int = 0; i < l; ++i) {
			if (list[i].x > highest) {
				highest = list[i].x;
				index = i;
			}
		}
		newList.Add(list[index]);
		list.RemoveAt(index);
		highest = int.MinValue;
	}
	newList.Add(GameObject.Find("Player").transform.position);
	return newList;
}

private function startGame():void
{
	var tutorialTriggerScript:TutorialTriggerScript = this.gameObject.GetComponent("TutorialTriggerScript") as TutorialTriggerScript;
	var playerInputScript:PlayerInputScript = GameObject.Find("Player").GetComponent("PlayerInputScript") as PlayerInputScript;
	
	
	tutorialTriggerScript.setMovementLeftEnabled(true);
	tutorialTriggerScript.setCameraMoving(false);
	playerInputScript.setMovementLeftEnabled(true);
	cameraScript.setMove(true);
	
	gameLogic.startBattery();
	
	Destroy(this);
}