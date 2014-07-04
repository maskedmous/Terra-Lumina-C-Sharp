using UnityEngine;
using System.Collections;

public class ReturnState : State
{

    private Vector3 startPosition = Vector3.zero;

    public void Start()
    {
        startPosition = parentScript.getStart();
    }

    public override void update()
    {
        Vector3 parentPosition = parent.transform.position;
        if (parentPosition.x < startPosition.x) speed = 70.0f;
        else if (parentPosition.x > startPosition.x) speed = -70.0f;
        parent.rigidbody.velocity = new Vector3(Time.deltaTime * speed, parent.rigidbody.velocity.y, parent.rigidbody.velocity.z);
        if (Vector3.Distance(parentPosition, startPosition) < 1.0f)
        {
            parentScript.toMoveState();
        }
    }
}