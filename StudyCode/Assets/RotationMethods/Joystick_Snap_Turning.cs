using System;
using System.IO;
using System.Collections;
using UnityEngine;

public class Joystick_Snap_Turning : RotationMethod
{
    public float angle;
    public float delay;

    public GameObject rotationSphere;

    public bool waiting;

    private float joystickX;

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

        angle = 11.25f;
        delay = 0.416f;
        waiting = false;
    }
    void OnEnable()
    {
        waiting = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (mainScript.mainHand.Equals("right"))
        {
            joystickX = OVRInput.Get(OVRInput.RawAxis2D.RThumbstick).x;
        }
        if (mainScript.mainHand.Equals("left"))
        {
            joystickX = OVRInput.Get(OVRInput.RawAxis2D.LThumbstick).x;
        }

        //Rotate left
        if (!waiting && joystickX < -0.1 && mainScript.rotationPossible)
        {
            StartCoroutine(Rotate(-1 * angle));
        }
        //Rotate right
        if (!waiting && joystickX > 0.1 && mainScript.rotationPossible)
        {
            StartCoroutine(Rotate(angle));
        }

        if (Math.Abs(joystickX) < 0.1)
        {
            StopAllCoroutines();
            waiting = false;
        }
    }

    private IEnumerator Rotate(float rotationAngle)
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

        yield return new WaitForSeconds(delay);

        waiting = false;
    }
    private IEnumerator LaserWait()
    {
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        laserPointer.SetActive(true);
    }
}
