using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Joystick_Discrete_Selection_Global : RotationMethod
{
    public GameObject hand;

    public GameObject rotationSphere;
    public GameObject cameraRig;
    public GameObject centerEyeAnchor;

    public bool waiting;

    private float joystickX;
    private float joystickY;
    private float joystickDirection;

    public Main mainScript;
    public Task1 task1Script;
    public Task2 task2Script;

    public GameObject laserPointer;

    // Start is called before the first frame update
    void Start()
    {
        waiting = false;
        mainScript = GetComponent<Main>();
        task1Script = GetComponent<Task1>();
        task2Script = GetComponent<Task2>();
    }
    void OnEnable()
    {
        waiting = false;
    }

    void Update()
    {
        if (mainScript.mainHand.Equals("right"))
        {
            joystickX = Math.Abs(OVRInput.Get(OVRInput.RawAxis2D.RThumbstick).x);
            joystickY = Math.Abs(OVRInput.Get(OVRInput.RawAxis2D.RThumbstick).y);
            joystickDirection = (Vector2.SignedAngle(OVRInput.Get(OVRInput.RawAxis2D.RThumbstick), Vector2.up) + 360) % 360;
        }
        else if (mainScript.mainHand.Equals("left"))
        {
            joystickX = Math.Abs(OVRInput.Get(OVRInput.RawAxis2D.LThumbstick).x);
            joystickY = Math.Abs(OVRInput.Get(OVRInput.RawAxis2D.LThumbstick).y);
            joystickDirection = (Vector2.SignedAngle(OVRInput.Get(OVRInput.RawAxis2D.LThumbstick), Vector2.up) + 360) % 360;
        }

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

        float pointingDirection = hand.transform.eulerAngles.y;

        float rotationAngle = (pointingDirection + joystickDirection - yawDif - transform.rotation.eulerAngles.y + 360) % 360;

        // Rotation
        if (!waiting && (joystickX > 0.2 || joystickY > 0.2) && GetComponent<Main>().rotationPossible)
        {
            Rotate(rotationAngle);
        }

        if (joystickX < 0.2 && joystickY < 0.2)
        {
            waiting = false;
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
    private IEnumerator LaserWait()
    {
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        laserPointer.SetActive(true);
    }
}
