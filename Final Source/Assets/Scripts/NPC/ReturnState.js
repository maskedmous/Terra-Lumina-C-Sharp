#pragma strict

class ReturnState extends State {
	
	private var startPosition:Vector3 = Vector3.zero;
	
	function Start () {
		startPosition = parentScript.getStart();
	}

	function update ():void {
		var parentPosition:Vector3 = parent.transform.position;
		if (parentPosition.x < startPosition.x) speed = 70.0f;
		else if (parentPosition.x > startPosition.x) speed = -70.0f;
		parent.rigidbody.velocity.x = Time.deltaTime * speed;
		if (Vector3.Distance(parentPosition, startPosition) < 1.0f) {
			parentScript.toMoveState();
		}
	}
}