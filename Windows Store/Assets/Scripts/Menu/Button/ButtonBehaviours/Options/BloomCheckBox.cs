using UnityEngine;
using System.Collections;

public class BloomCheckBox : ButtonBehaviour
{
    public override void executeButton()
    {
        //if bloom is enabled currently
        if (menu.bloom)
        {
            //disable it
            menu.bloom = false;
            //set texture to unchecked
            button.switchCheckBox(false);
            menu.changedSettings = true;
        }
        else
        {
            //enable it
            menu.bloom = true;
            //set texture to checked
            button.switchCheckBox(true);
            menu.changedSettings = true;
        }
    }

    public override void Update()
    {
        if(menu.bloom && !button.isEnabledBox)
        {
            button.switchCheckBox(true);
        }
        else if(!menu.bloom && button.isEnabledBox)
        {
            button.switchCheckBox(false);
        }
    }
}
