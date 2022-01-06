using System;
using System.IO;
using System.Collections;
using UnityEngine;

public class Joystick_Discrete_Selection_Local : RotationMethod
{
    public float delay;

    public GameObject rotationSphere;

    private bool waiting;

    private float joystickX;
    private float joystickY;
    private float rotationAngle;

    public Main mainScript;
    public Task1 task1Script;
    public Task2 task2Script;

    // Start is called before the first frame update
    void Start()
    {
        delay = 0.416f;
        waiting = false;
        mainScript = GetComponent<Main>();
        task1Script = GetComponent<Task1>();
        task2Script = GetComponent<Task2>();
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
            joystickX = Math.Abs(OVRInput.Get(OVRInput.RawAxis2D.RThumbstick).x);
            joystickY = Math.Abs(OVRInput.Get(OVRInput.RawAxis2D.RThumbstick).y);
            rotationAngle = (Vector2.SignedAngle(OVRInput.Get(OVRInput.RawAxis2D.RThumbstick), Vector2.up) + 360) % 360;
            Debug.Log(rotationAngle);
        }
        else if (mainScript.mainHand.Equals("left"))
        {
            joystickX = Math.Abs(OVRInput.Get(OVRInput.RawAxis2D.LThumbstick).x);
            joystickY = Math.Abs(OVRInput.Get(OVRInput.RawAxis2D.LThumbstick).y);
            rotationAngle = (Vector2.SignedAngle(OVRInput.Get(OVRInput.RawAxis2D.LThumbstick), Vector2.up) + 360) % 360;
        }

        // Rotation
        if (!waiting && (joystickX > 0.2 || joystickY > 0.2) && mainScript.rotationPossible)
        {
            StartCoroutine(Rotate(rotationAngle));
        }
        
        if (joystickX < 0.2 && joystickY < 0.2)
        {
            StopAllCoroutines();
            waiting = false;
        }
    }

    private IEnumerator Rotate(float rotationAngle)
    {
        waiting = true;

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
            mainScript.t1PlayerRotations += transform.rotation.eulerAngles.y + "; ";

        }
        else
        {
            transform.RotateAround(rotationSphere.transform.position, Vector3.up, rotationAngle);
        }

        yield return new WaitForSeconds(delay);

        waiting = false;
    }
}
