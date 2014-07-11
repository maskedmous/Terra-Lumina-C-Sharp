using UnityEngine;
using System.Collections;

public class QuitButton : ButtonBehaviour
{
    public override void executeButton()
    {
        menu.setRoverAnimation("exitBool", true);
        Application.Quit();
    }
}
