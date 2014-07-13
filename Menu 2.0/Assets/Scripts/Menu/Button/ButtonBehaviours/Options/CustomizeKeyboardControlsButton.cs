using UnityEngine;
using System.Collections;

public class CustomizeKeyboardControlsButton : ButtonBehaviour
{
    public override void executeButton()
    {
        menu.switchMenuState("SetKeyboardControls");
    }

    public override void Update()
    {
        if(menu.selectedInputType == 1)
        {
            if(!button.isEnabled) button.isEnabled = true;
        }
        else
        {
            if (button.isEnabled) button.isEnabled = false;
        }
    }
}
