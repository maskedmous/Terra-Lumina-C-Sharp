// Converted from UnityScript to C# at http://www.M2H.nl/files/js_to_c.php - by Mike Hergaarden
// Do test the code! You usually need to change a few small bits.

using UnityEngine;
using System.Collections;

public class Crystal : MonoBehaviour {
	
	
	private SoundEngineScript soundEngine = null;
	
	
	void Awake (){
		if(Application.loadedLevelName == "LevelLoaderScene")
		{
			soundEngine = GameObject.Find("SoundEngine").GetComponent<SoundEngineScript>() as SoundEngineScript;
		}
	}
	void  OnTriggerEnter ( Collider collider  ){
		if(collider.gameObject.name == "Player")
		{
			GameObject.Find("GameLogic").GetComponent<GameLogic>().addCrystalSample(this.gameObject);
			if(soundEngine != null)
			{
				soundEngine.playSoundEffect("pickup");
			}
			Destroy(this.gameObject);
		}
	}
}