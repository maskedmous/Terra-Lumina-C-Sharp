using UnityEngine;
using System.Collections;

public class OptionsButton : ButtonBehaviour
{
    public override void executeButton()
    {
        //animate the menu
        menu.animateMenu(button.nameOfButton);
    }
    public override void Update()
    {
        if (menu.isAnimationDone && menu.menuAnimationButton == button.nameOfButton)
        {
            menu.switchMenuState("Options");
            menu.setRoverAnimation("settingsBool", true);
            if (!menu.isHeimBuild) menu.loadSettings();
            menu.isAnimationDone = false;
            menu.menuAnimationButton = "";
        }
    }
}