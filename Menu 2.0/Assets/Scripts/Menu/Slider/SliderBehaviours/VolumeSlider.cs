using UnityEngine;
using System.Collections;

public class VolumeSlider : SliderBehaviour
{
    public override void initializeSetting()
    {
        //only non-heim builds
        if(!menu.isHeimBuild)
        {
            if(PlayerPrefs.HasKey("Volume"))
            {
                slider.setPosition(PlayerPrefs.GetFloat("Volume"));
            }
        }
        else
        {
            slider.setPosition(menu.sound.getVolume());
        }
    }
    public override void executeSliderFunction(Vector2 input)
    {
        //put the slider on the X position
        float xPosition = (input.x / slider.sliderScale.x) - (slider.sliderWidth / 2.0f);

        //clip it's position
        if (xPosition < slider.minRange) xPosition = slider.minRange;
        if (xPosition > slider.maxRange) xPosition = slider.maxRange;

        slider.sliderButtonX = xPosition;
        
        //calculate the position on the slider
        float percentageOfSlider = slider.percentageOfSlider();
        menu.changeVolume(percentageOfSlider);
    }
}
