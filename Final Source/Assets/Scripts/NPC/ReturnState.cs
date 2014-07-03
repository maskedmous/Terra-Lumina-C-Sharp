using UnityEngine;
using System.Collections;

public class ReturnState:State {
		
	private Vector3 startPosition = Vector3.zero;
	
	void  Start (){
		startPosition = parentScript.getStart();
	}
	
	void update (){
		Vector3 parentPosition = parent.transform.position;
		if (parentPosition.x < startPosition.x) speed = 70.0f;
		else if (parentPosition.x > startPosition.x) speed = -70.0f;
		parent.rigidbody.velocity.x = Time.deltaTime * speed;
		if (Vector3.Distance(parentPosition, startPosition) < 1.0f) {
			parentScript.toMoveState();
		}
	}
}