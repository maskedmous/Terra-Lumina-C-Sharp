using UnityEngine;
using System.Collections;

public class StartButton : ButtonBehaviour
{
    public override void executeButton()
    {
        menu.switchMenuState("DifficultyMenu");
        menu.setRoverAnimation("levelBool", true);
    }
}
