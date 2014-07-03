// Converted from UnityScript to C# at http://www.M2H.nl/files/js_to_c.php - by Mike Hergaarden
// Do test the code! You usually need to change a few small bits.

using UnityEngine;
using System.Collections;

public class State : MonoBehaviour {

	protected GameObject parent = null;
	protected SlugScript parentScript = null;
	
	protected GameObject target = null;
	
	protected float speed = 70.0f;
	
	protected int layerMask = 0;
	
	void  Awake (){
		parent = this.gameObject;
		parentScript = this.gameObject.GetComponent("SlugScript") as SlugScript;
		
		target = GameObject.Find("Player") as GameObject;
		
		layerMask = 1 << 8;
		layerMask = ~layerMask;
	}
	
	void update (){
	}
	
	void bouncePlayer ( string direction  ){
		if (direction == "right") {
			target.rigidbody.velocity.x = 15.0f;
		}
		else if (direction == "left") {
			target.rigidbody.velocity.x = -15.0f;
		}
	}
}