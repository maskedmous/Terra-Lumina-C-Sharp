using UnityEngine;
using System.Collections;

public class QuitButton : ButtonBehaviour
{
    public override void executeButton()
    {
        //animate the menu
        menu.animateMenu(button.nameOfButton);
        menu.setRoverAnimation("exitBool", true);
    }
    public override void Update()
    {
        if (menu.isAnimationDone && menu.menuAnimationButton == button.nameOfButton)
        {
            menu.menuAnimationButton = "";
            Application.Quit();
        }
    }
}
