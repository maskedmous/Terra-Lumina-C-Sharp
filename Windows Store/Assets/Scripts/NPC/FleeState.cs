﻿using UnityEngine;
using System.Collections;

public class FleeState : State
{

    public override void update()
    {
        toWaitState();
    }

    private void toWaitState()
    {
        float zero = 0.0f;
        parent.rigidbody.velocity = new Vector3(zero, parent.rigidbody.velocity.y, parent.rigidbody.velocity.z);
        parent.collider.enabled = false;
        parent.rigidbody.useGravity = false;
        parent.rigidbody.isKinematic = true;
        parentScript.toWaitState();
    }
}