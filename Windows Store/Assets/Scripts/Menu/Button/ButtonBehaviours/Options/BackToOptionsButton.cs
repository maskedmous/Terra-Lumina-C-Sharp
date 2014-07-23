using UnityEngine;
using System.Collections;

public class BackToOptionsButton : ButtonBehaviour
{
    public override void executeButton()
    {
        menu.switchMenuState("Options");
        menu.loadSettings();
        menu.isChangingKey = false;
    }
}
