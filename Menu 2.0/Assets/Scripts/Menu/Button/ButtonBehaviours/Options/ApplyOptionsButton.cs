using UnityEngine;
using System.Collections;

public class ApplyOptionsButton : ButtonBehaviour
{
    public override void executeButton()
    {
        menu.changedSettings = false;
        menu.enableGraphicTexture("AcceptSettingsBackground");
        menu.enableTextBox("RevertCountDown");
        menu.disableAllButtons();   //disable background buttons
        //set the timer to 10 to revert settings after 10 seconds
        menu.settingsTimeoutTimer = 10.0f;
        //set the timer to count down
        menu.settingsTimeout = true;
    }

    public override void Update()
    {
        if(menu.changedSettings && !button.isEnabled)
        {
            button.isEnabled = true;
        }
        else if(!menu.changedSettings && button.isEnabled)
        {
            button.isEnabled = false;
        }
    }
}
