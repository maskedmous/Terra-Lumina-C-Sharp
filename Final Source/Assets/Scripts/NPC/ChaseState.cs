// Converted from UnityScript to C# at http://www.M2H.nl/files/js_to_c.php - by Mike Hergaarden
// Do test the code! You usually need to change a few small bits.

using UnityEngine;
using System.Collections;

public class ChaseState:State {
	
	new void update (){
		
		Vector3 rayStart = parent.transform.position + new Vector3(0.0f, 0.3f, 0.0f);
		RaycastHit hitSide;
		Vector3 playerPos = target.transform.position;
		float distanceToPlayer= Vector3.Distance(parent.transform.position, playerPos);
		
		if (Mathf.Abs(parent.transform.position.y - playerPos.y) < 1.0f) {
			if (rayStart.x > playerPos.x) {
				if (Physics.Raycast(rayStart, Vector3.left, out hitSide, Mathf.Infinity, layerMask)) {
					Debug.Log(hitSide.collider.gameObject.name);
					if (hitSide.collider.name == "Player" || hitSide.distance > distanceToPlayer) {	
						moveToTarget(Vector3.left);
					}
					else {
						parentScript.toReturnState();
					}
				}
			}
			else {
				if (Physics.Raycast(rayStart, Vector3.right, out hitSide, Mathf.Infinity, layerMask)) {
					Debug.Log(hitSide.collider.gameObject.name);
					if (hitSide.collider.name == "Player" || hitSide.distance > distanceToPlayer) {	
						moveToTarget(Vector3.right);
					}
					else {
						parentScript.toReturnState();
					}
				}
			}
		}
		else {
			parentScript.toReturnState();
		}
	}
	
	private void moveToTarget ( Vector3 direction  ){
		parent.rigidbody.velocity = direction * Time.deltaTime * speed;
	}
}