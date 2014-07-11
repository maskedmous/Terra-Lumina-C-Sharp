using UnityEngine;
using System.Collections;

public class FunFactScript : MonoBehaviour
{

    private GameObject label = null;
    private LabelScript labelScript = null;

    void Awake()
    {
        label = (GameObject)Instantiate(Resources.Load("Prefabs/Label")) as GameObject;
        label.gameObject.name = "Label";
        labelScript = label.GetComponent("LabelScript") as LabelScript;
    }

    public void displayFact()
    {
        labelScript.displayFact();
    }

    public void stopDisplay()
    {
        labelScript.stopDisplay();
    }
}