using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Leaning_Discrete_Selection : RotationMethod
{
    public GameObject centerEyeAnchor;
    public GameObject pointOfReference;
    public GameObject cameraRig;
    public GameObject rotationSphere;

    public bool waiting;

    public Main mainScript;
    public Task1 task1Script;
    public Task2 task2Script;

    public GameObject laserPointer;


    // Start is called before the first frame update
    void Start()
    {
        mainScript = GetComponent<Main>();
        task1Script = GetComponent<Task1>();
        task2Script = GetComponent<Task2>();

        waiting = true;
    }
    void OnEnable()
    {
        waiting = true;
    }

    void Update()
    {
        Vector3 pORPos = transform.InverseTransformPoint(pointOfReference.transform.position);

        Vector3 headProjectionOnFloor = new Vector3(pORPos.x, 0, pORPos.z);

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

        float leaningAngle = Vector3.SignedAngle(Vector3.forward, headProjectionOnFloor, Vector3.up);
        float rotationAngle = leaningAngle - yawDif;

        // Rotation
        if (!waiting && mainScript.rotationPossible)
        {
            if (ContinueRotation(leaningAngle, pORPos.x, pORPos.z))
            {
                Rotate(rotationAngle);
            }
        }
        else
        {
            if (pORPos.x < 0.1f && pORPos.x > -0.1f && pORPos.z < 0.05f && pORPos.z > -0.05f)
            {
                waiting = false;
            }
        }
    }

    private void Rotate(float rotationAngle)
    {
        waiting = true;

        laserPointer.SetActive(false);

        if (task1Script.testRunning && !task1Script.training)
        {
            mainScript.t1PlayerRotations += transform.rotation.eulerAngles.y + "; ";
            transform.RotateAround(rotationSphere.transform.position, Vector3.up, rotationAngle);
            mainScript.t1PlayerRotations += transform.rotation.eulerAngles.y + "; ";
        }
        else if (task2Script.testRunning && !task2Script.training)
        {
            mainScript.t2PlayerRotations += transform.rotation.eulerAngles.y + "; ";
            transform.RotateAround(rotationSphere.transform.position, Vector3.up, rotationAngle);
            mainScript.t2PlayerRotations += transform.rotation.eulerAngles.y + "; ";
        }
        else
        {
            transform.RotateAround(rotationSphere.transform.position, Vector3.up, rotationAngle);
        }

        StartCoroutine(LaserWait());
    }

    private bool ContinueRotation(float angle, float x, float y)
    {
        float a = -0.15f;
        float b = 0.15f;
        float c = 0.15f;
        float d = -0.1f;

        if (angle < -90)
        {
            return Math.Pow(x, 2) / Math.Pow(a, 2) + Math.Pow(y, 2) / Math.Pow(d, 2) >= 1;
        }
        else if (angle < 0)
        {
            return Math.Pow(x, 2) / Math.Pow(a, 2) + Math.Pow(y, 2) / Math.Pow(b, 2) >= 1;
        }
        else if (angle < 90)
        {
            return Math.Pow(x, 2) / Math.Pow(c, 2) + Math.Pow(y, 2) / Math.Pow(b, 2) >= 1;
        }
        else
        {
            return Math.Pow(x, 2) / Math.Pow(c, 2) + Math.Pow(y, 2) / Math.Pow(d, 2) >= 1;
        }
    }
    private IEnumerator LaserWait()
    {
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        laserPointer.SetActive(true);
    }
}
