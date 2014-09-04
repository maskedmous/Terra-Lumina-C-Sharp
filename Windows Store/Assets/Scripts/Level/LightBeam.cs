using UnityEngine;
using System.Collections;

public class LightBeam : MonoBehaviour
{
    private float secondTimer = 0.0f;
    private float increaseBatteryTimer = 0.4f;

    private GameLogic gameLogic = null;

    public void Awake()
    {
        gameLogic = (GameLogic)GameObject.Find("GameLogic").GetComponent<GameLogic>() as GameLogic;
    }

    public void OnTriggerEnter(Collider hit)
    {
        if (hit.gameObject.name == "Player")
        {
            if (!gameLogic.getCharging())
            {
                PlayerParticleScript particleScript = hit.gameObject.GetComponent<PlayerParticleScript>();
                particleScript.playParticle("charging");
                gameLogic.setCharging(true);
            }
        }
    }


    public void OnTriggerStay(Collider hit)
    {
        //if it hits anything execute code
        if (hit.gameObject.name == "Player")
        {
            charge(hit.gameObject);
            //displayFact();
        }
    }

    public void OnTriggerExit(Collider hit)
    {
        if (hit.gameObject.name == "Player")
        {
            PlayerParticleScript particleScript = hit.gameObject.GetComponent<PlayerParticleScript>();
            particleScript.stopChargeParticle();
            gameLogic.setCharging(false);
            gameLogic.setFullyChargedFalse();
            //stopDisplay();
        }
    }

    private void charge(GameObject player)
    {
        if (secondTimer > increaseBatteryTimer)
        {

            if (gameLogic.getBattery() >= gameLogic.getBatteryCapacity())
            {
                PlayerParticleScript particleScript = player.gameObject.GetComponent<PlayerParticleScript>();
                particleScript.stopChargeParticle();
            }
            else
            {
                gameLogic.addBatteryPower();
            }

            secondTimer = 0.0f;
        }
        else
        {
            secondTimer += Time.deltaTime;
        }
    }
}