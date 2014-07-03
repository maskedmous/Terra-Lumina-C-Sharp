#pragma strict

class MoveState extends State {

	private var direction:String = "right";
	
	public var slugBoundA:GameObject;
	public var slugBoundB:GameObject;
	
	private var difficulty:String = "";
			
	function Start() {
		difficulty = parentScript.getDifficulty();
		slugBoundA = parentScript.getSlugBoundA();
		slugBoundB = parentScript.getSlugBoundB();
	}		
			
	function update():void {
		parent.rigidbody.velocity.x = speed * Time.deltaTime;
		
		if (difficulty == "Hard") {
		
			var rayStart:Vector3 = parent.transform.position + new Vector3(0.0f, 0.3f, 0.0f);
			var vectorDirection:Vector3 = Vector3.zero;
			var hitSide:RaycastHit;
			var playerPos:Vector3 = target.transform.position;
			var distanceToPlayer = Vector3.Distance(parent.transform.position, playerPos);
			
			if (Mathf.Abs(parent.transform.position.y - playerPos.y) < 1.0f) {
				if (rayStart.x > playerPos.x) {
					if (Physics.Raycast(rayStart, Vector3.left, hitSide, Mathf.Infinity, layerMask)) {
						Debug.Log(hitSide.collider.gameObject.name);
						if (hitSide.collider.name == "Player" || hitSide.distance > distanceToPlayer) {	
							parentScript.toChaseState();
						}
					}
				}
				else {
					if (Physics.Raycast(rayStart, Vector3.right, hitSide, Mathf.Infinity, layerMask)) {
						Debug.Log(hitSide.collider.gameObject.name);
						if (hitSide.collider.name == "Player" || hitSide.distance > distanceToPlayer) {	
							parentScript.toChaseState();
						}
					}
				}
			}
		}
	}
	
	function OnTriggerEnter(collider:Collider):void {
		if (collider.gameObject == slugBoundA || collider.gameObject == slugBoundB) {
			speed = -speed;
			parent.gameObject.transform.rotation.eulerAngles.y += 180;
			if (direction == "Right") direction = "Left";
			else direction = "Right";
		}
	}

	function setDirection(dir:String):void {
		direction = dir;
	}
}