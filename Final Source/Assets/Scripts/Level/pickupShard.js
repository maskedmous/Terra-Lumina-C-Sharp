#pragma strict

private var gameLogicScript:GameLogic;

function Start():void
{
	gameLogicScript = GameObject.Find("GameLogic").GetComponent(GameLogic) as GameLogic;
}

public function OnTriggerEnter(collider:Collider):void
{
	if(collider.gameObject.name == "Player")
	{
		gameLogicScript.addShardScore();
		Destroy(this.gameObject);
	}
}