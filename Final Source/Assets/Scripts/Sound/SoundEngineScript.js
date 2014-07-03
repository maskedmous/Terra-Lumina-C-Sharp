#pragma strict

private var volume:float = 1.0f;
//private var muteBool:boolean;
static var soundEngineExists:boolean = false;

private var aim:boolean = false;
private var drive:boolean = false;
private var driveTimer:float = 0.0f;
private var driveTimerBool:boolean = false;

private var menuSound:AudioClip;
private var easySound:AudioClip;
private var mediumSound:AudioClip;
private var hardSound:AudioClip;

private var bounceSound:AudioClip;
private var jumpSound:AudioClip;
private var shootingSound:AudioClip;
private var slugForwardSound:AudioClip;
private var slugBackwardSound:AudioClip;
private var roverDriveSound:AudioClip;
private var flashSound:AudioClip;
private var roverStartSound:AudioClip;
private var roverStopSound:AudioClip;
private var roverAimSound:AudioClip;
private var rockBreakingSound:AudioClip;
private var sunChargingSound:AudioClip;
private var crystalPickup:AudioClip;
private var winSound:AudioClip;
private var loseSound:AudioClip;

public function Awake():void
{
	
	
	if(soundEngineExists == false) {
		audio.volume = volume;
		audio.loop = true;
		soundEngineExists = true;
		DontDestroyOnLoad(this.gameObject);
		
		menuSound = Resources.Load("SoundEffects/Lumina Menu", AudioClip);
		easySound = Resources.Load("SoundEffects/MusicEasy", AudioClip);
		mediumSound = Resources.Load("SoundEffects/MusicMedium", AudioClip);
		hardSound = Resources.Load("SoundEffects/MusicHard", AudioClip);
		
		bounceSound = Resources.Load("SoundEffects/Shroom Bounce", AudioClip);
		jumpSound = Resources.Load("SoundEffects/Rover Jump New 3", AudioClip);
		shootingSound = Resources.Load("SoundEffects/Rover Shoot", AudioClip);
		slugForwardSound = Resources.Load("SoundEffects/Move forward", AudioClip);
		slugBackwardSound = Resources.Load("SoundEffects/Move backwards", AudioClip);
		roverDriveSound = Resources.Load("SoundEffects/Rover Drive New", AudioClip);
		roverStartSound = Resources.Load("SoundEffects/Rover Drive Start New", AudioClip);
		roverStopSound = Resources.Load("SoundEffects/Rover Drive Stop New", AudioClip);
		roverAimSound = Resources.Load("SoundEffects/Rover Aim", AudioClip);
		flashSound = Resources.Load("SoundEffects/Rover Flashlight", AudioClip);
		rockBreakingSound = Resources.Load("SoundEffects/Rock Barrier Break v2", AudioClip);
		crystalPickup = Resources.Load("SoundEffects/DiamondPickup", AudioClip);
		//sunChargingSound = Resources.Load("SoundEffects/...", AudioClip);
		winSound = Resources.Load("SoundEffects/Winsound v2", AudioClip);
		loseSound = Resources.Load("SoundEffects/Losesound v1", AudioClip);
		
		changeMusic("Menu");
	}
	else
	{
		Destroy(this.gameObject);
	}
}

public function Update():void {
	if(gameObject.Find("Player")){
		this.gameObject.transform.position = gameObject.Find("Player").transform.position;
	}
	if(driveTimerBool == true){
		driveTimer += Time.deltaTime;
		if(driveTimer >= 1.0f){
			driveTimerBool = false;
			driveTimer = 0.0f;
		}
	}
}

public function changeMusic(name:String):void
{
	audio.Stop();
	if(name == "Menu")
	{
		audio.clip = menuSound;
		audio.Play();
	}
	if(name == "Easy")
	{
		audio.clip = easySound;
		audio.Play();
	}
	if(name == "Medium")
	{
		audio.clip = mediumSound;
		audio.Play();
	}
	if(name == "Hard")
	{
		audio.clip = hardSound;
		audio.Play();
	}
}

// give a number between 0 and 1 for the volume
public function changeVolume(newVolume:float):void
{
	if(newVolume > 1.0f)
	{
		newVolume = 1.0f;
		Debug.LogError("Trying to set the volume higher than 1, corrected it to 1");
	}
	else if (newVolume < 0.0f)
	{
		newVolume = 0.0f;
		Debug.LogError("Trying to set the volume lower than 0, corrected it to 0");
	}
	
	audio.volume = newVolume;
	volume = newVolume;
}

public function getVolume():float
{
	return volume;
}

public function changeMute(newMute:boolean):void
{
	audio.mute = newMute;
}

public function changeLoop(newLoop:boolean):void
{
	audio.loop = newLoop;
}

public function playSoundEffect(name:String):void {
	if(name == "bounce"){
		audio.PlayOneShot(bounceSound);
	}
	if(name == "jump"){
		audio.PlayOneShot(jumpSound);
	}
	if(name == "shoot"){
		audio.PlayOneShot(shootingSound);
	}
	if(name == "slugForward"){
		audio.PlayOneShot(slugForwardSound);
	}
	if(name == "slugBackward"){
		audio.PlayOneShot(slugBackwardSound);
	}
	if(name == "roverDrive"){
		if(driveTimer == 0.0f)
		{
			audio.PlayOneShot(roverDriveSound);
			driveTimerBool = true;	
		}	
	}
	if(name == "roverStart"){
		audio.PlayOneShot(roverStartSound);
	}
	if(name == "roverStop"){
		audio.PlayOneShot(roverStopSound);
	}
	if(name == "aim"){
		if(aim == true){
			audio.PlayOneShot(roverAimSound);
		}
	}
	if(name == "flash"){
		audio.PlayOneShot(flashSound);
	}
	if(name == "rock"){
		audio.PlayOneShot(rockBreakingSound);
	}
	if(name == "pickup"){
		audio.PlayOneShot(crystalPickup);
	}
	if(name == "sun"){
		audio.PlayOneShot(sunChargingSound);
	}
	if(name == "win"){
		audio.PlayOneShot(winSound);
	}
	if(name == "lose"){
		audio.PlayOneShot(loseSound);
	}
}

public function setAim(aimBool:boolean):void {
	aim = aimBool;
}

public function setDrive(driveBool:boolean):void {
	drive = driveBool;
}

public function getDrive():boolean {
	return drive;
}

public function playMusic():void {
	this.audio.Play();
}

public function stopMusic():void {
	this.audio.Stop();
}