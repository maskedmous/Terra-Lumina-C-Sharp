// Converted from UnityScript to C# at http://www.M2H.nl/files/js_to_c.php - by Mike Hergaarden
// Do test the code! You usually need to change a few small bits.

using UnityEngine;
using System.Collections;

public class WaitState:State {

	private float waitTime = 10.2f;
	
	new void update (){
		waitTime -= Time.deltaTime;
		if (waitTime < 0) appear();
	}
	
	void toMoveState (){
		parentScript.toMoveState();
	}
	
	void appear (){
		
		parent.collider.enabled = true;
		parent.rigidbody.isKinematic = false;
		parent.rigidbody.useGravity = true;
		waitTime = 10.2f;
		
		toMoveState();
	}
}