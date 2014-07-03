// Converted from UnityScript to C# at http://www.M2H.nl/files/js_to_c.php - by Mike Hergaarden
// Do test the code! You usually need to change a few small bits.

using UnityEngine;
using System.Collections;

public class MoveState:State {
	
	private string direction = "right";
	
	public GameObject slugBoundA;
	public GameObject slugBoundB;
	
	private string difficulty = "";
	
	void  Start (){
		difficulty = parentScript.getDifficulty();
		slugBoundA = parentScript.getSlugBoundA();
		slugBoundB = parentScript.getSlugBoundB();
	}		
	
	void update (){
		parent.rigidbody.velocity.x = speed * Time.deltaTime;
		
		if (difficulty == "Hard") {
			
			Vector3 rayStart = parent.transform.position + new Vector3(0.0f, 0.3f, 0.0f);
			Vector3 vectorDirection = Vector3.zero;
			RaycastHit hitSide;
			Vector3 playerPos = target.transform.position;
			float distanceToPlayer= Vector3.Distance(parent.transform.position, playerPos);
			
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
	
	void OnTriggerEnter ( Collider collider  ){
		if (collider.gameObject == slugBoundA || collider.gameObject == slugBoundB) {
			speed = -speed;
			parent.gameObject.transform.rotation.eulerAngles.y += 180;
			if (direction == "Right") direction = "Left";
			else direction = "Right";
		}
	}
	
	void setDirection ( string dir  ){
		direction = dir;
	}
}