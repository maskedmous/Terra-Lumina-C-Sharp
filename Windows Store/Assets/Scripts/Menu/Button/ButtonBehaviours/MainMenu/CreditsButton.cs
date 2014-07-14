using UnityEngine;
using System.Collections;

public class CreditsButton : ButtonBehaviour
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
            menu.switchMenuState("Credits");
            menu.setRoverAnimation("creditsBool", true);
            menu.isAnimationDone = false;
            menu.menuAnimationButton = "";
        }
    }
}
