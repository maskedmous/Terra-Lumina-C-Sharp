using UnityEngine;
using System.Collections;

public class BackToMainMenuButton : ButtonBehaviour
{
    public override void executeButton()
    {
        menu.switchMenuState("MainMenu");
        menu.setRoverAnimation("levelBool", false);
        menu.setRoverAnimation("settingsBool", false);
        menu.setRoverAnimation("creditsBool", false);
    }
}
