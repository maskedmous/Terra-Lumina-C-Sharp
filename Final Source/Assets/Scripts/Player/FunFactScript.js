#pragma strict

private var label:GameObject = null;
private var labelScript:LabelScript = null;

function Awake():void {
	label = Instantiate(Resources.Load("Prefabs/Label")) as GameObject;
	label.gameObject.name = "Label";
	labelScript = label.GetComponent("LabelScript") as LabelScript;
}

public function displayFact():void {
	labelScript.displayFact();
}

public function stopDisplay():void {
	labelScript.stopDisplay();
}