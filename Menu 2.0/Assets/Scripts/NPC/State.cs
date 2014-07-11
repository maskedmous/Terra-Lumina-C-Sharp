using UnityEngine;
using System.Collections;

public class State : MonoBehaviour
{

    protected GameObject parent = null;
    protected SlugScript parentScript = null;

    protected GameObject target = null;

    protected float speed = 70.0f;

    protected int layerMask = 0;

    public void Awake()
    {
        parent = this.gameObject;
        parentScript = this.gameObject.GetComponent("SlugScript") as SlugScript;

        target = GameObject.Find("Player") as GameObject;

        layerMask = 1 << 8;
        layerMask = ~layerMask;
    }

    public virtual void update()
    {

    }

    public void bouncePlayer(string direction)
    {
        if (direction == "right")
        {
            Vector3 bounceRight = target.rigidbody.velocity;
            bounceRight.y = 15.0f;
            target.rigidbody.velocity = bounceRight;
        }
        else if (direction == "left")
        {
            Vector3 bounceLeft = target.rigidbody.velocity;
            bounceLeft.x = -15.0f;
            target.rigidbody.velocity = bounceLeft;
        }
    }
}