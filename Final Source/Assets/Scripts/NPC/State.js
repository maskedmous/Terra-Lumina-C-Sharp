#pragma strict

protected var parent:GameObject = null;
protected var parentScript:SlugScript = null;

protected var target:GameObject = null;

protected var speed:float = 70.0f;

protected var layerMask:int = 0;

function Awake () {
	parent = this.gameObject;
	parentScript = this.gameObject.GetComponent("SlugScript") as SlugScript;
	
	target = GameObject.Find("Player") as GameObject;
	
	layerMask = 1 << 8;
	layerMask = ~layerMask;
}

function update():void {
}

function bouncePlayer(direction:String):void {
	if (direction == "right") {
		target.rigidbody.velocity.x = 15.0f;
	}
	else if (direction == "left") {
		target.rigidbody.velocity.x = -15.0f;
	}
}