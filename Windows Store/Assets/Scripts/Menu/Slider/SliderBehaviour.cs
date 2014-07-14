using UnityEngine;
using System.Collections;

public class SliderBehaviour : MonoBehaviour
{
    protected Menu menu = null;
    protected Slider slider = null;

    public void Awake()
    {
        menu = GameObject.Find("Menu").GetComponent<Menu>();
    }

    public void initializeSlider(Slider sliderScript)
    {
        slider = sliderScript;
        initializeSetting();
    }

    public virtual void initializeSetting()
    {

    }

    public virtual void executeSliderFunction(Vector2 input)
    {
        Debug.LogError("WRONG BEHAVIOUR SLIDER");
    }
}
