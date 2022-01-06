/*
 * 
 * Short explanation:
 * The Index Triggers of both Oculus Touch controllers
 * determines the direction in which the virtual body
 * is rotated by a certain amount.
 * Pressing the left trigger will rotate left,
 * pressing the right trigger will rotate right.
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IndexTriggerSnapping : RotationMethod
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

    public Text methodText;
    private bool rotating;

    // Start is called before the first frame update
    void Start()
    {
        methodText.text = GetType().Name + "\nUse the Index Triggers to snap rotate";
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

    void OnEnable()
    {
        methodText.text = GetType().Name + "\nUse the Index Triggers to snap rotate";
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
        if (OVRInput.Get(OVRInput.Button.PrimaryIndexTrigger))
        {
            StartRotation(-1);
        }
        if (OVRInput.Get(OVRInput.Button.SecondaryIndexTrigger))
        {
            StartRotation(1);
        }

    }

    private IEnumerator Rotate(Vector3 angles, float duration)
    {
        rotating = true;
        Quaternion startRotation = transform.rotation;
        Quaternion endRotation = Quaternion.Euler(angles) * startRotation;
        for(float t=0; t<duration; t += Time.deltaTime)
        {
            transform.rotation = Quaternion.Lerp(startRotation, endRotation, t / duration);
            yield return null;
        }
        transform.rotation = endRotation;
        yield return new WaitForSeconds(delaySlider.value-duration);
        rotating = false;
    }

    public void StartRotation(int sign)
    {

        if (!rotating)
        {
            StartCoroutine(Rotate(new Vector3(0, sign*angleSlider.value, 0), durationSlider.value));
        }
    }
}
