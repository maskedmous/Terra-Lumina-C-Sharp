using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CameraStartScript : MonoBehaviour {

	private Camera cam;
	private float speed = 120.0f;
	private float timer = 1.5f;
	private float startTimer = 2.0f;
	
	private GameObject[] crystals;
	private List<Vector3> crystalPositions = new List<Vector3>();
	private GameObject endLevelTrigger = null;
	
	private int currentCrystal = 0;
	private Vector3 targetPos = Vector3.zero;
    private Vector3 endPos = Vector3.zero;
	private bool goingToPlayer = false;
	private float t = 0.0f;
	
	private GameLogic gameLogic = null;
	
	private Texture2D tutorialTexture = null;
	public float textureX = 500.0f;
	public float textureY = 500.0f;
	
	private CameraScript cameraScript = null;
	
	public void Awake (){
		cameraScript = Camera.main.GetComponent("CameraScript") as CameraScript;
		cameraScript.setMove(false);
	}
	
	public void Start (){
		cam = Camera.main;
		endLevelTrigger = GameObject.Find ("EndLevelTrigger") as GameObject;
        endPos = new Vector3(cam.transform.position.x, cam.transform.position.y, 6.0f);
		cam.transform.position = new Vector3 (endLevelTrigger.transform.position.x, cam.transform.position.y, cam.transform.position.z);
		
		crystals = GameObject.FindGameObjectsWithTag("Pickup");
		for (int i = 0; i < crystals.Length; ++i)
		{
			crystalPositions.Add(crystals[i].transform.position);
		}
		crystalPositions.RemoveAt(0);
		crystalPositions = sort(crystalPositions);

		targetPos = crystalPositions[0];
		
		gameLogic = GameObject.Find("GameLogic").GetComponent("GameLogic") as GameLogic;
		gameLogic.stopBattery();
		
		tutorialTexture = Resources.Load("Textures/ScoreExplanation") as Texture2D;
	}
	
	public void Update (){
		if (startTimer > 0) startTimer -= Time.deltaTime;
		else moveCamera();
		if (cam.transform.position.x < endPos.x + 0.01)
		{
			speed = 0.0f;
            cam.transform.position = endPos;//new Vector3(-19.082f, cam.transform.position.y, cam.transform.position.z);
			startGame();
		}
		if (Input.GetKey(KeyCode.S) && Input.GetKey(KeyCode.K))
		{
			speed = 0.0f;
			cam.transform.position = new Vector3(-19.082f, cam.transform.position.y, cam.transform.position.z);
			startGame();
		}
        if (Input.GetKeyDown(KeyCode.Q)) skip();
	}
	
	public void OnGUI (){
		if(tutorialTexture != null) GUI.DrawTexture(scaleRect(new Rect(textureX, textureY, tutorialTexture.width, tutorialTexture.height)), tutorialTexture);
	}
	
	private Rect scaleRect ( Rect rect  ){
		Vector2 scale = new Vector2(Screen.width / 1920.0f, Screen.height / 1080.0f);
		return new Rect(rect.x * scale.x, rect.y * scale.y, rect.width * scale.x, rect.height * scale.y);
	}
	
	private void moveCamera (){
		if (goingToPlayer) {
			t += Time.deltaTime;
			speed = 120.0f - 65 * Mathf.Pow (t, 1 / 3.0f);
		}

		Vector3 camPos = Camera.main.gameObject.transform.position;
		Camera.main.gameObject.transform.position = Vector3.MoveTowards(camPos, targetPos, speed * Time.deltaTime);
		Camera.main.gameObject.transform.position = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y, 6.0f);
		
		if (Mathf.Abs(camPos.x - targetPos.x) < 0.2f) {
			timer -= Time.deltaTime;
			speed = 0.0f;
			if (timer < 0) {
				timer = 1.5f;
				currentCrystal++;
				if (currentCrystal + 1 == crystalPositions.Count) goingToPlayer = true;
				targetPos = crystalPositions[currentCrystal];
				speed = 120.0f;
			}
		}
	}
	
	private List<Vector3> sort(List<Vector3> list)
	{
		List<Vector3> newList = new List<Vector3>();
		float highest = int.MinValue;
		int index = 0;
		int length = list.Count;
		
		for (int l = length; l > 0; --l) {
			for (int i = 0; i < l; ++i) {
				if (list[i].x > highest) {
					highest = list[i].x;
					index = i;
				}
			}
            list[index] += new Vector3(0.0f, 1.0f, 0.0f);
			newList.Add(list[index]);
			list.RemoveAt(index);
			highest = int.MinValue;
		}
        newList.Add(endPos);
		return newList;
	}

    private void skip()
    {
        targetPos = crystalPositions[crystalPositions.Count - 1];
        startTimer = -1.0f;
        speed = 120.0f;
        GameObject player = GameObject.Find("Player") as GameObject;
        PlayerInputScript playerInputScript = player.GetComponent("PlayerInputScript") as PlayerInputScript;
        playerInputScript.setSkipButtonEnabled(false);
    }
	
	private void startGame (){
		TutorialTriggerScript tutorialTriggerScript = this.gameObject.GetComponent("TutorialTriggerScript") as TutorialTriggerScript;
		GameObject player = GameObject.Find ("Player") as GameObject;
		PlayerInputScript playerInputScript = player.GetComponent("PlayerInputScript") as PlayerInputScript;

		tutorialTriggerScript.setMovementLeftEnabled(true);
		tutorialTriggerScript.setCameraMoving(false);
		playerInputScript.setMovementLeftEnabled(true);
        playerInputScript.setSkipButtonEnabled(false);
		cameraScript.setMove(true);
		
		gameLogic.startBattery();
		
		Destroy(this);
	}
}