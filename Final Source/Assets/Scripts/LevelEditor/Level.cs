using UnityEngine;
using System.Collections;

public class Level
{
    private int levelID = -1;

    private string easyXml = "";
    private string mediumXml = "";
    private string hardXml = "";

    public void setLevelID(int aID)
    {
        levelID = aID;
    }

    public void setLevelDifficultyXmlFile(string xmlFile, string levelDifficulty)
    {
        if (levelDifficulty == "Easy")
        {
            if (easyXml == "")
            {
                easyXml = xmlFile;
            }
            else
            {
                Debug.LogError("Trying to set another easy mode for this levelID: " + levelID);
                Debug.LogError("Xml File: " + xmlFile);
            }
        }
        else if (levelDifficulty == "Medium")
        {
            if (mediumXml == "")
            {
                mediumXml = xmlFile;
            }
            else
            {
                Debug.LogError("Trying to set another medium mode for this levelID: " + levelID);
                Debug.LogError("Xml File: " + xmlFile);
            }
        }
        else if (levelDifficulty == "Hard")
        {
            if (hardXml == "")
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

    private string getEasyXml()
    {
        if (easyXml == "")
        {
            Debug.LogError("easyXml asked but not set");
            return "";
        }
        return easyXml;
    }

    private string getMediumXml()
    {
        if (mediumXml == "")
        {
            Debug.LogError("mediumXml asked but not set");
            return "";
        }
        return mediumXml;
    }

    private string getHardXml()
    {
        if (hardXml == "")
        {
            Debug.LogError("hardXml asked but not set");
            return "";
        }
        return hardXml;
    }

    public int getLevelID()
    {
        return levelID;
    }

    public string getLevelXmlByDifficulty(string levelDifficulty)
    {
        if (levelDifficulty == "Easy")
        {
            return getEasyXml();
        }
        else if (levelDifficulty == "Medium")
        {
            return getMediumXml();
        }
        else if (levelDifficulty == "Hard")
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