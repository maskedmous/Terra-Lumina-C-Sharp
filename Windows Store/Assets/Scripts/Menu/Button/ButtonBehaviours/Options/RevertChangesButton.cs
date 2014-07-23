using UnityEngine;
using System.Collections;

public class RevertChangesButton : ButtonBehaviour
{
    public override void executeButton()
    {
        menu.loadSettings();
        //time out timer is false
        menu.settingsTimeout = false;
        //disable the overlay graphic
        menu.disableGraphicTexture("AcceptSettingsBackground");
        //
        menu.disableTextBox("RevertCountDown");
        //re-enable the options menu
        menu.enableAllButtons();
        //
        button.iterator = menu.selectedInputType;
    }

    public override void Update()
    {
        if(menu.settingsTimeout)
        {
            if (button.isDisabled) button.isDisabled = false;   //enable functionality of the button
            if (!button.isEnabled) button.isEnabled = true;     //enable rendering of the button

            //decrease timer
            menu.settingsTimeoutTimer -= Time.deltaTime;
            //set timer text box
            button.getTextbox.textBoxText = Mathf.RoundToInt(menu.settingsTimeoutTimer).ToString();
            
            //if the timer has passed then revert settings!
            if(menu.settingsTimeoutTimer <= 0.0f)
            {
                //reload settings
                menu.loadSettings();
                //time out timer is false
                menu.settingsTimeout = false;
                //disable the overlay graphic
                menu.disableGraphicTexture("AcceptSettingsBackground");
                menu.disableTextBox("RevertCountDown");
                //re-enable the options menu
                menu.enableAllButtons();
                button.iterator = menu.selectedInputType;
                menu.initalizeInput();
            }
        }
        else
        {
            if (!button.isDisabled) button.isDisabled = true;   //disable functionality of the button
            if (button.isEnabled) button.isEnabled = false;     //disable rendering of the button
        }
    }
}
