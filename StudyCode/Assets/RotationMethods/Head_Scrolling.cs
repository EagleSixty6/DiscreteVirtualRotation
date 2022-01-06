using System;
using UnityEngine;

public class Head_Scrolling : RotationMethod1
{
    public GameObject centerEyeAnchor;
    public GameObject cameraRig;
    public GameObject rotationSphere;

    public float rotationSpeed;

    public Calibration calibrationScript;

    // Start is called before the first frame update
    void Start()
    {
        calibrationScript = GetComponent<Calibration>();
    }

    void Update()
    {
        if (calibrationScript.rotationPossible)
        {
            float yawDif = centerEyeAnchor.transform.localRotation.eulerAngles.y + cameraRig.transform.localRotation.eulerAngles.y;
            if (yawDif > 360 || yawDif < -360)
            {
                yawDif = yawDif % 360;
            }
            if (yawDif > 180)
            {
                yawDif -= 360;
            }
            else if (yawDif < -180)
            {
                yawDif += 360;
            }

            //Rotate
            if (Math.Abs(yawDif) > 10)
            {
                transform.RotateAround(rotationSphere.transform.position, Vector3.up, rotationSpeed * (float)Math.Exp(Math.Abs(yawDif / 30)) * yawDif * Time.deltaTime);
            }
        }
    }
}
