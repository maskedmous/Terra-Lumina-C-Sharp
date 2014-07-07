using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerController : MonoBehaviour
{

    private SoundEngineScript soundEngine = null;
    //private string debugInfo = "";

    private string lastDirection = "Right";
    private float maxSpeed = 7.5f;
    private float accelerationSpeed = 13.0f;

    private LineRenderer lineRenderer = null;
    private float y0 = 0.0f;
    private float x1 = 0.0f;
    private float x2 = 0.0f;
    private float x3 = 0.0f;
    private float x4 = 0.0f;
    private float x5 = 0.0f;
    private float x6 = 0.0f;
    private float x7 = 0.0f;
    private float x8 = 1.15f;
    private bool increasing = true;
    private float v = 0.0f;
    private float vx = 0.0f;
    private float vy = 0.0f;

    private bool isJumping = false;
    private float jumpForce = 7.5f;
    private List<GameObject> wheels = new List<GameObject>();

    //shroom seed shooting
    //private bool  isShooting = false;								//is it shooting at the moment?
    private List<GameObject> shrooms = new List<GameObject>();		//list of shrooms to shoot (should be 2)
    private List<GameObject> crystals = new List<GameObject>();		//the crystals you collect

    private GameLogic gameLogic = null;
    private PlayerParticleScript particleScript = null;

    public GameObject flashlight;
    private bool flashBool = false;
    private float counter = 0.0f;

    private bool control = true;

    private Animator anim = null;

    private Material currentMaterial = null;
    private Texture2D currentRoverTexture = null;
    private Texture2D highBatteryRoverTexture = null;
    private Texture2D yellowBatteryRoverTexture = null;
    private Texture2D redBatteryRoverTexture = null;

    public void Awake()
    {
        gameLogic = GameObject.Find("GameLogic").GetComponent("GameLogic") as GameLogic;

        particleScript = this.gameObject.GetComponent("PlayerParticleScript") as PlayerParticleScript;

        anim = GetComponent("Animator") as Animator;

        if (GameObject.Find("TextureLoader") != null)
        {
            TextureLoader textureLoader = GameObject.Find("TextureLoader").GetComponent("TextureLoader") as TextureLoader;

            highBatteryRoverTexture = textureLoader.getTexture("FinalTexture");
            yellowBatteryRoverTexture = textureLoader.getTexture("FinalTextureYellowWarning");
            redBatteryRoverTexture = textureLoader.getTexture("FinalTextureRedWarning");

            GameObject roverMesh = this.gameObject.transform.FindChild("RoverBodyMesh").gameObject;
            currentMaterial = roverMesh.renderer.material;
            currentRoverTexture = currentMaterial.GetTexture(0) as Texture2D;
        }

        if (Application.loadedLevelName == "LevelLoaderScene")
        {
            soundEngine = GameObject.Find("SoundEngine").GetComponent("SoundEngineScript") as SoundEngineScript;
        }

        shrooms.Add(Resources.Load("Prefabs/NormalShroom") as GameObject);
        shrooms.Add(Resources.Load("Prefabs/BumpyShroom") as GameObject);
    }

    public void Start()
    {
        lineRenderer = this.gameObject.GetComponent("LineRenderer") as LineRenderer;
        lineRenderer.enabled = false;

        rigidbody.centerOfMass = new Vector3(-0.2f, -0.25f, 0.0f);

        wheels.Add(this.gameObject.transform.FindChild("Wheel1").gameObject);
        wheels.Add(this.gameObject.transform.FindChild("Wheel2").gameObject);
        wheels.Add(this.gameObject.transform.FindChild("Wheel3").gameObject);
        wheels.Add(this.gameObject.transform.FindChild("Wheel4").gameObject);
        wheels.Add(this.gameObject.transform.FindChild("Wheel5").gameObject);
        wheels.Add(this.gameObject.transform.FindChild("Wheel6").gameObject);
    }

    public void Update()
    {
        checkBatteryStatus();
        if (control)
        {
            checkIfJumping();

            if (flashBool == true)
            {
                flash();
                counter += Time.deltaTime;
                if (counter >= 2.0f)
                {
                    flashlight.SetActive(false);
                    flashBool = false;
                    counter = 0.0f;
                }
            }
        }
        if (isJumping)
        {
            anim.SetBool("inAir", true);
        }
        if (!isJumping)
        {
            anim.SetBool("inAir", false);
        }
    }

    public void stopMovement()
    {
        float zero = 0.0f;
        this.gameObject.rigidbody.velocity = new Vector3(zero, this.gameObject.rigidbody.velocity.y, this.gameObject.rigidbody.velocity.z);
        anim.SetBool("isMoving", false);
    }

    public void stopControl()
    {
        control = false;
    }

    private void checkIfJumping()
    {
        bool hitFound = false;
        RaycastHit hitDown;
        int layerMask = 1 << 8;
        layerMask = ~layerMask;

        for (int i = 0; i < wheels.Count; ++i)
        {
            if (Physics.Raycast(wheels[i].transform.position, Vector3.down, out hitDown, 0.5f, layerMask))
            {
                hitFound = true;
                break;
            }
        }

        if (hitFound)
        {
            if (isJumping) particleScript.playParticle("landDust");
            isJumping = false;
            this.gameObject.rigidbody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezePositionZ;
        }
        else
        {
            isJumping = true;
        }

        //making sure rover does not rotate beyond 45 degrees from original rotation
        float cRot = this.gameObject.transform.rotation.eulerAngles.z;
        if (cRot > 45.0f && cRot < 180.0f)
        {
            Vector3 newRotation = this.gameObject.transform.rotation.eulerAngles;
            newRotation.z = 45.0f;
            this.gameObject.transform.eulerAngles = newRotation;
        }
        if (cRot > 180.0f && cRot < 315.0f)
        {
            Vector3 newRotation = this.gameObject.transform.rotation.eulerAngles;
            newRotation.z = 315.0f;
            this.gameObject.transform.eulerAngles = newRotation;
        }
    }

    public void move(float mousePos)
    {
        if (soundEngine != null)
        {
            if (soundEngine.getDrive() == false)
            {
                soundEngine.playSoundEffect("roverStart");
                soundEngine.setDrive(true);
            }
            soundEngine.playSoundEffect("roverDrive");
        }
        if (mousePos > Screen.width / 2) moveRight();
        if (mousePos < Screen.width / 2) moveLeft();
    }

    public void moveLeft()
    {
        anim.SetBool("isMoving", true);
        if (this.gameObject.rigidbody.velocity.x > -maxSpeed)
        {
            float vX = this.gameObject.rigidbody.velocity.x - accelerationSpeed * Time.deltaTime;
            if (vX < -maxSpeed) vX = -maxSpeed;
            this.gameObject.rigidbody.velocity = new Vector3(vX, this.gameObject.rigidbody.velocity.y, this.gameObject.rigidbody.velocity.z);
        }
        if (this.getDirection() == "Right")
        {
            this.gameObject.transform.eulerAngles = new Vector3(-this.gameObject.transform.eulerAngles.x, 180.0f, -this.gameObject.transform.eulerAngles.z);
        }
        setDirection("Left");
    }

    public void moveRight()
    {
        anim.SetBool("isMoving", true);
        if (this.gameObject.rigidbody.velocity.x < maxSpeed)
        {
            float vX = this.gameObject.rigidbody.velocity.x + accelerationSpeed * Time.deltaTime;
            if (vX > maxSpeed) vX = maxSpeed;
            this.gameObject.rigidbody.velocity = new Vector3(vX, this.gameObject.rigidbody.velocity.y, this.gameObject.rigidbody.velocity.z);
        }
        if (this.getDirection() == "Left")
        {
            this.gameObject.transform.eulerAngles = new Vector3(-this.gameObject.transform.eulerAngles.x, 0.0f, -this.gameObject.transform.eulerAngles.z);
        }
        setDirection("Right");
    }

    public void brake()
    {
        if (!isJumping)
        {
            float vx = this.gameObject.rigidbody.velocity.x;
            if (Mathf.Abs(vx) > 0.01f) particleScript.playParticle("driveDust", Mathf.Abs(vx));
            if (vx > 0.10f)
            {
                float vX = this.gameObject.rigidbody.velocity.x;
                vX -= 5.0f * Time.deltaTime;
                this.gameObject.rigidbody.velocity = new Vector3(vX, this.rigidbody.velocity.y, this.rigidbody.velocity.z);
            }
            else if (vx < -0.10f)
            {
                float vX = this.gameObject.rigidbody.velocity.x;
                vX += 5.0f * Time.deltaTime;
                this.gameObject.rigidbody.velocity = new Vector3(vX, this.rigidbody.velocity.y, this.rigidbody.velocity.z);
            }
            else if (vx > -0.10f && vx < 0.10f)
            {
                if (vx != 0.0f)
                {
                    float vX = this.rigidbody.velocity.x;
                    vX = 0.0f;
                    this.gameObject.rigidbody.velocity = new Vector3(vX, this.rigidbody.velocity.y, this.rigidbody.velocity.z);
                    anim.SetBool("isMoving", false);
                    if (soundEngine != null)
                    {
                        if (soundEngine.getDrive() == true)
                        {
                            soundEngine.playSoundEffect("roverStop");
                            soundEngine.setDrive(false);
                        }
                    }
                }
            }
        }
        else particleScript.playParticle("driveDust", 0.0f);
    }

    public void jump()
    {
        if (!isJumping)
        {
            this.gameObject.rigidbody.velocity = new Vector3(this.gameObject.rigidbody.velocity.x, jumpForce, this.gameObject.rigidbody.velocity.z);
            isJumping = true;
            this.gameObject.rigidbody.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionZ;
            gameLogic.decreaseBatteryBy(3.0f);

            if (soundEngine != null)
            {
                soundEngine.playSoundEffect("jump");
            }
            if (particleScript != null)
            {
                particleScript.playParticle("jumpDust");
                particleScript.playParticle("engineJump");
            }
        }
    }

    public void chargeShot()
    {
        lineRenderer.enabled = true;

        if (soundEngine != null)
        {
            soundEngine.playSoundEffect("aim");
            soundEngine.setAim(true);
        }

        y0 = this.gameObject.transform.position.y;

        if (increasing)
        {
            vx += 3.0f * Time.deltaTime;
            vy += 2.25f * Time.deltaTime;
        }
        else
        {
            vx -= 3.0f * Time.deltaTime;
            vy -= 2.25f * Time.deltaTime;
        }
        v = vx * vx + vy * vy;

        //formula for max dist: d = (v*v*sin(2*angle)) / gravity
        //replaced sin(2*angle), which is .96, with a bigger number due to the trajectory not starting on the ground.
        if (vx < 1.5f)
        {
            increasing = true;
            vx = 1.5f;
            vy = 1.125f;
        }
        if (vx > 8.8f)
        {
            increasing = false;
        }

        x8 = v * 1.15f / 9.81f;
        x1 = x8 / 8.0f;
        x2 = 2.0f * x8 / 8.0f;
        x3 = 3.0f * x8 / 8.0f;
        x4 = 4.0f * x8 / 8.0f;
        x5 = 5.0f * x8 / 8.0f;
        x6 = 6.0f * x8 / 8.0f;
        x7 = 7.0f * x8 / 8.0f;

        //trajectory function: Y = Y0 + tan(angle) * X - (gravity*x*x)/(2*v*v*cos(angle)*cos(angle))
        float y1 = (y0 + x1 * 0.75f) - (9.81f * x1 * x1) / (1.28f * v) + 1;
        float y2 = (y0 + x2 * 0.75f) - (9.81f * x2 * x2) / (1.28f * v) + 1;
        float y3 = (y0 + x3 * 0.75f) - (9.81f * x3 * x3) / (1.28f * v) + 1;
        float y4 = (y0 + x4 * 0.75f) - (9.81f * x4 * x4) / (1.28f * v) + 1;
        float y5 = (y0 + x5 * 0.75f) - (9.81f * x5 * x5) / (1.28f * v) + 1;
        float y6 = (y0 + x6 * 0.75f) - (9.81f * x6 * x6) / (1.28f * v) + 1;
        float y7 = (y0 + x7 * 0.75f) - (9.81f * x7 * x7) / (1.28f * v) + 1;
        float y8 = (y0 + x8 * 0.75f) - (9.81f * x8 * x8) / (1.28f * v) + 1;
        //0 = y0 + x8 * tanAngle - (g * x8 * x8) / (2 * (v * v * cosAngle * cosAngle)), cosAngle = 0.6f, tanAngle = 0.75f, g = -9.81f

        if (getDirection() == "Left")
        {
            x1 = -x1;
            x2 = -x2;
            x3 = -x3;
            x4 = -x4;
            x5 = -x5;
            x6 = -x6;
            x7 = -x7;
            x8 = -x8;

            lineRenderer.SetPosition(0, new Vector3(this.gameObject.transform.position.x - 2, y0 + 1, this.gameObject.transform.position.z));
            lineRenderer.SetPosition(1, new Vector3(this.gameObject.transform.position.x - 2 + x1, y1, this.gameObject.transform.position.z));
            lineRenderer.SetPosition(2, new Vector3(this.gameObject.transform.position.x - 2 + x2, y2, this.gameObject.transform.position.z));
            lineRenderer.SetPosition(3, new Vector3(this.gameObject.transform.position.x - 2 + x3, y3, this.gameObject.transform.position.z));
            lineRenderer.SetPosition(4, new Vector3(this.gameObject.transform.position.x - 2 + x4, y4, this.gameObject.transform.position.z));
            lineRenderer.SetPosition(5, new Vector3(this.gameObject.transform.position.x - 2 + x5, y5, this.gameObject.transform.position.z));
            lineRenderer.SetPosition(6, new Vector3(this.gameObject.transform.position.x - 2 + x6, y6, this.gameObject.transform.position.z));
            lineRenderer.SetPosition(7, new Vector3(this.gameObject.transform.position.x - 2 + x7, y7, this.gameObject.transform.position.z));
            lineRenderer.SetPosition(8, new Vector3(this.gameObject.transform.position.x - 2 + x8, y8, this.gameObject.transform.position.z));
        }
        else
        {
            lineRenderer.SetPosition(0, new Vector3(this.gameObject.transform.position.x + 2, y0 + 1, this.gameObject.transform.position.z));
            lineRenderer.SetPosition(1, new Vector3(this.gameObject.transform.position.x + 2 + x1, y1, this.gameObject.transform.position.z));
            lineRenderer.SetPosition(2, new Vector3(this.gameObject.transform.position.x + 2 + x2, y2, this.gameObject.transform.position.z));
            lineRenderer.SetPosition(3, new Vector3(this.gameObject.transform.position.x + 2 + x3, y3, this.gameObject.transform.position.z));
            lineRenderer.SetPosition(4, new Vector3(this.gameObject.transform.position.x + 2 + x4, y4, this.gameObject.transform.position.z));
            lineRenderer.SetPosition(5, new Vector3(this.gameObject.transform.position.x + 2 + x5, y5, this.gameObject.transform.position.z));
            lineRenderer.SetPosition(6, new Vector3(this.gameObject.transform.position.x + 2 + x6, y6, this.gameObject.transform.position.z));
            lineRenderer.SetPosition(7, new Vector3(this.gameObject.transform.position.x + 2 + x7, y7, this.gameObject.transform.position.z));
            lineRenderer.SetPosition(8, new Vector3(this.gameObject.transform.position.x + 2 + x8, y8, this.gameObject.transform.position.z));
        }
    }

    public void resetShot()
    {
        vx = 0.0f;
        vy = 0.0f;
        x8 = 1.15f;
        lineRenderer.enabled = false;
    }

    public IEnumerator shoot(int shroomType)
    {
        if (soundEngine != null) soundEngine.setAim(false);

        if (!gameLogic.getInfiniteAmmo())
        {
            if (shroomType == 0) gameLogic.decreaseNormalSeeds();
            else if (shroomType == 1) gameLogic.decreaseBumpySeeds();
        }

        //create new seed
        GameObject newSeed = null;
        if (getDirection() == "Right")
        {
            newSeed = Instantiate(Resources.Load("Prefabs/Seed")) as GameObject;
            newSeed.transform.position = this.gameObject.transform.position + new Vector3(2, 1, 0);
        }
        else
        {
            newSeed = Instantiate(Resources.Load("Prefabs/Seed")) as GameObject;
            newSeed.transform.position = this.gameObject.transform.position - new Vector3(2, -1, 0);
        }
        newSeed.gameObject.name = "Seed";
        newSeed.gameObject.transform.parent = GameObject.Find("SeedContainer").gameObject.transform;
        SeedBehaviour seedBehaviour = newSeed.gameObject.GetComponent("SeedBehaviour") as SeedBehaviour;
        seedBehaviour.setShroomType(shrooms[shroomType]);

        if (soundEngine != null)
        {
            soundEngine.playSoundEffect("shoot");
        }

        if (getDirection() == "Left") vx = -vx;
        newSeed.rigidbody.velocity = new Vector3(vx, vy, 0.0f);

        yield return new WaitForSeconds(1.5f);	//wait for 1.5f sec to reset the shot, removing the arc
        resetShot();
    }

    public void flash()
    {
        RaycastHit hit;
        Vector3 direction = Vector3.zero;
        int layerMask = 1 << 8;
        layerMask = ~layerMask;
        if (getDirection() == "Right") direction = new Vector3(1.0f, 0.0f, 0.0f);
        else if (getDirection() == "Left") direction = new Vector3(-1.0f, 0.0f, 0.0f);
        if (Physics.Raycast(this.gameObject.transform.position, direction, out hit, 15.0f, layerMask))
        {
            if (hit.collider.gameObject.name == "Slug")
            {
                SlugScript slugScript = hit.collider.gameObject.GetComponent("SlugScript") as SlugScript;
                slugScript.toFleeState();
            }
        }
        if (flashBool == false)
        {
            flashlight.SetActive(true);
            flashBool = true;
            gameLogic.decreaseBatteryBy(5.0f);
            if (soundEngine != null)
            {
                soundEngine.playSoundEffect("flash");
            }
        }
    }

    public void addCrystal(GameObject crystal)
    {
        crystals.Add(crystal);
    }

    private void setDirection(string direction)
    {
        if (lastDirection != direction)
        {
            lastDirection = direction;
        }
    }

    private string getDirection()
    {
        return lastDirection;
    }

    public void bounceShroomY()
    {
        this.gameObject.rigidbody.velocity = new Vector3(0.0f, 15.0f, this.gameObject.rigidbody.velocity.z);
    }

    public void bounceShroomX()
    {
        float velocity = this.gameObject.rigidbody.velocity.x;
        if (Mathf.Abs(velocity) < 0.4f)
        {
            if (velocity < 0.0f) velocity = -3.0f;
            else if (velocity > 0.0f) velocity = 3.0f;
        }

        velocity *= -4.0f;
        if (velocity > 10.0f)
        {
            velocity = 10.0f;
        }
        else if (velocity < -10.0f)
        {
            velocity = -10.0f;
        }
        this.gameObject.rigidbody.velocity = new Vector3(velocity, this.gameObject.rigidbody.velocity.y, this.gameObject.rigidbody.velocity.z);
    }

    //check the battery status and change the texture if needed
    private void checkBatteryStatus()
    {
        float maxBattery = gameLogic.getBatteryCapacity();
        float currentBatteryPower = gameLogic.getBattery();

        int amountOfBatteryBars = Mathf.RoundToInt(currentBatteryPower / (maxBattery / 10));

        if (amountOfBatteryBars >= 6)
        {
            if (currentRoverTexture != highBatteryRoverTexture)
            {
                currentRoverTexture = highBatteryRoverTexture;
                currentMaterial.SetTexture(0, highBatteryRoverTexture);
            }
        }
        else if (amountOfBatteryBars >= 4 && amountOfBatteryBars <= 5)
        {
            if (currentRoverTexture != yellowBatteryRoverTexture)
            {
                currentRoverTexture = yellowBatteryRoverTexture;
                currentMaterial.SetTexture(0, yellowBatteryRoverTexture);
            }
        }

        else if (amountOfBatteryBars <= 3)
        {
            if (currentRoverTexture != redBatteryRoverTexture)
            {
                currentRoverTexture = redBatteryRoverTexture;
                currentMaterial.SetTexture(0, redBatteryRoverTexture);
            }
        }
    }
}