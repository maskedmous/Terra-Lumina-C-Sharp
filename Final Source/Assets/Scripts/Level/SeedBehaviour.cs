using UnityEngine;
using System.Collections;

public class SeedBehaviour : MonoBehaviour {
	
	
	//private float growTime = 3.0f;
	private GameObject shroomType = null;
	//private bool  growing = false;
	//private float startScale = 0.2f;
	//private float currentScale = 0.0f;
	//private float improveScale = 0.025f;
	private GameObject newShroom = null;
	private MeshRenderer meshRenderer = null;
	
	public void Awake (){
		meshRenderer = this.gameObject.GetComponent<MeshRenderer>();
	}
	
	public void Update (){
		if(this.gameObject.transform.position.y < -80) Destroy(this.gameObject);
	}
	
	public void OnCollisionEnter ( Collision collision  ){
		if (collision.gameObject.name.Contains("GroundPiece"))
		{
			if(shroomType != null && newShroom == null)
			{
				newShroom = (GameObject) Instantiate(shroomType, this.transform.position, Quaternion.identity);
				if(shroomType.name == "BumpyShroom") newShroom.transform.position += new Vector3(0.0f, 0.56f, 0.0f);
				else if(shroomType.name == "NormalShroom") newShroom.transform.position += new Vector3(0.0f, 0.9f, 0.0f);
				newShroom.gameObject.name = "Shroom";
				newShroom.gameObject.transform.parent = GameObject.Find("Level").transform;
				//			newShroom.gameObject.transform.localScale = Vector3(startScale, startScale, startScale);
				
				Destroy(this.gameObject);
			}
		}
	}
	
	public void setShroomType ( GameObject aShroomType  ){
		shroomType = aShroomType;
		
		if(shroomType.name == "NormalShroom")
		{
			meshRenderer.material = (Material) Resources.Load("Prefabs/Materials/NormalShroomMaterial");
		}
		else if(shroomType.name == "BumpyShroom")
		{
			meshRenderer.material = (Material) Resources.Load("Prefabs/Materials/BumpyShroomMaterial");
		}
	}
}