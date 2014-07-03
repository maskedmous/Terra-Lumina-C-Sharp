#pragma strict

private var soundEngine:SoundEngineScript = null;


function Awake():void
{
	if(Application.loadedLevelName == "LevelLoaderScene")
	{
		soundEngine = GameObject.Find("SoundEngine").GetComponent(SoundEngineScript) as SoundEngineScript;
	}
}
function OnTriggerEnter(collider:Collider)
{
	if(collider.gameObject.name == "Player")
	{
		GameObject.Find("GameLogic").GetComponent(GameLogic).addCrystalSample(this.gameObject);
		if(soundEngine != null)
		{
			soundEngine.playSoundEffect("pickup");
		}
		Destroy(this.gameObject);
	}
}