using UnityEngine;
using System.Collections;
using System.Xml;
using System.IO;

public class XmlToScene : MonoBehaviour
{
    private string xmlPath = "";
    private string xmlLevel = "";
    private bool isLoading = false;

    public void Awake()
    {
        xmlPath = Application.dataPath + "/LevelsXML/";	//standard XML Level Path
    }

    public void setLevel(string levelString)
    {
        xmlLevel = levelString;

        if (!xmlLevel.Contains(".xml"))
        {
            xmlLevel += ".xml";
        }
    }

    public void loadLevel()
    {
        if (!isLoading)
        {
            isLoading = true;
            string filePath = xmlPath + xmlLevel;
            XmlDocument xmlDocument = new XmlDocument();

            if (File.Exists(filePath))
            {
                xmlDocument.Load(filePath);
                //getting the necessary gameobjects into variables
                GameObject camera = Camera.main.gameObject;
                Vector3 cameraPosition = new Vector3();
                Vector3 cameraRotation = new Vector3();
                GameLogic gameLogic = GameObject.Find("GameLogic").GetComponent<GameLogic>();
                GameObject level = GameObject.Find("Level");
                GameObject player = GameObject.Find("Player");
                Vector3 playerPosition = new Vector3();
                Vector3 playerRotation = new Vector3();
                TextureLoader textureLoader = GameObject.Find("TextureLoader").GetComponent<TextureLoader>();

                XmlNode rootNode = xmlDocument.DocumentElement;
                XmlNodeList masterNode = rootNode.ChildNodes;

                foreach (XmlNode nodes in masterNode)
                {
                    if (nodes.Name == "Camera")
                    {
                        XmlNodeList cameraNodesList = nodes.ChildNodes;

                        foreach (XmlNode cameraStatsNodes in cameraNodesList)
                        {
                            //get position
                            if (cameraStatsNodes.Name == "Position")
                            {
                                //get x,y,z nodes
                                XmlNodeList cameraPositionStatsList = cameraStatsNodes.ChildNodes;

                                foreach (XmlNode cameraPositionStats in cameraPositionStatsList)
                                {
                                    if (cameraPositionStats.Name == "x")
                                    {
                                        cameraPosition.x = float.Parse(cameraPositionStats.InnerText);
                                    }
                                    if (cameraPositionStats.Name == "y")
                                    {
                                        cameraPosition.y = float.Parse(cameraPositionStats.InnerText);
                                    }
                                    if (cameraPositionStats.Name == "z")
                                    {
                                        cameraPosition.z = float.Parse(cameraPositionStats.InnerText);
                                    }
                                }
                            }
                            //get rotation
                            if (cameraStatsNodes.Name == "Rotation")
                            {
                                XmlNodeList cameraRotationStatsList = cameraStatsNodes.ChildNodes;

                                foreach (XmlNode cameraRotationStats in cameraRotationStatsList)
                                {
                                    if (cameraRotationStats.Name == "x")
                                    {
                                        cameraRotation.x = float.Parse(cameraRotationStats.InnerText);
                                    }
                                    if (cameraRotationStats.Name == "y")
                                    {
                                        cameraRotation.y = float.Parse(cameraRotationStats.InnerText);
                                    }
                                    if (cameraRotationStats.Name == "z")
                                    {
                                        cameraRotation.z = float.Parse(cameraRotationStats.InnerText);
                                    }
                                }
                            }
                        }

                    }

                    if (nodes.Name == "GameLogic")
                    {
                        XmlNodeList gameLogicStatsNodes = nodes.ChildNodes;

                        foreach (XmlNode gameLogicStats in gameLogicStatsNodes)
                        {
                            if (gameLogicStats.Name == "Battery")
                            {
                                gameLogic.setBattery(float.Parse(gameLogicStats.InnerText));
                            }
                            if (gameLogicStats.Name == "MaximumBatteryCapacity")
                            {
                                gameLogic.setBatteryCapacity(int.Parse(gameLogicStats.InnerText));
                            }
                            if (gameLogicStats.Name == "DecreaseTimer")
                            {
                                gameLogic.setDecreaseTimer(float.Parse(gameLogicStats.InnerText));
                            }
                            if (gameLogicStats.Name == "NegativeBatteryFlow")
                            {
                                gameLogic.setNegativeBatteryFlow(int.Parse(gameLogicStats.InnerText));
                            }
                            if (gameLogicStats.Name == "PositiveBatteryFlow")
                            {
                                gameLogic.setPositiveBatteryFlow(int.Parse(gameLogicStats.InnerText));
                            }
                            if (gameLogicStats.Name == "CrystalsToComplete")
                            {
                                gameLogic.setCrystalsToComplete(int.Parse(gameLogicStats.InnerText));
                            }

                            if (gameLogicStats.Name == "CurrentNormalAmmo")
                            {
                                gameLogic.setCurrentNormalSeeds(int.Parse(gameLogicStats.InnerText));
                            }

                            if (gameLogicStats.Name == "MaximumNormalAmmo")
                            {
                                gameLogic.setMaximumNormalSeeds(int.Parse(gameLogicStats.InnerText));
                            }
                            if (gameLogicStats.Name == "CurrentBumpyAmmo")
                            {
                                gameLogic.setCurrentBumpySeeds(int.Parse(gameLogicStats.InnerText));
                            }
                            if (gameLogicStats.Name == "MaximumBumpyAmmo")
                            {
                                gameLogic.setMaximumBumpySeeds(int.Parse(gameLogicStats.InnerText));
                            }

                            if (gameLogicStats.Name == "InfiniteAmmo")
                            {
                                gameLogic.setInfiniteAmmo(bool.Parse(gameLogicStats.InnerText));
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

                    if (nodes.Name == "Level")
                    {

                        XmlNodeList gameObjectNodeList = nodes.ChildNodes;

                        foreach (XmlNode gameObjectNodes in gameObjectNodeList)
                        {
                            XmlNodeList gameObjectStatsNodeList = gameObjectNodes.ChildNodes;

                            GameObject newGameObject = null;
                            string prefabName = "";
                            Vector3 position = new Vector3(9999, 9999, 9999);
                            Vector3 rotation = new Vector3(9999, 9999, 9999);
                            Vector3 scaling = new Vector3(9999, 9999, 9999);

                            //slug things
                            Vector3 slugBoundAPosition = new Vector3(9999, 9999, 9999);
                            Vector3 slugBoundBPosition = new Vector3(9999, 9999, 9999);

                            //ammo box things
                            int extraSeeds = 0;
                            bool normalType = false;
                            bool bumpyType = false;
                            float timeToRespawn = 9999.9f;
                            bool oneTimePickup = false;

                            //tutorial things
                            Vector3 boundingBox = new Vector3(9999, 9999, 9999);

                            string alphaObjectPrefabName = "";
                            Vector3 alphaObjectPosition = new Vector3(9999, 9999, 9999);
                            Vector3 alphaObjectRotation = new Vector3(9999, 9999, 9999);
                            Vector3 alphaObjectScaling = new Vector3(9999, 9999, 9999);

                            bool jumpButtonTutorial = false;
                            bool normalShroomButtonTutorial = false;
                            bool flashButtonTutorial = false;
                            bool bumpyShroomButtonTutorial = false;

                            bool lightTutorial = false;
                            bool slugTutorial = false;
                            bool crystalTutorial = false;

                            Vector3 slugObjectPosition = new Vector3(9999, 9999, 9999);
                            Vector3 blockObjectPosition = new Vector3(9999, 9999, 9999);

                            bool movementLeftEnabled = true;
                            bool movementRightEnabled = true;
                            bool jumpButtonEnabled = true;
                            bool flashButtonEnabled = true;
                            bool normalShroomButtonEnabled = true;
                            bool bumpyShroomButtonEnabled = true;

                            //touch
                            Texture2D tutorialTextureA = null;
                            float xPositionTexA = 0.0f;
                            float yPositionTexA = 0.0f;
                            float timerTexA = 0.0f;

                            Texture2D tutorialTextureB = null;
                            float xPositionTexB = 0.0f;
                            float yPositionTexB = 0.0f;
                            float timerTexB = 0.0f;

                            //keyboard
                            Texture2D keyboardTutorialTextureA = null;
                            float xPositionKeyboardTexA = 0.0f;
                            float yPositionKeyboardTexA = 0.0f;
                            float timerKeyboardTexA = 0.0f;
                            Texture2D keyboardTutorialTextureB = null;
                            float xPositionKeyboardTexB = 0.0f;
                            float yPositionKeyboardTexB = 0.0f;
                            float timerKeyboardTexB = 0.0f;
                            //xbox
                            Texture2D xboxTutorialTextureA = null;
                            float xPositionXboxTexA = 0.0f;
                            float yPositionXboxTexA = 0.0f;
                            float timerXboxTexA = 0.0f;
                            Texture2D xboxTutorialTextureB = null;
                            float xPositionXboxTexB = 0.0f;
                            float yPositionXboxTexB = 0.0f;
                            float timerXboxTexB = 0.0f;

                            bool destroyOnExit = false;
                            bool destroyOnCompletion = false;

                            foreach (XmlNode gameObjectStatsNodes in gameObjectStatsNodeList)
                            {
                                if (gameObjectStatsNodes.Name == "Prefab")
                                {
                                    prefabName = gameObjectStatsNodes.InnerText;
                                }
                                if (gameObjectStatsNodes.Name == "Position")
                                {
                                    XmlNodeList gameObjectPositionNodes = gameObjectStatsNodes.ChildNodes;

                                    foreach (XmlNode positionNode in gameObjectPositionNodes)
                                    {
                                        if (positionNode.Name == "x")
                                        {
                                            //newGameObject.transform.position.x = float.Parse(positionNode.InnerText);
                                            position.x = float.Parse(positionNode.InnerText);
                                        }
                                        if (positionNode.Name == "y")
                                        {
                                            //newGameObject.transform.position.y = float.Parse(positionNode.InnerText);
                                            position.y = float.Parse(positionNode.InnerText);
                                        }
                                        if (positionNode.Name == "z")
                                        {
                                            //newGameObject.transform.position.z = float.Parse(positionNode.InnerText);
                                            position.z = float.Parse(positionNode.InnerText);
                                        }
                                    }
                                }

                                if (gameObjectStatsNodes.Name == "Rotation")
                                {
                                    XmlNodeList gameObjectRotationNodes = gameObjectStatsNodes.ChildNodes;

                                    foreach (XmlNode rotationNode in gameObjectRotationNodes)
                                    {
                                        if (rotationNode.Name == "x")
                                        {
                                            rotation.x = float.Parse(rotationNode.InnerText);
                                        }
                                        if (rotationNode.Name == "y")
                                        {
                                            rotation.y = float.Parse(rotationNode.InnerText);
                                        }
                                        if (rotationNode.Name == "z")
                                        {
                                            rotation.z = float.Parse(rotationNode.InnerText);
                                        }
                                    }
                                }

                                if (gameObjectStatsNodes.Name == "Scaling")
                                {
                                    XmlNodeList gameObjectScalingNodes = gameObjectStatsNodes.ChildNodes;

                                    foreach (XmlNode scalingNode in gameObjectScalingNodes)
                                    {
                                        if (scalingNode.Name == "x")
                                        {
                                            scaling.x = float.Parse(scalingNode.InnerText);
                                        }
                                        if (scalingNode.Name == "y")
                                        {
                                            scaling.y = float.Parse(scalingNode.InnerText);
                                        }
                                        if (scalingNode.Name == "z")
                                        {
                                            scaling.z = float.Parse(scalingNode.InnerText);
                                        }
                                    }
                                }

                                if (gameObjectStatsNodes.Name == "AmmoBox")
                                {
                                    XmlNodeList ammoBoxStatsNodes = gameObjectStatsNodes.ChildNodes;
                                    foreach (XmlNode ammoBoxStats in ammoBoxStatsNodes)
                                    {
                                        if (ammoBoxStats.Name == "ExtraSeeds") extraSeeds = int.Parse(ammoBoxStats.InnerText);
                                        if (ammoBoxStats.Name == "NormalType") normalType = bool.Parse(ammoBoxStats.InnerText);
                                        if (ammoBoxStats.Name == "BumpyType") bumpyType = bool.Parse(ammoBoxStats.InnerText);
                                        if (ammoBoxStats.Name == "TimeToRespawn") timeToRespawn = float.Parse(ammoBoxStats.InnerText);
                                        if (ammoBoxStats.Name == "OneTimePickup") oneTimePickup = bool.Parse(ammoBoxStats.InnerText);
                                    }
                                }

                                if (gameObjectStatsNodes.Name == "Slug")
                                {
                                    XmlNodeList slugNode = gameObjectStatsNodes.ChildNodes;

                                    foreach (XmlNode slugStatsNode in slugNode)
                                    {
                                        if (slugStatsNode.Name == "SlugBoundA")
                                        {
                                            XmlNodeList slugBoundANode = slugStatsNode.ChildNodes;

                                            foreach (XmlNode slugBoundAPositionNodes in slugBoundANode)
                                            {
                                                if (slugBoundAPositionNodes.Name == "x") slugBoundAPosition.x = float.Parse(slugBoundAPositionNodes.InnerText);
                                                if (slugBoundAPositionNodes.Name == "y") slugBoundAPosition.y = float.Parse(slugBoundAPositionNodes.InnerText);
                                                if (slugBoundAPositionNodes.Name == "z") slugBoundAPosition.z = float.Parse(slugBoundAPositionNodes.InnerText);
                                            }
                                        }
                                        if (slugStatsNode.Name == "SlugBoundB")
                                        {
                                            XmlNodeList slugBoundBNode = slugStatsNode.ChildNodes;

                                            foreach (XmlNode slugBoundBPositionNodes in slugBoundBNode)
                                            {
                                                if (slugBoundBPositionNodes.Name == "x") slugBoundBPosition.x = float.Parse(slugBoundBPositionNodes.InnerText);
                                                if (slugBoundBPositionNodes.Name == "y") slugBoundBPosition.y = float.Parse(slugBoundBPositionNodes.InnerText);
                                                if (slugBoundBPositionNodes.Name == "z") slugBoundBPosition.z = float.Parse(slugBoundBPositionNodes.InnerText);

                                            }
                                        }
                                    }
                                }

                                if (gameObjectStatsNodes.Name == "Tutorial")
                                {
                                    XmlNodeList tutorialNode = gameObjectStatsNodes.ChildNodes;

                                    foreach (XmlNode tutorialNodeStats in tutorialNode)
                                    {
                                        if (tutorialNodeStats.Name == "AlphaObject")
                                        {
                                            XmlNodeList alphaObjectStatsNodes = tutorialNodeStats.ChildNodes;

                                            foreach (XmlNode alphaObjectStats in alphaObjectStatsNodes)
                                            {
                                                if (alphaObjectStats.Name == "Prefab")
                                                {
                                                    alphaObjectPrefabName = alphaObjectStats.InnerText;
                                                }
                                                if (alphaObjectStats.Name == "Position")
                                                {
                                                    foreach (XmlNode alphaObjectPositionNodes in alphaObjectStats.ChildNodes)
                                                    {
                                                        if (alphaObjectPositionNodes.Name == "x") alphaObjectPosition.x = float.Parse(alphaObjectPositionNodes.InnerText);
                                                        if (alphaObjectPositionNodes.Name == "y") alphaObjectPosition.y = float.Parse(alphaObjectPositionNodes.InnerText);
                                                        if (alphaObjectPositionNodes.Name == "z") alphaObjectPosition.z = float.Parse(alphaObjectPositionNodes.InnerText);
                                                    }
                                                }
                                                if (alphaObjectStats.Name == "Rotation")
                                                {
                                                    foreach (XmlNode alphaObjectRotationNodes in alphaObjectStats.ChildNodes)
                                                    {
                                                        if (alphaObjectRotationNodes.Name == "x") alphaObjectRotation.x = float.Parse(alphaObjectRotationNodes.InnerText);
                                                        if (alphaObjectRotationNodes.Name == "y") alphaObjectRotation.y = float.Parse(alphaObjectRotationNodes.InnerText);
                                                        if (alphaObjectRotationNodes.Name == "z") alphaObjectRotation.z = float.Parse(alphaObjectRotationNodes.InnerText);
                                                    }
                                                }
                                                if (alphaObjectStats.Name == "Scaling")
                                                {
                                                    foreach (XmlNode alphaObjectScalingNodes in alphaObjectStats.ChildNodes)
                                                    {
                                                        if (alphaObjectScalingNodes.Name == "x") alphaObjectScaling.x = float.Parse(alphaObjectScalingNodes.InnerText);
                                                        if (alphaObjectScalingNodes.Name == "y") alphaObjectScaling.y = float.Parse(alphaObjectScalingNodes.InnerText);
                                                        if (alphaObjectScalingNodes.Name == "z") alphaObjectScaling.z = float.Parse(alphaObjectScalingNodes.InnerText);
                                                    }
                                                }
                                            }
                                        }

                                        if (tutorialNodeStats.Name == "SlugObject")
                                        {
                                            XmlNodeList slugNode2 = tutorialNodeStats.ChildNodes;

                                            foreach (XmlNode slugStatsNode2 in slugNode2)
                                            {
                                                if (slugStatsNode2.Name == "Position")
                                                {
                                                    foreach (XmlNode slugPositionNodes2 in slugStatsNode2.ChildNodes)
                                                    {
                                                        if (slugPositionNodes2.Name == "x") slugObjectPosition.x = float.Parse(slugPositionNodes2.InnerText);
                                                        if (slugPositionNodes2.Name == "y") slugObjectPosition.y = float.Parse(slugPositionNodes2.InnerText);
                                                        if (slugPositionNodes2.Name == "z") slugObjectPosition.z = float.Parse(slugPositionNodes2.InnerText);
                                                    }
                                                }

                                                if (slugStatsNode2.Name == "SlugBoundA")
                                                {
                                                    XmlNodeList slugBoundANode2 = slugStatsNode2.ChildNodes;

                                                    foreach (XmlNode slugBoundAPositionNodes2 in slugBoundANode2)
                                                    {
                                                        if (slugBoundAPositionNodes2.Name == "x") slugBoundAPosition.x = float.Parse(slugBoundAPositionNodes2.InnerText);
                                                        if (slugBoundAPositionNodes2.Name == "y") slugBoundAPosition.y = float.Parse(slugBoundAPositionNodes2.InnerText);
                                                        if (slugBoundAPositionNodes2.Name == "z") slugBoundAPosition.z = float.Parse(slugBoundAPositionNodes2.InnerText);
                                                    }
                                                }
                                                if (slugStatsNode2.Name == "SlugBoundB")
                                                {
                                                    XmlNodeList slugBoundBNode2 = slugStatsNode2.ChildNodes;

                                                    foreach (XmlNode slugBoundBPositionNodes2 in slugBoundBNode2)
                                                    {
                                                        if (slugBoundBPositionNodes2.Name == "x") slugBoundBPosition.x = float.Parse(slugBoundBPositionNodes2.InnerText);
                                                        if (slugBoundBPositionNodes2.Name == "y") slugBoundBPosition.y = float.Parse(slugBoundBPositionNodes2.InnerText);
                                                        if (slugBoundBPositionNodes2.Name == "z") slugBoundBPosition.z = float.Parse(slugBoundBPositionNodes2.InnerText);

                                                    }
                                                }
                                            }
                                        }

                                        if (tutorialNodeStats.Name == "JumpButtonTutorial")
                                        {
                                            jumpButtonTutorial = bool.Parse(tutorialNodeStats.InnerText);
                                        }
                                        if (tutorialNodeStats.Name == "NormalShroomButtonTutorial")
                                        {
                                            normalShroomButtonTutorial = bool.Parse(tutorialNodeStats.InnerText);
                                        }
                                        if (tutorialNodeStats.Name == "FlashButtonTutorial")
                                        {
                                            flashButtonTutorial = bool.Parse(tutorialNodeStats.InnerText);
                                        }
                                        if (tutorialNodeStats.Name == "BumpyShroomButtonTutorial")
                                        {
                                            bumpyShroomButtonTutorial = bool.Parse(tutorialNodeStats.InnerText);
                                        }


                                        if (tutorialNodeStats.Name == "LightTutorial")
                                        {
                                            lightTutorial = bool.Parse(tutorialNodeStats.InnerText);
                                        }
                                        if (tutorialNodeStats.Name == "SlugTutorial")
                                        {
                                            slugTutorial = bool.Parse(tutorialNodeStats.InnerText);
                                        }
                                        if (tutorialNodeStats.Name == "CrystalTutorial")
                                        {
                                            crystalTutorial = bool.Parse(tutorialNodeStats.InnerText);
                                        }

                                        if (tutorialNodeStats.Name == "BlockObject")
                                        {
                                            foreach (XmlNode blockObjectStats in tutorialNodeStats.ChildNodes)
                                            {
                                                foreach (XmlNode blockObjectPositionNodes in blockObjectStats.ChildNodes)
                                                {
                                                    if (blockObjectPositionNodes.Name == "x")
                                                    {
                                                        blockObjectPosition.x = float.Parse(blockObjectPositionNodes.InnerText);
                                                    }
                                                    if (blockObjectPositionNodes.Name == "y")
                                                    {
                                                        blockObjectPosition.y = float.Parse(blockObjectPositionNodes.InnerText);
                                                    }
                                                    if (blockObjectPositionNodes.Name == "z")
                                                    {
                                                        blockObjectPosition.z = float.Parse(blockObjectPositionNodes.InnerText);
                                                    }
                                                }
                                            }
                                        }

                                        if (tutorialNodeStats.Name == "ButtonsEnabled")
                                        {
                                            XmlNodeList buttonNodes = tutorialNodeStats.ChildNodes;

                                            foreach (XmlNode buttonNodeStats in buttonNodes)
                                            {
                                                if (buttonNodeStats.Name == "MovementLeft")
                                                {
                                                    movementLeftEnabled = bool.Parse(buttonNodeStats.InnerText);
                                                }
                                                if (buttonNodeStats.Name == "MovementRight")
                                                {
                                                    movementRightEnabled = bool.Parse(buttonNodeStats.InnerText);
                                                }
                                                if (buttonNodeStats.Name == "JumpButton")
                                                {
                                                    jumpButtonEnabled = bool.Parse(buttonNodeStats.InnerText);
                                                }
                                                if (buttonNodeStats.Name == "FlashButton")
                                                {
                                                    flashButtonEnabled = bool.Parse(buttonNodeStats.InnerText);
                                                }
                                                if (buttonNodeStats.Name == "NormalShroomButton")
                                                {
                                                    normalShroomButtonEnabled = bool.Parse(buttonNodeStats.InnerText);
                                                }
                                                if (buttonNodeStats.Name == "BumpyShroomButton")
                                                {
                                                    bumpyShroomButtonEnabled = bool.Parse(buttonNodeStats.InnerText);
                                                }
                                            }
                                        }
                                        if (tutorialNodeStats.Name == "Textures")
                                        {
                                            XmlNodeList textureNodes = tutorialNodeStats.ChildNodes;

                                            foreach (XmlNode textureNodeStats in textureNodes)
                                            {
                                                //
                                                //touch input
                                                //
                                                if (textureNodeStats.Name == "TextureA")
                                                {
                                                    XmlNodeList textureANode = textureNodeStats.ChildNodes;

                                                    foreach (XmlNode textureANodeStats in textureANode)
                                                    {
                                                        if (textureANodeStats.Name == "Texturename")
                                                        {
                                                            tutorialTextureA = textureLoader.getTexture(textureANodeStats.InnerText);
                                                        }
                                                        if (textureANodeStats.Name == "x")
                                                        {
                                                            xPositionTexA = float.Parse(textureANodeStats.InnerText);
                                                        }
                                                        if (textureANodeStats.Name == "y")
                                                        {
                                                            yPositionTexA = float.Parse(textureANodeStats.InnerText);
                                                        }
                                                        if (textureANodeStats.Name == "Timer")
                                                        {
                                                            timerTexA = float.Parse(textureANodeStats.InnerText);
                                                        }
                                                    }
                                                }
                                                if (textureNodeStats.Name == "TextureB")
                                                {
                                                    XmlNodeList textureBNode = textureNodeStats.ChildNodes;

                                                    foreach (XmlNode textureBNodeStats in textureBNode)
                                                    {
                                                        if (textureBNodeStats.Name == "Texturename")
                                                        {
                                                            tutorialTextureB = textureLoader.getTexture(textureBNodeStats.InnerText);
                                                        }
                                                        if (textureBNodeStats.Name == "x")
                                                        {
                                                            xPositionTexB = float.Parse(textureBNodeStats.InnerText);
                                                        }
                                                        if (textureBNodeStats.Name == "y")
                                                        {
                                                            yPositionTexB = float.Parse(textureBNodeStats.InnerText);
                                                        }
                                                        if (textureBNodeStats.Name == "Timer")
                                                        {
                                                            timerTexB = float.Parse(textureBNodeStats.InnerText);
                                                        }
                                                    }
                                                }
                                                //

                                                //
                                                //keyboard input
                                                //
                                                if (textureNodeStats.Name == "KeyboardTextureA")
                                                {
                                                    XmlNodeList keyboardTextureANode = textureNodeStats.ChildNodes;

                                                    foreach (XmlNode textureANodeStats in keyboardTextureANode)
                                                    {
                                                        if (textureANodeStats.Name == "Texturename")
                                                        {
                                                            keyboardTutorialTextureA = textureLoader.getTexture(textureANodeStats.InnerText);
                                                        }
                                                        if (textureANodeStats.Name == "x")
                                                        {
                                                            xPositionKeyboardTexA = float.Parse(textureANodeStats.InnerText);
                                                        }
                                                        if (textureANodeStats.Name == "y")
                                                        {
                                                            yPositionKeyboardTexA = float.Parse(textureANodeStats.InnerText);
                                                        }
                                                        if (textureANodeStats.Name == "Timer")
                                                        {
                                                            timerKeyboardTexA = float.Parse(textureANodeStats.InnerText);
                                                        }
                                                    }
                                                }
                                                if (textureNodeStats.Name == "KeyboardTextureB")
                                                {
                                                    XmlNodeList keyboardTextureBNode = textureNodeStats.ChildNodes;

                                                    foreach (XmlNode textureBNodeStats in keyboardTextureBNode)
                                                    {
                                                        if (textureBNodeStats.Name == "Texturename")
                                                        {
                                                            keyboardTutorialTextureB = textureLoader.getTexture(textureBNodeStats.InnerText);
                                                        }
                                                        if (textureBNodeStats.Name == "x")
                                                        {
                                                            xPositionKeyboardTexB = float.Parse(textureBNodeStats.InnerText);
                                                        }
                                                        if (textureBNodeStats.Name == "y")
                                                        {
                                                            yPositionKeyboardTexB = float.Parse(textureBNodeStats.InnerText);
                                                        }
                                                        if (textureBNodeStats.Name == "Timer")
                                                        {
                                                            timerKeyboardTexB = float.Parse(textureBNodeStats.InnerText);
                                                        }
                                                    }
                                                }
                                                //

                                                //
                                                //xbox input
                                                //
                                                if (textureNodeStats.Name == "XboxTextureA")
                                                {
                                                    XmlNodeList xboxTextureANode = textureNodeStats.ChildNodes;

                                                    foreach (XmlNode textureANodeStats in xboxTextureANode)
                                                    {
                                                        if (textureANodeStats.Name == "Texturename")
                                                        {
                                                            xboxTutorialTextureA = textureLoader.getTexture(textureANodeStats.InnerText);
                                                        }
                                                        if (textureANodeStats.Name == "x")
                                                        {
                                                            xPositionXboxTexA = float.Parse(textureANodeStats.InnerText);
                                                        }
                                                        if (textureANodeStats.Name == "y")
                                                        {
                                                            yPositionXboxTexA = float.Parse(textureANodeStats.InnerText);
                                                        }
                                                        if (textureANodeStats.Name == "Timer")
                                                        {
                                                            timerXboxTexA = float.Parse(textureANodeStats.InnerText);
                                                        }
                                                    }
                                                }
                                                if (textureNodeStats.Name == "XboxTextureB")
                                                {
                                                    XmlNodeList xboxTextureBNode = textureNodeStats.ChildNodes;

                                                    foreach (XmlNode textureBNodeStats in xboxTextureBNode)
                                                    {
                                                        if (textureBNodeStats.Name == "Texturename")
                                                        {
                                                            xboxTutorialTextureB = textureLoader.getTexture(textureBNodeStats.InnerText);
                                                        }
                                                        if (textureBNodeStats.Name == "x")
                                                        {
                                                            xPositionXboxTexB = float.Parse(textureBNodeStats.InnerText);
                                                        }
                                                        if (textureBNodeStats.Name == "y")
                                                        {
                                                            yPositionXboxTexB = float.Parse(textureBNodeStats.InnerText);
                                                        }
                                                        if (textureBNodeStats.Name == "Timer")
                                                        {
                                                            timerXboxTexB = float.Parse(textureBNodeStats.InnerText);
                                                        }
                                                    }
                                                }
                                                //
                                            }
                                        }
                                        if (tutorialNodeStats.Name == "DestroyOnExit")
                                        {
                                            destroyOnExit = bool.Parse(tutorialNodeStats.InnerText);
                                        }
                                        if (tutorialNodeStats.Name == "DestroyOnCompletion")
                                        {
                                            destroyOnCompletion = bool.Parse(tutorialNodeStats.InnerText);
                                        }
                                        if (tutorialNodeStats.Name == "BoundingBox")
                                        {
                                            XmlNodeList boundingBoxStatsNode = tutorialNodeStats.ChildNodes;

                                            foreach (XmlNode boundingBoxStats in boundingBoxStatsNode)
                                            {
                                                if (boundingBoxStats.Name == "x")
                                                {
                                                    boundingBox.x = float.Parse(boundingBoxStats.InnerText);
                                                }
                                                if (boundingBoxStats.Name == "y")
                                                {
                                                    boundingBox.y = float.Parse(boundingBoxStats.InnerText);
                                                }
                                                if (boundingBoxStats.Name == "z")
                                                {
                                                    boundingBox.z = float.Parse(boundingBoxStats.InnerText);
                                                }
                                            }
                                        }
                                    }
                                }
                            }

                            //instantiate the things
                            if (prefabName != "" && position != new Vector3(9999, 9999, 9999) && rotation != new Vector3(9999, 9999, 9999) && scaling != new Vector3(9999, 9999, 9999))
                            {
                                if (Resources.Load(("Prefabs/" + prefabName)) != null)
                                {
                                    newGameObject = Instantiate(Resources.Load(("Prefabs/" + prefabName))) as GameObject;
                                    newGameObject.name = prefabName;
                                    newGameObject.transform.parent = level.transform;
                                    newGameObject.transform.position = position;
                                    newGameObject.transform.eulerAngles = rotation;
                                    newGameObject.transform.localScale = scaling;

                                    if (newGameObject.name == "AmmoBox")
                                    {
                                        AmmoBox ammoBoxScript = newGameObject.transform.FindChild("AmmoBox").GetComponent<AmmoBox>();
                                        ammoBoxScript.setExtraSeeds(extraSeeds);
                                        ammoBoxScript.setNormalType(normalType);
                                        ammoBoxScript.setBumpyType(bumpyType);
                                        ammoBoxScript.setTimeToRespawn(timeToRespawn);
                                        ammoBoxScript.setOneTimePickup(oneTimePickup);
                                    }

                                    if (newGameObject.name == "Slug" && slugBoundAPosition != new Vector3(9999, 9999, 9999) && slugBoundBPosition != new Vector3(9999, 9999, 9999))
                                    {
                                        GameObject slugBoundA = Instantiate(Resources.Load("Prefabs/SlugBound")) as GameObject;
                                        slugBoundA.name = "SlugBound";
                                        slugBoundA.transform.position = slugBoundAPosition;
                                        slugBoundA.transform.parent = level.transform;

                                        GameObject slugBoundB = Instantiate(Resources.Load("Prefabs/SlugBound")) as GameObject;
                                        slugBoundB.name = "SlugBound";
                                        slugBoundB.transform.position = slugBoundBPosition;
                                        slugBoundB.transform.parent = level.transform;

                                        SlugScript slugScript = newGameObject.GetComponent<SlugScript>();
                                        slugScript.setSlugBoundA(slugBoundA);
                                        slugScript.setSlugBoundB(slugBoundB);
                                    }

                                    if (newGameObject.name == "TutorialObject" && boundingBox != new Vector3(9999, 9999, 9999))
                                    {
                                        TutorialTriggerScript triggerScript = newGameObject.GetComponent<TutorialTriggerScript>();

                                        //alpha object
                                        if (alphaObjectPrefabName != "" && alphaObjectPosition != new Vector3(9999, 9999, 9999) && alphaObjectRotation != new Vector3(9999, 9999, 9999) && alphaObjectScaling != new Vector3(9999, 9999, 9999))
                                        {
                                            GameObject alphaObject = Instantiate(Resources.Load("Prefabs/" + alphaObjectPrefabName)) as GameObject;
                                            if (alphaObject != null)
                                            {
                                                alphaObject.name = alphaObjectPrefabName;
                                                alphaObject.transform.parent = newGameObject.transform;

                                                alphaObject.transform.position = alphaObjectPosition;
                                                alphaObject.transform.eulerAngles = alphaObjectRotation;
                                                alphaObject.transform.localScale = alphaObjectScaling;
                                            }
                                            else Debug.LogError("Couldn't find alphaobject prefab!");
                                        }

                                        if (slugObjectPosition != new Vector3(9999, 9999, 9999))
                                        {
                                            GameObject newSlug = Instantiate(Resources.Load("Prefabs/Slug")) as GameObject;
                                            newSlug.name = "Slug";
                                            newSlug.transform.position = slugObjectPosition;
                                            newSlug.transform.parent = level.transform;

                                            GameObject slugBoundA2 = Instantiate(Resources.Load("Prefabs/SlugBound")) as GameObject;
                                            slugBoundA2.name = "SlugBound";
                                            slugBoundA2.transform.position = slugBoundAPosition;
                                            slugBoundA2.transform.parent = level.transform;

                                            GameObject slugBoundB2 = Instantiate(Resources.Load("Prefabs/SlugBound")) as GameObject;
                                            slugBoundB2.name = "SlugBound";
                                            slugBoundB2.transform.position = slugBoundBPosition;
                                            slugBoundB2.transform.parent = level.transform;

                                            SlugScript slugScript2 = newSlug.GetComponent<SlugScript>();
                                            slugScript2.setSlugBoundA(slugBoundA2);
                                            slugScript2.setSlugBoundB(slugBoundB2);

                                            triggerScript.setSlugObject(newSlug);
                                        }

                                        if (blockObjectPosition != new Vector3(9999, 9999, 9999))
                                        {
                                            GameObject newBlockObject = Instantiate(Resources.Load("Prefabs/BarrierAnimated")) as GameObject;
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

                                        if (tutorialTextureA != null)
                                        {
                                            triggerScript.setTutorialTextureA(tutorialTextureA);        //set texture
                                            triggerScript.setXPositionTexA(xPositionTexA);              //set position
                                            triggerScript.setYPositionTexA(yPositionTexA);
                                            triggerScript.setTimerTexA(timerTexA);                      //set timer
                                        }

                                        if (tutorialTextureB != null)
                                        {
                                            triggerScript.setTutorialTextureB(tutorialTextureB);
                                            triggerScript.setXPositionTexB(xPositionTexB);
                                            triggerScript.setYPositionTexB(yPositionTexB);
                                            triggerScript.setTimerTexB(timerTexB);
                                        }
                                        if(keyboardTutorialTextureA != null)
                                        {
                                            triggerScript.setKeyboardTutorialTextureA(keyboardTutorialTextureA);
                                            triggerScript.setXPositionKeyboardTexA(xPositionKeyboardTexA);
                                            triggerScript.setYPositionKeyboardTexA(yPositionKeyboardTexA);
                                            triggerScript.setTimerKeyboardTexA(timerKeyboardTexA);
                                        }
                                        if(keyboardTutorialTextureB != null)
                                        {
                                            triggerScript.setKeyboardTutorialTextureB(keyboardTutorialTextureB);
                                            triggerScript.setXPositionKeyboardTexB(xPositionKeyboardTexB);
                                            triggerScript.setYPositionKeyboardTexB(yPositionKeyboardTexB);
                                            triggerScript.setTimerKeyboardTexB(timerKeyboardTexB);
                                        }
                                        if(xboxTutorialTextureA != null)
                                        {
                                            triggerScript.setXboxTutorialTextureA(xboxTutorialTextureA);
                                            triggerScript.setXPositionXboxTexA(xPositionXboxTexA);
                                            triggerScript.setYPositionXboxTexA(yPositionXboxTexA);
                                            triggerScript.setTimerXboxTexA(timerXboxTexA);
                                        }
                                        if(xboxTutorialTextureB != null)
                                        {
                                            triggerScript.setXboxTutorialTextureB(xboxTutorialTextureB);
                                            triggerScript.setXPositionXboxTexB(xPositionXboxTexB);
                                            triggerScript.setYPositionXboxTexB(yPositionXboxTexB);
                                            triggerScript.setTimerXboxTexB(timerXboxTexB);
                                        }

                                        //set destroy on exit
                                        triggerScript.setDestroyOnExit(destroyOnExit);
                                        triggerScript.setDestroyOnCompletion(destroyOnCompletion);

                                        //set bounding box
                                        newGameObject.GetComponent<BoxCollider>().size = boundingBox;
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

                    if (nodes.Name == "Player")
                    {
                        XmlNodeList playerStatsNodesList = nodes.ChildNodes;

                        foreach (XmlNode playerStats in playerStatsNodesList)
                        {
                            if (playerStats.Name == "Position")
                            {
                                XmlNodeList playerPositionNodeList = playerStats.ChildNodes;

                                foreach (XmlNode playerPositionNode in playerPositionNodeList)
                                {
                                    if (playerPositionNode.Name == "x")
                                    {
                                        playerPosition.x = float.Parse(playerPositionNode.InnerText);
                                    }

                                    if (playerPositionNode.Name == "y")
                                    {
                                        playerPosition.y = float.Parse(playerPositionNode.InnerText);
                                    }

                                    if (playerPositionNode.Name == "z")
                                    {
                                        playerPosition.z = float.Parse(playerPositionNode.InnerText);
                                    }
                                }
                            }

                            if (playerStats.Name == "Rotation")
                            {
                                XmlNodeList playerRotationNodeList = playerStats.ChildNodes;

                                foreach (XmlNode playerRotationNode in playerRotationNodeList)
                                {
                                    if (playerRotationNode.Name == "x")
                                    {
                                        playerRotation.x = float.Parse(playerRotationNode.InnerText);
                                    }

                                    if (playerRotationNode.Name == "y")
                                    {
                                        playerRotation.y = float.Parse(playerRotationNode.InnerText);
                                    }

                                    if (playerRotationNode.Name == "z")
                                    {
                                        playerRotation.z = float.Parse(playerRotationNode.InnerText);
                                    }
                                }
                            }
                        }
                    }
                }

                camera.transform.position = cameraPosition;
                camera.transform.eulerAngles = cameraRotation;

                player.transform.position = playerPosition;
                player.transform.eulerAngles = playerRotation;
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
}