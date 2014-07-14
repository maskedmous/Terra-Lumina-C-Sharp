using UnityEngine;
using System.Collections;

public class LeftArrowButton : ButtonBehaviour
{
    public override void executeButton()
    {
        button.decreaseIterator();
        menu.selectedInputType = button.iterator;
        menu.changedSettings = true;
    }
}
