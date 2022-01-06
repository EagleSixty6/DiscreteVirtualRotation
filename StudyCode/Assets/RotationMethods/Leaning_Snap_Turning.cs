using System.Collections;
using UnityEngine;

public class Leaning_Snap_Turning : RotationMethod
{
    public float angle;
    public float delay;

    public GameObject pointOfReference;
    public GameObject rotationSphere;
    public bool waiting;

    float rotThresholdLeft;
    float rotThresholdRight;

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

        rotThresholdLeft = -0.15f;
        rotThresholdRight = 0.15f;

        waiting = false;
    }
    void OnEnable()
    {
        waiting = false;
    }

    void Update()
    {
        Vector3 pOR = transform.InverseTransformPoint(pointOfReference.transform.position);

        if (!waiting && GetComponent<Main>().rotationPossible)
        {
            if (pOR.x < rotThresholdLeft)
            {
                StartCoroutine(Rotate(-1 * angle));
            }
            else if (pOR.x > rotThresholdRight)
            {
                StartCoroutine(Rotate(angle));
            }
        }
    }

    private IEnumerator Rotate(float rotationAngle)
    {
        waiting = true;

        laserPointer.SetActive(false);

        if (task1Script.testRunning && !GetComponent<Task1>().training)
        {

            mainScript.t1PlayerRotations += transform.rotation.eulerAngles.y + "; ";
            transform.RotateAround(rotationSphere.transform.position, Vector3.up, rotationAngle);
            mainScript.t1PlayerRotations += transform.rotation.eulerAngles.y + "; ";

        }
        else if (task2Script.testRunning && !GetComponent<Task2>().training)
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
