using UnityEngine;
using System.Collections;

public class SoundEngineScript : MonoBehaviour
{

    private bool pausedSound = false;
    private float volume = 0.9031847f;
    static bool soundEngineExists = false;

    private bool aim = false;
    private bool drive = false;
    private float driveTimer = 0.0f;
    private bool driveTimerBool = false;

    private AudioClip menuSound;
    private AudioClip easySound;
    private AudioClip mediumSound;
    private AudioClip hardSound;

    private AudioClip bounceSound;
    private AudioClip jumpSound;
    private AudioClip shootingSound;
    private AudioClip slugForwardSound;
    private AudioClip slugBackwardSound;
    private AudioClip roverDriveSound;
    private AudioClip flashSound;
    private AudioClip roverStartSound;
    private AudioClip roverStopSound;
    private AudioClip roverAimSound;
    private AudioClip rockBreakingSound;
    //private AudioClip sunChargingSound;
    private AudioClip crystalPickup;
    private AudioClip shardPickup;
    private AudioClip winSound;
    private AudioClip loseSound;

    public void Awake()
    {


        if (soundEngineExists == false)
        {
            audio.volume = volume;
            audio.loop = true;
            soundEngineExists = true;
            DontDestroyOnLoad(this.gameObject);

            menuSound = Resources.Load("SoundEffects/Lumina Menu") as AudioClip;
            easySound = Resources.Load("SoundEffects/MusicEasy") as AudioClip;
            mediumSound = Resources.Load("SoundEffects/MusicMedium") as AudioClip;
            hardSound = Resources.Load("SoundEffects/MusicHard") as AudioClip;

            bounceSound = Resources.Load("SoundEffects/Shroom Bounce") as AudioClip;
            jumpSound = Resources.Load("SoundEffects/Rover Jump New 3") as AudioClip;
            shootingSound = Resources.Load("SoundEffects/Rover Shoot") as AudioClip;
            slugForwardSound = Resources.Load("SoundEffects/Move forward") as AudioClip;
            slugBackwardSound = Resources.Load("SoundEffects/Move backwards") as AudioClip;
            roverDriveSound = Resources.Load("SoundEffects/Rover Drive New") as AudioClip;
            roverStartSound = Resources.Load("SoundEffects/Rover Drive Start New") as AudioClip;
            roverStopSound = Resources.Load("SoundEffects/Rover Drive Stop New") as AudioClip;
            roverAimSound = Resources.Load("SoundEffects/Rover Aim") as AudioClip;
            flashSound = Resources.Load("SoundEffects/Rover Flashlight") as AudioClip;
            rockBreakingSound = Resources.Load("SoundEffects/Rock Barrier Break v2") as AudioClip;
            crystalPickup = Resources.Load("SoundEffects/CrystalSound") as AudioClip;
            shardPickup = Resources.Load("SoundEffects/ShardSound") as AudioClip;
            //sunChargingSound = Resources.Load("SoundEffects/...") as AudioClip;
            winSound = Resources.Load("SoundEffects/Winsound v2") as AudioClip;
            loseSound = Resources.Load("SoundEffects/Losesound v1") as AudioClip;

            changeMusic("Menu");
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    public void Update()
    {
        if (GameObject.Find("Player"))
        {
            this.gameObject.transform.position = GameObject.Find("Player").transform.position;
        }
        if (driveTimerBool == true)
        {
            driveTimer += Time.deltaTime;
            if (driveTimer >= 1.0f)
            {
                driveTimerBool = false;
                driveTimer = 0.0f;
            }
        }
    }

    public void changeMusic(string name)
    {
        audio.Stop();
        if (name == "Menu")
        {
            audio.clip = menuSound;
            audio.Play();
        }
        if (name == "Easy")
        {
            audio.clip = easySound;
            audio.Play();
        }
        if (name == "Medium")
        {
            audio.clip = mediumSound;
            audio.Play();
        }
        if (name == "Hard")
        {
            audio.clip = hardSound;
            audio.Play();
        }
    }

    // give a number between 0 and 1 for the volume
    public void changeVolume(float newVolume)
    {
        if (newVolume > 1.0f)
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

    public float getVolume()
    {
        return volume;
    }

    public void changeMute(bool newMute)
    {
        audio.mute = newMute;
    }

    public void changeLoop(bool newLoop)
    {
        audio.loop = newLoop;
    }

    public void playSoundEffect(string name)
    {
        if (!pausedSound)
        {
            if (name == "bounce")
            {
                audio.PlayOneShot(bounceSound);
            }
            if (name == "jump")
            {
                audio.PlayOneShot(jumpSound);
            }
            if (name == "shoot")
            {
                audio.PlayOneShot(shootingSound);
            }
            if (name == "slugForward")
            {
                audio.PlayOneShot(slugForwardSound);
            }
            if (name == "slugBackward")
            {
                audio.PlayOneShot(slugBackwardSound);
            }
            if (name == "roverDrive")
            {
                if (driveTimer == 0.0f)
                {
                    audio.PlayOneShot(roverDriveSound);
                    driveTimerBool = true;
                }
            }
            if (name == "roverStart")
            {
                audio.PlayOneShot(roverStartSound);
            }
            if (name == "roverStop")
            {
                audio.PlayOneShot(roverStopSound);
            }
            if (name == "aim")
            {
                if (aim == true)
                {
                    audio.PlayOneShot(roverAimSound);
                }
            }
            if (name == "flash")
            {
                audio.PlayOneShot(flashSound);
            }
            if (name == "rock")
            {
                audio.PlayOneShot(rockBreakingSound);
            }
            if (name == "crystalPickup")
            {
                audio.PlayOneShot(crystalPickup);
            }
            if (name == "shardPickup")
            {
                audio.PlayOneShot(shardPickup);
            }
            if (name == "sun")
            {
                //audio.PlayOneShot(sunChargingSound);
            }
            if (name == "win")
            {
                audio.PlayOneShot(winSound);
            }
            if (name == "lose")
            {
                audio.PlayOneShot(loseSound);
            }
        }
    }

    public void pauseSound()
    {
        pausedSound = true;
    }

    public void unpauseSound()
    {
        pausedSound = false;
    }

    public void setAim(bool aimBool)
    {
        aim = aimBool;
    }

    public void setDrive(bool driveBool)
    {
        drive = driveBool;
    }

    public bool getDrive()
    {
        return drive;
    }

    public void playMusic()
    {
        this.audio.Play();
    }

    public void stopMusic()
    {
        this.audio.Stop();
    }
}