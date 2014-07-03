#pragma strict

private var counter:float = -1.0f;

private var startScale:float = 0.2f;
private var currentScale:float = 0.0f;
private var improveScale:float = 0.025f;
private var fullGrown:boolean = false;

private var animationController:Animator = null;
private var jumpedOnShroom:boolean = false;

public var slowdown:float = 0.65f;

public function Awake():void
{
	counter = 15.0f;
	currentScale = startScale;
	
	animationController = transform.parent.parent.GetComponent(Animator);
}

public function Update():void {
	if(!animationController.GetCurrentAnimatorStateInfo(0).IsName("Grow")){
		animationController.SetBool("doneGrowing", true);
		fullGrown = true;
	} 
	if (fullGrown) {	
		counter -= Time.deltaTime;
		if(counter <= 0.0f){
			animationController.Play("Decay");
			this.gameObject.transform.position.y -= Time.deltaTime * slowdown;
			//destroyShroom();
		}
	}
	if(animationController.GetCurrentAnimatorStateInfo(0).IsName("doneDecay"))
	{
		destroyShroom();
	}
}

private function destroyShroom():void
{
	Destroy(this.gameObject.transform.parent.parent.gameObject);
}

public function OnCollisionEnter(obj:Collision):void
{
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

public function OnCollisionExit(obj:Collision):void
{
	if(obj.gameObject.name == "Player")
	{
		jumpedOnShroom = false;
	}
}