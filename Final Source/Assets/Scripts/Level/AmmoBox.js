#pragma strict

public  var extraSeeds:int = 5;
public  var normalType:boolean = true;
public  var bumpyType:boolean = false;
public  var timeToRespawn:float = 20.0f;
public  var oneTimePickup:boolean = false;
private var counter:float = 0.0f;
private var available:boolean = true;
private var gameLogic:GameLogic = null;

public function Awake():void
{
	gameLogic = GameObject.Find("GameLogic").GetComponent(GameLogic) as GameLogic;
	if(gameLogic == null) Debug.LogError("GameLogic is null");
}

public function Update():void
{
	//if the pick up is not available (aka it is already picked up)
	if(available == false)
	{
		//if the counter is higher than 0
		if (counter > 0.0f)
		{
			counter -= Time.deltaTime;
			if(counter <= 0.0f) counter = 0.0f;	//set counter to 0.0f if lower than 0
		}
		//once the counter reached 0
		else if (counter == 0.0f)
		{
			turnPickUpOn();	//turn the pickup on
		}
	}
}

private function turnPickUpOn():void
{
	this.gameObject.renderer.enabled = true;
	this.gameObject.collider.enabled = true;
	available = true;
}

private function turnPickUpOffTemp():void
{
	this.gameObject.renderer.enabled = false;
	this.gameObject.collider.enabled = false;
	counter = timeToRespawn;
	available = false;
}

private function turnPickUpOffPerm():void
{
	if(this.gameObject.transform.parent.name == "AmmoBox")
	{
		Destroy(this.gameObject.transform.parent.gameObject);
	}
	else
	{
		Destroy(this.gameObject);
	}
}

public function OnTriggerEnter(object:Collider):void
{
	if(available == true)
	{
		if(object.gameObject.name == "Player")
		{
			sendAmmo();
			
			if(!oneTimePickup)
			{
				turnPickUpOffTemp();
			}
			else
			{
				turnPickUpOffPerm();
			}
		}
	}
}

private function sendAmmo():void
{
	if(gameLogic != null)
	{
		//if both types are selected
		if(normalType && bumpyType)
		{
			gameLogic.addAmmo(extraSeeds);
		}
		//if normal type seed
		else if(normalType == true && bumpyType == false)
		{
			gameLogic.addAmmo(extraSeeds, 0);
		}
		//if bympy type seed
		else if(bumpyType == true && normalType == false)
		{
			gameLogic.addAmmo(extraSeeds, 1);
		}
	}
	else Debug.LogError("Can't add Ammo cause gameLogic is null");
}


/*

Getters

*/
public function getTimeToRespawn():float
{
	return timeToRespawn;
}

public function getExtraSeeds():int
{
	return extraSeeds;
}

public function getOneTimePickup():boolean
{
	return oneTimePickup;
}

public function getNormalType():boolean
{
	return normalType;
}

public function getBumpyType():boolean
{
	return bumpyType;
}

/*

Setters

*/
public function setExtraSeeds(value:int):void
{
	extraSeeds = value;
}

public function setTimeToRespawn(value:float):void
{
	timeToRespawn = value;
}

public function setOneTimePickup(value:boolean):void
{
	oneTimePickup = value;
}

public function setNormalType(value:boolean):void
{
	normalType = value;
}

public function setBumpyType(value:boolean):void
{
	bumpyType = value;
}
