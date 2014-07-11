using UnityEngine;
using System.Collections;

public class OptionsButton : ButtonBehaviour
{
    public override void executeButton()
    {
        menu.switchMenuState("Options");
        menu.setRoverAnimation("settingsBool", true);
        if (!menu.isHeimBuild) menu.loadSettings();
    }
}