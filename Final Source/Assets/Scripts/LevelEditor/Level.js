#pragma strict


public class Level
{
private var levelID:int = -1;

private var easyXml:String = "";
private var mediumXml:String = "";
private var hardXml:String = "";



public function setLevelID(aID:int):void
{
	levelID = aID;
}

public function setLevelDifficultyXmlFile(xmlFile:String, levelDifficulty:String)
{
	if(levelDifficulty == "Easy")
	{
		if(easyXml == "")
		{
			easyXml = xmlFile;
		}
		else
		{
			Debug.LogError("Trying to set another easy mode for this levelID: " + levelID);
			Debug.LogError("Xml File: " + xmlFile);
		}
	}
	else if(levelDifficulty == "Medium")
	{
		if(mediumXml == "")
		{
			mediumXml = xmlFile;
		}
		else
		{
			Debug.LogError("Trying to set another medium mode for this levelID: " + levelID);
			Debug.LogError("Xml File: " + xmlFile);
		}
	}
	else if(levelDifficulty == "Hard")
	{
		if(hardXml == "")
		{
			hardXml = xmlFile;
		}
		else
		{
			Debug.LogError("Trying to set another hard mode for this levelID: " + levelID);
			Debug.LogError("Xml File: " + xmlFile);
		}
	}
	else
	{
		Debug.LogError("The difficulty was neither Easy, Medium or Hard: " + levelDifficulty);
	}
}

private function getEasyXml():String
{
	if(easyXml == "")
	{
		Debug.LogError("easyXml asked but not set");
		return "";
	}
	return easyXml;
}

private function getMediumXml():String
{
	if(mediumXml == "")
	{
		Debug.LogError("mediumXml asked but not set");
		return "";
	}
	return mediumXml;
}

private function getHardXml():String
{
	if(hardXml == "")
	{
		Debug.LogError("hardXml asked but not set");
		return "";
	}
	return hardXml;
}

public function getLevelID():int
{
	return levelID;
}

public function getLevelXmlByDifficulty(levelDifficulty:String):String
{
	if(levelDifficulty == "Easy")
	{
		return getEasyXml();
	}
	else if(levelDifficulty == "Medium")
	{
		return getMediumXml();
	}
	else if(levelDifficulty == "Hard")
	{
		return getHardXml();
	}
	else
	{
		Debug.LogError("Difficulty not even found! Impossible: " + levelDifficulty);
	}
	return "";
}
}