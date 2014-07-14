using UnityEngine;
using System.Collections;

public class StartButton : ButtonBehaviour
{
    public override void executeButton()
    {
        //animate the menu
        menu.animateMenu(button.nameOfButton);
    }

    public override void Update()
    {
        //if the menu animation is done
        if (menu.isAnimationDone && menu.menuAnimationButton == button.nameOfButton)
        {
            menu.switchMenuState("DifficultyMenu");
            menu.setRoverAnimation("levelBool", true);
            menu.isAnimationDone = false;
            menu.menuAnimationButton = "";
        }
    }
}
