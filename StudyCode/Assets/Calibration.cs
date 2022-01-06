using System;
using System.Collections.Generic;
using UnityEngine;

public class Calibration : MonoBehaviour
{
    public float speed;
    public bool audioPlayable;
    public bool firstCalibrationDone;

    public GameObject centerEyeAnchor;
    public GameObject rotationSphere;

    public bool rotationPossible;
    private bool calibrationRecordEnabled;
    private List<Vector3> hmdPositions;
    private List<Vector3> hmdForwards;
    private float samplingFrequency;
    private float samplingTimer;

    // Scripts
    public Main mainScript;

    // Audio Source
    public AudioSource audioSource;

    public Vector3 calibratedCenter;

    // Start is called before the first frame update
    void Start()
    {
        mainScript = GetComponent<Main>();

        audioPlayable = false;
        firstCalibrationDone = false;

        rotationPossible = false;
        calibrationRecordEnabled = false;
        samplingFrequency = 0.1f;
    }

    // Update is called once per frame
    void Update()
    {
        if (calibrationRecordEnabled)
        {
            RecordFrameForCalibration();
        }
        else
        {
            if (firstCalibrationDone && ((mainScript.mainHand.Equals("right") && OVRInput.Get(OVRInput.RawAxis2D.RThumbstick).y < -0.1) || (mainScript.mainHand.Equals("left") && OVRInput.Get(OVRInput.RawAxis2D.LThumbstick).y < -0.1)))
            {
                float newPosZ = (rotationSphere.transform.localPosition - Vector3.forward * speed).z;

                if (newPosZ > -1)
                {
                    rotationSphere.transform.localPosition += new Vector3(0, 0, newPosZ - rotationSphere.transform.localPosition.z);
                    audioPlayable = true;
                }
                else
                {
                    audioPlayable = false;
                }
            }
            else if (firstCalibrationDone && ((mainScript.mainHand.Equals("right") && OVRInput.Get(OVRInput.RawAxis2D.RThumbstick).y > 0.1) || (mainScript.mainHand.Equals("left") && OVRInput.Get(OVRInput.RawAxis2D.LThumbstick).y > 0.1)))
            {
                float newPosZ = (rotationSphere.transform.localPosition + Vector3.forward * speed).z;

                if (newPosZ < 1)
                {
                    rotationSphere.transform.localPosition += new Vector3(0, 0, newPosZ - rotationSphere.transform.localPosition.z);
                    audioPlayable = true;
                }
                else
                {
                    audioPlayable = false;
                }
            }
        }
        if (audioPlayable && Math.Abs(rotationSphere.transform.localPosition.z) > 0.99f)
        {
            audioSource.Play();
        }
    }

    private void RecordFrameForCalibration()
    {
        samplingTimer -= Time.deltaTime;
        if (samplingTimer < 0)
        {
            hmdPositions.Add(centerEyeAnchor.transform.position);
            hmdForwards.Add(centerEyeAnchor.transform.forward);
            samplingTimer = samplingFrequency;
        }
    }

    public void StartCalibration()
    {
        Debug.Log("Started calibration...");
        audioSource.Play();
        rotationPossible = false;
        calibrationRecordEnabled = true;
        samplingTimer = samplingFrequency;
        hmdPositions = new List<Vector3>();
        hmdForwards = new List<Vector3>();
    }

    public void EndCalibration()
    {
        StopCenterOfRotationCalibration();
        Debug.Log("Finished calibration...");
        audioSource.Play();
        mainScript.calibrationText.text = "Use the thumbstick on your main controller to change your center of rotation by pushing the thumbstick forward or backward.\nTurn your head to rotate and try to find the center of rotation you think feels the most pleasant.\n\"Calibrate\": calibrate again\n\"Reset\": reset your position\n\"Done\": end the study";
        rotationPossible = true;
        firstCalibrationDone = true;
    }

    private void StopCenterOfRotationCalibration()
    {
        calibrationRecordEnabled = false;
        int firstSample = hmdPositions.Count / 4;
        int secondSample = firstSample * 3;

        Plane saggitalPlane = new Plane();
        saggitalPlane.SetNormalAndPosition(centerEyeAnchor.transform.right, centerEyeAnchor.transform.position);

        float distanceToPlane;
        Ray ray = new Ray(hmdPositions[firstSample], -hmdForwards[firstSample]);
        saggitalPlane.Raycast(ray, out distanceToPlane);
        Vector3 firstTarget = ray.GetPoint(distanceToPlane);

        ray = new Ray(hmdPositions[secondSample], -hmdForwards[secondSample]);
        saggitalPlane.Raycast(ray, out distanceToPlane);
        Vector3 secondTarget = ray.GetPoint(distanceToPlane);

        Vector3 centerOfYawRotationGlobal = (firstTarget + secondTarget) / 2f;

        rotationSphere.transform.position = centerOfYawRotationGlobal;

        calibratedCenter = rotationSphere.transform.localPosition;

        rotationSphere.transform.position += transform.forward * 0.5f;
        rotationSphere.transform.position -= transform.forward * ((GetComponent<Main>().userID / 1000) - 1) * 0.25f;


    }
}
