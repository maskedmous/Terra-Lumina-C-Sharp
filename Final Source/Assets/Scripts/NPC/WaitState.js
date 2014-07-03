#pragma strict

private var waitTime:float = 10.2f;

private var player:GameObject = null;

class WaitState extends State {
	
	function update():void {
		waitTime -= Time.deltaTime;
		if (waitTime < 0) appear();
	}
	
	function toMoveState():void {
		parentScript.toMoveState();
	}
	
	function appear():void {
//		var startPosition:Vector3 = this.parentScript.getStart();
//		var playerPosition:Vector3 = target.transform.position;
//		
//		if (Vector3.Distance(startPosition, playerPosition) < 3.0f) {
//			findNewSpawnPoint();
//		}
//		else {
//			this.parent.transform.position = startPosition;
//		}
		
		parent.collider.enabled = true;
		parent.rigidbody.isKinematic = false;
		parent.rigidbody.useGravity = true;
		waitTime = 10.2f;
		
		toMoveState();
	}
}