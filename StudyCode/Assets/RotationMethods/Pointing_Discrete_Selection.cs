using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Pointing_Discrete_Selection : RotationMethod
{
    public bool noPointerOverButton;

    public GameObject hand;

    public GameObject rotationSphere;
    public GameObject cameraRig;
    public GameObject centerEyeAnchor;

    public GameObject laserPointer;

    public Main mainScript;
    public Task1 task1Script;
    public Task2 task2Script;

    // Start is called before the first frame update
    void Start()
    {
        mainScript = GetComponent<Main>();
        task1Script = GetComponent<Task1>();
        task2Script = GetComponent<Task2>();

        noPointerOverButton = true;
    }

    // Update is called once per frame
    void Update()
    {
        //Rotation
        if (mainScript.mainHand.Equals("right") && mainScript.rotationPossible && noPointerOverButton)
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

            float rotationAngle = (hand.transform.eulerAngles.y - yawDif - transform.rotation.eulerAngles.y + 360) % 360;

            if (OVRInput.GetDown(OVRInput.RawButton.RIndexTrigger))
            {

                Rotate(rotationAngle, true);
            }
        }
        else if (mainScript.mainHand.Equals("left") && mainScript.rotationPossible && noPointerOverButton)
        {
            if (OVRInput.GetDown(OVRInput.RawButton.LIndexTrigger))
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

                float rotationAngle = (hand.transform.eulerAngles.y - yawDif - transform.rotation.eulerAngles.y + 360) % 360;
                Rotate(rotationAngle, false);
            }

        }
    }

    private void Rotate(float rotationAngle, bool right)
    {
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

    public void PointerEnter()
    {
        noPointerOverButton = false;
    }
    public void PointerExit()
    {
        noPointerOverButton = true;
    }

    private IEnumerator LaserWait()
    {
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        laserPointer.SetActive(true);
    }
}
