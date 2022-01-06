/*
 * 
 * Short explanation:
 * The user points the right Oculus Touch controller 
 * to the left or right side of their FOV.
 * (in between 20°-90° in both directions, higher or lower
 *  values will not lead to a rotation)
 * By holding down the right index trigger button,
 * the virtual body is rotated at a certain speed in that direction.
 * 
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PointingContinuousRotation : RotationMethod
{
    public Slider angleSlider;
    public Text angleText;
    public Text angleValue;

    public Slider delaySlider;
    public Text delayText;
    public Text delayValue;

    public Slider durationSlider;
    public Text durationText;
    public Text durationValue;

    public Slider scopeSlider;
    public Text scopeText;
    public Text scopeValue;

    public Slider speedSlider;
    public Text speedText;
    public Text speedValue;

    public GameObject rightHand;
    public GameObject centerCam;
    public Text methodText;

    // Start is called before the first frame update
    void Start()
    {
        methodText.text = GetType().Name + "\nPoint left/right and use the right Index Trigger to continuously rotate";
        angleSlider.gameObject.SetActive(false);
        angleText.gameObject.SetActive(false);
        angleValue.gameObject.SetActive(false);

        delaySlider.gameObject.SetActive(false);
        delayText.gameObject.SetActive(false);
        delayValue.gameObject.SetActive(false);

        durationSlider.gameObject.SetActive(false);
        durationText.gameObject.SetActive(false);
        durationValue.gameObject.SetActive(false);

        scopeSlider.gameObject.SetActive(false);
        scopeText.gameObject.SetActive(false);
        scopeValue.gameObject.SetActive(false);

        speedSlider.gameObject.SetActive(true);
        speedText.gameObject.SetActive(true);
        speedValue.gameObject.SetActive(true);

    }

    void OnEnable()
    {
        methodText.text = GetType().Name + "\nPoint left/right and use the right Index Trigger to continuously rotate";
        angleSlider.gameObject.SetActive(false);
        angleText.gameObject.SetActive(false);
        angleValue.gameObject.SetActive(false);

        delaySlider.gameObject.SetActive(false);
        delayText.gameObject.SetActive(false);
        delayValue.gameObject.SetActive(false);

        durationSlider.gameObject.SetActive(false);
        durationText.gameObject.SetActive(false);
        durationValue.gameObject.SetActive(false);

        scopeSlider.gameObject.SetActive(false);
        scopeText.gameObject.SetActive(false);
        scopeValue.gameObject.SetActive(false);

        speedSlider.gameObject.SetActive(true);
        speedText.gameObject.SetActive(true);
        speedValue.gameObject.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        //Rotation
        if (OVRInput.Get(OVRInput.Axis1D.SecondaryIndexTrigger) > 0.1)
        {
            float blickrichtung = centerCam.transform.rotation.eulerAngles.y;
            float zeigerichtung = rightHand.transform.eulerAngles.y;
            float winkel = Quaternion.Angle(Quaternion.Euler(0, blickrichtung, 0), Quaternion.Euler(0, zeigerichtung, 0));

            if (20 < winkel && winkel < 90)
            {
                if (Vector3.Cross(centerCam.transform.rotation * Vector3.forward, rightHand.transform.rotation * Vector3.forward).y < 0)
                {
                    transform.Rotate(0, -1* speedSlider.value * Time.deltaTime, 0);
                }
                else
                {
                    transform.Rotate(0, speedSlider.value * Time.deltaTime, 0);
                }
            }
        }
    }
}
