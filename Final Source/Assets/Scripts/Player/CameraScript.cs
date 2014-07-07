using UnityEngine;

public class CameraScript : MonoBehaviour
{

    private GameObject target = null;
    private bool move = true;

    private float thisZ = 0.0f;

    void Start()
    {
        target = GameObject.Find("Player") as GameObject;
    }

    void Update()
    {
        if (move)
        {
            thisZ = this.gameObject.transform.position.z;
            this.gameObject.transform.position = new Vector3(target.transform.position.x, target.transform.position.y + 5.0f, thisZ);
        }
    }

    public void setMove(bool value)
    {
        move = value;
    }
}