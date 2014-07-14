using UnityEngine;
using System.Collections;
using System.Xml;
using System.IO;

public class SceneToXML : MonoBehaviour
{
    public string levelName = "";		//name of the xml it will be saved as
    public int levelID = 0;		//level ID (level0, level1, level2 etc)
    public bool overwrite = false;	//override the current .xml if it already exists

    public bool easy = false;		//bool  for level difficulty setting
    public bool medium = false;
    public bool hard = false;

    private string xmlPath = "";		//initialized in the awake

    private string[] prefabList;        //list of the prefabs available to check if there are any invalid prefabs saved to xml

    public void Awake()
    {
        //the creative tried to save the XML while still being in the LevelEditor scene, save it as a new scene first before saving it to XML as stated in the manual
        if (Application.loadedLevelName == "LevelEditor")
        {
            Debug.LogError("Please save this scene first as a new scene before saving the level");
        }
        else
        {
            xmlPath = Application.dataPath + "/LevelsXML/"; //the path to the xml

            if (levelName != "")    //extra failsaves if it isn't filled in correctly
            {
                if (!levelName.Contains(".xml"))
                {
                    levelName = levelName + ".xml";
                }

                if (saveLevel() == -1) //try to save the level, if it returns a -1 there has been an error and it is not saved, which error is stated in the saveLevel() itself before returning
                {
                    Debug.LogError("Xml not saved due to an error");
                }
            }
            else
            {
                Debug.LogError("You haven't filled in a Level Name, see SaveLevel"); //failsave to fill in the level name
            }
        }
    }

    private int saveLevel()
    {
        XmlDocument xmlDocument = new XmlDocument();
        string filePath = xmlPath + levelName;
        XmlElement masterNode = null;

        //check if the file already exists
        if (File.Exists(filePath))
        {
            //file already exists!
            //check if you may override or not (standard false)
            if (overwrite)
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
        XmlElement levelIDNode = xmlDocument.CreateElement("LevelID");
        masterNode.AppendChild(levelIDNode);
        levelIDNode.InnerText = levelID.ToString();
        Debug.Log("Level ID: " + levelID);

        //
        //Difficulty
        //
        XmlElement difficultyNode = xmlDocument.CreateElement("Difficulty");
        masterNode.AppendChild(difficultyNode);

        //failsaves to chose the right difficulty!, not 2 or 3.. just 1!
        if (easy != false || medium != false || hard != false)
        {
            if (easy == true && medium == false && hard == false)
            {
                difficultyNode.InnerText = "Easy";
                Debug.Log("Difficulty = easy");
            }
            else if (medium == true && easy == false && hard == false)
            {
                difficultyNode.InnerText = "Medium";
                Debug.Log("Difficulty = Medium");
            }
            else if (hard == true && easy == false && medium == false)
            {
                difficultyNode.InnerText = "Hard";
                Debug.Log("Difficulty = Hard");
            }
            else
            {
                Debug.LogError("Something went wrong with selecting the difficulty setting, thicked 2 boxes perhaps?"); //state the error what could've went wrong
                return -1;
            }
        }
        else
        {
            Debug.LogError("Please state the difficulty setting correctly");    //hasn't filled in the details so fill it in!
            return -1;
        }

        //save all the things to xml

        //
        //camera
        //
        GameObject mainCamera = Camera.main.gameObject;
        XmlElement cameraNode = xmlDocument.CreateElement("Camera");
        masterNode.AppendChild(cameraNode);

        //save position
        XmlElement cameraPositionNode = xmlDocument.CreateElement("Position");
        cameraNode.AppendChild(cameraPositionNode);

        XmlElement cameraXNode = xmlDocument.CreateElement("x");
        XmlElement cameraYNode = xmlDocument.CreateElement("y");
        XmlElement cameraZNode = xmlDocument.CreateElement("z");

        cameraPositionNode.AppendChild(cameraXNode);
        cameraPositionNode.AppendChild(cameraYNode);
        cameraPositionNode.AppendChild(cameraZNode);

        cameraXNode.InnerText = mainCamera.transform.position.x.ToString();
        cameraYNode.InnerText = mainCamera.transform.position.y.ToString();
        cameraZNode.InnerText = mainCamera.transform.position.z.ToString();

        //save the rotation
        XmlElement cameraRotationNode = xmlDocument.CreateElement("Rotation");
        cameraNode.AppendChild(cameraRotationNode);

        XmlElement cameraXRotationNode = xmlDocument.CreateElement("x");
        XmlElement cameraYRotationNode = xmlDocument.CreateElement("y");
        XmlElement cameraZRotationNode = xmlDocument.CreateElement("z");

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
        XmlElement gameLogic = xmlDocument.CreateElement("GameLogic");
        masterNode.AppendChild(gameLogic);

        GameLogic gameLogicObject = (GameLogic)GameObject.Find("GameLogic").GetComponent<GameLogic>();

        //save the variables into the GameLogic xml
        XmlElement batteryNode = xmlDocument.CreateElement("Battery");
        XmlElement maximumBatteryCapacityNode = xmlDocument.CreateElement("MaximumBatteryCapacity");
        XmlElement decreaseTimerNode = xmlDocument.CreateElement("DecreaseTimer");
        XmlElement negativeBatteryFlowNode = xmlDocument.CreateElement("NegativeBatteryFlow");
        XmlElement positiveBatteryFlowNode = xmlDocument.CreateElement("PositiveBatteryFlow");
        XmlElement crystalsToCompleteNode = xmlDocument.CreateElement("CrystalsToComplete");
        XmlElement speedNode = xmlDocument.CreateElement("Speed");

        XmlElement currentNormalAmmoNode = xmlDocument.CreateElement("CurrentNormalAmmo");
        XmlElement maxNormalAmmoNode = xmlDocument.CreateElement("MaximumNormalAmmo");
        XmlElement currentBumpyAmmoNode = xmlDocument.CreateElement("CurrentBumpyAmmo");
        XmlElement maxBumpyAmmoNode = xmlDocument.CreateElement("MaximumBumpyAmmo");

        XmlElement infiniteAmmoNode = xmlDocument.CreateElement("InfiniteAmmo");
        XmlElement jumpDrainNode = xmlDocument.CreateElement("JumpDrain");
        XmlElement flashDrainNode = xmlDocument.CreateElement("FlashDrain");

        XmlElement platinumTimeNode = xmlDocument.CreateElement("PlatinumTime");
        XmlElement goldTimeNode = xmlDocument.CreateElement("GoldTime");
        XmlElement silverTimeNode = xmlDocument.CreateElement("SilverTime");
        XmlElement bronzeTimeNode = xmlDocument.CreateElement("BronzeTime");


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
        batteryNode.InnerText = gameLogicObject.getBattery().ToString();
        maximumBatteryCapacityNode.InnerText = gameLogicObject.getBatteryCapacity().ToString();
        decreaseTimerNode.InnerText = gameLogicObject.getDecreaseTimer().ToString();
        negativeBatteryFlowNode.InnerText = gameLogicObject.getNegativeBatteryFlow().ToString();
        positiveBatteryFlowNode.InnerText = gameLogicObject.getPositiveBatteryFlow().ToString();
        crystalsToCompleteNode.InnerText = gameLogicObject.getCrystalsToComplete().ToString();

        currentNormalAmmoNode.InnerText = gameLogicObject.getCurrentNormalSeeds().ToString();
        maxNormalAmmoNode.InnerText = gameLogicObject.getMaximumNormalSeeds().ToString();
        currentBumpyAmmoNode.InnerText = gameLogicObject.getCurrentBumpySeeds().ToString();
        maxBumpyAmmoNode.InnerText = gameLogicObject.getMaximumBumpySeeds().ToString();

        infiniteAmmoNode.InnerText = gameLogicObject.getInfiniteAmmo().ToString();
        jumpDrainNode.InnerText = gameLogicObject.getJumpDrain().ToString();
        flashDrainNode.InnerText = gameLogicObject.getFlashDrain().ToString();

        platinumTimeNode.InnerText = gameLogicObject.getPlatinumTime().ToString();
        goldTimeNode.InnerText = gameLogicObject.getGoldTime().ToString();
        silverTimeNode.InnerText = gameLogicObject.getSilverTime().ToString();
        bronzeTimeNode.InnerText = gameLogicObject.getBronzeTime().ToString();
        //

        //
        //Level xml
        //
        XmlElement levelNode = xmlDocument.CreateElement("Level");
        masterNode.AppendChild(levelNode);

        GameObject levelObject = GameObject.Find("Level");
        foreach (Transform obj in levelObject.transform)
        {
            if (!checkValidPrefab(obj.name)) return -1; //failsave to check if the prefab is available in the prefab folder else it'll miss in the game which is a potential error!

            XmlElement objectNode = xmlDocument.CreateElement("GameObject");

            if (obj.gameObject.name != "SlugBound")
            {
                levelNode.AppendChild(objectNode);
                //save prefab name
                XmlElement prefabNode = xmlDocument.CreateElement("Prefab");
                objectNode.AppendChild(prefabNode);
                prefabNode.InnerText = obj.gameObject.name;

                //save position
                XmlElement positionNode = xmlDocument.CreateElement("Position");
                objectNode.AppendChild(positionNode);

                XmlElement xNode = xmlDocument.CreateElement("x");
                XmlElement yNode = xmlDocument.CreateElement("y");
                XmlElement zNode = xmlDocument.CreateElement("z");

                positionNode.AppendChild(xNode);
                positionNode.AppendChild(yNode);
                positionNode.AppendChild(zNode);

                xNode.InnerText = obj.gameObject.transform.position.x.ToString();
                yNode.InnerText = obj.gameObject.transform.position.y.ToString();
                zNode.InnerText = obj.gameObject.transform.position.z.ToString();

                //save the rotation
                XmlElement rotationNode = xmlDocument.CreateElement("Rotation");
                objectNode.AppendChild(rotationNode);

                XmlElement xRotationNode = xmlDocument.CreateElement("x");
                XmlElement yRotationNode = xmlDocument.CreateElement("y");
                XmlElement zRotationNode = xmlDocument.CreateElement("z");

                rotationNode.AppendChild(xRotationNode);
                rotationNode.AppendChild(yRotationNode);
                rotationNode.AppendChild(zRotationNode);

                xRotationNode.InnerText = obj.gameObject.transform.eulerAngles.x.ToString();
                yRotationNode.InnerText = obj.gameObject.transform.eulerAngles.y.ToString();
                zRotationNode.InnerText = obj.gameObject.transform.eulerAngles.z.ToString();

                //save the scale
                XmlElement scaleNode = xmlDocument.CreateElement("Scaling");
                objectNode.AppendChild(scaleNode);

                XmlElement xScaleNode = xmlDocument.CreateElement("x");
                XmlElement yScaleNode = xmlDocument.CreateElement("y");
                XmlElement zScaleNode = xmlDocument.CreateElement("z");

                scaleNode.AppendChild(xScaleNode);
                scaleNode.AppendChild(yScaleNode);
                scaleNode.AppendChild(zScaleNode);

                xScaleNode.InnerText = obj.gameObject.transform.localScale.x.ToString();
                yScaleNode.InnerText = obj.gameObject.transform.localScale.y.ToString();
                zScaleNode.InnerText = obj.gameObject.transform.localScale.z.ToString();
            }
            //special nodes
            if (obj.gameObject.name == "AmmoBox")
            {
                XmlElement ammoBoxNode = xmlDocument.CreateElement("AmmoBox");
                objectNode.AppendChild(ammoBoxNode);

                AmmoBox ammoBox = (AmmoBox)obj.FindChild("AmmoBox").GetComponent(typeof(AmmoBox));

                XmlElement extraSeedsNode = xmlDocument.CreateElement("ExtraSeeds");
                XmlElement normalTypeNode = xmlDocument.CreateElement("NormalType");
                XmlElement bumpyTypeNode = xmlDocument.CreateElement("BumpyType");
                XmlElement timeToRespawnNode = xmlDocument.CreateElement("TimeToRespawn");
                XmlElement oneTimePickupNode = xmlDocument.CreateElement("OneTimePickup");

                ammoBoxNode.AppendChild(extraSeedsNode);
                ammoBoxNode.AppendChild(normalTypeNode);
                ammoBoxNode.AppendChild(bumpyTypeNode);
                ammoBoxNode.AppendChild(timeToRespawnNode);
                ammoBoxNode.AppendChild(oneTimePickupNode);

                extraSeedsNode.InnerText = ammoBox.getExtraSeeds().ToString();
                normalTypeNode.InnerText = ammoBox.getNormalType().ToString();
                bumpyTypeNode.InnerText = ammoBox.getBumpyType().ToString();
                timeToRespawnNode.InnerText = ammoBox.getTimeToRespawn().ToString();
                oneTimePickupNode.InnerText = ammoBox.getOneTimePickup().ToString();

            }
            if (obj.gameObject.name == "Slug")
            {
                SlugScript slugScript = (SlugScript)obj.gameObject.GetComponent<SlugScript>();
                GameObject slugBoundA = slugScript.getSlugBoundA();
                GameObject slugBoundB = slugScript.getSlugBoundB();

                if (slugBoundA == null || slugBoundB == null)
                {
                    Debug.LogError("Slugbound is null!");
                    return -1;
                }

                XmlElement slugNode = xmlDocument.CreateElement("Slug");
                objectNode.AppendChild(slugNode);

                //BoundA
                XmlElement slugBoundANode = xmlDocument.CreateElement("SlugBoundA");
                slugNode.AppendChild(slugBoundANode);

                XmlElement slugBoundAxNode = xmlDocument.CreateElement("x");
                XmlElement slugBoundAyNode = xmlDocument.CreateElement("y");
                XmlElement slugBoundAzNode = xmlDocument.CreateElement("z");

                slugBoundANode.AppendChild(slugBoundAxNode);
                slugBoundANode.AppendChild(slugBoundAyNode);
                slugBoundANode.AppendChild(slugBoundAzNode);

                slugBoundAxNode.InnerText = slugBoundA.transform.position.x.ToString();
                slugBoundAyNode.InnerText = slugBoundA.transform.position.y.ToString();
                slugBoundAzNode.InnerText = slugBoundA.transform.position.z.ToString();

                //BoundB
                XmlElement slugBoundBNode = xmlDocument.CreateElement("SlugBoundB");
                slugNode.AppendChild(slugBoundBNode);

                XmlElement slugBoundBxNode = xmlDocument.CreateElement("x");
                XmlElement slugBoundByNode = xmlDocument.CreateElement("y");
                XmlElement slugBoundBzNode = xmlDocument.CreateElement("z");

                slugBoundBNode.AppendChild(slugBoundBxNode);
                slugBoundBNode.AppendChild(slugBoundByNode);
                slugBoundBNode.AppendChild(slugBoundBzNode);

                slugBoundBxNode.InnerText = slugBoundB.transform.position.x.ToString();
                slugBoundByNode.InnerText = slugBoundB.transform.position.y.ToString();
                slugBoundBzNode.InnerText = slugBoundB.transform.position.z.ToString();
            }

            //only if it is a tutorial object
            if (obj.gameObject.name == "TutorialObject")
            {
                //required objects
                TutorialTriggerScript triggerScript = (TutorialTriggerScript)obj.gameObject.GetComponent<TutorialTriggerScript>() as TutorialTriggerScript;
                BoxCollider boxCollider = obj.gameObject.GetComponent<BoxCollider>() as BoxCollider;
                Vector3 boundingBox = boxCollider.size;

                //tutorial node
                XmlElement tutorialNode = xmlDocument.CreateElement("Tutorial");
                objectNode.AppendChild(tutorialNode);

                //Alpha Object
                GameObject alphaObject = triggerScript.getAlphaObject();

                if (alphaObject != null)
                {
                    //main alphaobject node
                    XmlElement alphaObjectNode = xmlDocument.CreateElement("AlphaObject");
                    tutorialNode.AppendChild(alphaObjectNode);

                    //prefab name
                    XmlElement alphaPrefabNode = xmlDocument.CreateElement("Prefab");
                    alphaObjectNode.AppendChild(alphaPrefabNode);
                    alphaPrefabNode.InnerText = alphaObject.name;

                    //position
                    XmlElement alphaObjectPositionNode = xmlDocument.CreateElement("Position");
                    alphaObjectNode.AppendChild(alphaObjectPositionNode);

                    XmlElement alphaObjectXPositionNode = xmlDocument.CreateElement("x");
                    XmlElement alphaObjectYPositionNode = xmlDocument.CreateElement("y");
                    XmlElement alphaObjectZPositionNode = xmlDocument.CreateElement("z");

                    alphaObjectPositionNode.AppendChild(alphaObjectXPositionNode);
                    alphaObjectPositionNode.AppendChild(alphaObjectYPositionNode);
                    alphaObjectPositionNode.AppendChild(alphaObjectZPositionNode);

                    alphaObjectXPositionNode.InnerText = alphaObject.transform.position.x.ToString();
                    alphaObjectYPositionNode.InnerText = alphaObject.transform.position.y.ToString();
                    alphaObjectZPositionNode.InnerText = alphaObject.transform.position.z.ToString();

                    //rotation
                    XmlElement alphaObjectRotationNode = xmlDocument.CreateElement("Rotation");
                    alphaObjectNode.AppendChild(alphaObjectRotationNode);

                    XmlElement alphaObjectXRotationNode = xmlDocument.CreateElement("x");
                    XmlElement alphaObjectYRotationNode = xmlDocument.CreateElement("y");
                    XmlElement alphaObjectZRotationNode = xmlDocument.CreateElement("z");

                    alphaObjectRotationNode.AppendChild(alphaObjectXRotationNode);
                    alphaObjectRotationNode.AppendChild(alphaObjectYRotationNode);
                    alphaObjectRotationNode.AppendChild(alphaObjectZRotationNode);

                    alphaObjectXRotationNode.InnerText = alphaObject.transform.eulerAngles.x.ToString();
                    alphaObjectYRotationNode.InnerText = alphaObject.transform.eulerAngles.y.ToString();
                    alphaObjectZRotationNode.InnerText = alphaObject.transform.eulerAngles.z.ToString();


                    //save the scale
                    XmlElement alphaObjectScaleNode = xmlDocument.CreateElement("Scaling");
                    alphaObjectNode.AppendChild(alphaObjectScaleNode);

                    XmlElement alphaObjectScaleXNode = xmlDocument.CreateElement("x");
                    XmlElement alphaObjectScaleYNode = xmlDocument.CreateElement("y");
                    XmlElement alphaObjectScaleZNode = xmlDocument.CreateElement("z");

                    alphaObjectScaleNode.AppendChild(alphaObjectScaleXNode);
                    alphaObjectScaleNode.AppendChild(alphaObjectScaleYNode);
                    alphaObjectScaleNode.AppendChild(alphaObjectScaleZNode);

                    alphaObjectScaleXNode.InnerText = alphaObject.transform.localScale.x.ToString();
                    alphaObjectScaleYNode.InnerText = alphaObject.transform.localScale.y.ToString();
                    alphaObjectScaleZNode.InnerText = alphaObject.transform.localScale.z.ToString();

                }

                XmlElement jumpButtonTutorialNode = xmlDocument.CreateElement("JumpButtonTutorial");
                XmlElement normalShroomButtonTutorialNode = xmlDocument.CreateElement("NormalShroomButtonTutorial");
                XmlElement flashButtonTutorialNode = xmlDocument.CreateElement("FlashButtonTutorial");
                XmlElement bumpyShroomButtonTutorialNode = xmlDocument.CreateElement("BumpyShroomButtonTutorial");

                XmlElement lightTutorialNode = xmlDocument.CreateElement("LightTutorial");
                XmlElement slugTutorialNode = xmlDocument.CreateElement("SlugTutorial");
                XmlElement crystalTutorialNode = xmlDocument.CreateElement("CrystalTutorial");

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
                lightTutorialNode.InnerText = triggerScript.getLightTutorial().ToString();
                slugTutorialNode.InnerText = triggerScript.getSlugTutorial().ToString();
                crystalTutorialNode.InnerText = triggerScript.getCrystalTutorial().ToString();

                GameObject slugObject = triggerScript.getSlugObject();

                if (slugObject != null)
                {
                    XmlElement slugObjectNode = xmlDocument.CreateElement("SlugObject");
                    tutorialNode.AppendChild(slugObjectNode);
                    //position
                    XmlElement slugObjectPositionNode = xmlDocument.CreateElement("Position");
                    slugObjectNode.AppendChild(slugObjectPositionNode);

                    XmlElement slugObjectXPositionNode = xmlDocument.CreateElement("x");
                    XmlElement slugObjectYPositionNode = xmlDocument.CreateElement("y");
                    XmlElement slugObjectZPositionNode = xmlDocument.CreateElement("z");

                    slugObjectPositionNode.AppendChild(slugObjectXPositionNode);
                    slugObjectPositionNode.AppendChild(slugObjectYPositionNode);
                    slugObjectPositionNode.AppendChild(slugObjectZPositionNode);

                    slugObjectXPositionNode.InnerText = slugObject.transform.position.x.ToString();
                    slugObjectYPositionNode.InnerText = slugObject.transform.position.y.ToString();
                    slugObjectZPositionNode.InnerText = slugObject.transform.position.z.ToString();


                    SlugScript slugScript2 = slugObject.gameObject.GetComponent<SlugScript>();
                    GameObject slugBoundA2 = slugScript2.getSlugBoundA();
                    GameObject slugBoundB2 = slugScript2.getSlugBoundB();

                    if (slugBoundA2 == null || slugBoundB2 == null)
                    {
                        Debug.LogError("Slugbound is null!");
                        return -1;
                    }

                    //BoundA
                    XmlElement slugBoundANode2 = xmlDocument.CreateElement("SlugBoundA");
                    slugObjectNode.AppendChild(slugBoundANode2);

                    XmlElement slugBoundAxNode2 = xmlDocument.CreateElement("x");
                    XmlElement slugBoundAyNode2 = xmlDocument.CreateElement("y");
                    XmlElement slugBoundAzNode2 = xmlDocument.CreateElement("z");

                    slugBoundANode2.AppendChild(slugBoundAxNode2);
                    slugBoundANode2.AppendChild(slugBoundAyNode2);
                    slugBoundANode2.AppendChild(slugBoundAzNode2);

                    slugBoundAxNode2.InnerText = slugBoundA2.transform.position.x.ToString();
                    slugBoundAyNode2.InnerText = slugBoundA2.transform.position.y.ToString();
                    slugBoundAzNode2.InnerText = slugBoundA2.transform.position.z.ToString();

                    //BoundB
                    XmlElement slugBoundBNode2 = xmlDocument.CreateElement("SlugBoundB");
                    slugObjectNode.AppendChild(slugBoundBNode2);

                    XmlElement slugBoundBxNode2 = xmlDocument.CreateElement("x");
                    XmlElement slugBoundByNode2 = xmlDocument.CreateElement("y");
                    XmlElement slugBoundBzNode2 = xmlDocument.CreateElement("z");

                    slugBoundBNode2.AppendChild(slugBoundBxNode2);
                    slugBoundBNode2.AppendChild(slugBoundByNode2);
                    slugBoundBNode2.AppendChild(slugBoundBzNode2);

                    slugBoundBxNode2.InnerText = slugBoundB2.transform.position.x.ToString();
                    slugBoundByNode2.InnerText = slugBoundB2.transform.position.y.ToString();
                    slugBoundBzNode2.InnerText = slugBoundB2.transform.position.z.ToString();
                }

                GameObject blockObject = triggerScript.getBlockObject();

                if (blockObject != null)
                {
                    XmlElement blockObjectNode = xmlDocument.CreateElement("BlockObject");
                    tutorialNode.AppendChild(blockObjectNode);
                    //position
                    XmlElement blockObjectPositionNode = xmlDocument.CreateElement("Position");
                    blockObjectNode.AppendChild(blockObjectPositionNode);

                    XmlElement blockObjectXPositionNode = xmlDocument.CreateElement("x");
                    XmlElement blockObjectYPositionNode = xmlDocument.CreateElement("y");
                    XmlElement blockObjectZPositionNode = xmlDocument.CreateElement("z");

                    blockObjectPositionNode.AppendChild(blockObjectXPositionNode);
                    blockObjectPositionNode.AppendChild(blockObjectYPositionNode);
                    blockObjectPositionNode.AppendChild(blockObjectZPositionNode);

                    blockObjectXPositionNode.InnerText = blockObject.transform.position.x.ToString();
                    blockObjectYPositionNode.InnerText = blockObject.transform.position.y.ToString();
                    blockObjectZPositionNode.InnerText = blockObject.transform.position.z.ToString();
                }

                //button booleans
                XmlElement buttonsEnabledNode = xmlDocument.CreateElement("ButtonsEnabled");
                tutorialNode.AppendChild(buttonsEnabledNode);

                XmlElement movementLeftEnabledNode = xmlDocument.CreateElement("MovementLeft");
                XmlElement movementRightEnabledNode = xmlDocument.CreateElement("MovementRight");
                XmlElement jumpButtonEnabledNode = xmlDocument.CreateElement("JumpButton");
                XmlElement flashButtonEnabledNode = xmlDocument.CreateElement("FlashButton");
                XmlElement shootNormalShroomButtonEnabledNode = xmlDocument.CreateElement("NormalShroomButton");
                XmlElement shootBumpyShroomButtonEnabledNode = xmlDocument.CreateElement("BumpyShroomButton");

                buttonsEnabledNode.AppendChild(movementLeftEnabledNode);
                buttonsEnabledNode.AppendChild(movementRightEnabledNode);
                buttonsEnabledNode.AppendChild(jumpButtonEnabledNode);
                buttonsEnabledNode.AppendChild(flashButtonEnabledNode);
                buttonsEnabledNode.AppendChild(shootNormalShroomButtonEnabledNode);
                buttonsEnabledNode.AppendChild(shootBumpyShroomButtonEnabledNode);

                movementLeftEnabledNode.InnerText = triggerScript.getMovementLeftEnabled().ToString();
                movementRightEnabledNode.InnerText = triggerScript.getMovementRightEnabled().ToString();
                jumpButtonEnabledNode.InnerText = triggerScript.getJumpButtonEnabled().ToString();
                flashButtonEnabledNode.InnerText = triggerScript.getFlashButtonEnabled().ToString();
                shootNormalShroomButtonEnabledNode.InnerText = triggerScript.getNormalShroomButtonEnabled().ToString();
                shootBumpyShroomButtonEnabledNode.InnerText = triggerScript.getBumpyShroomButtonEnabled().ToString();

                if (triggerScript.getTutorialTextureA() != null || triggerScript.getTutorialTextureB() != null || triggerScript.getKeyboardTutorialTextureA() != null || triggerScript.getKeyboardTutorialTextureB() != null || triggerScript.getXboxTutorialTextureA() != null || triggerScript.getXboxTutorialTextureB() != null)
                {
                    //textures
                    XmlElement tutorialTexturesNode = xmlDocument.CreateElement("Textures");
                    tutorialNode.AppendChild(tutorialTexturesNode);

                    //
                    //touchinput
                    //
                    //textureA
                    if (triggerScript.getTutorialTextureA() != null)
                    {
                        XmlElement textureANode = xmlDocument.CreateElement("TextureA");
                        tutorialTexturesNode.AppendChild(textureANode);


                        XmlElement textureANameNode = xmlDocument.CreateElement("Texturename");
                        XmlElement textureAXPositionNode = xmlDocument.CreateElement("x");
                        XmlElement textureAYPositionNode = xmlDocument.CreateElement("y");
                        XmlElement textureATimerNode = xmlDocument.CreateElement("Timer");

                        textureANode.AppendChild(textureANameNode);
                        textureANode.AppendChild(textureAXPositionNode);
                        textureANode.AppendChild(textureAYPositionNode);
                        textureANode.AppendChild(textureATimerNode);

                        textureANameNode.InnerText = triggerScript.getTutorialTextureA().name;
                        textureAXPositionNode.InnerText = triggerScript.getXPositionTexA().ToString();
                        textureAYPositionNode.InnerText = triggerScript.getYPositionTexA().ToString();
                        textureATimerNode.InnerText = triggerScript.getTimerTexA().ToString();
                    }
                    //textureB
                    if (triggerScript.getTutorialTextureB() != null)
                    {
                        XmlElement textureBNode = xmlDocument.CreateElement("TextureB");
                        tutorialTexturesNode.AppendChild(textureBNode);

                        XmlElement textureBNameNode = xmlDocument.CreateElement("Texturename");
                        XmlElement textureBXPositionNode = xmlDocument.CreateElement("x");
                        XmlElement textureBYPositionNode = xmlDocument.CreateElement("y");
                        XmlElement textureBTimerNode = xmlDocument.CreateElement("Timer");

                        textureBNode.AppendChild(textureBNameNode);
                        textureBNode.AppendChild(textureBXPositionNode);
                        textureBNode.AppendChild(textureBYPositionNode);
                        textureBNode.AppendChild(textureBTimerNode);

                        textureBNameNode.InnerText = triggerScript.getTutorialTextureB().name;
                        textureBXPositionNode.InnerText = triggerScript.getXPositionTexB().ToString();
                        textureBYPositionNode.InnerText = triggerScript.getYPositionTexB().ToString();
                        textureBTimerNode.InnerText = triggerScript.getTimerTexB().ToString();
                    }
                    //
                    //keyboard input
                    //
                    if (triggerScript.getKeyboardTutorialTextureA() != null)
                    {
                        XmlElement keyboardTextureANode = xmlDocument.CreateElement("KeyboardTextureA");
                        tutorialTexturesNode.AppendChild(keyboardTextureANode);

                        XmlElement keyboardTextureANameNode = xmlDocument.CreateElement("Texturename");
                        XmlElement keyboardTextureAXPositionNode = xmlDocument.CreateElement("x");
                        XmlElement keyboardTextureAYPositionNode = xmlDocument.CreateElement("y");
                        XmlElement keyboardTextureATimerNode = xmlDocument.CreateElement("Timer");

                        keyboardTextureANode.AppendChild(keyboardTextureANameNode);
                        keyboardTextureANode.AppendChild(keyboardTextureAXPositionNode);
                        keyboardTextureANode.AppendChild(keyboardTextureAYPositionNode);
                        keyboardTextureANode.AppendChild(keyboardTextureATimerNode);

                        keyboardTextureANameNode.InnerText = triggerScript.getKeyboardTutorialTextureA().name;
                        keyboardTextureAXPositionNode.InnerText = triggerScript.getXPositionKeyboardTexA().ToString();
                        keyboardTextureAYPositionNode.InnerText = triggerScript.getYPositionKeyboardTexA().ToString();
                        keyboardTextureATimerNode.InnerText = triggerScript.getTimerKeyboardTexA().ToString();

                    }
                    if (triggerScript.getKeyboardTutorialTextureB() != null)
                    {
                        XmlElement keyboardTextureBNode = xmlDocument.CreateElement("KeyboardTextureB");
                        tutorialTexturesNode.AppendChild(keyboardTextureBNode);

                        XmlElement keyboardTextureBNameNode = xmlDocument.CreateElement("Texturename");
                        XmlElement keyboardTextureBXPositionNode = xmlDocument.CreateElement("x");
                        XmlElement keyboardTextureBYPositionNode = xmlDocument.CreateElement("y");
                        XmlElement keyboardTextureBTimerNode = xmlDocument.CreateElement("Timer");

                        keyboardTextureBNode.AppendChild(keyboardTextureBNameNode);
                        keyboardTextureBNode.AppendChild(keyboardTextureBXPositionNode);
                        keyboardTextureBNode.AppendChild(keyboardTextureBYPositionNode);
                        keyboardTextureBNode.AppendChild(keyboardTextureBTimerNode);

                        keyboardTextureBNameNode.InnerText = triggerScript.getKeyboardTutorialTextureB().name;
                        keyboardTextureBXPositionNode.InnerText = triggerScript.getXPositionKeyboardTexB().ToString();
                        keyboardTextureBYPositionNode.InnerText = triggerScript.getYPositionKeyboardTexB().ToString();
                        keyboardTextureBTimerNode.InnerText = triggerScript.getTimerKeyboardTexB().ToString();
                    }
                    //

                    //
                    //xbox input
                    //
                    if (triggerScript.getXboxTutorialTextureA() != null)
                    {
                        XmlElement xboxTextureANode = xmlDocument.CreateElement("XboxTextureA");
                        tutorialTexturesNode.AppendChild(xboxTextureANode);

                        XmlElement xboxTextureANameNode = xmlDocument.CreateElement("Texturename");
                        XmlElement xboxTextureAXPositionNode = xmlDocument.CreateElement("x");
                        XmlElement xboxTextureAYPositionNode = xmlDocument.CreateElement("y");
                        XmlElement xboxTextureATimerNode = xmlDocument.CreateElement("Timer");

                        xboxTextureANode.AppendChild(xboxTextureANameNode);
                        xboxTextureANode.AppendChild(xboxTextureAXPositionNode);
                        xboxTextureANode.AppendChild(xboxTextureAYPositionNode);
                        xboxTextureANode.AppendChild(xboxTextureATimerNode);

                        xboxTextureANameNode.InnerText = triggerScript.getXboxTutorialTextureA().name;
                        xboxTextureAXPositionNode.InnerText = triggerScript.getXPositionXboxTexA().ToString();
                        xboxTextureAYPositionNode.InnerText = triggerScript.getYPositionXboxTexA().ToString();
                        xboxTextureATimerNode.InnerText = triggerScript.getTimerXboxTexA().ToString();
                    }
                    if (triggerScript.getXboxTutorialTextureB() != null)
                    {
                        XmlElement xboxTextureBNode = xmlDocument.CreateElement("XboxTextureB");
                        tutorialTexturesNode.AppendChild(xboxTextureBNode);

                        XmlElement xboxTextureBNameNode = xmlDocument.CreateElement("Texturename");
                        XmlElement xboxTextureBXPositionNode = xmlDocument.CreateElement("x");
                        XmlElement xboxTextureBYPositionNode = xmlDocument.CreateElement("y");
                        XmlElement xboxTextureBTimerNode = xmlDocument.CreateElement("Timer");

                        xboxTextureBNode.AppendChild(xboxTextureBNameNode);
                        xboxTextureBNode.AppendChild(xboxTextureBXPositionNode);
                        xboxTextureBNode.AppendChild(xboxTextureBYPositionNode);
                        xboxTextureBNode.AppendChild(xboxTextureBTimerNode);

                        xboxTextureBNameNode.InnerText = triggerScript.getXboxTutorialTextureB().name;
                        xboxTextureBXPositionNode.InnerText = triggerScript.getXPositionXboxTexB().ToString();
                        xboxTextureBYPositionNode.InnerText = triggerScript.getYPositionXboxTexB().ToString();
                        xboxTextureBTimerNode.InnerText = triggerScript.getTimerXboxTexB().ToString();
                    }
                    //
                }


                //destroy on exit bool
                XmlElement destroyOnExitNode = xmlDocument.CreateElement("DestroyOnExit");
                tutorialNode.AppendChild(destroyOnExitNode);
                destroyOnExitNode.InnerText = triggerScript.getDestroyOnExit().ToString();

                XmlElement destroyOnCompletionNode = xmlDocument.CreateElement("DestroyOnCompletion");
                tutorialNode.AppendChild(destroyOnCompletionNode);
                destroyOnCompletionNode.InnerText = triggerScript.getDestroyOnCompletion().ToString();

                //bounding box
                XmlElement boundingBoxNode = xmlDocument.CreateElement("BoundingBox");
                tutorialNode.AppendChild(boundingBoxNode);

                XmlElement xBoundingBoxNode = xmlDocument.CreateElement("x");
                XmlElement yBoundingBoxNode = xmlDocument.CreateElement("y");
                XmlElement zBoundingBoxNode = xmlDocument.CreateElement("z");

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
        GameObject player = GameObject.Find("Player");

        XmlElement playerNode = xmlDocument.CreateElement("Player");
        masterNode.AppendChild(playerNode);

        //save position
        XmlElement playerPositionNode = xmlDocument.CreateElement("Position");
        playerNode.AppendChild(playerPositionNode);

        XmlElement playerXNode = xmlDocument.CreateElement("x");
        XmlElement playerYNode = xmlDocument.CreateElement("y");
        XmlElement playerZNode = xmlDocument.CreateElement("z");

        playerPositionNode.AppendChild(playerXNode);
        playerPositionNode.AppendChild(playerYNode);
        playerPositionNode.AppendChild(playerZNode);

        playerXNode.InnerText = player.transform.position.x.ToString();
        playerYNode.InnerText = player.transform.position.y.ToString();
        playerZNode.InnerText = player.transform.position.z.ToString();

        //save the rotation
        XmlElement playerRotationNode = xmlDocument.CreateElement("Rotation");
        playerNode.AppendChild(playerRotationNode);

        XmlElement playerXRotationNode = xmlDocument.CreateElement("x");
        XmlElement playerYRotationNode = xmlDocument.CreateElement("y");
        XmlElement playerZRotationNode = xmlDocument.CreateElement("z");

        playerRotationNode.AppendChild(playerXRotationNode);
        playerRotationNode.AppendChild(playerYRotationNode);
        playerRotationNode.AppendChild(playerZRotationNode);

        playerXRotationNode.InnerText = player.transform.eulerAngles.x.ToString();
        playerYRotationNode.InnerText = player.transform.eulerAngles.y.ToString();
        playerZRotationNode.InnerText = player.transform.eulerAngles.z.ToString();
        //

        xmlDocument.Save(filePath);
        Debug.Log("Xml saved to: Assets/LevelsXML/" + levelName);   //the xml has been successfully saved
        return 0;
    }
    //check if the prefab is in the prefab folder
    private bool checkValidPrefab(string prefabName)
    {
        foreach (string prefab in prefabList)
        {
            if (Path.GetFileNameWithoutExtension(prefab) == prefabName)
            {
                return true;
            }
        }
        //can't find it so returning false
        Debug.LogError("Can't find prefab with the name: " + prefabName);
        Debug.LogError("Please make sure you made a prefab of the object and is located in Resources/Prefabs");
        return false;
    }
}