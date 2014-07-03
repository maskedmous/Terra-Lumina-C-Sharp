#pragma strict

private var target:GameObject = null;
private var move:boolean = true;

function Start ():void {
	target = GameObject.Find("Player") as GameObject;
}

function Update ():void {
	 if (move)
	 {	
	 	this.gameObject.transform.position.x = target.transform.position.x;
	 	this.gameObject.transform.position.y = target.transform.position.y + 5.0f;
	 }
}

public function setMove(value:boolean):void
{
	move = value;
}