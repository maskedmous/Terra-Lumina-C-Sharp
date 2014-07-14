using UnityEngine;
using System.Collections;

public class AcceptChangesButton : ButtonBehaviour
{
    public override void executeButton()
    {
        //save the settings
        menu.saveSettings();
        //time out timer is false
        menu.settingsTimeout = false;
        //disable the overlay graphic
        menu.disableGraphicTexture("AcceptSettingsBackground");
        //
        menu.disableTextBox("RevertCountDown");
        //re-enable the options menu
        menu.enableAllButtons();
    }

    public override void Update()
    {
        if(menu.settingsTimeout)
        {
            if (button.isDisabled) button.isDisabled = false;   //enable functionality of the button
            if (!button.isEnabled) button.isEnabled = true;     //enable rendering of the button
        }
        else
        {
            if (!button.isDisabled) button.isDisabled = true;   //disable functionality of the button
            if (button.isEnabled) button.isEnabled = false;     //disable rendering of the button
        }
    }
}
