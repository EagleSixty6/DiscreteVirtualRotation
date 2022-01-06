using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Task1 : MonoBehaviour
{
    // Cubes
    public GameObject cube;
    public GameObject centerCube;

    // Direction Arrows
    public GameObject directionRight;
    public GameObject directionLeft;

    // Pointers
    public GameObject pointCanvas;
    public GameObject circularProgressBar;
    public Image loadingBar;
    public GameObject laserPointer;

    // CameraRig
    public GameObject cameraRig;

    // Controller
    public GameObject pointingHand;

    // Parameters
    public int maxTries;
    public int triesLeft;
    public float duration;

    // Object to rotate around
    public GameObject rotationSphere;

    // Bools
    public bool cubeFound;
    public bool centerCubeSelected;
    public bool startedFirstSave;
    public bool savedFirstDirection;
    public bool training;
    public bool testRunning;

    // Audio Source
    public AudioSource audioSource;

    // Trial Timer
    public float trialTimer;

    // Pointing Error
    public float pointingError;

    // Randomizer
    public System.Random rand1;
    public System.Random rand2;

    // Scripts
    public Main mainScript;
    public BoxCollider centerCubeCollider;
    public BoxCollider targetCubeCollider;
    public MonoBehaviour task1Event;
    public MonoBehaviour task2Event;
    public RadialProgress progressBarScript;
    public LineRenderer lineRenderer;
    public Pointing_Discrete_Selection pointingScript;

    // Cubes Order
    public List<int> order;
    public List<int> trialOrder;
    public List<List<int>> cubeOrders;
    public List<List<float>> rotationOrders;

    // Pointer test
    public AnimationCurve thickLine;
    public AnimationCurve thinLine;

    // Start is called before the first frame update
    void Start()
    {
        mainScript = GetComponent<Main>();
        centerCubeCollider = centerCube.GetComponent<BoxCollider>();
        targetCubeCollider = cube.GetComponent<BoxCollider>();
        task1Event = cameraRig.GetComponents<MonoBehaviour>()[3];
        task2Event = cameraRig.GetComponents<MonoBehaviour>()[4];
        progressBarScript = circularProgressBar.GetComponent<RadialProgress>();
        lineRenderer = laserPointer.GetComponent<LineRenderer>();
        pointingScript = GetComponent<Pointing_Discrete_Selection>();

        savedFirstDirection = false;
        centerCubeSelected = false;
        cubeFound = false;
        triesLeft = maxTries;
        pointingError = 0;

        rand1 = new System.Random(0);
        rand2 = new System.Random(0);

        order = new List<int>() { -3, -3, -3, -3, -2, -2, -2, -2, -1, -1, -1, -1, 0, 0, 0, 0, 1, 1, 1, 1, 2, 2, 2, 2, 3, 3, 3, 3 };
        trialOrder = new List<int>();
        cubeOrders = new List<List<int>>();
        rotationOrders = new List<List<float>>();

        for (int i = 0; i < 5; i++)
        {
            trialOrder = order.OrderBy(x => rand1.Next()).ToList();
            cubeOrders.Add(trialOrder);
            rotationOrders.Add(new List<float>());
            for (int j = 0; j < 28; j++)
            {
                rotationOrders[i].Add((float)rand1.NextDouble() * 360);
            }
        }

        training = true;

        thickLine = new AnimationCurve();
        thickLine.AddKey(0.0f, 1.0f);
        thickLine.AddKey(0.0f, 1.0f);

        thinLine = new AnimationCurve();
        thinLine.AddKey(0.0f, 0.4f);
        thinLine.AddKey(0.0f, 0.4f);

        pointingError = 0;

        StopTest1(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (testRunning)
        {
            if (!training)
            {
                trialTimer += Time.deltaTime;
            }


            if (!centerCubeSelected && centerCubeCollider.bounds.Contains(lineRenderer.GetPosition(lineRenderer.positionCount - 1)))
            {
                progressBarScript.speed = 90 / duration;
            }
            else if (!centerCubeSelected && !centerCubeCollider.bounds.Contains(lineRenderer.GetPosition(lineRenderer.positionCount - 1)))
            {
                progressBarScript.speed = 0;
                progressBarScript.currentValue = 0;
            }

            if (!centerCubeSelected && loadingBar.fillAmount >= 1)
            {
                audioSource.Play();

                trialTimer = 0;

                mainScript.rotationPossible = true;

                loadingBar.fillAmount = 0;
                progressBarScript.currentValue = 0;
                centerCubeSelected = true;
                cube.SetActive(true);
                centerCube.SetActive(false);
            }

            if (centerCubeSelected)
            {
                if (!cubeFound && targetCubeCollider.bounds.Contains(lineRenderer.GetPosition(lineRenderer.positionCount - 1)))
                {
                    progressBarScript.speed = 90 / duration;
                }
                if (!cubeFound && !targetCubeCollider.bounds.Contains(lineRenderer.GetPosition(lineRenderer.positionCount - 1)))
                {
                    progressBarScript.speed = 0;
                    progressBarScript.currentValue = 0;
                }

                if (loadingBar.fillAmount >= 1 && targetCubeCollider.bounds.Contains(lineRenderer.GetPosition(lineRenderer.positionCount - 1)))
                {
                    audioSource.Play();

                    if (!training)
                    {
                        mainScript.t1TrialTimes += trialTimer + "; ";
                    }

                    loadingBar.fillAmount = 0;
                    progressBarScript.currentValue = 0;
                    cubeFound = true;
                    pointCanvas.SetActive(false);
                    cube.SetActive(false);
                    mainScript.rotationPossible = false;

                    lineRenderer.material.SetColor("_Color", Color.red);
                    lineRenderer.widthCurve = thickLine;
                }

                if (cubeFound && !startedFirstSave)
                {
                    StartCoroutine(SaveHandDirection(true));
                }

                if (cubeFound && savedFirstDirection && ((mainScript.mainHand.Equals("right") && OVRInput.Get(OVRInput.RawButton.RIndexTrigger)) || (mainScript.mainHand.Equals("left") && OVRInput.Get(OVRInput.RawButton.LIndexTrigger))))
                {
                    lineRenderer.material.SetColor("_Color", Color.blue);
                    lineRenderer.widthCurve = thinLine;

                    StartCoroutine(SaveHandDirection(false));

                    if (!training)
                    {
                        triesLeft--;
                        mainScript.t1TrialTimes += trialTimer;
                        if (triesLeft != 0)
                        {
                            mainScript.t1TrialTimes += "; ";
                        }
                    }

                    if (triesLeft > 0)
                    {
                        StartTest1(training);
                    }
                    else
                    {
                        StopTest1(true);
                    }

                }
            }
        }
    }

    void TurnArrowLeft()
    {
        directionLeft.SetActive(true);
        directionRight.SetActive(false);
    }
    void TurnArrowRight()
    {
        directionLeft.SetActive(false);
        directionRight.SetActive(true);
    }

    public void StartTest1(bool t)
    {
        pointingScript.noPointerOverButton = true;

        float nextRot = 0;
        int nextCube = 0;

        if (!t)
        {
            nextCube = cubeOrders[(mainScript.step - 1) / 4][0];
            cubeOrders[(mainScript.step - 1) / 4].RemoveAt(0);
            nextRot = rotationOrders[(mainScript.step - 1) / 4][0];
            rotationOrders[(mainScript.step - 1) / 4].RemoveAt(0);
        }
        else if (t)
        {
            rand2 = new System.Random();
            nextRot = (float)(rand2.NextDouble() * 360);
            nextCube = rand2.Next(7) - 3;
        }

        transform.RotateAround(rotationSphere.transform.position, Vector3.up, -1 * transform.rotation.eulerAngles.y);
        transform.RotateAround(rotationSphere.transform.position, Vector3.up, nextRot);

        if (nextCube > 0)
        {
            TurnArrowLeft();
        }
        else
        {
            TurnArrowRight();
        }

        centerCube.transform.position = new Vector3(transform.position.x, centerCube.transform.position.y, transform.position.z) + transform.forward * 4;
        centerCube.transform.rotation = transform.rotation;
        centerCube.SetActive(true);

        cube.transform.position = new Vector3(transform.position.x, cube.transform.position.y, transform.position.z) - transform.forward * 4;
        cube.transform.rotation = transform.rotation;
        cube.transform.RotateAround(transform.position, Vector3.up, nextCube * 30);
        cube.SetActive(false);

        task1Event.enabled = true;
        task2Event.enabled = false;

        pointCanvas.SetActive(true);

        loadingBar.fillAmount = 0;
        progressBarScript.currentValue = 0;
        startedFirstSave = false;
        savedFirstDirection = false;
        centerCubeSelected = false;
        cubeFound = false;
        training = t;
        testRunning = true;
        mainScript.rotationPossible = false;
    }

    public void StopTest1(bool b)
    {
        cube.SetActive(false);
        centerCube.SetActive(false);

        task1Event.enabled = false;
        task2Event.enabled = true;

        pointCanvas.SetActive(false);

        loadingBar.fillAmount = 0;
        progressBarScript.currentValue = 0;
        savedFirstDirection = false;
        centerCubeSelected = false;
        cubeFound = false;
        triesLeft = maxTries;
        testRunning = false;

        trialOrder = new List<int>();

        if (!training)
        {
            mainScript.rotationPossible = false;
            mainScript.restText.text = "Take off your HMD to take a break if you need one.";
            mainScript.restCanvas.SetActive(true);
        }
        else if (training && b)
        {
            mainScript.step++;
            mainScript.waiting = false;
        }
    }

    private IEnumerator SaveHandDirection(bool wait)
    {
        startedFirstSave = true;

        if (wait)
        {
            yield return new WaitForSeconds(0.5f);
        }

        audioSource.Play();

        Vector3 handDirection = Vector3.ProjectOnPlane(mainScript.rightHand.transform.forward, Vector3.up);
        if (mainScript.mainHand.Equals("left"))
        {
            handDirection = Vector3.ProjectOnPlane(mainScript.leftHand.transform.forward, Vector3.up);
        }
        Vector3 centerCubeProjection = Vector3.ProjectOnPlane(centerCube.transform.position, Vector3.up);
        pointingError = Vector3.SignedAngle(centerCubeProjection, handDirection, Vector3.up);

        if (training)
        {
            mainScript.trainText.text = mainScript.explanationList[(mainScript.step - 1) / 4] + "\n\nYou were off by: " + pointingError + "°.";
        }
        else
        {
            mainScript.t1PointingError += pointingError;

            if (triesLeft != 0)
            {
                mainScript.t1PointingError += "; ";
            }
        }

        savedFirstDirection = true;
    }


    public void SortByArray(int[] array)
    {
        List<List<int>> copy = new List<List<int>>();
        List<List<float>> copy2 = new List<List<float>>();
        foreach (int i in array)
        {
            copy.Add(cubeOrders[i]);
            copy2.Add(rotationOrders[i]);
        }
        cubeOrders.Clear();
        rotationOrders.Clear();
        cubeOrders.AddRange(copy);
        rotationOrders.AddRange(copy2);
    }
}
