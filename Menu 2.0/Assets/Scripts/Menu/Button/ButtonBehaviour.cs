using UnityEngine;
using System.Collections;

public class ButtonBehaviour : MonoBehaviour
{
    protected Menu menu = null;
    protected Button button = null;

    public void initialize(Button aButton)
    {
        menu = GameObject.Find("Menu").GetComponent<Menu>();
        button = aButton;
    }

    public virtual void Update()
    {

    }

    //executes the button
    public virtual void executeButton()
    {
        Debug.Log("WRONG BEHAVIOUR");
    }
}
