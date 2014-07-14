using UnityEngine;
using System.Collections;

public class MediumButton : ButtonBehaviour
{
    public override void executeButton()
    {
        //we only have 1 level for now just use the loadHeimLevel
        menu.difficultySetting = "Medium";
        menu.sound.changeMusic("Medium");
        menu.switchMenuState("LoadingLevel");
        menu.loadHeimLevel();
    }
}
