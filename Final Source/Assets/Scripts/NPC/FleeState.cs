// Converted from UnityScript to C# at http://www.M2H.nl/files/js_to_c.php - by Mike Hergaarden
// Do test the code! You usually need to change a few small bits.

using UnityEngine;
using System.Collections;

public class FleeState:State {
	
	new void update (){
		toWaitState();
	}
	
	void toWaitState (){
		float zero = 0.0f;
		parent.rigidbody.velocity = new Vector3(zero, parent.rigidbody.velocity.y, parent.rigidbody.velocity.z);
		parent.collider.enabled = false;
		parent.rigidbody.useGravity = false;
		parent.rigidbody.isKinematic = true;
		parentScript.toWaitState();
	}
}