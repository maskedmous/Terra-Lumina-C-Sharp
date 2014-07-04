using UnityEngine;
using System.Collections;

public class SlugScript : MonoBehaviour
{


    private State currentState = null;
    private FleeState fleeState = null;
    private MoveState moveState = null;
    private WaitState waitState = null;
    private ChaseState chaseState = null;
    private ReturnState returnState = null;

    private Vector3 startPosition = Vector3.zero;
    public GameObject slugBoundA;
    public GameObject slugBoundB;

    public string difficulty = "";

    private Animator anim = null;

    public void Awake()
    {
        fleeState = this.gameObject.AddComponent("FleeState") as FleeState;
        moveState = this.gameObject.AddComponent("MoveState") as MoveState;
        waitState = this.gameObject.AddComponent("WaitState") as WaitState;

        currentState = moveState;

        anim = this.gameObject.GetComponent("Animator") as Animator;
    }

    public void Start()
    {
        startPosition = this.gameObject.transform.position;
    }

    public void Update()
    {
        Vector3 myPos = this.gameObject.transform.position;
        if (myPos.x > slugBoundA.transform.position.x && myPos.x > slugBoundB.transform.position.x) resetPosition();
        else if (myPos.x < slugBoundA.transform.position.x && myPos.x < slugBoundB.transform.position.x) resetPosition();

        currentState.update();
    }

    public State getState()
    {
        return currentState;
    }

    public void toFleeState()
    {
        if (currentState == moveState || currentState == chaseState || currentState == returnState) currentState = fleeState;
        anim.SetBool("isMoving", false);
    }

    public void toMoveState()
    {
        currentState = moveState;
        anim.SetBool("isMoving", true);
    }

    public void toWaitState()
    {
        currentState = waitState;
    }

    public void toReturnState()
    {
        currentState = returnState;
    }

    public void toChaseState()
    {
        currentState = chaseState;
    }

    private void resetPosition()
    {
        this.gameObject.transform.position = startPosition;
    }

    public string getDifficulty()
    {
        return difficulty;
    }

    public bool isWaitState()
    {
        if (currentState == waitState)
        {
            return true;
        }

        return false;
    }

    public void OnCollisionEnter(Collision collision)
    {
        string name = collision.collider.gameObject.name;
        if (name.Contains("Wheel") || name == "Player")
        {
            if (collision.collider.gameObject.transform.position.x > this.gameObject.transform.position.x)
            {
                currentState.bouncePlayer("right");
            }
            else
            {
                currentState.bouncePlayer("left");
            }
        }
    }

    public GameObject getSlugBoundA()
    {
        return slugBoundA;
    }

    public void setSlugBoundA(GameObject boundA)
    {
        slugBoundA = boundA;
    }

    public GameObject getSlugBoundB()
    {
        return slugBoundB;
    }

    public void setSlugBoundB(GameObject boundB)
    {
        slugBoundB = boundB;
    }

    public Vector3 getStart()
    {
        return startPosition;
    }
}