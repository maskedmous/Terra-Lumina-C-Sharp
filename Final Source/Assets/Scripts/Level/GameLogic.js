 #pragma strict

/*
	Battery variables
*/
public var battery:float = 100.0f;					//current battery
public var maximumBatteryCapacity:int = 100.0f;		//max battery
public var decreaseTimer:float = 1.0f;				//every x seconds it decreases battery
public var negativeBatteryFlow:int = 1;			//amount of battery that it decreases
public var positiveBatteryFlow:int = 2;			//amount of battery that it increases
private var charging:boolean = false;
private var stopBatteryBool = false;
/*
	Win Variables
*/
public var crystalsToComplete:int = 0;
public var maxCrystals:int = 3;
private var score:int = 0;
private var lose:boolean = false;

/*
	Player Variables
*/
private var playerController:PlayerController = null;
private var playerInput:PlayerInputScript = null;
private var soundEngine:SoundEngineScript = null;
public var infiniteAmmo:boolean = false;
public var currentNormalSeeds:int = 0;
public var maximumNormalSeeds:int = 0;
public var currentBumpySeeds:int = 0;
public var maximumBumpySeeds:int = 0;

/*
	Action energy cost
*/
private var jumpDrain:float = 0.0f;
private var shootDrain:float = 0.0f;
private var pickUpDrain:float = 0.0f;
private var placeDrain:float = 0.0f;
private var flashDrain:float = 0.0f;
private var collectDrain:float = 0.0f;

/*
	Timer variables
*/
private var secondTimer:float = 0.0f;			//counting seconds
private var levelTimer:float = 0.0f;
private var runTimer:boolean = true;
private var timerInt:int = 0;

/*
	Score Variables
*/
public var techieTime:int 	= 45;

public var platinumTime:int = 50;
public var goldTime:int 	= 80;
public var silverTime:int 	= 130;
public var bronzeTime:int 	= 180;

public var shardScore:int 	= 100;

/*
	Array Variables
*/
private var crystalSamples:Array = new Array();

/*
	Other
*/
private var levelTriggerScript:LevelTrigger = null;
private var anim:Animator;


public function Start():void
{
	startTimer();
	var player:GameObject = GameObject.Find("Player");
	playerController = player.GetComponent(PlayerController) as PlayerController;
	playerInput = player.GetComponent(PlayerInputScript) as PlayerInputScript;
	
	if(Application.loadedLevelName == "LevelLoaderScene")
	{
		soundEngine = GameObject.Find("SoundEngine").GetComponent(SoundEngineScript) as SoundEngineScript;
	}
	
	anim = player.GetComponent(Animator);
}


public function Update():void
{
	if(levelTriggerScript == null && GameObject.Find("EndLevelTrigger") != null)
	{
		levelTriggerScript = GameObject.Find("EndLevelTrigger").GetComponent(LevelTrigger) as LevelTrigger;
	}

	if(runTimer == true)
	{
		levelTimer += Time.deltaTime;
		timerInt = levelTimer;
	}
	if(!charging)
	{
		decreaseBattery();
		anim.SetBool("isCharging", false);
	}
	else if(secondTimer != 0.0f)
	{
		secondTimer = 0.0f;
	}
	
	if(lose == false)
	{
		checkLose();
	}
}

function decreaseBattery():void
{	
	if(stopBatteryBool == false){
		if(secondTimer > decreaseTimer)
		{
			battery -= negativeBatteryFlow;
			if(battery < 0.0f) battery = 0.0f;
			secondTimer = 0.0f;
		}
		else
		{
			secondTimer += Time.deltaTime;
		}
	}
}

public function decreaseBatteryBy(value:float):void 
{
	battery -= value;
	
	if(battery < 0.0f) battery = 0.0f;	//no negative battery values
}

function addBatteryPower():void
{
	anim.SetBool("isCharging", true);
	if(battery < maximumBatteryCapacity)
	{
		battery += positiveBatteryFlow;
	}
	
	if(battery > maximumBatteryCapacity)
	{
		battery = maximumBatteryCapacity;
		anim.SetBool("isFullyCharged", true);
	}
}

public function stopBattery():void
{
	stopBatteryBool = true;
}

public function startBattery():void
{
	stopBatteryBool = false;
}

function checkLose():void
{
	if(battery <= 0.1f)
	{
		gameOverLose();
		lose = true;
	}
}

function addCrystalSample(newSample:GameObject):void
{
	crystalSamples.push(newSample);
	addScore(100);
}

function getCrystalsSampleCount():int
{
	return crystalSamples.length;
}

function checkWin():boolean 
{
	if(crystalsToComplete <= getCrystalsSampleCount())
	{
		if(getCrystalsSampleCount() == maxCrystals)
		{
			addScore(200);
			return true;
		}
		else return true;
	}
	
	if(crystalsToComplete >= getMaxCrystals())
	{
		if(getCrystalsSampleCount() == getMaxCrystals())
		{
			return true;
		}
	}
	
	return false;
}

function gameOverWin():void
{
	levelTriggerScript.setFinished(true);
	soundEngine.playSoundEffect("win");
}

function gameOverLose():void
{
	levelTriggerScript.setLost(true);
	soundEngine.playSoundEffect("lose");
}

function startTimer():void
{
	runTimer = true;
}

function stopTimer():void
{
	runTimer = false;
	if(levelTriggerScript.getFinished()){
		if(timerInt <= techieTime) 									addScore(500);
		if(timerInt <= platinumTime && timerInt >= techieTime+1) 	addScore(300);
		if(timerInt <= goldTime && timerInt >= platinumTime+1)		addScore(200);
		if(timerInt <= silverTime && timerInt >= goldTime+1) 		addScore(150);
		if(timerInt <= bronzeTime && timerInt >= silverTime+1) 		addScore(100);
		if(timerInt >= bronzeTime) 									addScore(0);
	}
}


/*

Adders

*/

public function addScore(value:int):void
{
	score += value;
}

public function addShardScore():void 
{
	addScore(shardScore);
}

//add ammo to both instances
public function addAmmo(amount:int):void
{
	if(!infiniteAmmo)
	{
		currentNormalSeeds += amount;
		currentBumpySeeds += amount;
		
		if(currentNormalSeeds > maximumNormalSeeds) currentNormalSeeds = maximumNormalSeeds;
		if(currentBumpySeeds > maximumBumpySeeds) currentBumpySeeds = maximumBumpySeeds;
	}
}
//function overload to specify type
public function addAmmo(amount:int, ammoType:int):void
{
	if(!infiniteAmmo)
	{
		//Type 0 = normal, Type 1 = bumpy
		if(ammoType == 0)
		{
			currentNormalSeeds += amount;
			if(currentNormalSeeds > maximumNormalSeeds) currentNormalSeeds = maximumNormalSeeds;
		}
		
		if(ammoType == 1)
		{
			currentBumpySeeds += amount;
			if(currentBumpySeeds > maximumBumpySeeds) currentBumpySeeds = maximumBumpySeeds;
		}
	}
}

/*

Decreasers

*/

public function decreaseNormalSeeds():void
{
	currentNormalSeeds --;
	
	if(currentNormalSeeds == 0) playerInput.setNormalShroomButtonEnabled(false);
}

public function decreaseBumpySeeds():void
{
	currentBumpySeeds --;
	
	if(currentBumpySeeds == 0) playerInput.setBumpyShroomButtonEnabled(false);
}

/*

Getters

*/

//
//Ammo
//

public function getInfiniteAmmo():boolean
{
	return infiniteAmmo;
}

public function getCurrentNormalSeeds():int
{
	return currentNormalSeeds;
}

public function getMaximumNormalSeeds():int
{
	return maximumNormalSeeds;
}

public function getCurrentBumpySeeds():int
{
	return currentBumpySeeds;
}

public function getMaximumBumpySeeds():int
{
	return maximumBumpySeeds;
}

//
//Battery
//
public function getBattery():float
{
	return battery;
}

public function getBatteryCapacity():int
{
	return maximumBatteryCapacity;
}

public function getDecreaseTimer():float
{
	return decreaseTimer;
}

public function getNegativeBatteryFlow():int
{
	return negativeBatteryFlow;
}

public function getPositiveBatteryFlow():int
{
	return positiveBatteryFlow;
}

//
//Player
//
public function getScore():int
{
	return score;
}

public function getJumpDrain():float
{
	return jumpDrain;
}

public function getFlashDrain():float
{
	return flashDrain;
}

public function getCharging():boolean
{
	return charging;
}

//
//Crystals
//

public function getCrystalsToComplete():int
{
	return crystalsToComplete;
}

public function getMaxCrystals():int
{
	return maxCrystals;
}

/*

Timers

*/

public function getPlatinumTime():int
{
	return platinumTime;
}

public function getGoldTime():int
{
	return goldTime;
}

public function getSilverTime():int
{
	return silverTime;
}

public function getBronzeTime():int
{
	return bronzeTime;
}



/*

Setters

*/

//
//Ammo
//
public function setInfiniteAmmo(value:boolean):void
{
	infiniteAmmo = value;
}

public function setCurrentNormalSeeds(value:int):void
{
	currentNormalSeeds = value;
}

public function setMaximumNormalSeeds(value:int):void
{
	maximumNormalSeeds = value;
}

public function setCurrentBumpySeeds(value:int):void
{
	currentBumpySeeds = value;
}

public function setMaximumBumpySeeds(value:int):void
{
	maximumBumpySeeds = value;
}

//
//Battery
//

public function setBattery(value:float):void
{
	battery = value;
}

public function setBatteryCapacity(value:int):void
{
	maximumBatteryCapacity = value;
}

public function setDecreaseTimer(value:float):void
{
	decreaseTimer = value;
}

public function setNegativeBatteryFlow(value:int):void
{
	negativeBatteryFlow = value;
}

function setPositiveBatteryFlow(value:int):void
{
	positiveBatteryFlow = value;
}

//
//Player
//

public function setJumpDrain(value:float):void
{
	jumpDrain = value;
}

public function setFlashDrain(value:float):void
{
	flashDrain = value;
}

public function setCharging(value:boolean):void
{
	charging = value;
}

public function setFullyChargedFalse():void
{
	anim.SetBool("isFullyCharged", false);
}

//
//Crystals
//
public function setCrystalsToComplete(value:int):void
{
	crystalsToComplete = value;
}

public function setMaxCrystals(value:int):void
{
	maxCrystals = value;
}

/*

Timers

*/

public function setPlatinumTime(value:int):void
{
	platinumTime = value;
}

public function setGoldTime(value:int):void
{
	goldTime = value;
}

public function setSilverTime(value:int):void
{
	silverTime = value;
}

public function setBronzeTime(value:int):void
{
	bronzeTime = value;
}











