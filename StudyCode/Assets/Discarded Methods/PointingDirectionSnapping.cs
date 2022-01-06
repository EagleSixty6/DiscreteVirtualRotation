/*
 * 
 * Short explanation:
 * The user points the right Oculus Touch controller 
 * to the left or right side of their FOV.
 * (in between 20°-90° in both directions, higher or lower
 *  values will not lead to a rotation)
 * By pressing the right index trigger button,
 * the virtual body is rotated a certain amount in that direction.
 * 
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PointingDirectionSnapping : RotationMethod
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
    private bool rotating;

    // Start is called before the first frame update
    void Start()
    {
        methodText.text = GetType().Name + "\nPoint left/right and use the right Index Trigger to snap rotate";
        angleSlider.gameObject.SetActive(true);
        angleText.gameObject.SetActive(true);
        angleValue.gameObject.SetActive(true);

        delaySlider.gameObject.SetActive(true);
        delayText.gameObject.SetActive(true);
        delayValue.gameObject.SetActive(true);

        durationSlider.gameObject.SetActive(true);
        durationText.gameObject.SetActive(true);
        durationValue.gameObject.SetActive(true);

        scopeSlider.gameObject.SetActive(false);
        scopeText.gameObject.SetActive(false);
        scopeValue.gameObject.SetActive(false);

        speedSlider.gameObject.SetActive(false);
        speedText.gameObject.SetActive(false);
        speedValue.gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        methodText.text = GetType().Name + "\nPoint left/right and use the right Index Trigger to snap rotate";
        angleSlider.gameObject.SetActive(true);
        angleText.gameObject.SetActive(true);
        angleValue.gameObject.SetActive(true);

        delaySlider.gameObject.SetActive(true);
        delayText.gameObject.SetActive(true);
        delayValue.gameObject.SetActive(true);

        durationSlider.gameObject.SetActive(true);
        durationText.gameObject.SetActive(true);
        durationValue.gameObject.SetActive(true);

        scopeSlider.gameObject.SetActive(false);
        scopeText.gameObject.SetActive(false);
        scopeValue.gameObject.SetActive(false);

        speedSlider.gameObject.SetActive(false);
        speedText.gameObject.SetActive(false);
        speedValue.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        //Rotation
        if (OVRInput.Get(OVRInput.Button.SecondaryIndexTrigger))
        {
            float blickrichtung = centerCam.transform.rotation.eulerAngles.y;
            float zeigerichtung = rightHand.transform.eulerAngles.y;
            float winkel = Quaternion.Angle(Quaternion.Euler(0, blickrichtung, 0), Quaternion.Euler(0, zeigerichtung, 0));

            if (20 < winkel && winkel < 90)
            {
                if(Vector3.Cross(centerCam.transform.rotation * Vector3.forward, rightHand.transform.rotation * Vector3.forward).y < 0)
                {
                    StartRotation(-1);
                }
                else
                {
                    StartRotation(1);
                }
            }         
        }
    }


    private IEnumerator Rotate(Vector3 angles, float duration)
    {
        rotating = true;
        Quaternion startRotation = transform.rotation;
        Quaternion endRotation = Quaternion.Euler(angles) * startRotation;
        for (float t = 0; t < duration; t += Time.deltaTime)
        {
            transform.rotation = Quaternion.Lerp(startRotation, endRotation, t / duration);
            yield return null;
        }
        transform.rotation = endRotation;
        yield return new WaitForSeconds(delaySlider.value - duration);
        rotating = false;
    }

    public void StartRotation(int sign)
    {

        if (!rotating)
        {
            StartCoroutine(Rotate(new Vector3(0, sign * angleSlider.value, 0), durationSlider.value));
        }
    }
}
