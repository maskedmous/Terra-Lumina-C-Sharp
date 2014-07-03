using UnityEngine;
using System.Collections;

public class MoveState:State
{
	
	private string direction = "right";
	
	public GameObject slugBoundA;
	public GameObject slugBoundB;
	
	private string difficulty = "";
	
	public void  Start (){
		difficulty = parentScript.getDifficulty();
		slugBoundA = parentScript.getSlugBoundA();
		slugBoundB = parentScript.getSlugBoundB();
	}		
	
	public override void update ()
	{
		parent.rigidbody.velocity = new Vector3(speed * Time.deltaTime, parent.rigidbody.velocity.y, parent.rigidbody.velocity.z);
		
		if (difficulty == "Hard")
		{
			Vector3 rayStart = parent.transform.position + new Vector3(0.0f, 0.3f, 0.0f);
			RaycastHit hitSide;
			Vector3 playerPos = target.transform.position;
			float distanceToPlayer= Vector3.Distance(parent.transform.position, playerPos);
			
			if (Mathf.Abs(parent.transform.position.y - playerPos.y) < 1.0f) {
				if (rayStart.x > playerPos.x) {
					if (Physics.Raycast(rayStart, Vector3.left, out hitSide, Mathf.Infinity, layerMask)) {
						Debug.Log(hitSide.collider.gameObject.name);
						if (hitSide.collider.name == "Player" || hitSide.distance > distanceToPlayer) {	
							parentScript.toChaseState();
						}
					}
				}
				else {
					if (Physics.Raycast(rayStart, Vector3.right, out hitSide, Mathf.Infinity, layerMask)) {
						Debug.Log(hitSide.collider.gameObject.name);
						if (hitSide.collider.name == "Player" || hitSide.distance > distanceToPlayer) {	
							parentScript.toChaseState();
						}
					}
				}
			}
		}
	}
	
	public void OnTriggerEnter ( Collider collider  )
	{
		if (collider.gameObject == slugBoundA || collider.gameObject == slugBoundB) {
			speed = -speed;
			Vector3 newRotation = parent.transform.rotation.eulerAngles;
			//newRotation.x = parent.transform.rotation.eulerAngles.x;
			newRotation.y = parent.transform.rotation.eulerAngles.y + 180.0f;
			//newRotation.z = parent.transform.rotation.eulerAngles.z;
			parent.transform.eulerAngles = newRotation;
			if (direction == "Right") direction = "Left";
			else direction = "Right";
		}
	}
	
	public void setDirection ( string dir  ){
		direction = dir;
	}
}