using UnityEngine;
using System.Collections;

public class pickupShard : MonoBehaviour {
	
	
	private GameLogic gameLogicScript;
	
	void Start (){
		gameLogicScript = GameObject.Find("GameLogic").GetComponent<GameLogic>() as GameLogic;
	}
	
	public void OnTriggerEnter ( Collider collider  ){
		if(collider.gameObject.name == "Player")
		{
			gameLogicScript.addShardScore();
			Destroy(this.gameObject);
		}
	}
}