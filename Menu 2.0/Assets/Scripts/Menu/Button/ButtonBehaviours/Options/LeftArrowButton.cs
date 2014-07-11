using UnityEngine;
using System.Collections;

public class LeftArrowButton : ButtonBehaviour
{
    public override void executeButton()
    {
        button.decreaseIterator();
    }
}
