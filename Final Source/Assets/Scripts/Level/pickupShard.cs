using UnityEngine;
using System.Collections;

public class pickupShard : MonoBehaviour
{


    private GameLogic gameLogicScript;
    private SoundEngineScript soundEngine = null;

    void Start()
    {
        gameLogicScript = GameObject.Find("GameLogic").GetComponent<GameLogic>() as GameLogic;
        if (GameObject.Find("SoundEngine") != null) soundEngine = GameObject.Find("SoundEngine").GetComponent<SoundEngineScript>();
    }

    public void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.name == "Player")
        {
            gameLogicScript.addShardScore();
            soundEngine.playSoundEffect("shardPickup");
            Destroy(this.gameObject);
        }
    }
}