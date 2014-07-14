using UnityEngine;
using System.Collections;

public class RightArrowButton : ButtonBehaviour
{
    public override void executeButton()
    {
        button.increaseIterator();
        menu.selectedInputType = button.iterator;
        menu.changedSettings = true;
    }
}
