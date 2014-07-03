#pragma strict

private var growTime:float = 3.0f;
private var shroomType:GameObject = null;
private var growing:boolean = false;
private var startScale:float = 0.2f;
private var currentScale:float = 0.0f;
private var improveScale:float = 0.025f;
private var newShroom:GameObject = null;
private var meshRenderer:MeshRenderer = null;

public function Awake():void
{
	meshRenderer = this.gameObject.GetComponent(MeshRenderer);
}

public function Update():void
{
	if(this.gameObject.transform.position.y < -80) Destroy(this.gameObject);
}

public function OnCollisionEnter(collision:Collision):void
{
	if (collision.gameObject.name.Contains("GroundPiece"))
	{
		if(shroomType != null && newShroom == null)
		{
			newShroom = Instantiate(shroomType, this.transform.position, Quaternion.identity);
			if(shroomType.name == "BumpyShroom") newShroom.transform.position += Vector3(0.0f, 0.56f, 0.0f);
			else if(shroomType.name == "NormalShroom") newShroom.transform.position += Vector3(0.0f, 0.9f, 0.0f);
			newShroom.gameObject.name = "Shroom";
			newShroom.gameObject.transform.parent = GameObject.Find("Level").transform;
//			newShroom.gameObject.transform.localScale = Vector3(startScale, startScale, startScale);
			
			Destroy(this.gameObject);
		}
	}
}

public function setShroomType(aShroomType:GameObject):void
{
	shroomType = aShroomType;
	
	if(shroomType.name == "NormalShroom")
	{
		meshRenderer.material = Resources.Load("Prefabs/Materials/NormalShroomMaterial", Material);
	}
	else if(shroomType.name == "BumpyShroom")
	{
		meshRenderer.material = Resources.Load("Prefabs/Materials/BumpyShroomMaterial", Material);
	}
}