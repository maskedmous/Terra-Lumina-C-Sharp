using UnityEngine;
using System.Collections;

public class Crystal : MonoBehaviour
{


    private SoundEngineScript soundEngine = null;


    void Awake()
    {
        if (Application.loadedLevelName == "LevelLoaderScene")
        {
            soundEngine = GameObject.Find("SoundEngine").GetComponent<SoundEngineScript>() as SoundEngineScript;
        }
    }
    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.name == "Player")
        {
            GameObject.Find("GameLogic").GetComponent<GameLogic>().addCrystalSample(this.gameObject);
            if (soundEngine != null)
            {
                soundEngine.playSoundEffect("pickup");
            }
            Destroy(this.gameObject);
        }
    }
}