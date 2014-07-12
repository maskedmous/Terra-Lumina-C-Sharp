using UnityEngine;
using System.Collections;

public class CustomizeKeyboardControlsButton : ButtonBehaviour
{
    public override void executeButton()
    {
        menu.switchMenuState("SetKeyboardControls");
    }
}
