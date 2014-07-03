#pragma strict

private var up:Vector3;
private var cam:Camera;

private var target:GameObject;

private var factList:List.<String> = new List.<String>();

function Awake() {
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
	factList.Add("De zon is al 5 miljard jaar oud \n en zal nog 5.5 miljard jaar branden");
	factList.Add("De omtrek van de aarde is 40.000 km. \n Die van de zon is 4.4 miljoen!");
	factList.Add("Zonnepanelen werken zeker 20 jaar");
	factList.Add("Je kunt ook minder zonnepanelen op het \n dak leggen dan je eigenlijknodig hebt");
	factList.Add("Als je meer elektriciteit opwekt dan je gebruikt \n kun je dat verkopen aan het elektriciteitsbedrijf");
	factList.Add("Als de zon te weinig warmte heeft gegeven \n warmt de CV ketel het water verder op");
	factList.Add("Zonnepanelen voor elektriciteit heb je \n binnen 8 tot 10 jaar terugverdiend");
	factList.Add("Het kost electricteitscollectoren langer dan \n warmtecollectoren om hun geld terug te verdienen");
}

function Start () {

}

function Update () {
	this.gameObject.transform.position = cam.WorldToViewportPoint(target.gameObject.transform.position + 5 * up);
}

function displayFact():void {
	if (this.guiText.text == "") {
		var random:int = Random.value * factList.Count;
		this.guiText.text = factList[random];	
	}
}

function stopDisplay():void {
	this.guiText.text = "";
}