using UnityEngine;

public class CameraScript : MonoBehaviour
{
    enum cameraState { none, cameraStatic, cameraFront, cameraBehind, cameraClose, cameraFree, cameraYClipDisabled, cameraCrystal }
    cameraState currentCameraState = cameraState.cameraFree;
    private GameObject target = null;

    public bool randomCamera = false;
    public bool cameraStatic = true;
    public bool cameraFront = false;
    public bool cameraBehind = false;
    public bool cameraClose = false;
    public bool cameraFree = false;
    public bool cameraYClipDisabled = false;
    public bool cameraFoilage = false;
    public bool cameraCrystal = false;
    public bool cameraRightToLeft = false;

    private float appropriateZAxis = 0.0f;

    private float randomCameraTimer = 10.0f;

    public GameObject crystalTarget = null;
    private bool busyAnimation = false;

    void Start()
    {
        target = GameObject.Find("Player") as GameObject;
        appropriateZAxis = this.gameObject.transform.position.z;
    }

    void Update()
    {
        if(randomCamera)
        {
            randomCameraTimer -= Time.deltaTime;
            if(randomCameraTimer <= 0.0f)
            {
                randomCameraState();
                randomCameraTimer = 10.0f;
            }
        }
        checkCamera();
        switch (currentCameraState)
        {
            case (cameraState.cameraStatic):
                this.gameObject.transform.position = new Vector3(target.transform.position.x, target.transform.position.y + 5.0f, 3.0f);
                if(this.gameObject.transform.eulerAngles != new Vector3(12.5f, 0.0f, 0.0f)) this.gameObject.transform.eulerAngles = new Vector3(12.5f, 0.0f, 0.0f);
                break;
            case (cameraState.cameraFront):
                this.gameObject.transform.position = new Vector3(target.transform.position.x + 3, target.transform.position.y + 2.0f, 15.0f);
                if (this.gameObject.transform.eulerAngles != new Vector3(29.0f, -90.0f, 0.0f)) this.gameObject.transform.eulerAngles = new Vector3(29.0f, -90.0f, 0.0f);
                break;
            case (cameraState.cameraBehind):
                this.gameObject.transform.position = new Vector3(target.transform.position.x - 3, target.transform.position.y + 2.0f, 15.0f);
                if (this.gameObject.transform.eulerAngles != new Vector3(22.0f, 90.0f, 0.0f)) this.gameObject.transform.eulerAngles = new Vector3(22.0f,90.0f,0.0f);
                break;
            case (cameraState.cameraClose):
                this.gameObject.transform.position = new Vector3(target.transform.position.x, target.transform.position.y + 2.0f, 13.0f);
                if (this.gameObject.transform.eulerAngles != new Vector3(34.5f, 0.0f, 0.0f)) this.gameObject.transform.eulerAngles = new Vector3(34.5f, 0.0f, 0.0f);
                break;
            case (cameraState.cameraFree):
                //don't clip to rover
                break;
            case (cameraState.cameraYClipDisabled):
                this.gameObject.transform.position = new Vector3(target.transform.position.x, this.gameObject.transform.position.y, appropriateZAxis);
                if(this.gameObject.transform.eulerAngles != new Vector3(12.5f, 0.0f, 0.0f)) this.gameObject.transform.eulerAngles = new Vector3(12.5f, 0.0f, 0.0f);
                break;
            case (cameraState.cameraCrystal):
                crystalCloseUpAnimation();
                break;
        }
    }

    private void crystalCloseUpAnimation()
    {
        if (!busyAnimation)
        {
            this.gameObject.transform.position = new Vector3(crystalTarget.transform.position.x - 2.0f, crystalTarget.transform.position.y, crystalTarget.transform.position.z - 3);
            this.gameObject.transform.eulerAngles = new Vector3(0.0f, 90.0f, 0);
            busyAnimation = true;
        }
        else if (busyAnimation)
        {
            if (!cameraRightToLeft)
            {
                this.transform.LookAt(crystalTarget.transform);
                transform.Translate(Vector3.right * Time.deltaTime);
            }
            else
            {
                this.transform.LookAt(crystalTarget.transform);
                transform.Translate(Vector3.left * Time.deltaTime);
            }
        }
    }

    private void randomCameraState()
    {
        int randomNumber = Mathf.RoundToInt(Random.Range(1.0f, 4.0f));
        
        if (randomNumber == 1) currentCameraState = cameraState.cameraStatic;
        if (randomNumber == 2) currentCameraState = cameraState.cameraFront;
        if (randomNumber == 3) currentCameraState = cameraState.cameraBehind;
        if (randomNumber == 4) currentCameraState = cameraState.cameraClose;

    }

    private void checkCamera()
    {
        if (cameraStatic)
        {
            currentCameraState = cameraState.cameraStatic;
        }
        else if (cameraFront)
        {
            currentCameraState = cameraState.cameraFront;
        }
        else if (cameraBehind)
        {
            currentCameraState = cameraState.cameraBehind;
        }
        else if (cameraClose)
        {
            currentCameraState = cameraState.cameraClose;
        }
        else if(cameraFree)
        {
            currentCameraState = cameraState.cameraFree;
        }
        else if (cameraYClipDisabled)
        {
            currentCameraState = cameraState.cameraYClipDisabled;
        }
        else if (cameraCrystal)
        {
            currentCameraState = cameraState.cameraCrystal;
        }
        else currentCameraState = cameraState.none;
    }

    public void setCameraStatic(bool value)
    {
        cameraStatic = value;
    }
}