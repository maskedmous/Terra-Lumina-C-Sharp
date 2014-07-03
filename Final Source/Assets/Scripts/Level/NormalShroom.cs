using UnityEngine;
using System.Collections;

public class NormalShroom : MonoBehaviour {
	
	
	private float counter = -1.0f;
	
	//private float startScale = 0.2f;
	//private float currentScale = 0.0f;
	//private float improveScale = 0.025f;
	private bool  fullGrown = false;
	
	private Animator animationController = null;
	private bool  jumpedOnShroom = false;
	
	public float slowdown = 0.65f;
	
	public void Awake (){
		counter = 15.0f;
		//currentScale = startScale;
		
		animationController = transform.parent.parent.GetComponent<Animator>();
	}
	
	public void Update (){
		if(!animationController.GetCurrentAnimatorStateInfo(0).IsName("Grow")){
			animationController.SetBool("doneGrowing", true);
			fullGrown = true;
		} 
		if (fullGrown) {	
			counter -= Time.deltaTime;
			if(counter <= 0.0f){
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
		if(jumpedOnShroom == false)
		{
			if(obj.gameObject.name == "Player")
			{
				if(animationController.GetBool("doneGrowing") == true){
					jumpedOnShroom = true;
					animationController.Play("OnJumping");
				}
			}
		}
	}
	
	public void OnCollisionExit ( Collision obj  ){
		if(obj.gameObject.name == "Player")
		{
			jumpedOnShroom = false;
		}
	}
}