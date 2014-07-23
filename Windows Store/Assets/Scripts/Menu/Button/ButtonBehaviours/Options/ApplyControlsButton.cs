using UnityEngine;
using System.Collections;

public class ApplyControlsButton : ButtonBehaviour
{
    public override void executeButton()
    {
        //if button is pressed then save and go back to options menu
        menu.saveSettings();
        menu.loadSettings();
        menu.applyKeyboardSettingsToKeys();
        menu.switchMenuState("Options");
        button.isEnabled = false;
    }

    public override void Update()
    {
        //look if there are changes
        if (menu.hasKeyboardChanges())
        {
            //if there are changes enable this button
            if (!button.isEnabled) button.isEnabled = true;
        }
        else
        {
            //if there are aren't any changes disable this button
            if (button.isEnabled) button.isEnabled = false;
        }
    }
}
