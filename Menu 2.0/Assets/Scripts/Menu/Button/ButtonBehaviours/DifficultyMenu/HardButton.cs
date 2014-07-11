using UnityEngine;
using System.Collections;

public class HardButton : ButtonBehaviour
{
    public override void executeButton()
    {
        //we only have 1 level for now just use the loadHeimLevel
        menu.difficultySetting = "Hard";
        menu.sound.changeMusic("Hard");
        menu.switchMenuState("LoadingLevel");
        menu.loadHeimLevel();
    }
}
