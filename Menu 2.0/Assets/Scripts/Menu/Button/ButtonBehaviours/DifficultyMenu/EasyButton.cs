using UnityEngine;
using System.Collections;

public class EasyButton : ButtonBehaviour
{
    public override void executeButton()
    {
        //we only have 1 level for now just use the loadHeimLevel
        menu.difficultySetting = "Easy";
        menu.sound.changeMusic("Easy");
        menu.switchMenuState("LoadingLevel");
        menu.loadHeimLevel();
        
        //if (menu.isHeimBuild)
        //{
        //    //heim build makes use of 1 level
        //    menu.switchMenuState("LoadingLevel");
        //    menu.loadHeimLevel();
        //}
        //else
        //{

        //}
    }
}
