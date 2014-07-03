// Converted from UnityScript to C# at http://www.M2H.nl/files/js_to_c.php - by Mike Hergaarden
// Do test the code! You usually need to change a few small bits.

using UnityEngine;
using System.Collections;

public class BumpyShroom : MonoBehaviour {
	
	
	private float counter = 10.0f;
	//private float growCounter = 2.5f;
	
	//private float startScale = 0.2f;
	//private float currentScale = 0.0f;
	//private float improveScale = 0.025f;
	private bool  fullGrown = false;
	private Animator animationController = null;
	
	private float yDifference = 1.0f;
	
	private SoundEngineScript soundEngine = null;
	
	public float slowdown = 1.1f;
	
	public void Awake (){
		//	counter = 30.0ff;
		//	currentScale = startScale;
		this.gameObject.transform.parent.transform.eulerAngles = new Vector3(0, Random.Range(0, 360), 0);
		if(GameObject.Find("SoundEngine") != null) soundEngine = GameObject.Find("SoundEngine").GetComponent<SoundEngineScript>();
		animationController = transform.parent.parent.GetComponent<Animator>();
		animationController.Play("Grow");
	}
	
	public void Update (){
		if(!animationController.GetCurrentAnimatorStateInfo(0).IsName("Grow")){
			animationController.SetBool("doneGrowing", true);
			fullGrown = true;
		} 
		if (fullGrown)
		{	
			counter -= Time.deltaTime;
			if(counter <= 0.0f)
			{
				animationController.Play("Decay");
				Vector3 shroomPosition = this.gameObject.transform.position;
				shroomPosition.y -= Time.deltaTime * slowdown;
				this.gameObject.transform.position = shroomPosition;
				//destroyShroom();
			}
		}
		if(animationController.GetCurrentAnimatorStateInfo(0).IsName("doneDecay"))
		{
			destroyShroom();
		}
	}
	
	private void destroyShroom (){
		Destroy(this.gameObject.transform.parent.parent.gameObject);
	}
	
	public void OnCollisionEnter ( Collision obj  ){
		//collision and executed once
		if(obj.gameObject.name == "Player")
		{
			if(animationController.GetBool("doneGrowing") == true){
				if(Mathf.Abs(this.gameObject.transform.position.y - obj.gameObject.transform.position.y) > yDifference)
				{
					
					obj.gameObject.GetComponent<PlayerController>().bounceShroomY();
					if(soundEngine != null)
					{
						soundEngine.playSoundEffect("bounce");
					}
					animationController.Play("Bounce");
					Transform bounceParticle = this.gameObject.transform.FindChild("shroomJump");
					bounceParticle.particleSystem.Clear();
					bounceParticle.particleSystem.Play();
				}
				else
				{
					obj.gameObject.GetComponent<PlayerController>().bounceShroomX();
				}
			}
		}
	}
}