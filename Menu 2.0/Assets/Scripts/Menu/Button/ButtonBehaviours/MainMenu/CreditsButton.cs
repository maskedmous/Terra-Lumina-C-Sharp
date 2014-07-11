using UnityEngine;
using System.Collections;

public class CreditsButton : ButtonBehaviour
{
    public override void executeButton()
    {
        menu.switchMenuState("Credits");
        menu.setRoverAnimation("creditsBool", true);
    }
}
