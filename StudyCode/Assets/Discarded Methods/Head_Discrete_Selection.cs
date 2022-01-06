/*
 * 
 * Short explanation:
 * After saving the positions for sitting upright,
 * leaning left, right, forward and backward, rotate
 * by leaning in the direction you want to rotate to
 * 
*/

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Head_Discrete_Selection : RotationMethod
{
    public Slider delaySlider;

    public Slider scopeSlider;

    public Text methodText;

    public GameObject head;
    public GameObject headSphere;
    public GameObject bodyArrowEnd;
    public GameObject rotationSphere;
    public GameObject temp;

    private Vector3[] positions;
    private int index;
    private bool calibrating;
    private bool waiting;
    private bool hold;

    public Text test;

    // Start is called before the first frame update
    void Start()
    {
        methodText.text = "Calibrating:\nSit with your head upright and press the right shoulder button.";
        positions = new Vector3[5];
        index = 0;
        calibrating = true;
        waiting = false;
        hold = false;
    }
    void OnEnable()
    {
        methodText.text = "Calibrating:\nSit with your head upright and press the right shoulder button.";
        positions = new Vector3[5];
        index = 0;
        calibrating = true;
        waiting = false;
        hold = false;
    }

    void Update()
    {
        // Getting upright positions and comfortable left and right leaning posiitons
        if (OVRInput.GetDown(OVRInput.RawButton.RIndexTrigger) && calibrating)
        {
            if(index < 5)
            {
                positions[index] = headSphere.transform.position;
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
                methodText.text = "Calibrating:\nTilt your head down and press the right shoulder button.";
            }
            if (index == 4)
            {
                methodText.text = "Calibrating:\nTilt your head up and press the right shoulder button.";
            }
            if (index == 5)
            {
                methodText.text = "Calibration complete. Press the right shoulder button to continue.";
            }
        }
        if (index > 5 && OVRInput.GetDown(OVRInput.RawButton.RIndexTrigger))
        {
            methodText.text = "Tilt your head in the direction you want to rotate to.\nPress the right thumbstick to calibrate again.";
            index = 0;
            calibrating = false;
        }
        // Reset positions
        if (OVRInput.Get(OVRInput.RawButton.RThumbstick))
        {
            methodText.text = "Calibrating:\nSit with your head upright and press the right shoulder button.";
            positions = new Vector3[5];
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

        Vector3 headSpherePos = headSphere.transform.position;
        float currentHeadPosX = headSpherePos.x;
        float currentHeadPosZ = headSpherePos.z;

        Vector3 bodyArrowEndPos = bodyArrowEnd.transform.position;
        Vector3 headSphereProjection = Vector3.ProjectOnPlane(headSpherePos, Vector3.up);
        float rotationAngle = Vector3.SignedAngle(bodyArrowEndPos, headSphereProjection, Vector3.up);

        float a = positions[1].x;
        float b = positions[3].z;
        float c = positions[2].x;
        float d = positions[4].z;

        test.text = "hSP: " + headSpherePos.ToString("0.000")
                    + "\n" + rotationAngle
                    + "\n1: " + (Math.Pow(currentHeadPosX, 2) / Math.Pow(a, 2) + Math.Pow(currentHeadPosZ, 2) / Math.Pow(d, 2)+scopeSlider.value)
                    + "\n2: " + (Math.Pow(currentHeadPosX, 2) / Math.Pow(a, 2) + Math.Pow(currentHeadPosZ, 2) / Math.Pow(b, 2)+scopeSlider.value)
                    + "\n3: " + (Math.Pow(currentHeadPosX, 2) / Math.Pow(c, 2) + Math.Pow(currentHeadPosZ, 2) / Math.Pow(b, 2)+scopeSlider.value)
                    + "\n4: " + (Math.Pow(currentHeadPosX, 2) / Math.Pow(c, 2) + Math.Pow(currentHeadPosZ, 2) / Math.Pow(d, 2)+scopeSlider.value)
                    + (!calibrating && !waiting && !hold);
        

        // Rotation
        if (!calibrating && !waiting && !hold)
        {
            

            if (ContinueRotation(rotationAngle, currentHeadPosX, currentHeadPosZ))
            {
                StartCoroutine(Rotate(rotationAngle));
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

    private bool ContinueRotation(float angle, float x, float y)
    {
        float a = positions[1].x;
        float b = positions[3].z;
        float c = positions[2].x;
        float d = positions[4].z;

        if(angle < -90)
        {
            return Math.Pow(x, 2) / Math.Pow(a, 2) + Math.Pow(y, 2) / Math.Pow(d, 2) + scopeSlider.value >= 2;
        }
        else if(angle < 0)
        {
            return Math.Pow(x, 2) / Math.Pow(a, 2) + Math.Pow(y, 2) / Math.Pow(b, 2) + scopeSlider.value >= 2;
        }
        else if(angle < 90)
        {
            return Math.Pow(x, 2) / Math.Pow(c, 2) + Math.Pow(y, 2) / Math.Pow(b, 2) + scopeSlider.value >= 2;
        }
        else
        {
            return Math.Pow(x, 2) / Math.Pow(c, 2) + Math.Pow(y, 2) / Math.Pow(d, 2) + scopeSlider.value >= 2;
        }
    }
}
