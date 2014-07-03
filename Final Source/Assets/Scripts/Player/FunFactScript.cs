// Converted from UnityScript to C# at http://www.M2H.nl/files/js_to_c.php - by Mike Hergaarden
// Do test the code! You usually need to change a few small bits.

using UnityEngine;
using System.Collections;

public class FunFactScript : MonoBehaviour {
	
	private GameObject label = null;
	private LabelScript labelScript = null;
	
	void Awake (){
		label = Instantiate(Resources.Load("Prefabs/Label")) as GameObject;
		label.gameObject.name = "Label";
		labelScript = label.GetComponent("LabelScript") as LabelScript;
	}
	
	public void displayFact (){
		labelScript.displayFact();
	}
	
	public void stopDisplay (){
		labelScript.stopDisplay();
	}
}