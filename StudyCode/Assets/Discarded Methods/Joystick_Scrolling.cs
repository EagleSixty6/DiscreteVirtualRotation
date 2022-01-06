/*
 * 
 * Short explanation:
 * The right joystick determines direction in
 * which the virtual body is rotated at a
 * certain speed. Flicking left will rotate left,
 * flicking right will rotate right. 
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Joystick_Scrolling : RotationMethod
{
    public Slider angleSlider;
    public Text angleText;
    public Text angleValue;

    public Slider delaySlider;
    public Text delayText;
    public Text delayValue;

    public Slider scopeSlider;
    public Text scopeText;
    public Text scopeValue;

    public Slider speedSlider;
    public Text speedText;
    public Text speedValue;

    public Button automaticButton;
    public Text automaticText;

    public Button instantButton;
    public Text instantText;

    public Text methodText;

    // Start is called before the first frame update
    void Start()
    {
        methodText.text = "Use the right joystick (flick left/right) to scroll rotate";
        angleSlider.gameObject.SetActive(false);
        angleText.gameObject.SetActive(false);
        angleValue.gameObject.SetActive(false);

        delaySlider.gameObject.SetActive(false);
        delayText.gameObject.SetActive(false);
        delayValue.gameObject.SetActive(false);

        scopeSlider.gameObject.SetActive(false);
        scopeText.gameObject.SetActive(false);
        scopeValue.gameObject.SetActive(false);

        speedSlider.gameObject.SetActive(true);
        speedText.gameObject.SetActive(true);
        speedValue.gameObject.SetActive(true);

        automaticButton.gameObject.SetActive(false);
        automaticText.gameObject.SetActive(false);

        instantButton.gameObject.SetActive(false);
        instantText.gameObject.SetActive(false);
    }
        void OnEnable()
    {
        methodText.text = "Use the right joystick (flick left/right) to scroll rotate";
        angleSlider.gameObject.SetActive(false);
        angleText.gameObject.SetActive(false);
        angleValue.gameObject.SetActive(false);

        delaySlider.gameObject.SetActive(false);
        delayText.gameObject.SetActive(false);
        delayValue.gameObject.SetActive(false);

        scopeSlider.gameObject.SetActive(false);
        scopeText.gameObject.SetActive(false);
        scopeValue.gameObject.SetActive(false);

        speedSlider.gameObject.SetActive(true);
        speedText.gameObject.SetActive(true);
        speedValue.gameObject.SetActive(true);

        automaticButton.gameObject.SetActive(false);
        automaticText.gameObject.SetActive(false);

        instantButton.gameObject.SetActive(false);
        instantText.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        //Rotate left
        if (OVRInput.Get(OVRInput.RawAxis2D.RThumbstick).x < -0.1)
        {
            transform.Rotate(0, -1* speedSlider.value * Time.deltaTime, 0);
        }
        //Rotate right
        if (OVRInput.Get(OVRInput.RawAxis2D.RThumbstick).x > 0.1)
        {
            transform.Rotate(0, speedSlider.value * Time.deltaTime, 0);
        }
    }
}
