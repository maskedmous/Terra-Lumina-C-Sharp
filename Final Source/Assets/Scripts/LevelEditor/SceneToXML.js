#pragma strict

import System.Xml;	//needed for XML reading
import System.IO;	//needed for File IO (example: File.Exists)

public var levelName:String = "";		//name of the xml it will be saved as
public var levelID:int 		= 0;		//level ID (level0, level1, level2 etc)
public var overwrite:boolean = false;	//override the current .xml if it already exists

public var easy:boolean = false;		//boolean for level difficulty setting
public var medium:boolean = false;
public var hard:boolean = false;

private var xmlPath:String 	= "";		//initialized in the awake

private var prefabList:String[];

function Awake()
{
	if(Application.loadedLevelName == "LevelEditor")
	{	
		Debug.LogError("Please save this scene first as a new scene before saving the level");
	}
	else
	{
		xmlPath = Application.dataPath + "/LevelsXML/";
		
		if(levelName != "")
		{
			if(!levelName.Contains(".xml"))
			{
				levelName = levelName + ".xml";
			}
			
			if(saveLevel() == -1)
			{
				Debug.LogError("Xml not saved due to an error");
			}
		}
		else
		{
			Debug.LogError("You haven't filled in a Level Name, see SaveLevel");
		}
	}
}

private function saveLevel():int
{
	var xmlDocument:XmlDocument = new XmlDocument();
	var filePath:String = xmlPath + levelName;
	var masterNode:XmlElement = null;
	
	//check if the file already exists
	if(File.Exists(filePath))
	{
		//file already exists!
		//check if you may override or not (standard false)
		if(overwrite)
		{
			Debug.Log("overwriting = true, overwriting xml file");
			xmlDocument.Load(filePath);
			masterNode = xmlDocument.DocumentElement;
			masterNode.RemoveAll();
		}
		else
		{
			Debug.LogError("This file already exists and override is set to false");
			return -1;
		}
		
	}
	else
	{
		//file does not exist yet so we are going to create it
		File.WriteAllText(filePath, "<Root></Root>");
		//load the xml document
		xmlDocument.Load(filePath);
		//set the masternode for appending level stuff
		masterNode = xmlDocument.DocumentElement;
	}
	
	//begin saving the level elements!
	prefabList = Directory.GetFiles(Application.dataPath + "/Resources/Prefabs", "*.prefab", SearchOption.TopDirectoryOnly);
	//
	//Level ID
	//
		var levelIDNode:XmlElement = xmlDocument.CreateElement("LevelID");
		masterNode.AppendChild(levelIDNode);
		levelIDNode.InnerText = levelID.ToString();
		Debug.Log("Level ID: " + levelID);
	
	//
	//Difficulty
	//
		var difficultyNode:XmlElement = xmlDocument.CreateElement("Difficulty");
		masterNode.AppendChild(difficultyNode);
		
		if(easy != false || medium != false || hard != false)
		{
			if(easy == true && medium == false && hard == false)
			{
				difficultyNode.InnerText = "Easy";
				Debug.Log("Difficulty = easy");
			}
			else if(medium == true && easy == false && hard == false)
			{
				difficultyNode.InnerText = "Medium";
				Debug.Log("Difficulty = Medium");
			}
			else if(hard == true && easy == false && medium == false)
			{
				difficultyNode.InnerText = "Hard";
				Debug.Log("Difficulty = Hard");
			}
			else
			{
				Debug.LogError("Something went wrong with selecting the difficulty setting, thicked 2 boxes perhaps?");
				return -1;
			}
		}
		else
		{
			Debug.LogError("Please state the difficulty setting correctly");
			return -1;
		}
	
	//
	//camera
	//
		var mainCamera:GameObject = Camera.main.gameObject;
		var cameraNode:XmlElement = xmlDocument.CreateElement("Camera");
		masterNode.AppendChild(cameraNode);
		
		//save position
		var cameraPositionNode:XmlElement = xmlDocument.CreateElement("Position");
		cameraNode.AppendChild(cameraPositionNode);
		
		var cameraXNode:XmlElement = xmlDocument.CreateElement("x");
		var cameraYNode:XmlElement = xmlDocument.CreateElement("y");
		var cameraZNode:XmlElement = xmlDocument.CreateElement("z");
		
		cameraPositionNode.AppendChild(cameraXNode);
		cameraPositionNode.AppendChild(cameraYNode);
		cameraPositionNode.AppendChild(cameraZNode);
		
		cameraXNode.InnerText = mainCamera.transform.position.x.ToString();
		cameraYNode.InnerText = mainCamera.transform.position.y.ToString();
		cameraZNode.InnerText = mainCamera.transform.position.z.ToString();
		
		//save the rotation
		var cameraRotationNode:XmlElement = xmlDocument.CreateElement("Rotation");
		cameraNode.AppendChild(cameraRotationNode);
		
		var cameraXRotationNode:XmlElement = xmlDocument.CreateElement("x");
		var cameraYRotationNode:XmlElement = xmlDocument.CreateElement("y");
		var cameraZRotationNode:XmlElement = xmlDocument.CreateElement("z");
		
		cameraRotationNode.AppendChild(cameraXRotationNode);
		cameraRotationNode.AppendChild(cameraYRotationNode);
		cameraRotationNode.AppendChild(cameraZRotationNode);
		
		cameraXRotationNode.InnerText = mainCamera.transform.eulerAngles.x.ToString();
		cameraYRotationNode.InnerText = mainCamera.transform.eulerAngles.y.ToString();
		cameraZRotationNode.InnerText = mainCamera.transform.eulerAngles.z.ToString();
	//
	
	//
	//GameLogic xml
	//
		var gameLogic:XmlElement = xmlDocument.CreateElement("GameLogic");
		masterNode.AppendChild(gameLogic);
		
		var gameLogicObject:GameLogic = GameObject.Find("GameLogic").GetComponent(GameLogic);
		
		//save the variables into the GameLogic xml
		var batteryNode:XmlElement 					= xmlDocument.CreateElement("Battery");
		var maximumBatteryCapacityNode:XmlElement 	= xmlDocument.CreateElement("MaximumBatteryCapacity");
		var decreaseTimerNode:XmlElement 			= xmlDocument.CreateElement("DecreaseTimer");
		var negativeBatteryFlowNode:XmlElement 		= xmlDocument.CreateElement("NegativeBatteryFlow");
		var positiveBatteryFlowNode:XmlElement 		= xmlDocument.CreateElement("PositiveBatteryFlow");
		var crystalsToCompleteNode:XmlElement		= xmlDocument.CreateElement("CrystalsToComplete");
		var speedNode:XmlElement					= xmlDocument.CreateElement("Speed");
		
		var currentNormalAmmoNode:XmlElement		= xmlDocument.CreateElement("CurrentNormalAmmo");
		var maxNormalAmmoNode:XmlElement			= xmlDocument.CreateElement("MaximumNormalAmmo");
		var currentBumpyAmmoNode:XmlElement			= xmlDocument.CreateElement("CurrentBumpyAmmo");
		var maxBumpyAmmoNode:XmlElement				= xmlDocument.CreateElement("MaximumBumpyAmmo");
		
		var infiniteAmmoNode:XmlElement				= xmlDocument.CreateElement("InfiniteAmmo");
		var jumpDrainNode:XmlElement				= xmlDocument.CreateElement("JumpDrain"); 
		var flashDrainNode:XmlElement				= xmlDocument.CreateElement("FlashDrain");
		
		var platinumTimeNode:XmlElement				= xmlDocument.CreateElement("PlatinumTime");
		var goldTimeNode:XmlElement					= xmlDocument.CreateElement("GoldTime");
		var silverTimeNode:XmlElement				= xmlDocument.CreateElement("SilverTime");
		var bronzeTimeNode:XmlElement				= xmlDocument.CreateElement("BronzeTime");

			
		//append them
		gameLogic.AppendChild(batteryNode);
		gameLogic.AppendChild(maximumBatteryCapacityNode);
		gameLogic.AppendChild(decreaseTimerNode);
		gameLogic.AppendChild(negativeBatteryFlowNode);
		gameLogic.AppendChild(positiveBatteryFlowNode);
		gameLogic.AppendChild(crystalsToCompleteNode);
		gameLogic.AppendChild(speedNode);
		
		gameLogic.AppendChild(currentNormalAmmoNode);
		gameLogic.AppendChild(maxNormalAmmoNode);
		gameLogic.AppendChild(currentBumpyAmmoNode);
		gameLogic.AppendChild(maxBumpyAmmoNode);
		
		
		gameLogic.AppendChild(infiniteAmmoNode);
		gameLogic.AppendChild(jumpDrainNode);
		gameLogic.AppendChild(flashDrainNode);
		
		gameLogic.AppendChild(platinumTimeNode);
		gameLogic.AppendChild(goldTimeNode);
		gameLogic.AppendChild(silverTimeNode);
		gameLogic.AppendChild(bronzeTimeNode);
		
		
		//write text to it
		batteryNode.InnerText 					= gameLogicObject.getBattery().ToString();
		maximumBatteryCapacityNode.InnerText	= gameLogicObject.getBatteryCapacity().ToString();
		decreaseTimerNode.InnerText				= gameLogicObject.getDecreaseTimer().ToString();
		negativeBatteryFlowNode.InnerText		= gameLogicObject.getNegativeBatteryFlow().ToString();
		positiveBatteryFlowNode.InnerText		= gameLogicObject.getPositiveBatteryFlow().ToString();
		crystalsToCompleteNode.InnerText		= gameLogicObject.getCrystalsToComplete().ToString();
		
		currentNormalAmmoNode.InnerText			= gameLogicObject.getCurrentNormalSeeds().ToString();
		maxNormalAmmoNode.InnerText				= gameLogicObject.getMaximumNormalSeeds().ToString();
		currentBumpyAmmoNode.InnerText			= gameLogicObject.getCurrentBumpySeeds().ToString();
		maxBumpyAmmoNode.InnerText				= gameLogicObject.getMaximumBumpySeeds().ToString();
		
		infiniteAmmoNode.InnerText				= gameLogicObject.getInfiniteAmmo().ToString();
		jumpDrainNode.InnerText					= gameLogicObject.getJumpDrain().ToString();
		flashDrainNode.InnerText				= gameLogicObject.getFlashDrain().ToString();
		
		platinumTimeNode.InnerText				= gameLogicObject.getPlatinumTime().ToString();
		goldTimeNode.InnerText					= gameLogicObject.getGoldTime().ToString();
		silverTimeNode.InnerText				= gameLogicObject.getSilverTime().ToString();
		bronzeTimeNode.InnerText				= gameLogicObject.getBronzeTime().ToString();
	//
	
	//
	//Level xml
	//
		var levelNode:XmlElement = xmlDocument.CreateElement("Level");
		masterNode.AppendChild(levelNode);
		
		var levelObject:GameObject = GameObject.Find("Level");
		for(var _obj in levelObject.transform)
		{
			var obj:Transform 	= _obj as Transform;
			if(!checkValidPrefab(obj.name)) return -1;
			if(obj.gameObject.name != "SlugBound")
			{
				var objectNode:XmlElement 	= xmlDocument.CreateElement("GameObject");
				levelNode.AppendChild(objectNode);
				
				//save prefab name
				var prefabNode:XmlElement 	= xmlDocument.CreateElement("Prefab");
				objectNode.AppendChild(prefabNode);
				prefabNode.InnerText 		= obj.gameObject.name;
				
				//save position
				var positionNode:XmlElement = xmlDocument.CreateElement("Position");
				objectNode.AppendChild(positionNode);
				
				var xNode:XmlElement = xmlDocument.CreateElement("x");
				var yNode:XmlElement = xmlDocument.CreateElement("y");
				var zNode:XmlElement = xmlDocument.CreateElement("z");
				
				positionNode.AppendChild(xNode);
				positionNode.AppendChild(yNode);
				positionNode.AppendChild(zNode);
				
				xNode.InnerText = obj.gameObject.transform.position.x.ToString();
				yNode.InnerText = obj.gameObject.transform.position.y.ToString();
				zNode.InnerText = obj.gameObject.transform.position.z.ToString();
				
				//save the rotation
				var rotationNode:XmlElement = xmlDocument.CreateElement("Rotation");
				objectNode.AppendChild(rotationNode);
				
				var xRotationNode:XmlElement = xmlDocument.CreateElement("x");
				var yRotationNode:XmlElement = xmlDocument.CreateElement("y");
				var zRotationNode:XmlElement = xmlDocument.CreateElement("z");
				
				rotationNode.AppendChild(xRotationNode);
				rotationNode.AppendChild(yRotationNode);
				rotationNode.AppendChild(zRotationNode);
				
				xRotationNode.InnerText = obj.gameObject.transform.eulerAngles.x.ToString();
				yRotationNode.InnerText = obj.gameObject.transform.eulerAngles.y.ToString();
				zRotationNode.InnerText = obj.gameObject.transform.eulerAngles.z.ToString();
				
				//save the scale
				var scaleNode:XmlElement = xmlDocument.CreateElement("Scaling");
				objectNode.AppendChild(scaleNode);
				
				var xScaleNode:XmlElement = xmlDocument.CreateElement("x");
				var yScaleNode:XmlElement = xmlDocument.CreateElement("y");
				var zScaleNode:XmlElement = xmlDocument.CreateElement("z");
				
				scaleNode.AppendChild(xScaleNode);
				scaleNode.AppendChild(yScaleNode);
				scaleNode.AppendChild(zScaleNode);
				
				xScaleNode.InnerText = obj.gameObject.transform.localScale.x.ToString();
				yScaleNode.InnerText = obj.gameObject.transform.localScale.y.ToString();
				zScaleNode.InnerText = obj.gameObject.transform.localScale.z.ToString();
			}
			//special nodes
			if(obj.gameObject.name == "AmmoBox")
			{
				var ammoBoxNode:XmlElement = xmlDocument.CreateElement("AmmoBox");
				objectNode.AppendChild(ammoBoxNode);
				
				var ammoBox:AmmoBox = obj.FindChild("AmmoBox").GetComponent(AmmoBox) as AmmoBox;
				
				var extraSeedsNode:XmlElement    = xmlDocument.CreateElement("ExtraSeeds");
				var normalTypeNode:XmlElement    = xmlDocument.CreateElement("NormalType");
				var bumpyTypeNode:XmlElement 	 = xmlDocument.CreateElement("BumpyType");
				var timeToRespawnNode:XmlElement = xmlDocument.CreateElement("TimeToRespawn");
				var oneTimePickupNode:XmlElement = xmlDocument.CreateElement("OneTimePickup");
				
				ammoBoxNode.AppendChild(extraSeedsNode);
				ammoBoxNode.AppendChild(normalTypeNode);
				ammoBoxNode.AppendChild(bumpyTypeNode);
				ammoBoxNode.AppendChild(timeToRespawnNode);
				ammoBoxNode.AppendChild(oneTimePickupNode);
				
				extraSeedsNode.InnerText 	= ammoBox.getExtraSeeds().ToString();
				normalTypeNode.InnerText 	= ammoBox.getNormalType().ToString();
				bumpyTypeNode.InnerText 	= ammoBox.getBumpyType().ToString();
				timeToRespawnNode.InnerText = ammoBox.getTimeToRespawn().ToString();
				oneTimePickupNode.InnerText = ammoBox.getOneTimePickup().ToString();
				
			}
			if(obj.gameObject.name == "Slug")
			{
				var slugScript:SlugScript = obj.gameObject.GetComponent(SlugScript);
				var slugBoundA:GameObject = slugScript.getSlugBoundA();
				var slugBoundB:GameObject = slugScript.getSlugBoundB();
				
				if(slugBoundA == null || slugBoundB == null)
				{
					Debug.LogError("Slugbound is null!");
					return -1;
				}
				
				var slugNode:XmlElement = xmlDocument.CreateElement("Slug");
				objectNode.AppendChild(slugNode);
				
				//BoundA
				var slugBoundANode:XmlElement 	= xmlDocument.CreateElement("SlugBoundA");
				slugNode.AppendChild(slugBoundANode);
				
				var slugBoundAxNode:XmlElement = xmlDocument.CreateElement("x");
				var slugBoundAyNode:XmlElement = xmlDocument.CreateElement("y");
				var slugBoundAzNode:XmlElement = xmlDocument.CreateElement("z");
				
				slugBoundANode.AppendChild(slugBoundAxNode);
				slugBoundANode.AppendChild(slugBoundAyNode);
				slugBoundANode.AppendChild(slugBoundAzNode);
				
				slugBoundAxNode.InnerText = slugBoundA.transform.position.x.ToString();
				slugBoundAyNode.InnerText = slugBoundA.transform.position.y.ToString();
				slugBoundAzNode.InnerText = slugBoundA.transform.position.z.ToString();
				
				//BoundB
				var slugBoundBNode:XmlElement 	= xmlDocument.CreateElement("SlugBoundB");
				slugNode.AppendChild(slugBoundBNode);
				
				var slugBoundBxNode:XmlElement = xmlDocument.CreateElement("x");
				var slugBoundByNode:XmlElement = xmlDocument.CreateElement("y");
				var slugBoundBzNode:XmlElement = xmlDocument.CreateElement("z");
				
				slugBoundBNode.AppendChild(slugBoundBxNode);
				slugBoundBNode.AppendChild(slugBoundByNode);
				slugBoundBNode.AppendChild(slugBoundBzNode);
				
				slugBoundBxNode.InnerText = slugBoundB.transform.position.x.ToString();
				slugBoundByNode.InnerText = slugBoundB.transform.position.y.ToString();
				slugBoundBzNode.InnerText = slugBoundB.transform.position.z.ToString();
			}
			
			//only if it is a tutorial object
			if(obj.gameObject.name == "TutorialObject")
			{
				//required objects
				var triggerScript:TutorialTriggerScript = obj.gameObject.GetComponent(TutorialTriggerScript) as TutorialTriggerScript;
				var boxCollider:BoxCollider = obj.gameObject.GetComponent(BoxCollider) as BoxCollider;
				var boundingBox:Vector3 = boxCollider.size;
				
				//tutorial node
				var tutorialNode:XmlElement = xmlDocument.CreateElement("Tutorial");
				objectNode.AppendChild(tutorialNode);
				
				//Alpha Object
				var alphaObject:GameObject = triggerScript.getAlphaObject();
				
				if(alphaObject != null)
				{
					//main alphaobject node
					var alphaObjectNode:XmlElement = xmlDocument.CreateElement("AlphaObject");
					tutorialNode.AppendChild(alphaObjectNode);
					
					//prefab name
					var alphaPrefabNode:XmlElement = xmlDocument.CreateElement("Prefab");
					alphaObjectNode.AppendChild(alphaPrefabNode);
					alphaPrefabNode.InnerText = alphaObject.name;
					
					//position
					var alphaObjectPositionNode:XmlElement = xmlDocument.CreateElement("Position");
					alphaObjectNode.AppendChild(alphaObjectPositionNode);
					
					var alphaObjectXPositionNode:XmlElement = xmlDocument.CreateElement("x");
					var alphaObjectYPositionNode:XmlElement = xmlDocument.CreateElement("y");
					var alphaObjectZPositionNode:XmlElement = xmlDocument.CreateElement("z");
					
					alphaObjectPositionNode.AppendChild(alphaObjectXPositionNode);
					alphaObjectPositionNode.AppendChild(alphaObjectYPositionNode);
					alphaObjectPositionNode.AppendChild(alphaObjectZPositionNode);
					
					alphaObjectXPositionNode.InnerText = alphaObject.transform.position.x.ToString();
					alphaObjectYPositionNode.InnerText = alphaObject.transform.position.y.ToString();
					alphaObjectZPositionNode.InnerText = alphaObject.transform.position.z.ToString();
					
					//rotation
					var alphaObjectRotationNode:XmlElement = xmlDocument.CreateElement("Rotation");
					alphaObjectNode.AppendChild(alphaObjectRotationNode);
					
					var alphaObjectXRotationNode:XmlElement = xmlDocument.CreateElement("x");
					var alphaObjectYRotationNode:XmlElement = xmlDocument.CreateElement("y");
					var alphaObjectZRotationNode:XmlElement = xmlDocument.CreateElement("z");
					
					alphaObjectRotationNode.AppendChild(alphaObjectXRotationNode);
					alphaObjectRotationNode.AppendChild(alphaObjectYRotationNode);
					alphaObjectRotationNode.AppendChild(alphaObjectZRotationNode);
					
					alphaObjectXRotationNode.InnerText = alphaObject.transform.eulerAngles.x.ToString();
					alphaObjectYRotationNode.InnerText = alphaObject.transform.eulerAngles.y.ToString();
					alphaObjectZRotationNode.InnerText = alphaObject.transform.eulerAngles.z.ToString(); 
					
									
					//save the scale
					var alphaObjectScaleNode:XmlElement = xmlDocument.CreateElement("Scaling");
					alphaObjectNode.AppendChild(alphaObjectScaleNode);
					
					var alphaObjectScaleXNode:XmlElement = xmlDocument.CreateElement("x");
					var alphaObjectScaleYNode:XmlElement = xmlDocument.CreateElement("y");
					var alphaObjectScaleZNode:XmlElement = xmlDocument.CreateElement("z");
					
					alphaObjectScaleNode.AppendChild(alphaObjectScaleXNode);
					alphaObjectScaleNode.AppendChild(alphaObjectScaleYNode);
					alphaObjectScaleNode.AppendChild(alphaObjectScaleZNode);
					
					alphaObjectScaleXNode.InnerText = alphaObject.transform.localScale.x.ToString();
					alphaObjectScaleYNode.InnerText = alphaObject.transform.localScale.y.ToString();
					alphaObjectScaleZNode.InnerText = alphaObject.transform.localScale.z.ToString();
					    
				}
				
				var jumpButtonTutorialNode:XmlElement = xmlDocument.CreateElement("JumpButtonTutorial");
				var normalShroomButtonTutorialNode:XmlElement = xmlDocument.CreateElement("NormalShroomButtonTutorial");
				var flashButtonTutorialNode:XmlElement = xmlDocument.CreateElement("FlashButtonTutorial");
				var bumpyShroomButtonTutorialNode:XmlElement = xmlDocument.CreateElement("BumpyShroomButtonTutorial");
				
				var lightTutorialNode:XmlElement = xmlDocument.CreateElement("LightTutorial");
				var slugTutorialNode:XmlElement = xmlDocument.CreateElement("SlugTutorial");
				var crystalTutorialNode:XmlElement = xmlDocument.CreateElement("CrystalTutorial");
				
				tutorialNode.AppendChild(jumpButtonTutorialNode);
				tutorialNode.AppendChild(normalShroomButtonTutorialNode);
				tutorialNode.AppendChild(flashButtonTutorialNode);
				tutorialNode.AppendChild(bumpyShroomButtonTutorialNode);
				tutorialNode.AppendChild(lightTutorialNode);
				tutorialNode.AppendChild(slugTutorialNode);
				tutorialNode.AppendChild(crystalTutorialNode);
				
				
				jumpButtonTutorialNode.InnerText = triggerScript.getJumpButtonTutorial().ToString();
				normalShroomButtonTutorialNode.InnerText = triggerScript.getNormalShroomButtonTutorial().ToString();
				flashButtonTutorialNode.InnerText = triggerScript.getFlashButtonTutorial().ToString();
				bumpyShroomButtonTutorialNode.InnerText = triggerScript.getBumpyShroomButtonTutorial().ToString();
				lightTutorialNode.InnerText 	= triggerScript.getLightTutorial().ToString();
				slugTutorialNode.InnerText		= triggerScript.getSlugTutorial().ToString();
				crystalTutorialNode.InnerText 	= triggerScript.getCrystalTutorial().ToString();
				
				var slugObject:GameObject = triggerScript.getSlugObject();
				
				if(slugObject != null)
				{
					var slugObjectNode:XmlElement = xmlDocument.CreateElement("SlugObject");
					tutorialNode.AppendChild(slugObjectNode);
					//position
					var slugObjectPositionNode:XmlElement = xmlDocument.CreateElement("Position");
					slugObjectNode.AppendChild(slugObjectPositionNode);
					
					var slugObjectXPositionNode:XmlElement = xmlDocument.CreateElement("x");
					var slugObjectYPositionNode:XmlElement = xmlDocument.CreateElement("y");
					var slugObjectZPositionNode:XmlElement = xmlDocument.CreateElement("z");
					
					slugObjectPositionNode.AppendChild(slugObjectXPositionNode);
					slugObjectPositionNode.AppendChild(slugObjectYPositionNode);
					slugObjectPositionNode.AppendChild(slugObjectZPositionNode);
					
					slugObjectXPositionNode.InnerText = slugObject.transform.position.x.ToString();
					slugObjectYPositionNode.InnerText = slugObject.transform.position.y.ToString();
					slugObjectZPositionNode.InnerText = slugObject.transform.position.z.ToString();
					
					
					var slugScript2:SlugScript = slugObject.gameObject.GetComponent(SlugScript);
					var slugBoundA2:GameObject = slugScript2.getSlugBoundA();
					var slugBoundB2:GameObject = slugScript2.getSlugBoundB();
					
					if(slugBoundA2 == null || slugBoundB2 == null)
					{
						Debug.LogError("Slugbound is null!");
						return -1;
					}
					
					//BoundA
					var slugBoundANode2:XmlElement 	= xmlDocument.CreateElement("SlugBoundA");
					slugObjectNode.AppendChild(slugBoundANode2);
					
					var slugBoundAxNode2:XmlElement = xmlDocument.CreateElement("x");
					var slugBoundAyNode2:XmlElement = xmlDocument.CreateElement("y");
					var slugBoundAzNode2:XmlElement = xmlDocument.CreateElement("z");
					
					slugBoundANode2.AppendChild(slugBoundAxNode2);
					slugBoundANode2.AppendChild(slugBoundAyNode2);
					slugBoundANode2.AppendChild(slugBoundAzNode2);
					
					slugBoundAxNode2.InnerText = slugBoundA2.transform.position.x.ToString();
					slugBoundAyNode2.InnerText = slugBoundA2.transform.position.y.ToString();
					slugBoundAzNode2.InnerText = slugBoundA2.transform.position.z.ToString();
					
					//BoundB
					var slugBoundBNode2:XmlElement 	= xmlDocument.CreateElement("SlugBoundB");
					slugObjectNode.AppendChild(slugBoundBNode2);
					
					var slugBoundBxNode2:XmlElement = xmlDocument.CreateElement("x");
					var slugBoundByNode2:XmlElement = xmlDocument.CreateElement("y");
					var slugBoundBzNode2:XmlElement = xmlDocument.CreateElement("z");
					
					slugBoundBNode2.AppendChild(slugBoundBxNode2);
					slugBoundBNode2.AppendChild(slugBoundByNode2);
					slugBoundBNode2.AppendChild(slugBoundBzNode2);
					
					slugBoundBxNode2.InnerText = slugBoundB2.transform.position.x.ToString();
					slugBoundByNode2.InnerText = slugBoundB2.transform.position.y.ToString();
					slugBoundBzNode2.InnerText = slugBoundB2.transform.position.z.ToString();
				}
				
				var blockObject:GameObject = triggerScript.getBlockObject();
				
				if(blockObject != null)
				{
					var blockObjectNode:XmlElement = xmlDocument.CreateElement("BlockObject");
					tutorialNode.AppendChild(blockObjectNode);
					//position
					var blockObjectPositionNode:XmlElement = xmlDocument.CreateElement("Position");
					blockObjectNode.AppendChild(blockObjectPositionNode);
					
					var blockObjectXPositionNode:XmlElement = xmlDocument.CreateElement("x");
					var blockObjectYPositionNode:XmlElement = xmlDocument.CreateElement("y");
					var blockObjectZPositionNode:XmlElement = xmlDocument.CreateElement("z");
					
					blockObjectPositionNode.AppendChild(blockObjectXPositionNode);
					blockObjectPositionNode.AppendChild(blockObjectYPositionNode);
					blockObjectPositionNode.AppendChild(blockObjectZPositionNode);
					
					blockObjectXPositionNode.InnerText = blockObject.transform.position.x.ToString();
					blockObjectYPositionNode.InnerText = blockObject.transform.position.y.ToString();
					blockObjectZPositionNode.InnerText = blockObject.transform.position.z.ToString();
				}
				
				//button booleans
				var buttonsEnabledNode:XmlElement = xmlDocument.CreateElement("ButtonsEnabled");
				tutorialNode.AppendChild(buttonsEnabledNode);
				
				var movementLeftEnabledNode				:XmlElement = xmlDocument.CreateElement("MovementLeft");
				var movementRightEnabledNode			:XmlElement = xmlDocument.CreateElement("MovementRight");
				var jumpButtonEnabledNode				:XmlElement	= xmlDocument.CreateElement("JumpButton");
				var flashButtonEnabledNode				:XmlElement = xmlDocument.CreateElement("FlashButton");
				var shootNormalShroomButtonEnabledNode	:XmlElement = xmlDocument.CreateElement("NormalShroomButton");
				var shootBumpyShroomButtonEnabledNode	:XmlElement = xmlDocument.CreateElement("BumpyShroomButton");
				
				buttonsEnabledNode.AppendChild(movementLeftEnabledNode);
				buttonsEnabledNode.AppendChild(movementRightEnabledNode);
				buttonsEnabledNode.AppendChild(jumpButtonEnabledNode);
				buttonsEnabledNode.AppendChild(flashButtonEnabledNode);
				buttonsEnabledNode.AppendChild(shootNormalShroomButtonEnabledNode);
				buttonsEnabledNode.AppendChild(shootBumpyShroomButtonEnabledNode);
				
				movementLeftEnabledNode.InnerText 				= triggerScript.getMovementLeftEnabled().ToString();
				movementRightEnabledNode.InnerText 				= triggerScript.getMovementRightEnabled().ToString();
				jumpButtonEnabledNode.InnerText 				= triggerScript.getJumpButtonEnabled().ToString();
				flashButtonEnabledNode.InnerText				= triggerScript.getFlashButtonEnabled().ToString();
				shootNormalShroomButtonEnabledNode.InnerText 	= triggerScript.getNormalShroomButtonEnabled().ToString();
				shootBumpyShroomButtonEnabledNode.InnerText 	= triggerScript.getBumpyShroomButtonEnabled().ToString();
				
				if(triggerScript.getTutorialTextureA() != "" || triggerScript.getTutorialTextureB() != "")
				{
					//textures
					var tutorialTexturesNode:XmlElement = xmlDocument.CreateElement("Textures");
					tutorialNode.AppendChild(tutorialTexturesNode);
					
				
					//textureA
					if(triggerScript.getTutorialTextureA() != "")
					{
						var textureANode:XmlElement = xmlDocument.CreateElement("TextureA");
						tutorialTexturesNode.AppendChild(textureANode);
					
					
						var textureANameNode:XmlElement = xmlDocument.CreateElement("Texturename");
						var textureAXPositionNode:XmlElement = xmlDocument.CreateElement("x");
						var textureAYPositionNode:XmlElement = xmlDocument.CreateElement("y");
						var textureATimerNode:XmlElement = xmlDocument.CreateElement("Timer");
						
						textureANode.AppendChild(textureANameNode);
						textureANode.AppendChild(textureAXPositionNode);
						textureANode.AppendChild(textureAYPositionNode);
						textureANode.AppendChild(textureATimerNode);
						
						textureANameNode.InnerText 		= triggerScript.getTutorialTextureA();
						textureAXPositionNode.InnerText = triggerScript.getXPositionTexA().ToString();
						textureAYPositionNode.InnerText = triggerScript.getYPositionTexA().ToString();
						textureATimerNode.InnerText 	= triggerScript.getTimerTexA().ToString();
					}
					//textureB
					if(triggerScript.getTutorialTextureB() != "")
					{
						var textureBNode:XmlElement = xmlDocument.CreateElement("TextureB");
						tutorialTexturesNode.AppendChild(textureBNode);
						
						var textureBNameNode:XmlElement = xmlDocument.CreateElement("Texturename");
						var textureBXPositionNode:XmlElement = xmlDocument.CreateElement("x");
						var textureBYPositionNode:XmlElement = xmlDocument.CreateElement("y");
						var textureBTimerNode:XmlElement = xmlDocument.CreateElement("Timer");
						
						textureBNode.AppendChild(textureBNameNode);
						textureBNode.AppendChild(textureBXPositionNode);
						textureBNode.AppendChild(textureBYPositionNode);
						textureBNode.AppendChild(textureBTimerNode);
						
						textureBNameNode.InnerText 		= triggerScript.getTutorialTextureB();
						textureBXPositionNode.InnerText = triggerScript.getXPositionTexB().ToString();
						textureBYPositionNode.InnerText = triggerScript.getYPositionTexB().ToString();
						textureBTimerNode.InnerText 	= triggerScript.getTimerTexB().ToString();
					}
				}

				
				//destroy on exit boolean
				var destroyOnExitNode:XmlElement = xmlDocument.CreateElement("DestroyOnExit");
				tutorialNode.AppendChild(destroyOnExitNode);
				destroyOnExitNode.InnerText = triggerScript.getDestroyOnExit().ToString();
				
				var destroyOnCompletionNode:XmlElement = xmlDocument.CreateElement("DestroyOnCompletion");
				tutorialNode.AppendChild(destroyOnCompletionNode);
				destroyOnCompletionNode.InnerText = triggerScript.getDestroyOnCompletion().ToString();
				
				//bounding box
				var boundingBoxNode:XmlElement = xmlDocument.CreateElement("BoundingBox");
				tutorialNode.AppendChild(boundingBoxNode);
				
				var xBoundingBoxNode:XmlElement = xmlDocument.CreateElement("x");
				var yBoundingBoxNode:XmlElement = xmlDocument.CreateElement("y");
				var zBoundingBoxNode:XmlElement = xmlDocument.CreateElement("z");
				
				boundingBoxNode.AppendChild(xBoundingBoxNode);
				boundingBoxNode.AppendChild(yBoundingBoxNode);
				boundingBoxNode.AppendChild(zBoundingBoxNode);
				
				xBoundingBoxNode.InnerText = boundingBox.x.ToString();
				yBoundingBoxNode.InnerText = boundingBox.y.ToString();
				zBoundingBoxNode.InnerText = boundingBox.z.ToString();
			}
		}
	//
	
	//player
	//
			var player:GameObject = GameObject.Find("Player");
			
			var playerNode:XmlElement = xmlDocument.CreateElement("Player");
			masterNode.AppendChild(playerNode);
			
			//save position
			var playerPositionNode:XmlElement = xmlDocument.CreateElement("Position");
			playerNode.AppendChild(playerPositionNode);
			
			var playerXNode:XmlElement = xmlDocument.CreateElement("x");
			var playerYNode:XmlElement = xmlDocument.CreateElement("y");
			var playerZNode:XmlElement = xmlDocument.CreateElement("z");
			
			playerPositionNode.AppendChild(playerXNode);
			playerPositionNode.AppendChild(playerYNode);
			playerPositionNode.AppendChild(playerZNode);
			
			playerXNode.InnerText = player.transform.position.x.ToString();
			playerYNode.InnerText = player.transform.position.y.ToString();
			playerZNode.InnerText = player.transform.position.z.ToString();
			
			//save the rotation
			var playerRotationNode:XmlElement = xmlDocument.CreateElement("Rotation");
			playerNode.AppendChild(playerRotationNode);
			
			var playerXRotationNode:XmlElement = xmlDocument.CreateElement("x");
			var playerYRotationNode:XmlElement = xmlDocument.CreateElement("y");
			var playerZRotationNode:XmlElement = xmlDocument.CreateElement("z");
			
			playerRotationNode.AppendChild(playerXRotationNode);
			playerRotationNode.AppendChild(playerYRotationNode);
			playerRotationNode.AppendChild(playerZRotationNode);
			
			playerXRotationNode.InnerText = player.transform.eulerAngles.x.ToString();
			playerYRotationNode.InnerText = player.transform.eulerAngles.y.ToString();
			playerZRotationNode.InnerText = player.transform.eulerAngles.z.ToString();
	//
	
	xmlDocument.Save(filePath);
	Debug.Log("Xml saved to: Assets/LevelsXML/" + levelName);
	return 0;
}

private function checkValidPrefab(prefabName:String):boolean
{	
	for each(var prefab:String in prefabList)
	{
		if(Path.GetFileNameWithoutExtension(prefab) == prefabName)
		{
			return true;
		}
	}
	//can't find it so returning false
	Debug.LogError("Can't find prefab with the name: " + prefabName);
	Debug.LogError("Please make sure you made a prefab of the object and is located in Resources/Prefabs");
	return false;
}