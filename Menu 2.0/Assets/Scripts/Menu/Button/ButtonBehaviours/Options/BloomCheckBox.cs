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
        }

        else
        {
            //enable it
            menu.bloom = true;
            //set texture to checked
            button.switchCheckBox(true);
        }
    }
}
