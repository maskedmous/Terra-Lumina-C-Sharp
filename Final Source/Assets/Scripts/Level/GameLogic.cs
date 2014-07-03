// Converted from UnityScript to C# at http://www.M2H.nl/files/js_to_c.php - by Mike Hergaarden
// Do test the code! You usually need to change a few small bits.

using UnityEngine;
using System.Collections;

public class GameLogic : MonoBehaviour
{
	
	
/*
	Battery variables
*/
	public float battery = 100.0f;					//current battery
	public int maximumBatteryCapacity = 100;		//max battery
	public float decreaseTimer = 1.0f;				//every x seconds it decreases battery
	public int negativeBatteryFlow = 1;			//amount of battery that it decreases
	public int positiveBatteryFlow = 2;			//amount of battery that it increases
	private bool charging = false;
	private bool stopBatteryBool= false;
/*
	Win Variables
*/
	public int crystalsToComplete = 0;
	public int maxCrystals = 3;
	private int score = 0;
	private bool  lose = false;
	
/*
	Player Variables
*/
	private PlayerController playerController = null;
	private PlayerInputScript playerInput = null;
	private SoundEngineScript soundEngine = null;
	public bool  infiniteAmmo = false;
	public int currentNormalSeeds = 0;
	public int maximumNormalSeeds = 0;
	public int currentBumpySeeds = 0;
	public int maximumBumpySeeds = 0;
	
/*
	Action energy cost
*/
	private float jumpDrain = 0.0f;
	private float shootDrain = 0.0f;
	private float pickUpDrain = 0.0f;
	private float placeDrain = 0.0f;
	private float flashDrain = 0.0f;
	private float collectDrain = 0.0f;
	
/*
	Timer variables
*/
	private float secondTimer = 0.0f;			//counting seconds
	private float levelTimer = 0.0f;
	private bool  runTimer = true;
	private int timerInt = 0;
	
	/*
	Score Variables
*/
	public int techieTime 	= 45;
	
	public int platinumTime = 50;
	public int goldTime 	= 80;
	public int silverTime 	= 130;
	public int bronzeTime 	= 180;
	
	public int shardScore 	= 100;
	
/*
	Array Variables
*/
	private ArrayList crystalSamples = new ArrayList();
	
	/*
	Other
*/
	private LevelTrigger levelTriggerScript = null;
	private Animator anim;
	
	
	public void Start (){
		startTimer();
		GameObject player = GameObject.Find("Player");
		playerController = player.GetComponent<PlayerController>() as PlayerController;
		playerInput = player.GetComponent<PlayerInputScript>() as PlayerInputScript;
		
		if(Application.loadedLevelName == "LevelLoaderScene")
		{
			soundEngine = GameObject.Find("SoundEngine").GetComponent<SoundEngineScript>() as SoundEngineScript;
		}
		
		anim = player.GetComponent<Animator>();
	}
	
	
	public void Update (){
		if(levelTriggerScript == null && GameObject.Find("EndLevelTrigger") != null)
		{
			levelTriggerScript = GameObject.Find("EndLevelTrigger").GetComponent<LevelTrigger>() as LevelTrigger;
		}
		
		if(runTimer == true)
		{
			levelTimer += Time.deltaTime;
			timerInt = Mathf.RoundToInt(levelTimer);
		}
		if(!charging)
		{
			decreaseBattery();
			anim.SetBool("isCharging", false);
		}
		else if(secondTimer != 0.0f)
		{
			secondTimer = 0.0f;
		}
		
		if(lose == false)
		{
			checkLose();
		}
	}
	
	public void decreaseBattery (){	
		if(stopBatteryBool == false){
			if(secondTimer > decreaseTimer)
			{
				battery -= negativeBatteryFlow;
				if(battery < 0.0f) battery = 0.0f;
				secondTimer = 0.0f;
			}
			else
			{
				secondTimer += Time.deltaTime;
			}
		}
	}
	
	public void decreaseBatteryBy ( float value  ){
		battery -= value;
		
		if(battery < 0.0f) battery = 0.0f;	//no negative battery values
	}
	
	public void addBatteryPower (){
		anim.SetBool("isCharging", true);
		if(battery < maximumBatteryCapacity)
		{
			battery += positiveBatteryFlow;
		}
		
		if(battery > maximumBatteryCapacity)
		{
			battery = maximumBatteryCapacity;
			anim.SetBool("isFullyCharged", true);
		}
	}
	
	public void stopBattery (){
		stopBatteryBool = true;
	}
	
	public void startBattery (){
		stopBatteryBool = false;
	}
	
	void checkLose (){
		if(battery <= 0.1f)
		{
			gameOverLose();
			lose = true;
		}
	}
	
	public void addCrystalSample ( GameObject newSample  )
	{
		crystalSamples.Add(newSample);
		addScore(100);
	}
	
	public int getCrystalsSampleCount (){
		return crystalSamples.Count;
	}
	
	public bool checkWin ()
	{
		if(crystalsToComplete <= getCrystalsSampleCount())
		{
			if(getCrystalsSampleCount() == maxCrystals)
			{
				addScore(200);
				return true;
			}
			else return true;
		}
		
		if(crystalsToComplete >= getMaxCrystals())
		{
			if(getCrystalsSampleCount() == getMaxCrystals())
			{
				return true;
			}
		}
		
		return false;
	}
	
	public void gameOverWin (){
		levelTriggerScript.setFinished(true);
		soundEngine.playSoundEffect("win");
	}
	
	public void gameOverLose (){
		levelTriggerScript.setLost(true);
		soundEngine.playSoundEffect("lose");
	}
	
	public void startTimer (){
		runTimer = true;
	}
	
	public void stopTimer (){
		runTimer = false;
		if(levelTriggerScript.getFinished()){
			if(timerInt <= techieTime) 									addScore(500);
			if(timerInt <= platinumTime && timerInt >= techieTime+1) 	addScore(300);
			if(timerInt <= goldTime && timerInt >= platinumTime+1)		addScore(200);
			if(timerInt <= silverTime && timerInt >= goldTime+1) 		addScore(150);
			if(timerInt <= bronzeTime && timerInt >= silverTime+1) 		addScore(100);
			if(timerInt >= bronzeTime) 									addScore(0);
		}
	}
	
	
	/*

Adders

*/
	
	public void addScore ( int value  ){
		score += value;
	}
	
	public void addShardScore (){
		addScore(shardScore);
	}
	
	//add ammo to both instances
	public void addAmmo ( int amount  ){
		if(!infiniteAmmo)
		{
			currentNormalSeeds += amount;
			currentBumpySeeds += amount;
			
			if(currentNormalSeeds > maximumNormalSeeds) currentNormalSeeds = maximumNormalSeeds;
			if(currentBumpySeeds > maximumBumpySeeds) currentBumpySeeds = maximumBumpySeeds;
		}
	}
	//function overload to specify type
	public void addAmmo ( int amount ,   int ammoType  ){
		if(!infiniteAmmo)
		{
			//Type 0 = normal, Type 1 = bumpy
			if(ammoType == 0)
			{
				currentNormalSeeds += amount;
				if(currentNormalSeeds > maximumNormalSeeds) currentNormalSeeds = maximumNormalSeeds;
			}
			
			if(ammoType == 1)
			{
				currentBumpySeeds += amount;
				if(currentBumpySeeds > maximumBumpySeeds) currentBumpySeeds = maximumBumpySeeds;
			}
		}
	}
	
	/*

Decreasers

*/
	
	public void decreaseNormalSeeds (){
		currentNormalSeeds --;
		
		if(currentNormalSeeds == 0) playerInput.setNormalShroomButtonEnabled(false);
	}
	
	public void decreaseBumpySeeds (){
		currentBumpySeeds --;
		
		if(currentBumpySeeds == 0) playerInput.setBumpyShroomButtonEnabled(false);
	}
	
	/*

Getters

*/
	
	//
	//Ammo
	//
	
	public bool getInfiniteAmmo (){
		return infiniteAmmo;
	}
	
	public int getCurrentNormalSeeds (){
		return currentNormalSeeds;
	}
	
	public int getMaximumNormalSeeds (){
		return maximumNormalSeeds;
	}
	
	public int getCurrentBumpySeeds (){
		return currentBumpySeeds;
	}
	
	public int getMaximumBumpySeeds (){
		return maximumBumpySeeds;
	}
	
	//
	//Battery
	//
	public float getBattery (){
		return battery;
	}
	
	public int getBatteryCapacity (){
		return maximumBatteryCapacity;
	}
	
	public float getDecreaseTimer (){
		return decreaseTimer;
	}
	
	public int getNegativeBatteryFlow (){
		return negativeBatteryFlow;
	}
	
	public int getPositiveBatteryFlow (){
		return positiveBatteryFlow;
	}
	
	//
	//Player
	//
	public int getScore (){
		return score;
	}
	
	public float getJumpDrain (){
		return jumpDrain;
	}
	
	public float getFlashDrain (){
		return flashDrain;
	}
	
	public bool getCharging (){
		return charging;
	}
	
	//
	//Crystals
	//
	
	public int getCrystalsToComplete (){
		return crystalsToComplete;
	}
	
	public int getMaxCrystals (){
		return maxCrystals;
	}
	
	/*

Timers

*/
	
	public int getPlatinumTime (){
		return platinumTime;
	}
	
	public int getGoldTime (){
		return goldTime;
	}
	
	public int getSilverTime (){
		return silverTime;
	}
	
	public int getBronzeTime (){
		return bronzeTime;
	}
	
	
	
	/*

Setters

*/
	
	//
	//Ammo
	//
	public void setInfiniteAmmo ( bool value  ){
		infiniteAmmo = value;
	}
	
	public void setCurrentNormalSeeds ( int value  ){
		currentNormalSeeds = value;
	}
	
	public void setMaximumNormalSeeds ( int value  ){
		maximumNormalSeeds = value;
	}
	
	public void setCurrentBumpySeeds ( int value  ){
		currentBumpySeeds = value;
	}
	
	public void setMaximumBumpySeeds ( int value  ){
		maximumBumpySeeds = value;
	}
	
	//
	//Battery
	//
	
	public void setBattery ( float value  ){
		battery = value;
	}
	
	public void setBatteryCapacity ( int value  ){
		maximumBatteryCapacity = value;
	}
	
	public void setDecreaseTimer ( float value  ){
		decreaseTimer = value;
	}
	
	public void setNegativeBatteryFlow ( int value  ){
		negativeBatteryFlow = value;
	}
	
	public void setPositiveBatteryFlow ( int value  ){
		positiveBatteryFlow = value;
	}
	
	//
	//Player
	//
	
	public void setJumpDrain ( float value  ){
		jumpDrain = value;
	}
	
	public void setFlashDrain ( float value  ){
		flashDrain = value;
	}
	
	public void setCharging ( bool value  ){
		charging = value;
	}
	
	public void setFullyChargedFalse (){
		anim.SetBool("isFullyCharged", false);
	}
	
	//
	//Crystals
	//
	public void setCrystalsToComplete ( int value  ){
		crystalsToComplete = value;
	}
	
	public void setMaxCrystals ( int value  ){
		maxCrystals = value;
	}
	
/*

Timers

*/
	public void setPlatinumTime ( int value  ){
		platinumTime = value;
	}
	
	public void setGoldTime ( int value  ){
		goldTime = value;
	}
	
	public void setSilverTime ( int value  ){
		silverTime = value;
	}
	
	public void setBronzeTime ( int value  ){
		bronzeTime = value;
	}	
}