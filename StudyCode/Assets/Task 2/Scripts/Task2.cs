using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Task2 : MonoBehaviour
{
    // Objects
    public List<GameObject> objects;

    // Pointer
    public GameObject pointerCanvas;
    public GameObject laserPointer;
    public GameObject circularProgressBar;
    public Image loadingBar;

    // CameraRig
    public GameObject cameraRig;

    // Parameters
    public int maxTries;
    public int triesLeft;
    public float duration;

    // Next object
    public string objectToFind;
    private int nextObject;

    // Bools
    public bool testRunning;
    public bool training;
    public bool newTraining;

    // Audio Source
    public AudioSource audioSource;

    // Tiral Timer
    public float trialTimer;

    // Scripts
    public Main mainScript;
    public PlaceObjects placeObjectsScript;
    public MonoBehaviour task1Event;
    public MonoBehaviour task2Event;
    public RadialProgress progressBarScript;
    public LineRenderer lineRenderer;
    public Pointing_Discrete_Selection pointingScript;

    // Randomizer
    private System.Random rand;

    // Object Order
    public List<int> counts;
    public List<List<int>> objectOrders;

    // Start is called before the first frame update
    void Start()
    {
        mainScript = GetComponent<Main>();
        placeObjectsScript = GetComponent<PlaceObjects>();
        task1Event = cameraRig.GetComponents<MonoBehaviour>()[3];
        task2Event = cameraRig.GetComponents<MonoBehaviour>()[4];
        progressBarScript = circularProgressBar.GetComponent<RadialProgress>();
        lineRenderer = laserPointer.GetComponent<LineRenderer>();
        pointingScript = GetComponent<Pointing_Discrete_Selection>();

        objects = new List<GameObject>();

        triesLeft = maxTries;
        testRunning = false;

        rand = new System.Random(0);

        counts = new List<int>() { 76, 88, 81, 71, 72 };
        objectOrders = new List<List<int>>();


        /* objectOrders code
        for(int i=0; i<5; i++)
        {
            placeObjectsScript.NewEnvironment(i);
            objectOrders.Add(new List<int>());
            for(int j=0; j<28; j++)
            {
                int next = rand.Next(0, counts[i]);
                if (j > 0) 
                {
                    while (Vector3.Distance(placeObjectsScript.allObjects[next].transform.position, placeObjectsScript.allObjects[objectOrders[i][j-1]].transform.position) < 5)
                    {
                        next = rand.Next(0, counts[i]);
                    }
                }
                objectOrders[i].Add(next);
            }
        }

        placeObjectsScript.DeleteAllObjects();
        */

        objectOrders.Add(new List<int>() { 55, 62, 58, 68, 33, 74, 22, 74, 2, 75, 51, 62, 75, 2, 53, 39, 52, 41, 6, 14, 22, 75, 48, 57, 2, 28, 26, 72 });
        objectOrders.Add(new List<int>() { 44, 63, 24, 69, 29, 12, 19, 36, 63, 54, 42, 77, 72, 64, 75, 59, 54, 19, 78, 7, 73, 14, 56, 72, 19, 87, 57, 63 });
        objectOrders.Add(new List<int>() { 65, 8, 10, 38, 43, 26, 54, 37, 67, 65, 61, 75, 23, 42, 53, 66, 15, 50, 56, 51, 75, 5, 7, 12, 8, 49, 77, 11 });
        objectOrders.Add(new List<int>() { 48, 40, 16, 0, 60, 4, 19, 50, 68, 37, 19, 35, 4, 19, 35, 43, 52, 9, 49, 43, 40, 13, 70, 40, 4, 18, 59, 40 });
        objectOrders.Add(new List<int>() { 46, 37, 69, 5, 36, 58, 46, 39, 48, 71, 21, 4, 33, 34, 15, 5, 16, 32, 1, 58, 14, 65, 2, 36, 69, 1, 47, 20 });

        training = true;

        StopTest2(false);

        mainScript.enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (testRunning)
        {
            trialTimer += Time.deltaTime;

            if (IsTargeted(objects[nextObject]))
            {
                progressBarScript.speed = 90 / duration;
            }
            else
            {
                progressBarScript.speed = 0;
                progressBarScript.currentValue = 0;
            }

            if (loadingBar.fillAmount >= 1)
            {
                progressBarScript.currentValue = 0;
                progressBarScript.currentValue = 0;
                audioSource.Play();

                int gain = 150 - ((int)trialTimer * 20);
                if (gain < 0)
                {
                    gain = 0;
                }
                mainScript.score += (150 + gain);
                mainScript.scoreText.text = (int)mainScript.score + "";

                if (!training)
                {
                    triesLeft--;
                    mainScript.t2TrialTimes += trialTimer;

                    if (triesLeft != 0)
                    {
                        mainScript.t2TrialTimes += "; ";
                    }
                    else
                    {
                        mainScript.taskCanvas.SetActive(false);
                        mainScript.scoreCanvas.SetActive(false);
                        StopTest2(true);
                    }
                }

                if (testRunning && triesLeft > 0)
                {
                    StartTest2(training);
                }
            }
        }
    }

    public void StartTest2(bool t)
    {
        pointingScript.noPointerOverButton = true;
        training = t;
        trialTimer = 0;

        if (!training)
        {
            if (triesLeft == maxTries)
            {
                transform.RotateAround(mainScript.rotationSphere.transform.position, Vector3.up, -1 * transform.rotation.eulerAngles.y);
                objects.Clear();
                objects.AddRange(placeObjectsScript.allObjects);
            }
            mainScript.scoreCanvas.transform.localPosition = new Vector3(0, 0.706f, 1.013f);
            if (triesLeft == maxTries)
            {
                mainScript.score = 0;
            }
            nextObject = objectOrders[(mainScript.step - 1) / 4][0];
            objectOrders[(mainScript.step - 1) / 4].RemoveAt(0);
            objectToFind = "Select the " + objects[nextObject].name + ".";
            mainScript.taskText.text = objectToFind;
        }

        else
        {
            if (newTraining)
            {
                mainScript.score = 0;
                mainScript.scoreText.text = "0";
                newTraining = false;
                objects.Clear();
                objects.AddRange(placeObjectsScript.allObjects);
            }
            mainScript.scoreCanvas.transform.localPosition = new Vector3(0, 0.87f, 1.125f);
            nextObject = rand.Next(0, objects.Count);
            objectToFind = "\n\nSelect the " + objects[nextObject].name + ".";
            mainScript.trainText.text = mainScript.explanationList[(mainScript.step - 1) / 4] + objectToFind;
        }

        task1Event.enabled = false;
        task2Event.enabled = true;

        pointerCanvas.SetActive(true);
        loadingBar.fillAmount = 0;
        progressBarScript.currentValue = 0;

        testRunning = true;

        if (!mainScript.infoCanvas.activeSelf)
        {
            mainScript.rotationPossible = true;
        }
    }

    public void StopTest2(bool b)
    {
        testRunning = false;

        objects.Clear();

        task1Event.enabled = true;
        task2Event.enabled = false;

        pointerCanvas.SetActive(false);
        loadingBar.fillAmount = 0;
        progressBarScript.currentValue = 0;

        triesLeft = maxTries;

        if (!training)
        {
            mainScript.t2Scores += mainScript.score;
            mainScript.rotationPossible = false;
            mainScript.restText.text = "Take off your HMD to fill out the questionaire and to take a break if you need one.";
            mainScript.restCanvas.SetActive(true);
        }
        else if (training && b)
        {
            mainScript.step++;
            mainScript.waiting = false;
        }
        mainScript.score = 0;
        mainScript.scoreText.text = "0";
        newTraining = true;
    }

    private bool IsTargeted(GameObject o)
    {
        if (o == null)
        {
            return false;
        }

        Vector3 lineEnd = lineRenderer.GetPosition(lineRenderer.positionCount - 1);

        List<GameObject> children = new List<GameObject>();

        for (int i = 0; i < o.transform.childCount; i++)
        {
            children.Add(o.transform.GetChild(i).gameObject);
        }

        if (o.name.Contains("Door") || o.name.Contains("Car") || o.name.Contains("Pyramid"))
        {
            foreach (GameObject child in children)
            {
                if (child.GetComponent<Collider>().bounds.Contains(lineEnd))
                {
                    return true;
                }
            }
        }

        else if (o.name.Contains("Potted") || o.name.Contains("Balloon")
            || o.name.Contains("Guitar") || o.name.Contains("Chair")
            || o.name.Contains("Grill") || o.name.Contains("Lamp")
            || o.name.Contains("Laptop") || o.name.Contains("Piano")
            || o.name.Contains("Ping") || o.name.Contains("Pirate")
            || (o.name.Contains("Glass") && !o.name.Contains("on"))
            || (o.name.Contains("Shelf") && !o.name.Contains("in")))
        {
            foreach (GameObject child in children)
            {
                if (child.GetComponent<Collider>().bounds.Contains(lineEnd))
                {
                    return true;
                }
            }
            return false;
        }
        return o.GetComponent<Collider>().bounds.Contains(lineEnd);
    }
    public void SortByArray(int[] array)
    {
        List<List<int>> copy = new List<List<int>>();
        foreach (int i in array)
        {
            copy.Add(objectOrders[i]);
        }
        objectOrders.Clear();
        objectOrders.AddRange(copy);
    }
}
