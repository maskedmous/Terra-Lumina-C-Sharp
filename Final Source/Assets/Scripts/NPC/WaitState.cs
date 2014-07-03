using UnityEngine;
using System.Collections;

public class WaitState:State {

	private float waitTime = 10.2f;
	
	public override void update ()
	{
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