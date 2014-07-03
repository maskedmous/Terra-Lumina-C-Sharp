using UnityEngine;
using System.Collections;

public class PlayerParticleScript : MonoBehaviour {

	public GameObject jumpDust;
	public GameObject landDust;
	
	public Transform chargingEffect;
	public Transform driveDust;
	public Transform engineJump;
	
	private float engineJumpTimer = 0.0f;
	
	public void Start (){
		jumpDust = Instantiate(jumpDust, Vector3.zero, Quaternion.identity) as GameObject;
		jumpDust.transform.eulerAngles = new Vector3 (270.0f, jumpDust.transform.eulerAngles.y, jumpDust.transform.eulerAngles.z);
		
		landDust = Instantiate(landDust, Vector3.zero, Quaternion.identity) as GameObject;
		
		chargingEffect = this.gameObject.transform.FindChild("ChargingEffect");
		
		driveDust = this.gameObject.transform.FindChild("DriveDust");
		driveDust.gameObject.transform.localPosition = new Vector3(-0.68f, -0.40f, -0.4f);
		driveDust.gameObject.transform.eulerAngles = new Vector3 (driveDust.gameObject.transform.eulerAngles.x, 270.0f, driveDust.gameObject.transform.eulerAngles.z);
		
		engineJump = this.gameObject.transform.FindChild("Engine");
		engineJump.gameObject.transform.localPosition = new Vector3(-0.9f, -0.25f, 0.0f);
	}
	
	public void Update (){
		if (engineJump.particleEmitter.emit)
		{
			engineJumpTimer -= Time.deltaTime;
			if (engineJumpTimer < 0.0f) engineJump.particleEmitter.emit = false;
		}
	}
	
	public void playParticle ( string name  ){
		switch(name)
		{
		case "jumpDust":
			jumpDust.transform.position = this.gameObject.transform.position - new Vector3(0.0f, 0.5f, 0.0f);
			jumpDust.particleSystem.Clear();
			jumpDust.particleSystem.Play();
			break;
			
		case "engineJump":
			engineJump.particleEmitter.emit = true;
			engineJumpTimer = 0.2f;
			break;
			
		case "landDust":
			landDust.transform.position = this.gameObject.transform.position - new Vector3(0.0f, 0.5f, 0.0f);
			landDust.particleSystem.Clear();
			landDust.particleSystem.Play();
			break;
			
		case "charging":
			chargingEffect.particleSystem.Play();
			break;
		}
	}
	
	public void playParticle ( string name ,   float speed  ){
		switch(name)
		{
		case "driveDust":
			if (speed > 0.08f)
			{
				driveDust.particleSystem.startSpeed = speed;
				if (!driveDust.particleSystem.isPlaying) driveDust.particleSystem.Play();
			}
			else if (driveDust.particleSystem.isPlaying)
			{
				driveDust.particleSystem.Stop();
			}
			break;
		}
	}
	
	public void stopChargeParticle (){
		chargingEffect.particleSystem.Stop();
	}
}