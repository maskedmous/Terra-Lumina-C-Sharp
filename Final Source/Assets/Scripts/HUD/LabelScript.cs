// Converted from UnityScript to C# at http://www.M2H.nl/files/js_to_c.php - by Mike Hergaarden
// Do test the code! You usually need to change a few small bits.

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LabelScript : MonoBehaviour {
	
	
	private Vector3 up;
	private Camera cam;
	
	private GameObject target;
	
	private List<string> factList = new List<string>();
	
	void  Awake (){
		up = Vector3.up;
		cam = Camera.main;
		
		target = GameObject.Find("Player");
		
		factList.Add("Zonne-energie is helemaal gratis en het vervuilt niet.");
		factList.Add("Zonnepanelen zijn helaas nog duur, \n maar dit gaat veranderen in de toekomst");
		factList.Add("Zonnepanelen werken ook op bewolkte dagen");
		factList.Add("Er zijn 2 soorten zonnepanelen: zonnecellen voor \n electriciteit- en warmtecollectoren voor warm water");
		factList.Add("Het installeren van zonnepanelen \n op een huis kost zo’n 7500-8500 euro");
		factList.Add("Met een volledige installatie van zonnepanelen \n bespaar je ongeveer 125 euro per jaar");
		factList.Add("Je hebt gemiddeld 12-25 zonnepanelen \n nodig om je huis te voorzien van stroom");
		factList.Add("Zonnecellen zetten zonlicht om in energie");
		factList.Add("Warmtecollectoren vangen warmte \n van de zon op en maken warm water");
		factList.Add("Zonne energie is te gebruiken tot de zon op is");
		factList.Add("De zon is al 5 miljard jaar oud \n en zal nog 5.5f miljard jaar branden");
		factList.Add("De omtrek van de aarde is 40.000f km. \n Die van de zon is 4.4f miljoen!");
		factList.Add("Zonnepanelen werken zeker 20 jaar");
		factList.Add("Je kunt ook minder zonnepanelen op het \n dak leggen dan je eigenlijknodig hebt");
		factList.Add("Als je meer elektriciteit opwekt dan je gebruikt \n kun je dat verkopen aan het elektriciteitsbedrijf");
		factList.Add("Als de zon te weinig warmte heeft gegeven \n warmt de CV ketel het water verder op");
		factList.Add("Zonnepanelen voor elektriciteit heb je \n binnen 8 tot 10 jaar terugverdiend");
		factList.Add("Het kost electricteitscollectoren langer dan \n warmtecollectoren om hun geld terug te verdienen");
	}
	
	void  Start (){
		
	}
	
	void  Update (){
		this.gameObject.transform.position = cam.WorldToViewportPoint(target.gameObject.transform.position + 5 * up);
	}
	
	public void displayFact (){
		if (this.guiText.text == "") {
			int random = Mathf.RoundToInt(Random.value * factList.Count);
			this.guiText.text = factList[random];	
		}
	}
	
	public void stopDisplay (){
		this.guiText.text = "";
	}
}