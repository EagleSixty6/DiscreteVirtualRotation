/*
 * 
 * Short explanation:
 * After saving the positions for sitting upright, leaning left and right, 
 * snap rotate by leaning left or right
 * 
*/

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Head_Roll_Snap_Turning : RotationMethod
{
    public Slider angleSlider;

    public Slider delaySlider;

    public Slider scopeSlider;

    public Text methodText;

    public GameObject head;
    public GameObject rotationSphere;
    public GameObject temp;

    private Vector3[] rotations;
    private int index;
    private bool calibrating;
    private bool waiting;
    private bool hold;

    public Text test;

    // Start is called before the first frame update
    void Start()
    {
        methodText.text = "Calibrating:\nSit with your head upright and press the right shoulder button.";
        rotations = new Vector3[3];
        index = 0;
        calibrating = true;
        waiting = false;
        hold = false;
        scopeSlider.value = 0f;
    }
    void OnEnable()
    {
        methodText.text = "Calibrating:\nSit with your head upright and press the right shoulder button.";
        rotations = new Vector3[3];
        index = 0;
        calibrating = true;
        waiting = false;
        hold = false;
        scopeSlider.value = 0f;
    }

    void Update()
    {
        // Getting upright position and comfortable left and right leaning positions
        if (OVRInput.GetDown(OVRInput.RawButton.RIndexTrigger) && calibrating)
        {
            if (index < 3)
            {
                rotations[index] = head.transform.rotation.eulerAngles;
            }
            index++;
            if (index == 1)
            {
                methodText.text = "Calibrating:\nTilt your head left and press the right shoulder button.";
            }
            if (index == 2)
            {
                methodText.text = "Calibrating:\nTilt your head right and press the right shoulder button.";
            }
            if (index == 3)
            {
                methodText.text = "Calibration complete. Press the right shoulder button to continue.";
            }
        }
        if (index > 3 && OVRInput.GetDown(OVRInput.RawButton.RIndexTrigger))
        {
            methodText.text = "Tilt your head left and right to snap rotate.\nPress the right thumbstick to calibrate again.";
            index = 0;
            calibrating = false;
        }
        // Reset positions, restart calibration
        if (OVRInput.Get(OVRInput.RawButton.RThumbstick))
        {
            methodText.text = "Calibrating:\nSit with your head upright and press the right shoulder button.";
            rotations = new Vector3[3];
            calibrating = true;
        }

        if (OVRInput.GetDown(OVRInput.RawButton.RIndexTrigger) && !calibrating)
        {
            hold = true;
        }
        if (OVRInput.GetUp(OVRInput.RawButton.RIndexTrigger) && !calibrating)
        {
            hold = false;
        }

        Vector3 currentHeadRot = head.transform.localRotation.eulerAngles;
        float currentHeadRotZ = currentHeadRot.z;
        if(currentHeadRotZ > 180)
        {
            currentHeadRotZ = currentHeadRotZ - 360;
        }
        float rotThresholdLeft = rotations[1].z - 10*scopeSlider.value;
        float rotThresholdRight = rotations[2].z + 10*scopeSlider.value - 360;

        test.text = "thresholds: " + rotThresholdLeft + ", " + rotThresholdRight
                + "\ncurrent Z: " + currentHeadRotZ;

        if (!calibrating && !waiting && !hold)
        {
            if(currentHeadRotZ > rotThresholdLeft)
            {
                StartCoroutine(Rotate(-1*angleSlider.value));
            }
            else if (currentHeadRotZ < rotThresholdRight)
            {
                StartCoroutine(Rotate(angleSlider.value));
            }
        }

    }

    private IEnumerator Rotate(float rotationAngle)
    {
        waiting = true;

        transform.RotateAround(rotationSphere.transform.position, Vector3.up, rotationAngle);

        yield return new WaitForSeconds(delaySlider.value);

        waiting = false;
    }

}
