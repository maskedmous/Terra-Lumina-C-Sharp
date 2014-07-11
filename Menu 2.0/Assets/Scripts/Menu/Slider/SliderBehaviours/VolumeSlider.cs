using UnityEngine;
using System.Collections;

public class VolumeSlider : SliderBehaviour
{
    public override void executeSliderFunction(Vector2 input)
    {
        //put the slider on the X position
        float xPosition = (input.x / slider.sliderScale.x) - (slider.sliderWidth / 2.0f);

        //clip it's position
        if (xPosition < slider.minRange - (slider.sliderWidth * slider.sliderScale.x)) xPosition = slider.minRange - (slider.sliderWidth / 2);
        if (xPosition > slider.maxRange - (slider.sliderWidth * slider.sliderScale.x)) xPosition = slider.maxRange - (slider.sliderWidth / 2);

        slider.sliderButtonX = xPosition;
        
        //calculate the position on the slider
        float percentageOfSlider = slider.percentageOfSlider();
        menu.changeVolume(percentageOfSlider);
    }
}
