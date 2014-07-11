using UnityEngine;
using System.Collections;

public class GameHUD : MonoBehaviour
{
    //public variables of X and Y position for creatives so they can move around the HUD to where ever they want

    //
    //Show Battery power
    //
    public Texture2D currentBatteryTexture = null;
    private Rect currentBatteryRect;
    private float currentBatteryX = -50.0f;
    private float currentBatteryY = -19.0f;

    private float blinkingCounter = 0.5f;
    private bool showBattery = true;

    public ArrayList batteryBarTextures = new ArrayList();
    private Texture2D batteryBarTex = null;
    private Rect batteryBarRect;
    public float batteryBarX = 46.0f;
    public float batteryBarY = 33.0f;
    private int amountOfBatteryBars = 0;

    public Texture2D highBatteryTexture = null;
    public Texture2D lowBatteryTexture = null;

    public Texture2D scoreTexture = null;
    private Rect scoreRect;
    public float scoreX = 200.0f;
    public float scoreY = -60.0f;

    private Rect scoreTextRect;
    public float scoreTextX = 420.0f;
    public float scoreTextY = 38.0f;

    public Texture2D normalSeedsTexture = null;
    private Rect normalSeedsRect;
    public float normalSeedsX = 20.0f;
    public float normalSeedsY = 280.0f;
    private Rect normalSeedsInfinityRect;
    public float normalSeedsInfinityX = 117.0f;
    public float normalSeedsInfinityY = 297.0f;
    public float normalSeedsAmmoCountX = 130.0f;
    public float normalSeedsAmmoCountY = 320.0f;

    public Texture2D bumpySeedsTexture = null;
    private Rect bumpySeedsRect;
    public float bumpySeedsX = 20.0f;
    public float bumpySeedsY = 365.0f;
    private Rect bumpySeedsInfinityRect;
    public float bumpySeedsInfinityX = 117.0f;
    public float bumpySeedsInfinityY = 385.0f;
    public float bumpySeedsAmmoCountX = 130.0f;
    public float bumpySeedsAmmoCountY = 407.0f;

    private GUIStyle fontSkin = new GUIStyle();

    public Texture2D infinity = null;
    private bool infiniteAmmo = false;

    //crystals hud
    private int crystalsTotal;
    private int crystalsCollected;

    public Texture2D crystalActive = null;
    private Rect crystalActiveRect;
    public float crystalActiveX = 205.0f;
    public float crystalActiveY = 80.0f;

    public Texture2D crystalInactive = null;
    private Rect crystalInactiveRect;
    public float crystalInactiveX = 205.0f;
    public float crystalInactiveY = 80.0f;

    private Vector3 scale;
    private float originalWidth = 1920.0f;
    private float originalHeight = 1080.0f;

    private GameLogic gameLogic = null;

    public void Awake()
    {
        TextureLoader textureLoader = null;
        if (GameObject.Find("TextureLoader") != null) textureLoader = GameObject.Find("TextureLoader").GetComponent<TextureLoader>();

        if (textureLoader != null)
        {
            highBatteryTexture = textureLoader.getTexture("BatteryNormal");
            lowBatteryTexture = textureLoader.getTexture("BatteryDanger");
            currentBatteryTexture = highBatteryTexture;

            infinity = textureLoader.getTexture("infinity");
            normalSeedsTexture = textureLoader.getTexture("SeedNormal");
            bumpySeedsTexture = textureLoader.getTexture("SeedBumpy");

            crystalActive = textureLoader.getTexture("Crystal Active");
            crystalInactive = textureLoader.getTexture("Crystal Inactive");

            scoreTexture = textureLoader.getTexture("Score");

            for (int i = 0; i < 10; ++i)
            {
                batteryBarTextures.Add(textureLoader.getTexture("BatteryBar" + i.ToString()));
            }

            batteryBarTex = batteryBarTextures[0] as Texture2D;
        }
        fontSkin.font = (Font)Resources.Load("Fonts/sofachrome rg") as Font;
        //font colour
        Color fontColor = fontSkin.normal.textColor;

        fontColor.b = 197.0f / 255.0f;
        fontColor.g = 185.0f / 255.0f;
        fontColor.r = 147.0f / 255.0f;

        fontSkin.normal.textColor = fontColor;

        gameLogic = GameObject.Find("GameLogic").GetComponent<GameLogic>();
        StartCoroutine(checkInfiniteAmmo());
    }

    private IEnumerator checkInfiniteAmmo()
    {
        yield return new WaitForEndOfFrame();
        infiniteAmmo = gameLogic.getInfiniteAmmo();
    }

    public void OnGUI()
    {
        checkBatteryState();
        scaleHud();
        crystalsTotal = gameLogic.getCrystalsToComplete();
        crystalsCollected = gameLogic.getCrystalsSampleCount();


        if (showBattery && currentBatteryTexture != null)
        {
            GUI.DrawTexture(currentBatteryRect, currentBatteryTexture);

            if (batteryBarTextures.Count > 0)
            {
                for (int j = 0; j < amountOfBatteryBars; j++)
                {
                    Texture2D batteryBarTexture = batteryBarTextures[j] as Texture2D;
                    //draw from bot to top
                    GUI.DrawTexture(new Rect(batteryBarRect.x, batteryBarRect.y, batteryBarRect.width, batteryBarRect.height), batteryBarTexture);
                }
            }
        }
        if (scoreTexture != null)
        {
            string scoreText = gameLogic.getScore().ToString();
            GUI.DrawTexture(scoreRect, scoreTexture);
            fontSkin.fontSize = Mathf.RoundToInt(scale.x * 30);
            GUI.Label(scoreTextRect, scoreText, fontSkin);
        }

        if (crystalActive != null && crystalInactive != null)
        {
            //draw the crystals
            for (int i = 0; i < crystalsTotal; ++i)
            {
                //amount of crystals you've picked up
                if (i < crystalsCollected && crystalsCollected != 0)
                {
                    GUI.DrawTexture(new Rect(crystalActiveRect.x + i * crystalActiveRect.width, crystalActiveRect.y, crystalActiveRect.width, crystalActiveRect.height), crystalActive);
                }
                //amount of crystals you've not picked up yet
                else
                {
                    GUI.DrawTexture(new Rect(crystalInactiveRect.x + i * crystalInactiveRect.width, crystalInactiveRect.y, crystalInactiveRect.width, crystalInactiveRect.height), crystalInactive);
                }
            }
        }
        if (normalSeedsTexture != null && bumpySeedsTexture != null)
        {
            GUI.DrawTexture(normalSeedsRect, normalSeedsTexture);
            GUI.DrawTexture(bumpySeedsRect, bumpySeedsTexture);
        }
        if (infiniteAmmo == true && infinity != null)
        {
            //draw infinite sign
            GUI.DrawTexture(normalSeedsInfinityRect, infinity);
            GUI.DrawTexture(bumpySeedsInfinityRect, infinity);
        }
        else
        {
            fontSkin.fontSize = Mathf.RoundToInt(scale.x * 26);

            int currentNormalAmmo = gameLogic.getCurrentNormalSeeds();
            int maximumNormalAmmo = gameLogic.getMaximumNormalSeeds();

            GUI.Label(scaleRect(new Rect(normalSeedsAmmoCountX, normalSeedsAmmoCountY, 100, 100)), currentNormalAmmo.ToString() + "/" + maximumNormalAmmo.ToString(), fontSkin);

            int currentBumpyAmmo = gameLogic.getCurrentBumpySeeds();
            int maximumBumpyAmmo = gameLogic.getMaximumBumpySeeds();

            GUI.Label(scaleRect(new Rect(bumpySeedsAmmoCountX, bumpySeedsAmmoCountY, 100, 100)), currentBumpyAmmo.ToString() + "/" + maximumBumpyAmmo.ToString(), fontSkin);
        }
    }

    private void scaleHud()
    {
        //get the current scale by using the current screen size and the original screen size
        //original width / height is defined in a variable at top, we use an aspect ratio of 16:9 and original screen size of 1920x1080
        scale.x = Screen.width / originalWidth;		//X scale is the current width divided by the original width
        scale.y = Screen.height / originalHeight;	//Y scale is the current height divided by the original height


        //battery bar holder
        if (currentBatteryTexture != null)
        {
            currentBatteryRect = new Rect(currentBatteryX, currentBatteryY, currentBatteryTexture.width, currentBatteryTexture.height);
            currentBatteryRect = scaleRect(currentBatteryRect);
        }
        //bars
        if (batteryBarTex != null)
        {
            batteryBarRect = new Rect(batteryBarX, batteryBarY, batteryBarTex.width, batteryBarTex.height);
            batteryBarRect = scaleRect(batteryBarRect);
        }

        if (scoreTexture != null)
        {
            scoreRect = new Rect(scoreX, scoreY, scoreTexture.width, scoreTexture.height);
            scoreRect = scaleRect(scoreRect);

            scoreTextRect = new Rect(scoreTextX, scoreTextY, 200, 80);
            scoreTextRect = scaleRect(scoreTextRect);
        }

        if (crystalInactive != null && crystalActive != null)
        {
            crystalInactiveRect = new Rect(crystalInactiveX, crystalInactiveY, crystalInactive.width, crystalInactive.height);
            crystalInactiveRect = scaleRect(crystalInactiveRect);

            crystalActiveRect = new Rect(crystalActiveX, crystalActiveY, crystalActive.width, crystalActive.height);
            crystalActiveRect = scaleRect(crystalActiveRect);
        }

        if (normalSeedsTexture != null && bumpySeedsTexture != null)
        {
            normalSeedsRect = new Rect(normalSeedsX, normalSeedsY, normalSeedsTexture.width, normalSeedsTexture.height);
            normalSeedsRect = scaleRect(normalSeedsRect);

            bumpySeedsRect = new Rect(bumpySeedsX, bumpySeedsY, bumpySeedsTexture.width, bumpySeedsTexture.height);
            bumpySeedsRect = scaleRect(bumpySeedsRect);

        }
        if (infiniteAmmo && infinity != null)
        {
            normalSeedsInfinityRect = new Rect(normalSeedsInfinityX, normalSeedsInfinityY, infinity.width, infinity.height);
            normalSeedsInfinityRect = scaleRect(normalSeedsInfinityRect);
            bumpySeedsInfinityRect = new Rect(bumpySeedsInfinityX, bumpySeedsInfinityY, infinity.width, infinity.height);
            bumpySeedsInfinityRect = scaleRect(bumpySeedsInfinityRect);
        }
    }

    private Rect scaleRect(Rect rect)
    {
        Rect newRect = new Rect(rect.x * scale.x, rect.y * scale.y, rect.width * scale.x, rect.height * scale.y);
        return newRect;
    }

    private void checkBatteryState()
    {
        float maxBattery = gameLogic.getBatteryCapacity();
        float currentBatteryPower = gameLogic.getBattery();

        amountOfBatteryBars = Mathf.RoundToInt(currentBatteryPower / (maxBattery / 10));

        if (amountOfBatteryBars >= 5)
        {
            if (currentBatteryTexture != highBatteryTexture)
            {
                currentBatteryTexture = highBatteryTexture;
            }

            if (!showBattery) showBattery = true;
        }
        else if (amountOfBatteryBars <= 4)
        {

            if (currentBatteryTexture != lowBatteryTexture)
            {
                currentBatteryTexture = lowBatteryTexture;
            }
            if (amountOfBatteryBars >= 4 && !showBattery) showBattery = true;

            if (amountOfBatteryBars <= 3)
            {
                blinkingCounter -= Time.deltaTime;

                if (blinkingCounter < 0)
                {
                    blinkingCounter = 0.75f;
                    if (showBattery) showBattery = false;
                    else showBattery = true;
                }
            }
        }
    }
}