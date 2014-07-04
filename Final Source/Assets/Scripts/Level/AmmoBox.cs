using UnityEngine;
using System.Collections;

public class AmmoBox : MonoBehaviour
{


    public int extraSeeds = 5;
    public bool normalType = true;
    public bool bumpyType = false;
    public float timeToRespawn = 20.0f;
    public bool oneTimePickup = false;
    private float counter = 0.0f;
    private bool available = true;
    private GameLogic gameLogic = null;

    public void Awake()
    {
        gameLogic = GameObject.Find("GameLogic").GetComponent<GameLogic>() as GameLogic;
        if (gameLogic == null) Debug.LogError("GameLogic is null");
    }

    public void Update()
    {
        //if the pick up is not available (aka it is already picked up)
        if (available == false)
        {
            //if the counter is higher than 0
            if (counter > 0.0f)
            {
                counter -= Time.deltaTime;
                if (counter <= 0.0f) counter = 0.0f;	//set counter to 0.0ff if lower than 0
            }
            //once the counter reached 0
            else if (counter == 0.0f)
            {
                turnPickUpOn();	//turn the pickup on
            }
        }
    }

    private void turnPickUpOn()
    {
        this.gameObject.renderer.enabled = true;
        this.gameObject.collider.enabled = true;
        available = true;
    }

    private void turnPickUpOffTemp()
    {
        this.gameObject.renderer.enabled = false;
        this.gameObject.collider.enabled = false;
        counter = timeToRespawn;
        available = false;
    }

    private void turnPickUpOffPerm()
    {
        if (this.gameObject.transform.parent.name == "AmmoBox")
        {
            Destroy(this.gameObject.transform.parent.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    public void OnTriggerEnter(Collider _object)
    {
        if (available == true)
        {
            if (_object.gameObject.name == "Player")
            {
                sendAmmo();

                if (!oneTimePickup)
                {
                    turnPickUpOffTemp();
                }
                else
                {
                    turnPickUpOffPerm();
                }
            }
        }
    }

    private void sendAmmo()
    {
        if (gameLogic != null)
        {
            //if both types are selected
            if (normalType && bumpyType)
            {
                gameLogic.addAmmo(extraSeeds);
            }
            //if normal type seed
            else if (normalType == true && bumpyType == false)
            {
                gameLogic.addAmmo(extraSeeds, 0);
            }
            //if bympy type seed
            else if (bumpyType == true && normalType == false)
            {
                gameLogic.addAmmo(extraSeeds, 1);
            }
        }
        else Debug.LogError("Can't add Ammo cause gameLogic is null");
    }


    /*

Getters

*/
    public float getTimeToRespawn()
    {
        return timeToRespawn;
    }

    public int getExtraSeeds()
    {
        return extraSeeds;
    }

    public bool getOneTimePickup()
    {
        return oneTimePickup;
    }

    public bool getNormalType()
    {
        return normalType;
    }

    public bool getBumpyType()
    {
        return bumpyType;
    }

    /*

Setters

*/
    public void setExtraSeeds(int value)
    {
        extraSeeds = value;
    }

    public void setTimeToRespawn(float value)
    {
        timeToRespawn = value;
    }

    public void setOneTimePickup(bool value)
    {
        oneTimePickup = value;
    }

    public void setNormalType(bool value)
    {
        normalType = value;
    }

    public void setBumpyType(bool value)
    {
        bumpyType = value;
    }

}