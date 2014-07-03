#pragma strict

import System.Xml;	//needed for XML reading
import System.IO;	//needed for File IO (example: File.Exists)

private var xmlPath:String = "";
private var xmlLevel:String = "";
private var isLoading:boolean = false;

public function Awake():void
{
	xmlPath = Application.dataPath + "/LevelsXML/";	//standard XML Level Path
}

public function setLevel(levelString:String):void
{
	xmlLevel = levelString;
	
	if(!xmlLevel.Contains(".xml"))
	{
		xmlLevel += ".xml";
	}
}

public function loadLevel():void
{
	if(!isLoading)
	{
		isLoading = true;
		var filePath:String = xmlPath + xmlLevel;
		var xmlDocument:XmlDocument = new XmlDocument();
		
		if(File.Exists(filePath))
		{		
			xmlDocument.Load(filePath);
			//getting the necessary gameobjects into variables
			var camera:GameObject 	= Camera.main.gameObject;
			var gameLogic:GameLogic = GameObject.Find("GameLogic").GetComponent(GameLogic);
			var level:GameObject 	= GameObject.Find("Level");
			var player:GameObject 	= GameObject.Find("Player");
			var textureLoader:TextureLoader = GameObject.Find("TextureLoader").GetComponent(TextureLoader);
			
			var rootNode:XmlNode 		= xmlDocument.DocumentElement;
			var masterNode:XmlNodeList 	= rootNode.ChildNodes;
			
			for each(var _nodes in masterNode)
			{
				var nodes:XmlNode = _nodes as XmlNode;
				if(nodes.Name == "Camera")
				{
					var cameraNodesList:XmlNodeList = nodes.ChildNodes;
					
					for each(var _cameraStatsNodes in cameraNodesList)
					{
						var cameraStatsNodes:XmlNode = _cameraStatsNodes as XmlNode;
						//get position
						if(cameraStatsNodes.Name == "Position")
						{
							//get x,y,z nodes
							var cameraPositionStatsList:XmlNodeList = cameraStatsNodes.ChildNodes;
							
							for each(var _cameraPositionStats in cameraPositionStatsList)
							{
								var cameraPositionStats:XmlNode = _cameraPositionStats as XmlNode;
								
								if(cameraPositionStats.Name == "x")
								{
									camera.transform.position.x = float.Parse(cameraPositionStats.InnerText);
								}
								if(cameraPositionStats.Name == "y")
								{
									camera.transform.position.y = float.Parse(cameraPositionStats.InnerText);
								}
								if(cameraPositionStats.Name == "z")
								{
									camera.transform.position.z = float.Parse(cameraPositionStats.InnerText);
								}
							}
						}
						//get rotation
						if(cameraStatsNodes.Name == "Rotation")
						{
							var cameraRotationStatsList:XmlNodeList = cameraStatsNodes.ChildNodes;
							
							for each(var _cameraRotationStats in cameraRotationStatsList)
							{
								var cameraRotationStats:XmlNode = _cameraRotationStats as XmlNode;
								
								if(cameraRotationStats.Name == "x")
								{
									camera.transform.rotation.eulerAngles.x = float.Parse(cameraRotationStats.InnerText);
								}
								if(cameraRotationStats.Name == "y")
								{
									camera.transform.rotation.eulerAngles.y = float.Parse(cameraRotationStats.InnerText);
								}
								if(cameraRotationStats.Name == "z")
								{
									camera.transform.rotation.eulerAngles.z = float.Parse(cameraRotationStats.InnerText);
								}
							}
						}
					}
					
				}
				
				if(nodes.Name == "GameLogic")
				{
					var gameLogicStatsNodes:XmlNodeList = nodes.ChildNodes;
					
					for each(var _gameLogicStats in gameLogicStatsNodes)
					{
						var gameLogicStats:XmlNode = _gameLogicStats as XmlNode;
						
						if(gameLogicStats.Name == "Battery")
						{
							gameLogic.setBattery(float.Parse(gameLogicStats.InnerText));
						}
						if(gameLogicStats.Name == "MaximumBatteryCapacity")
						{
							gameLogic.setBatteryCapacity(int.Parse(gameLogicStats.InnerText));
						}
						if(gameLogicStats.Name == "DecreaseTimer")
						{
							gameLogic.setDecreaseTimer(float.Parse(gameLogicStats.InnerText));
						}
						if(gameLogicStats.Name == "NegativeBatteryFlow")
						{
							gameLogic.setNegativeBatteryFlow(int.Parse(gameLogicStats.InnerText));
						}
						if(gameLogicStats.Name == "PositiveBatteryFlow")
						{
							gameLogic.setPositiveBatteryFlow(int.Parse(gameLogicStats.InnerText));
						}
						if(gameLogicStats.Name == "CrystalsToComplete")
						{
							gameLogic.setCrystalsToComplete(int.Parse(gameLogicStats.InnerText));
						}
						
						if(gameLogicStats.Name == "CurrentNormalAmmo")
						{
							gameLogic.setCurrentNormalSeeds(int.Parse(gameLogicStats.InnerText));
						}
						
						if(gameLogicStats.Name == "MaximumNormalAmmo")
						{
							gameLogic.setMaximumNormalSeeds(int.Parse(gameLogicStats.InnerText));
						}
						if(gameLogicStats.Name == "CurrentBumpyAmmo")
						{
							gameLogic.setCurrentBumpySeeds(int.Parse(gameLogicStats.InnerText));
						}
						if(gameLogicStats.Name == "MaximumBumpyAmmo")
						{
							gameLogic.setMaximumBumpySeeds(int.Parse(gameLogicStats.InnerText));
						}
						
						if(gameLogicStats.Name == "InfiniteAmmo")
						{
							gameLogic.setInfiniteAmmo(boolean.Parse(gameLogicStats.InnerText));
						}
						
						if (gameLogicStats.Name == "JumpDrain")
						{
							gameLogic.setJumpDrain(float.Parse(gameLogicStats.InnerText));
						}
						
						if (gameLogicStats.Name == "FlashDrain")
						{
							gameLogic.setFlashDrain(float.Parse(gameLogicStats.InnerText));
						}
			
						if (gameLogicStats.Name == "PlatinumTime")
						{
							gameLogic.setPlatinumTime(int.Parse(gameLogicStats.InnerText));
						}
						if (gameLogicStats.Name == "GoldTime")
						{
							gameLogic.setGoldTime(int.Parse(gameLogicStats.InnerText));
						}
						if (gameLogicStats.Name == "SilverTime")
						{
							gameLogic.setSilverTime(int.Parse(gameLogicStats.InnerText));
						}
						if (gameLogicStats.Name == "BronzeTime")
						{
							gameLogic.setBronzeTime(int.Parse(gameLogicStats.InnerText));
						}
					}
				}
				
				if(nodes.Name == "Level")
				{
					
					var gameObjectNodeList:XmlNodeList = nodes.ChildNodes;
					
					for each(var _gameObjectNodes in gameObjectNodeList)
					{
						var gameObjectNodes:XmlNode = _gameObjectNodes as XmlNode;
						var gameObjectStatsNodeList = gameObjectNodes.ChildNodes;
						
						var newGameObject	:GameObject = null;
						var prefabName		:String 	= "";
						var position		:Vector3 	= new Vector3(9999,9999,9999);
						var rotation		:Vector3 	= new Vector3(9999,9999,9999);
						var scaling			:Vector3 	= new Vector3(9999,9999,9999);
						
						//slug things
						var slugBoundAPosition	:Vector3	= new Vector3(9999,9999,9999);
						var slugBoundBPosition	:Vector3	= new Vector3(9999,9999,9999);
						
						//ammo box things
						var extraSeeds:int = 0;
						var normalType:boolean = false;
						var bumpyType:boolean = false;
						var timeToRespawn:float = 9999.9f;
						var oneTimePickup:boolean = false;
						
						//tutorial things
						var boundingBox				:Vector3 	= new Vector3(9999,9999,9999);
						
						var alphaObjectPrefabName	:String		= "";
						var alphaObjectPosition		:Vector3	= new Vector3(9999,9999,9999);
						var alphaObjectRotation		:Vector3	= new Vector3(9999,9999,9999);
						var alphaObjectScaling		:Vector3	= new Vector3(9999,9999,9999);
						
						var jumpButtonTutorial				:boolean	= false;
						var normalShroomButtonTutorial		:boolean	= false;
						var flashButtonTutorial				:boolean	= false;
						var bumpyShroomButtonTutorial		:boolean	= false;
						
						var lightTutorial			:boolean	= false;
						var slugTutorial			:boolean	= false;
						var crystalTutorial			:boolean	= false;
						
						var slugObjectPosition		:Vector3	= new Vector3(9999,9999,9999);
						var blockObjectPosition		:Vector3	= new Vector3(9999,9999,9999);
						
						var movementLeftEnabled		:boolean	= true;
						var movementRightEnabled	:boolean	= true;
						var jumpButtonEnabled		:boolean	= true;
						var flashButtonEnabled		:boolean	= true;
						var normalShroomButtonEnabled:boolean	= true;
						var bumpyShroomButtonEnabled:boolean	= true;
						
						var tutorialTextureA		:Texture2D	= null;
						var xPositionTexA			:float		= 0;
						var yPositionTexA			:float		= 0;
						var timerTexA				:float		= 0;
						
						var tutorialTextureB		:Texture2D	= null;
						var xPositionTexB			:float		= 0;
						var yPositionTexB			:float		= 0;
						var timerTexB				:float		= 0;
						
						var destroyOnExit			:boolean	= false;
						var destroyOnCompletion		:boolean	= false;
						
						for each(var _gameObjectStatsNodes in gameObjectStatsNodeList)
						{
							var gameObjectStatsNodes:XmlNode = _gameObjectStatsNodes as XmlNode;
					
							if(gameObjectStatsNodes.Name == "Prefab")
							{
								prefabName = gameObjectStatsNodes.InnerText;
							}
							if(gameObjectStatsNodes.Name == "Position")
							{
								var gameObjectPositionNodes:XmlNodeList = gameObjectStatsNodes.ChildNodes;
								
								for each(var _positionNodes in gameObjectPositionNodes)
								{
									var positionNode:XmlNode = _positionNodes as XmlNode;
									
									if(positionNode.Name == "x")
									{
										//newGameObject.transform.position.x = float.Parse(positionNode.InnerText);
										position.x = float.Parse(positionNode.InnerText);
									}
									if(positionNode.Name == "y")
									{
										//newGameObject.transform.position.y = float.Parse(positionNode.InnerText);
										position.y = float.Parse(positionNode.InnerText);
									}
									if(positionNode.Name == "z")
									{
										//newGameObject.transform.position.z = float.Parse(positionNode.InnerText);
										position.z = float.Parse(positionNode.InnerText);
									}
								}
							}
							
							if(gameObjectStatsNodes.Name == "Rotation")
							{
								var gameObjectRotationNodes:XmlNodeList = gameObjectStatsNodes.ChildNodes;
								
								for each(var _rotationNodes in gameObjectRotationNodes)
								{
									var rotationNode:XmlNode = _rotationNodes as XmlNode;
									
									if(rotationNode.Name == "x")
									{
										rotation.x = float.Parse(rotationNode.InnerText);
									}
									if(rotationNode.Name == "y")
									{
										rotation.y = float.Parse(rotationNode.InnerText);
									}
									if(rotationNode.Name == "z")
									{
										rotation.z = float.Parse(rotationNode.InnerText);
									}
								}
							}
							
							if(gameObjectStatsNodes.Name == "Scaling")
							{
								var gameObjectScalingNodes:XmlNodeList = gameObjectStatsNodes.ChildNodes;
								
								for each(var _scalingNodes in gameObjectScalingNodes)
								{
									var scalingNode:XmlNode = _scalingNodes as XmlNode;
									
									if(scalingNode.Name == "x")
									{
										scaling.x = float.Parse(scalingNode.InnerText);
									}
									if(scalingNode.Name == "y")
									{
										scaling.y = float.Parse(scalingNode.InnerText);
									}
									if(scalingNode.Name == "z")
									{
										scaling.z = float.Parse(scalingNode.InnerText);
									}
								}
							}
							
							if(gameObjectStatsNodes.Name == "AmmoBox")
							{
								var ammoBoxStatsNodes:XmlNodeList = gameObjectStatsNodes.ChildNodes;
								for each(var _ammoBoxStats in ammoBoxStatsNodes)
								{
									var ammoBoxStats:XmlNode = _ammoBoxStats as XmlNode;
									
									if(ammoBoxStats.Name == "ExtraSeeds") extraSeeds = int.Parse(ammoBoxStats.InnerText);
									if(ammoBoxStats.Name == "NormalType") normalType = boolean.Parse(ammoBoxStats.InnerText);
									if(ammoBoxStats.Name == "BumpyType") bumpyType = boolean.Parse(ammoBoxStats.InnerText);
									if(ammoBoxStats.Name == "TimeToRespawn") timeToRespawn = float.Parse(ammoBoxStats.InnerText);
									if(ammoBoxStats.Name == "OneTimePickup") oneTimePickup = boolean.Parse(ammoBoxStats.InnerText);
								}
							}
							
							if(gameObjectStatsNodes.Name == "Slug")
							{
								var slugNode:XmlNodeList = gameObjectStatsNodes.ChildNodes;
								
								for each(var _slugStatsNode in slugNode)
								{
									var slugStatsNode:XmlNode = _slugStatsNode as XmlNode;
									
									if(slugStatsNode.Name == "SlugBoundA")
									{
										var slugBoundANode:XmlNodeList = slugStatsNode.ChildNodes;
										
										for each(var _slugBoundAPositionNodes in slugBoundANode)
										{
											var slugBoundAPositionNodes:XmlNode = _slugBoundAPositionNodes as XmlNode;
											
											if(slugBoundAPositionNodes.Name == "x") slugBoundAPosition.x = float.Parse(slugBoundAPositionNodes.InnerText);
											if(slugBoundAPositionNodes.Name == "y") slugBoundAPosition.y = float.Parse(slugBoundAPositionNodes.InnerText);
											if(slugBoundAPositionNodes.Name == "z") slugBoundAPosition.z = float.Parse(slugBoundAPositionNodes.InnerText);
										}
									}
									if(slugStatsNode.Name == "SlugBoundB")
									{
										var slugBoundBNode:XmlNodeList = slugStatsNode.ChildNodes;
										
										for each(var _slugBoundBPositionNodes in slugBoundBNode)
										{
											var slugBoundBPositionNodes:XmlNode = _slugBoundBPositionNodes as XmlNode;
											
											if(slugBoundBPositionNodes.Name == "x") slugBoundBPosition.x = float.Parse(slugBoundBPositionNodes.InnerText);
											if(slugBoundBPositionNodes.Name == "y") slugBoundBPosition.y = float.Parse(slugBoundBPositionNodes.InnerText);
											if(slugBoundBPositionNodes.Name == "z") slugBoundBPosition.z = float.Parse(slugBoundBPositionNodes.InnerText);
											
										}
									}
								}
							}
							
							if(gameObjectStatsNodes.Name == "Tutorial")
							{
								var tutorialNode:XmlNodeList = gameObjectStatsNodes.ChildNodes;
								
								for each(var _tutorialNodeStats in tutorialNode)
								{
									var tutorialNodeStats:XmlNode = _tutorialNodeStats as XmlNode;
									
									if(tutorialNodeStats.Name == "AlphaObject")
									{
										var alphaObjectStatsNodes:XmlNodeList = tutorialNodeStats.ChildNodes;
										
										for each(var _alphaObjectStats in alphaObjectStatsNodes)
										{
											var alphaObjectStats:XmlNode = _alphaObjectStats as XmlNode;
											
											if(alphaObjectStats.Name == "Prefab")
											{
												alphaObjectPrefabName = alphaObjectStats.InnerText;
											}
											if(alphaObjectStats.Name == "Position")
											{
												for each(var _alphaObjectPositionNodes in alphaObjectStats.ChildNodes)
												{
													var alphaObjectPositionNodes:XmlNode = _alphaObjectPositionNodes as XmlNode;
													
													if(alphaObjectPositionNodes.Name == "x") alphaObjectPosition.x = float.Parse(alphaObjectPositionNodes.InnerText);
													if(alphaObjectPositionNodes.Name == "y") alphaObjectPosition.y = float.Parse(alphaObjectPositionNodes.InnerText);
													if(alphaObjectPositionNodes.Name == "z") alphaObjectPosition.z = float.Parse(alphaObjectPositionNodes.InnerText);
												}
											}
											if(alphaObjectStats.Name == "Rotation")
											{
												for each(var _alphaObjectRotationNodes in alphaObjectStats.ChildNodes)
												{
													var alphaObjectRotationNodes:XmlNode = _alphaObjectRotationNodes as XmlNode;
													if(alphaObjectRotationNodes.Name == "x") alphaObjectRotation.x = float.Parse(alphaObjectRotationNodes.InnerText);
													if(alphaObjectRotationNodes.Name == "y") alphaObjectRotation.y = float.Parse(alphaObjectRotationNodes.InnerText);
													if(alphaObjectRotationNodes.Name == "z") alphaObjectRotation.z = float.Parse(alphaObjectRotationNodes.InnerText);
												}
											}
											if(alphaObjectStats.Name == "Scaling")
											{
												for each(var _alphaObjectScalingNodes in alphaObjectStats.ChildNodes)
												{
													var alphaObjectScalingNodes:XmlNode = _alphaObjectScalingNodes as XmlNode;
													if(alphaObjectScalingNodes.Name == "x") alphaObjectScaling.x = float.Parse(alphaObjectScalingNodes.InnerText);
													if(alphaObjectScalingNodes.Name == "y") alphaObjectScaling.y = float.Parse(alphaObjectScalingNodes.InnerText);
													if(alphaObjectScalingNodes.Name == "z") alphaObjectScaling.z = float.Parse(alphaObjectScalingNodes.InnerText);
												}
											}
										}
									}
									
									if(tutorialNodeStats.Name == "SlugObject")
									{
										var slugNode2:XmlNodeList = tutorialNodeStats.ChildNodes;
										
										for each(var _slugStatsNode2 in slugNode2)
										{
											var slugStatsNode2:XmlNode = _slugStatsNode2 as XmlNode;
											
											if(slugStatsNode2.Name == "Position")
											{
												for each(var _slugPositionNodes2 in slugStatsNode2.ChildNodes)
												{
													var slugPositionNodes2:XmlNode = _slugPositionNodes2 as XmlNode;
													if(slugPositionNodes2.Name == "x") slugObjectPosition.x = float.Parse(slugPositionNodes2.InnerText);
													if(slugPositionNodes2.Name == "y") slugObjectPosition.y = float.Parse(slugPositionNodes2.InnerText);
													if(slugPositionNodes2.Name == "z") slugObjectPosition.z = float.Parse(slugPositionNodes2.InnerText);
												}
											}
											
											if(slugStatsNode2.Name == "SlugBoundA")
											{
												var slugBoundANode2:XmlNodeList = slugStatsNode2.ChildNodes;
												
												for each(var _slugBoundAPositionNodes2 in slugBoundANode2)
												{
													var slugBoundAPositionNodes2:XmlNode = _slugBoundAPositionNodes2 as XmlNode;
													
													if(slugBoundAPositionNodes2.Name == "x") slugBoundAPosition.x = float.Parse(slugBoundAPositionNodes2.InnerText);
													if(slugBoundAPositionNodes2.Name == "y") slugBoundAPosition.y = float.Parse(slugBoundAPositionNodes2.InnerText);
													if(slugBoundAPositionNodes2.Name == "z") slugBoundAPosition.z = float.Parse(slugBoundAPositionNodes2.InnerText);
												}
											}
											if(slugStatsNode2.Name == "SlugBoundB")
											{
												var slugBoundBNode2:XmlNodeList = slugStatsNode2.ChildNodes;
												
												for each(var _slugBoundBPositionNodes2 in slugBoundBNode2)
												{
													var slugBoundBPositionNodes2:XmlNode = _slugBoundBPositionNodes2 as XmlNode;
													
													if(slugBoundBPositionNodes2.Name == "x") slugBoundBPosition.x = float.Parse(slugBoundBPositionNodes2.InnerText);
													if(slugBoundBPositionNodes2.Name == "y") slugBoundBPosition.y = float.Parse(slugBoundBPositionNodes2.InnerText);
													if(slugBoundBPositionNodes2.Name == "z") slugBoundBPosition.z = float.Parse(slugBoundBPositionNodes2.InnerText);
													
												}
											}
										}
									}
									
									if(tutorialNodeStats.Name == "JumpButtonTutorial")
									{
										jumpButtonTutorial = boolean.Parse(tutorialNodeStats.InnerText);
									}
									if(tutorialNodeStats.Name == "NormalShroomButtonTutorial")
									{
									 	normalShroomButtonTutorial = boolean.Parse(tutorialNodeStats.InnerText);
									}
									if(tutorialNodeStats.Name == "FlashButtonTutorial")
									{
									 	flashButtonTutorial = boolean.Parse(tutorialNodeStats.InnerText);
									}
									if(tutorialNodeStats.Name == "BumpyShroomButtonTutorial")
									{
										bumpyShroomButtonTutorial = boolean.Parse(tutorialNodeStats.InnerText);
									}
									
									
									if(tutorialNodeStats.Name == "LightTutorial")
									{
										lightTutorial = boolean.Parse(tutorialNodeStats.InnerText);
									}
									if(tutorialNodeStats.Name == "SlugTutorial")
									{
										slugTutorial = boolean.Parse(tutorialNodeStats.InnerText);
									}
									if(tutorialNodeStats.Name == "CrystalTutorial")
									{
										crystalTutorial = boolean.Parse(tutorialNodeStats.InnerText);
									}
									
									if(tutorialNodeStats.Name == "BlockObject")
									{
										for each(var _blockObjectStats in tutorialNodeStats.ChildNodes)
										{
											var blockObjectStats:XmlNode = _blockObjectStats as XmlNode;
											
											for each(var _blockObjectPositionNodes in blockObjectStats.ChildNodes)
											{
												var blockObjectPositionNodes:XmlNode = _blockObjectPositionNodes as XmlNode;
												
												if(blockObjectPositionNodes.Name == "x")
												{
													blockObjectPosition.x = float.Parse(blockObjectPositionNodes.InnerText);
												}
												if(blockObjectPositionNodes.Name == "y")
												{
													blockObjectPosition.y = float.Parse(blockObjectPositionNodes.InnerText);
												}
												if(blockObjectPositionNodes.Name == "z")
												{
													blockObjectPosition.z = float.Parse(blockObjectPositionNodes.InnerText);
												}
											}
										}
									}
									
									if(tutorialNodeStats.Name == "ButtonsEnabled")
									{
										var buttonNodes:XmlNodeList = tutorialNodeStats.ChildNodes;
										
										for each(var _buttonNodeStats in buttonNodes)
										{
											var buttonNodeStats:XmlNode = _buttonNodeStats as XmlNode;
											
											if(buttonNodeStats.Name == "MovementLeft")
											{
												movementLeftEnabled = boolean.Parse(buttonNodeStats.InnerText);
											}
											if(buttonNodeStats.Name == "MovementRight")
											{
												movementRightEnabled = boolean.Parse(buttonNodeStats.InnerText);
											}
											if(buttonNodeStats.Name == "JumpButton")
											{
												jumpButtonEnabled = boolean.Parse(buttonNodeStats.InnerText);
											}
											if(buttonNodeStats.Name == "FlashButton")
											{
												flashButtonEnabled = boolean.Parse(buttonNodeStats.InnerText);
											}
											if(buttonNodeStats.Name == "NormalShroomButton")
											{
												normalShroomButtonEnabled = boolean.Parse(buttonNodeStats.InnerText);
											}
											if(buttonNodeStats.Name == "BumpyShroomButton")
											{
												bumpyShroomButtonEnabled = boolean.Parse(buttonNodeStats.InnerText);
											}
										}
									}
									if(tutorialNodeStats.Name == "Textures")
									{
										var textureNodes:XmlNodeList = tutorialNodeStats.ChildNodes;
										
										for each(var _textureNodeStats in textureNodes)
										{
											var textureNodeStats:XmlNode = _textureNodeStats as XmlNode;
											
											if(textureNodeStats.Name == "TextureA")
											{
												var textureANode:XmlNodeList = textureNodeStats.ChildNodes;
												
												for each(var _textureANodeStats in textureANode)
												{
													var textureANodeStats:XmlNode = _textureANodeStats as XmlNode;
													
													if(textureANodeStats.Name == "Texturename")
													{
														tutorialTextureA = textureLoader.getTexture(textureANodeStats.InnerText);
													}
													if(textureANodeStats.Name == "x")
													{
														xPositionTexA = float.Parse(textureANodeStats.InnerText);
													}
													if(textureANodeStats.Name == "y")
													{
														yPositionTexA = float.Parse(textureANodeStats.InnerText);
													}
													if(textureANodeStats.Name == "Timer")
													{
														timerTexA = float.Parse(textureANodeStats.InnerText);
													}
												}
											}
											if(textureNodeStats.Name == "TextureB")
											{
												var textureBNode:XmlNodeList = textureNodeStats.ChildNodes;
												
												for each(var _textureBNodeStats in textureBNode)
												{
													var textureBNodeStats:XmlNode = _textureBNodeStats as XmlNode;
													
													if(textureBNodeStats.Name == "Texturename")
													{
														tutorialTextureB = textureLoader.getTexture(textureBNodeStats.InnerText);
													}
													if(textureBNodeStats.Name == "x")
													{
														xPositionTexB = float.Parse(textureBNodeStats.InnerText);
													}
													if(textureBNodeStats.Name == "y")
													{
														yPositionTexB = float.Parse(textureBNodeStats.InnerText);
													}
													if(textureBNodeStats.Name == "Timer")
													{
														timerTexB = float.Parse(textureBNodeStats.InnerText);
													}
												}
											}
										}
									}
									if(tutorialNodeStats.Name == "DestroyOnExit")
									{
										destroyOnExit = boolean.Parse(tutorialNodeStats.InnerText);
									}
									if(tutorialNodeStats.Name == "DestroyOnCompletion")
									{
										destroyOnCompletion = boolean.Parse(tutorialNodeStats.InnerText);
									}
									if(tutorialNodeStats.Name == "BoundingBox")
									{
										var boundingBoxStatsNode:XmlNodeList = tutorialNodeStats.ChildNodes;
										
										for each(_boundingBoxStats in boundingBoxStatsNode)
										{
											var boundingBoxStats:XmlNode = _boundingBoxStats as XmlNode;
											
											if(boundingBoxStats.Name == "x")
											{
												boundingBox.x = float.Parse(boundingBoxStats.InnerText);
											}
											if(boundingBoxStats.Name == "y")
											{
												boundingBox.y = float.Parse(boundingBoxStats.InnerText);
											}
											if(boundingBoxStats.Name == "z")
											{
												boundingBox.z = float.Parse(boundingBoxStats.InnerText);
											}
										}
									}
								}
							}
						}
						
						//instantiate the things
						if(prefabName != "" && position != Vector3(9999,9999,9999) && rotation != Vector3(9999,9999,9999) && scaling != Vector3(9999,9999,9999))
						{
							if(Resources.Load(("Prefabs/" + prefabName)) != null)
							{
								newGameObject 						= Instantiate(Resources.Load(("Prefabs/" + prefabName))) as GameObject;
								newGameObject.name 					= prefabName;
								newGameObject.transform.parent 		= level.transform;
								newGameObject.transform.position 	= position;
								newGameObject.transform.eulerAngles = rotation;
								newGameObject.transform.localScale 	= scaling;
								
								if(newGameObject.name == "AmmoBox")
								{
									var ammoBoxScript:AmmoBox = newGameObject.transform.FindChild("AmmoBox").GetComponent(AmmoBox);
									ammoBoxScript.setExtraSeeds(extraSeeds);
									ammoBoxScript.setNormalType(normalType);
									ammoBoxScript.setBumpyType(bumpyType);
									ammoBoxScript.setTimeToRespawn(timeToRespawn);
									ammoBoxScript.setOneTimePickup(oneTimePickup);
								}
								
								if(newGameObject.name == "Slug" && slugBoundAPosition != Vector3(9999,9999,9999) && slugBoundBPosition != Vector3(9999,9999,9999))
								{
									var slugBoundA:GameObject = Instantiate(Resources.Load("Prefabs/SlugBound")) as GameObject;
									slugBoundA.name = "SlugBound";
									slugBoundA.transform.position = slugBoundAPosition;
									slugBoundA.transform.parent = level.transform;
									
									var slugBoundB:GameObject = Instantiate(Resources.Load("Prefabs/SlugBound")) as GameObject;
									slugBoundB.name = "SlugBound";
									slugBoundB.transform.position = slugBoundBPosition;
									slugBoundB.transform.parent = level.transform;
									
									var slugScript = newGameObject.GetComponent(SlugScript);
									slugScript.setSlugBoundA(slugBoundA);
									slugScript.setSlugBoundB(slugBoundB);
								}
								
								if(newGameObject.name == "TutorialObject" && boundingBox != Vector3(9999,9999,9999))
								{
									var triggerScript:TutorialTriggerScript = newGameObject.GetComponent(TutorialTriggerScript);
									
									//alpha object
									if(alphaObjectPrefabName != "" && alphaObjectPosition != Vector3(9999,9999,9999) && alphaObjectRotation != Vector3(9999,9999,9999) && alphaObjectScaling != Vector3(9999,9999,9999))
									{
										var alphaObject:GameObject = Instantiate(Resources.Load("Prefabs/" + alphaObjectPrefabName)) as GameObject;
										if(alphaObject != null)
										{
											alphaObject.name = alphaObjectPrefabName;
											alphaObject.transform.parent = newGameObject.transform;
											
											alphaObject.transform.position = alphaObjectPosition;
											alphaObject.transform.eulerAngles = alphaObjectRotation;
											alphaObject.transform.localScale = alphaObjectScaling;
										}
										else Debug.LogError("Couldn't find alphaobject prefab!");
									}
									
									if(slugObjectPosition != Vector3(9999,9999,9999))
									{
										var newSlug:GameObject = Instantiate(Resources.Load("Prefabs/Slug")) as GameObject;
										newSlug.name = "Slug";
										newSlug.transform.position = slugObjectPosition;
										newSlug.transform.parent = level.transform;
										
										var slugBoundA2:GameObject = Instantiate(Resources.Load("Prefabs/SlugBound")) as GameObject;
										slugBoundA2.name = "SlugBound";
										slugBoundA2.transform.position = slugBoundAPosition;
										slugBoundA2.transform.parent = level.transform;
										
										var slugBoundB2:GameObject = Instantiate(Resources.Load("Prefabs/SlugBound")) as GameObject;
										slugBoundB2.name = "SlugBound";
										slugBoundB2.transform.position = slugBoundBPosition;
										slugBoundB2.transform.parent = level.transform;
										
										var slugScript2 = newSlug.GetComponent(SlugScript);
										slugScript2.setSlugBoundA(slugBoundA2);
										slugScript2.setSlugBoundB(slugBoundB2);
										
										triggerScript.setSlugObject(newSlug);
									}
									
									if(blockObjectPosition != Vector3(9999,9999,9999))
									{
										var newBlockObject:GameObject = Instantiate(Resources.Load("Prefabs/BarrierAnimated")) as GameObject;
										newBlockObject.name = "BarrierAnimated";
										newBlockObject.transform.position = blockObjectPosition;
										newBlockObject.transform.parent = level.transform;
										triggerScript.setBlockObject(newBlockObject);
									}
									
									triggerScript.setJumpButtonTutorial(jumpButtonTutorial);
									triggerScript.setNormalShroomButtonTutorial(normalShroomButtonTutorial);
									triggerScript.setFlashButtonTutorial(flashButtonTutorial);
									triggerScript.setBumpyShroomButtonTutorial(bumpyShroomButtonTutorial);
									triggerScript.setLightTutorial(lightTutorial);
									triggerScript.setSlugTutorial(slugTutorial);
									triggerScript.setCrystalTutorial(crystalTutorial);
									
									//set button booleans
									triggerScript.setMovementLeftEnabled(movementLeftEnabled);
									triggerScript.setMovementRightEnabled(movementRightEnabled);
									triggerScript.setJumpButtonEnabled(jumpButtonEnabled);
									triggerScript.setFlashButtonEnabled(flashButtonEnabled);
									triggerScript.setNormalShroomButtonEnabled(normalShroomButtonEnabled);
									triggerScript.setBumpyShroomButtonEnabled(bumpyShroomButtonEnabled);
									
									if(tutorialTextureA != null)
									{									
										triggerScript.setTutorialTextureA(tutorialTextureA);
										triggerScript.setXPositionTexA(xPositionTexA);
										triggerScript.setYPositionTexA(yPositionTexA);
										triggerScript.setTimerTexA(timerTexA);
									}
									
									if(tutorialTextureB != null)
									{
										triggerScript.setTutorialTextureB(tutorialTextureB);
										triggerScript.setXPositionTexB(xPositionTexB);
										triggerScript.setYPositionTexB(yPositionTexB);
										triggerScript.setTimerTexB(timerTexB);
									}
									
									//set destroy on exit
									triggerScript.setDestroyOnExit(destroyOnExit);
									triggerScript.setDestroyOnCompletion(destroyOnCompletion);
									
									//set bounding box
									newGameObject.GetComponent(BoxCollider).size = boundingBox;
								}
								//Debug.Log("created: " + prefabName + " at: " + position);
							}
							else
							{
								Debug.LogError("something went wrong creating the new object, prefab might not exist: " + prefabName);
							}
						}
						else
						{
							Debug.LogError("Something went wrong creating this: " + prefabName + " " + position + " " + rotation + " " + scaling);
						}
					}
				}
				
				if(nodes.Name == "Player")
				{
					var playerStatsNodesList:XmlNodeList = nodes.ChildNodes;
					
					for each(var _playerStats in playerStatsNodesList)
					{
						var playerStats = _playerStats as XmlNode;
						
						if(playerStats.Name == "Position")
						{
							var playerPositionNodeList:XmlNodeList = playerStats.ChildNodes;
							
							for each(var _playerPosition in playerPositionNodeList)
							{
								var playerPosition:XmlNode = _playerPosition as XmlNode;
								
								if(playerPosition.Name == "x")
								{
									player.transform.position.x = float.Parse(playerPosition.InnerText);
								}
								
								if(playerPosition.Name == "y")
								{
									player.transform.position.y = float.Parse(playerPosition.InnerText);
								}
								
								if(playerPosition.Name == "z")
								{
									player.transform.position.z = float.Parse(playerPosition.InnerText);
								}
							}
						}
						
						if(playerStats.Name == "Rotation")
						{
							var playerRotationNodeList:XmlNodeList = playerStats.ChildNodes;
							
							for each(var _playerRotation in playerRotationNodeList)
							{
								var playerRotation:XmlNode = _playerRotation as XmlNode;
								
								if(playerRotation.Name == "x")
								{
									player.transform.eulerAngles.x = float.Parse(playerRotation.InnerText);
								}
								
								if(playerRotation.Name == "y")
								{
									player.transform.eulerAngles.y = float.Parse(playerRotation.InnerText);
								}
								
								if(playerRotation.Name == "z")
								{
									player.transform.eulerAngles.z = float.Parse(playerRotation.InnerText);
								}
							}
						}
					}
				}
			}
		}
		else
		{
			Debug.LogError("This level doesn't even exist!");
			Debug.Log("Loading back to menu again as this is a non existant level");
			Application.LoadLevel("Menu");
		}
		
		Debug.Log("Finished Loading Level: " + xmlLevel);
		Destroy(this.gameObject);
	
	}
	else Debug.LogError("Am already loading level...");
}