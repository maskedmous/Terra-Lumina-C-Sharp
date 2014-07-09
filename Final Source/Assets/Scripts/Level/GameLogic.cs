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
    private bool stopBatteryBool = false;
    /*
        Win Variables
    */
    public int crystalsToComplete = 0;
    public int maxCrystals = 3;
    private int score = 0;
    private bool lose = false;

    /*
        Player Variables
    */
    private PlayerInputScript playerInput = null;
    private SoundEngineScript soundEngine = null;
    public bool infiniteAmmo = false;
    public int currentNormalSeeds = 0;
    public int maximumNormalSeeds = 0;
    public int currentBumpySeeds = 0;
    public int maximumBumpySeeds = 0;

    /*
        Action energy cost
    */
    private float jumpDrain = 0.0f;
    private float flashDrain = 0.0f;

    /*
        Timer variables
    */
    private float secondTimer = 0.0f;			//counting seconds
    private float levelTimer = 0.0f;
    private bool runTimer = true;
    private int timerInt = 0;

    /*
    Score Variables
    */
    public int platinumTime = 50;
    public int goldTime = 80;
    public int silverTime = 130;
    public int bronzeTime = 180;

    public int shardScore = 100;

    /*
        Array Variables
    */
    private ArrayList crystalSamples = new ArrayList();

    /*
    Other
    */
    private LevelTrigger levelTriggerScript = null;
    private Animator roverAnim;
    private bool gamePaused = false;


    public void Start()
    {
        startTimer();   //start the timer
        GameObject player = GameObject.Find("Player");  //get the player
        playerInput = player.GetComponent<PlayerInputScript>() as PlayerInputScript; // get the input of the player

        if (Application.loadedLevelName == "LevelLoaderScene")
        {
            soundEngine = GameObject.Find("SoundEngine").GetComponent<SoundEngineScript>() as SoundEngineScript;    //only try to get the soundengine if its played through the texture loader
        }

        roverAnim = player.GetComponent<Animator>();    //animator component of the rover to call / set animations

        //enabled / disable bloom via playerPrefs -- Windows Build
        if (PlayerPrefs.HasKey("Bloom")) //Heim build has no keys
        {
            bool bloom = false;
            if (PlayerPrefs.GetInt("Bloom") == 1) bloom = true;

            if (!bloom) Camera.main.GetComponent<BloomAndLensFlares>().enabled = false;
        }
    }


    public void Update()
    {
        //if level trigger script is null, it should search for end level trigger, if found add it
        //fix because leveltrigger was not initialised at start for some reason
        if (levelTriggerScript == null && GameObject.Find("EndLevelTrigger") != null)
        {
            levelTriggerScript = GameObject.Find("EndLevelTrigger").GetComponent<LevelTrigger>() as LevelTrigger;
        }

        //if the timer is running
        if (runTimer == true)
        {
            levelTimer += Time.deltaTime;   //increase the timer by delta time (time between frames)
            timerInt = Mathf.RoundToInt(levelTimer);    //round to int for full seconds
        }
        //if its not charging it should drain de battery
        if (!charging)
        {
            decreaseBattery();  //call to decrease the battery (has a timer in there so its not every frame)
            roverAnim.SetBool("isCharging", false); //it ain't charging so set it to false
        }
        else if (secondTimer != 0.0f)
        {
            //if it is charging reset the second timer so it takes a while to decrease the battery
            //wouldn't be fair if the player would be drained when just going outside the sunlight
            secondTimer = 0.0f;
        }

        if (lose == false)
        {
            //check if the player has lost
            checkLose();
        }
    }

    public void pauseGame()
    {
        //pausing the game by setting the time scale to 0.0 which stops time / movement
        Time.timeScale = 0.0f;
        //we want to pause the incoming sound effects only keep the music running
        if (soundEngine != null) soundEngine.pauseSound();
        //game is paused
        gamePaused = true;
    }

    public void unpauseGame()
    {
        //we unpaused the game so the time scale is 1.0 again
        Time.timeScale = 1.0f;
        //unpause the sound so sound effects are going again
        if (soundEngine != null) soundEngine.unpauseSound();
        //well the game has been unpaused
        gamePaused = false;
    }

    public void decreaseBattery()
    {
        //only decrease the battery if the stopBattery is false
        if (stopBatteryBool == false)
        {
            //if the second timer is larger than the decrease timer (decrease timer is the amount of seconds it takes to drain the battery)
            if (secondTimer > decreaseTimer)
            {
                battery -= negativeBatteryFlow; //drain the battery by the negative flow (which is customizable by creatives in the level design)
                if (battery < 0.0f) battery = 0.0f; //if the battery is lower than 0.0f then clip it
                secondTimer = 0.0f; //second timer reset to 0
            }
            else
            {
                //the time is increased till it is larger than the decreaseTimer
                secondTimer += Time.deltaTime;
            }
        }
    }

    public void decreaseBatteryBy(float value)
    {
        //drain the battery by an amount set by the tech
        battery -= value;
        //if the battery is lower than 0 clip it so there is no negative battery value
        if (battery < 0.0f) battery = 0.0f;
    }

    public void addBatteryPower()
    {
        //set the animation to charging
        roverAnim.SetBool("isCharging", true);
        //if the battery is lower than its capacity increase it
        if (battery < maximumBatteryCapacity)
        {
            battery += positiveBatteryFlow;
        }
        //if the battery is over its capacity then clip it and put fully charged to true to stop charging animations
        if (battery > maximumBatteryCapacity)
        {
            battery = maximumBatteryCapacity;
            roverAnim.SetBool("isFullyCharged", true);
        }
    }

    //stop the battery from draining
    public void stopBattery()
    {
        stopBatteryBool = true;
    }
    //start the battery to drain
    public void startBattery()
    {
        stopBatteryBool = false;
    }
    //cjecl of the player has lost
    void checkLose()
    {
        //the battery is clipped so it is 0.0f but it doesn't hurt to check for negative as well
        if (battery <= 0.0f)
        {
            gameOverLose(); //the player has lost the game because the battery is drained to 0
            lose = true;
        }
    }

    //add the collected crystal to the array and add a score of 100
    public void addCrystalSample(GameObject newSample)
    {
        crystalSamples.Add(newSample);
        addScore(100);
    }

    //get the current crystal that the player has collected
    public int getCrystalsSampleCount()
    {
        return crystalSamples.Count;
    }

    //check if the player has won
    public bool checkWin()
    {
        //if the player has gotten enough crystals
        if (crystalsToComplete <= getCrystalsSampleCount())
        {
            //get the amount of and check if it is the maximum amount of crystals
            if (getCrystalsSampleCount() == maxCrystals)
            {
                //if the player collected all the crystals then add a bonus of 300
                addScore(300);
                return true;
            }
            else return true;
        }
        //if the player collected enough crystals to complete he won the game
        if (crystalsToComplete >= getMaxCrystals())
        {
            if (getCrystalsSampleCount() == getMaxCrystals())
            {
                return true;
            }
        }
        //player hasn't collected enough so he hasn't won return false
        return false;
    }

    //game is over with a win
    public void gameOverWin()
    {
        levelTriggerScript.setFinished(true);
        soundEngine.playSoundEffect("win");
    }
    //game is over with a loss
    public void gameOverLose()
    {
        levelTriggerScript.setLost(true);
        soundEngine.playSoundEffect("lose");
    }

    //start the timer
    public void startTimer()
    {
        runTimer = true;
    }

    //stop the timer and calculate the bonus score given
    public void stopTimer()
    {
        runTimer = false;
        if (levelTriggerScript.getFinished())
        {
            if (timerInt <= platinumTime) addScore(300);
            if (timerInt <= goldTime && timerInt >= platinumTime + 1) addScore(200);
            if (timerInt <= silverTime && timerInt >= goldTime + 1) addScore(150);
            if (timerInt <= bronzeTime && timerInt >= silverTime + 1) addScore(100);
            if (timerInt >= bronzeTime) addScore(0);
        }
    }


    /*

Adders

*/
    //adds score
    public void addScore(int value)
    {
        score += value;
    }
    //adds the shard score defined
    public void addShardScore()
    {
        addScore(shardScore);
    }

    //add ammo to both instances
    public void addAmmo(int amount)
    {
        if (!infiniteAmmo)
        {
            currentNormalSeeds += amount;
            currentBumpySeeds += amount;

            if (currentNormalSeeds > maximumNormalSeeds) currentNormalSeeds = maximumNormalSeeds;
            if (currentBumpySeeds > maximumBumpySeeds) currentBumpySeeds = maximumBumpySeeds;
        }
    }
    //function overload to specify type of ammo that needs to be added
    public void addAmmo(int amount, int ammoType)
    {
        //theres no use to add something to infinite ammo
        if (!infiniteAmmo)
        {
            //Type 0 = normal, Type 1 = bumpy
            if (ammoType == 0)
            {
                currentNormalSeeds += amount;
                if (currentNormalSeeds > maximumNormalSeeds) currentNormalSeeds = maximumNormalSeeds;
            }

            if (ammoType == 1)
            {
                currentBumpySeeds += amount;
                if (currentBumpySeeds > maximumBumpySeeds) currentBumpySeeds = maximumBumpySeeds;
            }
        }
    }

    /*

Decreasers

*/
    //decrease the seed by 1 and if the player is out of seeds disable the button
    public void decreaseNormalSeeds()
    {
        currentNormalSeeds--;

        if (currentNormalSeeds == 0) playerInput.setNormalShroomButtonEnabled(false);
    }

    public void decreaseBumpySeeds()
    {
        currentBumpySeeds--;

        if (currentBumpySeeds == 0) playerInput.setBumpyShroomButtonEnabled(false);
    }

    /*

Getters

*/
    //
    //Game
    //
    public bool isPaused()
    {
        return gamePaused;
    }
    //
    //Ammo
    //

    public bool getInfiniteAmmo()
    {
        return infiniteAmmo;
    }

    public int getCurrentNormalSeeds()
    {
        return currentNormalSeeds;
    }

    public int getMaximumNormalSeeds()
    {
        return maximumNormalSeeds;
    }

    public int getCurrentBumpySeeds()
    {
        return currentBumpySeeds;
    }

    public int getMaximumBumpySeeds()
    {
        return maximumBumpySeeds;
    }

    //
    //Battery
    //
    public float getBattery()
    {
        return battery;
    }

    public int getBatteryCapacity()
    {
        return maximumBatteryCapacity;
    }

    public float getDecreaseTimer()
    {
        return decreaseTimer;
    }

    public int getNegativeBatteryFlow()
    {
        return negativeBatteryFlow;
    }

    public int getPositiveBatteryFlow()
    {
        return positiveBatteryFlow;
    }

    //
    //Player
    //
    public int getScore()
    {
        return score;
    }

    public float getJumpDrain()
    {
        return jumpDrain;
    }

    public float getFlashDrain()
    {
        return flashDrain;
    }

    public bool getCharging()
    {
        return charging;
    }

    //
    //Crystals
    //

    public int getCrystalsToComplete()
    {
        return crystalsToComplete;
    }

    public int getMaxCrystals()
    {
        return maxCrystals;
    }

    /*

Timers

*/

    public int getPlatinumTime()
    {
        return platinumTime;
    }

    public int getGoldTime()
    {
        return goldTime;
    }

    public int getSilverTime()
    {
        return silverTime;
    }

    public int getBronzeTime()
    {
        return bronzeTime;
    }



    /*

Setters

*/

    //
    //Ammo
    //
    public void setInfiniteAmmo(bool value)
    {
        infiniteAmmo = value;
    }

    public void setCurrentNormalSeeds(int value)
    {
        currentNormalSeeds = value;
    }

    public void setMaximumNormalSeeds(int value)
    {
        maximumNormalSeeds = value;
    }

    public void setCurrentBumpySeeds(int value)
    {
        currentBumpySeeds = value;
    }

    public void setMaximumBumpySeeds(int value)
    {
        maximumBumpySeeds = value;
    }

    //
    //Battery
    //

    public void setBattery(float value)
    {
        battery = value;
    }

    public void setBatteryCapacity(int value)
    {
        maximumBatteryCapacity = value;
    }

    public void setDecreaseTimer(float value)
    {
        decreaseTimer = value;
    }

    public void setNegativeBatteryFlow(int value)
    {
        negativeBatteryFlow = value;
    }

    public void setPositiveBatteryFlow(int value)
    {
        positiveBatteryFlow = value;
    }

    //
    //Player
    //

    public void setJumpDrain(float value)
    {
        jumpDrain = value;
    }

    public void setFlashDrain(float value)
    {
        flashDrain = value;
    }

    public void setCharging(bool value)
    {
        charging = value;
    }

    public void setFullyChargedFalse()
    {
        roverAnim.SetBool("isFullyCharged", false);
    }

    //
    //Crystals
    //
    public void setCrystalsToComplete(int value)
    {
        crystalsToComplete = value;
    }

    public void setMaxCrystals(int value)
    {
        maxCrystals = value;
    }

    /*

    Timers

    */
    public void setPlatinumTime(int value)
    {
        platinumTime = value;
    }

    public void setGoldTime(int value)
    {
        goldTime = value;
    }

    public void setSilverTime(int value)
    {
        silverTime = value;
    }

    public void setBronzeTime(int value)
    {
        bronzeTime = value;
    }
}