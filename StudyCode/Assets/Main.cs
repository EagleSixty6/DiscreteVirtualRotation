using System;
using System.Text;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Main : MonoBehaviour
{
    // Methods
    public List<int[][]> latinSquare;
    public int[] methodOrder;

    // Controller
    public string mainHand;
    public GameObject rightHand;
    public GameObject leftHand;

    // Texts
    public string explanationPDS;
    public string explanationLST;
    public string explanationLDS;
    public string explanationJST;
    public string explanationJDSG;
    public List<string> explanationList;
    public List<string> explanationTasks;
    public int expTaskIndex;

    // Canvases
    public GameObject infoCanvas;
    public GameObject infoImage;
    public Text infoText;
    public Button infoNext;
    public Button infoPrev;
    public Button infoHide;

    public GameObject trainCanvas;
    public GameObject trainImage;
    public Text trainText;
    public Button trainStart;
    public Button trainHide;

    public GameObject taskCanvas;
    public GameObject taskImage;
    public Text taskText;
    public Button taskHide;

    public GameObject restCanvas;
    public GameObject restImage;
    public Text restText;
    public Button continueButton;

    public GameObject userIDCanvas;
    public Text userIDText;
    public Button button1;
    public Button button2;
    public Button button3;
    public Button button4;
    public Button button5;
    public Button button6;
    public Button button7;
    public Button button8;
    public Button button9;
    public Button buttonDelete;
    public Button buttonEnter;
    public int userID;

    public GameObject scoreCanvas;
    public GameObject scoreImage;
    public float score;
    public Text scoreText;

    public float angTimer;
    public float lastFrameHeadRot;
    public float currentHeadRot;
    public float angularVelFrame;

    public GameObject calibrationCanvas;
    public GameObject calibrationImage;
    public Text calibrationText;
    public Button calibrationButton;
    public Button doneButton;
    public Button resetButton;
    public Button calibrationHide;

    // Event Systems
    public GameObject uiHelpers;
    public EventSystem eventSystemPoint;
    public GameObject pointingProgressCanvas;

    // Files
    public string filePrefix;
    public string mainPath;
    public string loadData;
    public string t1PointingError;
    public string t1PEFile;
    public string t1TrialTimes;
    public string t1TTFile;
    public string t1PlayerRotations;
    public string t1PRFile;
    public string t1AngularVelocity;
    public string t1AVFile;
    public string t2TrialTimes;
    public string t2TTFile;
    public string t2PlayerRotations;
    public string t2PRFile;
    public string t2AngularVelocity;
    public string t2AVFile;
    public string t2Scores;
    public string t2SFile;

    // Scripts
    public PlaceObjects placeObjectsScript;
    public Task1 task1Script;
    public Task2 task2Script;
    public ScriptHandling handlingScript;
    public Calibration calibrationScript;
    public Pointing_Discrete_Selection pointingScript;
    public Leaning_Discrete_Selection leaningClockScript;
    public Leaning_Snap_Turning leaningSnapScript;
    public Joystick_Discrete_Selection_Global joystickClockScript;
    public Joystick_Snap_Turning joystickSnapScript;

    // Rotation and Camera
    public GameObject rotationSphere;
    public GameObject pointOfReference;
    public GameObject centerEyeAnchor;
    public GameObject cameraRig;
    public GameObject horizonSphere;

    // Other
    public bool waiting;
    public bool controllerSelected;
    public bool idEntered;
    public bool rotationPossible;
    public int step;

    // Start is called before the first frame update
    void OnEnable()
    {
        placeObjectsScript = GetComponent<PlaceObjects>();
        task1Script = GetComponent<Task1>();
        task2Script = GetComponent<Task2>();
        handlingScript = GetComponent<ScriptHandling>();
        calibrationScript = GetComponent<Calibration>();
        pointingScript = GetComponent<Pointing_Discrete_Selection>();
        leaningClockScript = GetComponent<Leaning_Discrete_Selection>();
        leaningSnapScript = GetComponent<Leaning_Snap_Turning>();
        joystickClockScript = GetComponent<Joystick_Discrete_Selection_Global>();
        joystickSnapScript = GetComponent<Joystick_Snap_Turning>();

        latinSquare = new List<int[][]>
        {
            new int[][]{
                new int[]{0, 1, 4, 2, 3 },
                new int[]{1, 4, 0, 3, 2 },
                new int[]{4, 2, 3, 0, 1 },
                new int[]{2, 3, 1, 4, 0 },
                new int[]{3, 0, 2, 1, 4 }
            },
            new int[][]{
                new int[]{0, 2, 4, 3, 1 },
                new int[]{4, 3, 1, 0, 2 },
                new int[]{1, 4, 0, 2, 3 },
                new int[]{3, 1, 2, 4, 0 },
                new int[]{2, 0, 3, 1, 4 }
            },
            new int[][]{
                new int[]{0, 2, 3, 1, 4 },
                new int[]{2, 4, 0, 3, 1 },
                new int[]{4, 3, 1, 0, 2 },
                new int[]{1, 0, 2, 4, 3 },
                new int[]{3, 1, 4, 2, 0 }
            },
            new int[][]{
                new int[]{2, 1, 3, 0, 4 },
                new int[]{0, 2, 1, 4, 3 },
                new int[]{1, 4, 0, 3, 2 },
                new int[]{4, 3, 2, 1, 0 },
                new int[]{3, 0, 4, 2, 1 }
            }
        };

        loadData = "_LoadData.txt";
        mainPath = Application.persistentDataPath + "/LoggingData/";
        //mainPath = "C:\\Users\\Jonas\\Desktop\\LoggingData\\";
        Directory.CreateDirectory(mainPath);

        t1PointingError = "";
        t1TrialTimes = "";
        t1PlayerRotations = "";
        t1AngularVelocity = "";
        t2TrialTimes = "";
        t2PlayerRotations = "";
        t2AngularVelocity = "";
        t2Scores = "";

        t1PEFile = "_T1PE.txt";
        t1TTFile = "_T1TT.txt";
        t1PRFile = "_T1PR.txt";
        t1AVFile = "_T1AV.txt";
        t2TTFile = "_T2TT.txt";
        t2PRFile = "_T2PR.txt";
        t2AVFile = "_T2AV.txt";
        t2SFile = "_T2SC.txt";


        explanationJDSG = "Thumbstick Clock:\nUse the thumbstick on your main controller and point it in the direction you want to virtually rotate to.\nThe direction is measured relative to your direction of view.";
        explanationJST = "Thumbstick Snapping:\nUse the thumbstick on your main controller and point it left or right to virtually rotate a small amount in that direction.";
        explanationLDS = "Leaning Clock:\nLean your upper body in the direction you want to rotate to.";
        explanationLST = "Leaning Snapping:\nLean left and right with your upper body to virtually rotate a small amount in that direction.";
        explanationPDS = "Pointing:\nPoint your main controller at the location you want to rotate to. Confirm your choice and virtually rotate to that location by pressing the trigger button on your main controller.";

        explanationList = new List<string>
        {
            explanationJDSG,
            explanationJST,
            explanationLDS,
            explanationLST,
            explanationPDS
        };

        waiting = true;
        infoCanvas.SetActive(true);

        explanationTasks = new List<string>()
        {
             "Your controller has been selected.\nFrom now on the " + mainHand + " controller will be refered to as \"main controller\".\nUse the arrows below to navigate through the instructions.\nPress buttons by using the trigger button on your main controller."
            , "In the upcoming study, you will have to complete two different tasks five times using a different virtual rotation method each time."
            , "With every new method you first get an unlimited amount of time to test it."
            , "For the first task you see a pink cube (center element) floating infront of you with an arrow above it.\nRemember the direction of the arrow."
            , "To start the task, you will need to point at the center element."
            , "You will see a circle filling up on your pointer, showing your progress. Keep pointing until the circle is completely filled, the center element disappears and you hear a sound."
            , "After that you will have to find another floating cube (target) somewhere behind you.\nThe arrow above the previous center element indicates the direction in which the target can be found."
            , "Virtually rotate towards and point at the target until it disappears and you hear a sound again."
            , "Then your pointer will change colour and you will have to quickly point towards the location where the center element was located.\nHalf a second after the target disappeared a sound will play and the direction in that your main controller is pointing in that moment will be saved."
            , "After that you get unlimited time to possibly correct your former choice. Again point towards the location of the center element and this time, confirm your choice by pressing the trigger button on your main controller."
            , "During the testing you will also see by how many degrees you missed the center element, to get a better feeling of your intuition being right or not.\n(Negative values mean you pointed too far left of the center element, positive values too far right.)"
            , "After your confirmation you will get turned to a random direction and a new center element will appear infront of you.\nThen you will start again from the beginning and repeat until the task is fully completed."
            , "A short recap of what you will have to do:\n- Point at the cube, remember the arrow.\n- Find the next cube, point at it.\n- Point at previous cube, wait for sound.\n- Point at previous cube, press the index trigger.\n- Repeat."
            , "If there is anything left that is unclear you can either go back and read again or ask now.\nContinuing will start the training. Press the \"Start\"-Button during testing once you have trained enough to start the actual task."
            , ""
            , "For the second task you will have to look for and point at specific items that are placed somewhere around you.\nThe item that you have to find will be named here on this tablet."
            , "The item will be selected by pointing at it with your main controller and again waiting for a circle to fill up and a sound to play.\nRepeat until you found all items."
            , "Selecting the correct item will award you points in this task. You get more points the faster you find the next item, but you also lose points for moving your head too much and too fast."
            , "If there is anything left that is unclear you can either go back and read again or ask now.\nContinuing will start the training. Press the \"Start\"-Button during testing once you have trained enough to start the actual task."
            , ""
        };
        if (File.Exists(mainPath + loadData))
        {
            LoadData();
            SelectController(mainHand);
            idEntered = true;
            waiting = false;
            infoCanvas.SetActive(false);
            methodOrder = latinSquare[((userID / 100) % 10) - 1][(userID / 1000) - 1];
            handlingScript.SortByArray(methodOrder);
            task1Script.SortByArray(methodOrder);
            task2Script.SortByArray(methodOrder);
            SortByArray(explanationList, methodOrder);
            placeObjectsScript.SortByArray(methodOrder);
            for (int i = 0; i < explanationList.Count; i++)
            {
                explanationList[i] = "Method #" + (i + 1) + " - " + explanationList[i];
            }
            ResetViewAndPosition(false);
        }
        else
        {
            filePrefix = DateTime.Now.Year + "_0" + DateTime.Now.Month + "_0" + DateTime.Now.Day + "_" + DateTime.Now.Hour + "-" + DateTime.Now.Minute + "-" + DateTime.Now.Second;
            idEntered = false;
            step = 0;
            horizonSphere.SetActive(true);
            ResetViewAndPosition(false);
        }

        infoNext.onClick.AddListener(delegate
        {
            expTaskIndex++;
            if (!idEntered)
            {
                infoCanvas.SetActive(false);
                userIDCanvas.SetActive(true);
            }
            else
            {
                infoPrev.gameObject.SetActive(true);

                task1Script.centerCube.SetActive(expTaskIndex >= 3 && expTaskIndex <= 5);

                if (expTaskIndex == 14 || expTaskIndex == 19)
                {
                    expTaskIndex++;

                    step++;
                    waiting = false;

                    rotationPossible = (expTaskIndex == 19);
                    task2Script.trialTimer = 0;
                }
                else
                {
                    infoText.text = explanationTasks[expTaskIndex];
                }
            }
        });
        infoPrev.onClick.AddListener(delegate
        {
            expTaskIndex--;
            infoNext.gameObject.SetActive(true);

            task1Script.centerCube.SetActive(expTaskIndex >= 3 && expTaskIndex <= 5);

            infoText.text = explanationTasks[expTaskIndex];

            if (expTaskIndex == 0 || expTaskIndex == 13)
            {
                infoPrev.gameObject.SetActive(false);
                pointingScript.noPointerOverButton = true;
            }
        });
        infoHide.onClick.AddListener(delegate
        {
            infoImage.SetActive(!infoImage.activeSelf);
            if (!infoImage.activeSelf)
            {
                infoHide.GetComponentInChildren<Text>().text = "Show";
            }
            else
            {
                infoHide.GetComponentInChildren<Text>().text = "Hide";
            }
        });

        trainStart.onClick.AddListener(delegate
        {
            if (step % 4 == 1)
            {
                task1Script.StopTest1(true);
            }
            if (step % 4 == 3)
            {
                task2Script.StopTest2(true);
            }
        });
        trainHide.onClick.AddListener(delegate
        {
            trainImage.SetActive(!trainImage.activeSelf);
            if (!trainImage.activeSelf)
            {
                trainHide.GetComponentInChildren<Text>().text = "Show";
                if (task2Script.testRunning)
                {
                    scoreCanvas.SetActive(false);
                }
            }
            else
            {
                trainHide.GetComponentInChildren<Text>().text = "Hide";
                if (task2Script.testRunning)
                {
                    scoreCanvas.SetActive(true);
                }
            }
        });

        taskHide.onClick.AddListener(delegate
        {
            taskImage.SetActive(!taskImage.activeSelf);
            if (!taskImage.activeSelf)
            {
                taskHide.GetComponentInChildren<Text>().text = "Show";
                if (task2Script.testRunning)
                {
                    scoreCanvas.SetActive(false);
                }
            }
            else
            {
                taskHide.GetComponentInChildren<Text>().text = "Hide";
                if (task2Script.testRunning)
                {
                    scoreCanvas.SetActive(true);
                }
            }
        });

        continueButton.onClick.AddListener(delegate
        {
            if (step == 2)
            {
                infoCanvas.SetActive(true);
                infoText.text = explanationTasks[expTaskIndex];
                trainCanvas.SetActive(false);
                taskCanvas.SetActive(false);
                infoPrev.gameObject.SetActive(false);
                infoNext.gameObject.SetActive(true);
                pointingScript.noPointerOverButton = true;
            }
            else
            {
                step++;
                waiting = false;
            }
            restCanvas.SetActive(false);

        });

        calibrationButton.onClick.AddListener(delegate
        {
            ResetViewAndPosition(false);
            calibrationScript.StartCalibration();
            StartCoroutine(CalibrationStop());
        });
        calibrationHide.onClick.AddListener(delegate
        {
            calibrationImage.SetActive(!calibrationImage.activeSelf);
            if (!calibrationImage.activeSelf)
            {
                calibrationHide.GetComponentInChildren<Text>().text = "Show";
            }
            else
            {
                calibrationHide.GetComponentInChildren<Text>().text = "Hide";
            }
        });
        doneButton.onClick.AddListener(delegate
        {
            step = 30;
            waiting = false;
        });
        resetButton.onClick.AddListener(delegate
        {
            ResetViewAndPosition(false);
        });

        button1.onClick.AddListener(delegate { ButtonPress(1); });
        button2.onClick.AddListener(delegate { ButtonPress(2); });
        button3.onClick.AddListener(delegate { ButtonPress(3); });
        button4.onClick.AddListener(delegate { ButtonPress(4); });
        button5.onClick.AddListener(delegate { ButtonPress(5); });
        button6.onClick.AddListener(delegate { ButtonPress(6); });
        button7.onClick.AddListener(delegate { ButtonPress(7); });
        button8.onClick.AddListener(delegate { ButtonPress(8); });
        button9.onClick.AddListener(delegate { ButtonPress(9); });
        buttonDelete.onClick.AddListener(delegate
        {
            buttonEnter.gameObject.SetActive(false);
            pointingScript.noPointerOverButton = true;
            userIDText.text = userIDText.text.Substring(0, userIDText.text.Length - 1);
            if (userIDText.text.Length == 0)
            {
                buttonDelete.gameObject.SetActive(false);
                pointingScript.noPointerOverButton = true;
            }
        });
        buttonEnter.onClick.AddListener(delegate
        {
            idEntered = true;
            userID = int.Parse(userIDText.text);
            userIDCanvas.SetActive(false);
            infoText.text = explanationTasks[expTaskIndex];
            infoCanvas.SetActive(true);
            methodOrder = latinSquare[((userID / 100) % 10) - 1][(userID / 1000) - 1];
            handlingScript.SortByArray(methodOrder);
            task1Script.SortByArray(methodOrder);
            task2Script.SortByArray(methodOrder);

            SortByArray(explanationList, methodOrder);
            placeObjectsScript.SortByArray(methodOrder);
            for (int i = 0; i < explanationList.Count; i++)
            {
                explanationList[i] = "Method #" + (i + 1) + " - " + explanationList[i];
            }
        });

        lastFrameHeadRot = 0;
        currentHeadRot = 0;
        angularVelFrame = 0;
        angTimer = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (OVRInput.GetDown(OVRInput.RawButton.RIndexTrigger) && OVRInput.Get(OVRInput.RawButton.RHandTrigger) && OVRInput.Get(OVRInput.RawButton.A) && OVRInput.Get(OVRInput.RawButton.B)
          || OVRInput.GetDown(OVRInput.RawButton.LIndexTrigger) && OVRInput.Get(OVRInput.RawButton.LHandTrigger) && OVRInput.Get(OVRInput.RawButton.X) && OVRInput.Get(OVRInput.RawButton.Y))
        {
            ResetViewAndPosition(false);
        }
        currentHeadRot = centerEyeAnchor.transform.localRotation.eulerAngles.y + cameraRig.transform.localRotation.eulerAngles.y;
        if (currentHeadRot > 360 || currentHeadRot < -360)
        {
            currentHeadRot = currentHeadRot % 360;
        }
        if (currentHeadRot > 180)
        {
            currentHeadRot -= 360;
        }
        else if (currentHeadRot < -180)
        {
            currentHeadRot += 360;
        }

        angularVelFrame = (currentHeadRot - lastFrameHeadRot) / Time.deltaTime;
        lastFrameHeadRot = currentHeadRot;

        if (task1Script.testRunning && !task1Script.training)
        {
            t1AngularVelocity += angularVelFrame + "; ";
        }
        else if (task2Script.testRunning)
        {
            if (!task2Script.training)
            {
                t2AngularVelocity += angularVelFrame + "; ";
            }
            if (angularVelFrame > 20)
            {
                scoreImage.GetComponent<Image>().color = new Color(1, 0.2313726f, 0, 1);
                score -= (angularVelFrame / 100);
                scoreText.text = (int)score + "";
            }
            else if (angularVelFrame < 1)
            {
                scoreImage.GetComponent<Image>().color = new Color(0, 0.7019608f, 1, 1);
            }
        }

        if (!controllerSelected && (OVRInput.GetDown(OVRInput.RawButton.RIndexTrigger) || OVRInput.GetDown(OVRInput.RawButton.LIndexTrigger)))
        {
            StartCoroutine(WaitForControllerSelection());
        }
        if (!controllerSelected && (OVRInput.GetUp(OVRInput.RawButton.RIndexTrigger) || OVRInput.GetUp(OVRInput.RawButton.LIndexTrigger)))
        {
            StopAllCoroutines();
        }

        if (!waiting)
        {
            if (step % 2 == 1 && step % 4 == 1 && step < 21)
            {
                //train task 1
                if (step == 1)
                {
                    SaveData(0);
                }
                else
                {
                    SaveData(2);
                }

                placeObjectsScript.NewEnvironment((step - 1) / 4);

                handlingScript.OnlyEnableOne((step - 1) / 4, handlingScript.scripts);

                infoCanvas.SetActive(false);
                trainCanvas.SetActive(true);
                taskCanvas.SetActive(false);
                scoreCanvas.SetActive(false);

                trainText.text = explanationList[(step - 1) / 4] + "\n\nYou were off by: ";
                trainImage.SetActive(true);
                trainHide.GetComponentInChildren<Text>().text = "Hide";
                pointingScript.noPointerOverButton = true;

                task1Script.StartTest1(true);

                waiting = true;
            }
            else if (step % 2 == 0 && step % 4 == 2 && step < 21)
            {
                //do task 1
                infoCanvas.SetActive(false);
                trainCanvas.SetActive(false);
                taskCanvas.SetActive(false);

                task1Script.StartTest1(false);

                waiting = true;
            }
            else if (step % 2 == 1 && step % 4 == 3 && step < 21)
            {
                //train task 2
                SaveData(1);

                placeObjectsScript.NewEnvironment((step - 1) / 4);

                handlingScript.OnlyEnableOne((step - 1) / 4, handlingScript.scripts);

                infoCanvas.SetActive(false);
                trainCanvas.SetActive(true);
                taskCanvas.SetActive(false);
                scoreCanvas.SetActive(true);

                trainText.text = explanationList[(step - 1) / 4];
                trainImage.SetActive(true);
                trainHide.GetComponentInChildren<Text>().text = "Hide";
                pointingScript.noPointerOverButton = true;

                task2Script.StartTest2(true);

                waiting = true;
            }
            else if (step % 2 == 0 && step % 4 == 0 && step < 21)
            {
                //do task 2
                infoCanvas.SetActive(false);
                trainCanvas.SetActive(false);
                taskCanvas.SetActive(true);
                scoreCanvas.SetActive(true);

                task2Script.StartTest2(false);

                waiting = true;
            }
            else if (step == 21)
            {
                SaveData(2);
                scoreCanvas.SetActive(false);
                taskCanvas.SetActive(false);
                restCanvas.SetActive(true);

                waiting = true;
            }
            else if (step == 22)
            {
                // last task
                task1Script.enabled = false;
                task2Script.enabled = false;
                pointingProgressCanvas.SetActive(false);

                restCanvas.SetActive(false);
                calibrationCanvas.SetActive(true);

                placeObjectsScript.NewEnvironment(297);

                handlingScript.DisableAll();
                calibrationScript.enabled = true;
                GetComponent<Head_Scrolling>().enabled = true;

                waiting = true;
            }
            else if (step > 22)
            {
                //ende
                try
                {
                    List<string> lines = new List<string>();

                    foreach (string line in File.ReadLines(mainPath + loadData))
                    {
                        lines.Add(line);
                    }

                    Vector3 headPos = GetComponent<Calibration>().calibratedCenter;

                    TextWriter tw = new StreamWriter(mainPath + loadData, true);
                    tw.WriteLine(headPos.x);
                    tw.WriteLine(headPos.y);
                    tw.WriteLine(headPos.z);
                    tw.WriteLine(rotationSphere.transform.localPosition.z - headPos.z);
                    tw.Close();
                }

                catch (Exception e) { Debug.Log("################Saving rotation axis failed!###################\n" + e.ToString()); }

                File.Move(mainPath + loadData, mainPath + userID + "_" + filePrefix + loadData);

                infoCanvas.SetActive(true);
                trainCanvas.SetActive(false);
                calibrationCanvas.SetActive(false);

                infoText.text = "Congratulations! You completed every task.\nThank you for participating.";

                infoNext.gameObject.SetActive(false);
                infoPrev.gameObject.SetActive(false);
                infoHide.gameObject.SetActive(false);

                uiHelpers.SetActive(false);
                GetComponent<Head_Scrolling>().enabled = false;

                waiting = true;
            }
        }
    }

    private void LoadData()
    {
        List<string> lines = new List<string>();
        try
        {
            foreach (string line in File.ReadLines(mainPath + loadData))
            {
                lines.Add(line);
            }
            mainHand = lines[0];
            expTaskIndex = int.Parse(lines[1]);
            filePrefix = lines[2];
            userID = int.Parse(lines[3]);
            rotationSphere.transform.localPosition = new Vector3(float.Parse(lines[4]), float.Parse(lines[5]), float.Parse(lines[6]));
            step = int.Parse(lines[7]);
            if (step % 2 == 0)
            {
                step--;
            }
        }
        catch (Exception e) { Debug.Log("################Loading Data failed!###################\n" + e.ToString()); }

    }

    private void SaveData(int task)
    {
        try
        {
            int k = (userID % 100) / 10 + userID % 10;

            TextWriter tw = new StreamWriter(mainPath + loadData, false);
            tw.WriteLine(mainHand);
            tw.WriteLine(expTaskIndex);
            tw.WriteLine(filePrefix);
            tw.WriteLine(userID);
            tw.WriteLine(rotationSphere.transform.localPosition.x);
            tw.WriteLine(rotationSphere.transform.localPosition.y);
            tw.WriteLine(rotationSphere.transform.localPosition.z);
            tw.WriteLine(step);
            tw.Close();

            if (step != 1)
            {
                if (task == 1 && !t1TrialTimes.Equals(""))
                {
                    t1PlayerRotations = t1PlayerRotations.Substring(0, t1PlayerRotations.Length - 2);
                    t1AngularVelocity = t1AngularVelocity.Substring(0, t1AngularVelocity.Length - 2);
                    t1PointingError = t1PointingError.Substring(0, t1PointingError.Length - 2);

                    //string t1pe = Encrypt(t1PointingError, k);
                    //string t1tt = Encrypt(t1TrialTimes, k);
                    //string t1pr = Encrypt(t1PlayerRotations, k);
                    //string t1av = Encrypt(t1AngularVelocity, k);

                    TextWriter tw2 = new StreamWriter(mainPath + userID + "_" + filePrefix + t1PEFile, true);
                    TextWriter tw3 = new StreamWriter(mainPath + userID + "_" + filePrefix + t1TTFile, true);
                    TextWriter tw4 = new StreamWriter(mainPath + userID + "_" + filePrefix + t1PRFile, true);
                    TextWriter tw5 = new StreamWriter(mainPath + userID + "_" + filePrefix + t1AVFile, true);
                    //TextWriter tw2e = new StreamWriter(mainPath + userID + "_" + filePrefix + "_en" + t1PEFile, true);
                    //TextWriter tw3e = new StreamWriter(mainPath + userID + "_" + filePrefix + "_en" + t1TTFile, true);
                    //TextWriter tw4e = new StreamWriter(mainPath + userID + "_" + filePrefix + "_en" + t1PRFile, true);
                    //TextWriter tw5e = new StreamWriter(mainPath + userID + "_" + filePrefix + "_en" + t1AVFile, true);


                    tw2.WriteLine(t1PointingError);
                    tw3.WriteLine(t1TrialTimes);
                    tw4.WriteLine(t1PlayerRotations);
                    tw5.WriteLine(t1AngularVelocity);
                    //tw2e.WriteLine(t1pe);
                    //tw3e.WriteLine(t1tt);
                    //tw4e.WriteLine(t1pr);
                    //tw5e.WriteLine(t1av);

                    tw2.Close();
                    tw3.Close();
                    tw4.Close();
                    tw5.Close();
                    //tw2e.Close();
                    //tw3e.Close();
                    //tw4e.Close();
                    //tw5e.Close();

                    t1PointingError = "";
                    t1TrialTimes = "";
                    t1PlayerRotations = "";
                    t1AngularVelocity = "";
                }
                else if (task == 2 && !t2TrialTimes.Equals(""))
                {
                    t2PlayerRotations = t2PlayerRotations.Substring(0, t2PlayerRotations.Length - 2);
                    t2AngularVelocity = t2AngularVelocity.Substring(0, t2AngularVelocity.Length - 2);
                    t2Scores = t2Scores.Split(',', '.')[0];

                    //string t2tt = Encrypt(t2TrialTimes, k);
                    //string t2pr = Encrypt(t2PlayerRotations, k);
                    //string t2av = Encrypt(t2AngularVelocity, k);
                    //string t2s = Encrypt(t2Scores, k);

                    TextWriter tw6 = new StreamWriter(mainPath + userID + "_" + filePrefix + t2TTFile, true);
                    TextWriter tw7 = new StreamWriter(mainPath + userID + "_" + filePrefix + t2PRFile, true);
                    TextWriter tw8 = new StreamWriter(mainPath + userID + "_" + filePrefix + t2AVFile, true);
                    TextWriter tw9 = new StreamWriter(mainPath + userID + "_" + filePrefix + t2SFile, true);
                    //TextWriter tw6e = new StreamWriter(mainPath + userID + "_" + filePrefix + "_en" + t2TTFile, true);
                    //TextWriter tw7e = new StreamWriter(mainPath + userID + "_" + filePrefix + "_en" + t2PRFile, true);
                    //TextWriter tw8e = new StreamWriter(mainPath + userID + "_" + filePrefix + "_en" + t2AVFile, true);
                    //TextWriter tw9e = new StreamWriter(mainPath + userID + "_" + filePrefix + "_en" + t2SFile, true);

                    tw6.WriteLine(t2TrialTimes);
                    tw7.WriteLine(t2PlayerRotations);
                    tw8.WriteLine(t2AngularVelocity);
                    tw9.WriteLine(t2Scores);
                    //tw7e.WriteLine(t2pr);
                    //tw6e.WriteLine(t2tt);
                    //tw8e.WriteLine(t2av);
                    //tw9e.WriteLine(t2s);

                    tw6.Close();
                    tw7.Close();
                    tw8.Close();
                    tw9.Close();
                    //tw6e.Close();
                    //tw7e.Close();
                    //tw8e.Close();
                    //tw9e.Close();

                    t2TrialTimes = "";
                    t2PlayerRotations = "";
                    t2AngularVelocity = "";
                    t2Scores = "";
                }

            }

        }
        catch (Exception e)
        {
            Debug.Log("################Saving Data failed!###################\n" + e.ToString());
        }
    }

    private IEnumerator WaitForControllerSelection()
    {
        while (true)
        {
            if (OVRInput.Get(OVRInput.RawButton.RIndexTrigger))
            {
                yield return new WaitForSeconds(1);
                horizonSphere.SetActive(false);

                SelectController("right");

                uiHelpers.SetActive(true);
                infoText.text = explanationTasks[0];
                infoNext.gameObject.SetActive(true);
                infoPrev.gameObject.SetActive(false);
                infoHide.gameObject.SetActive(true);

                ResetViewAndPosition(false);
                yield return new WaitForEndOfFrame();
                ResetViewAndPosition(true);

                StopAllCoroutines();
            }
            else if (OVRInput.Get(OVRInput.RawButton.LIndexTrigger))
            {
                yield return new WaitForSeconds(1);
                horizonSphere.SetActive(false);

                SelectController("left");

                uiHelpers.SetActive(true);
                infoText.text = explanationTasks[0];
                infoNext.gameObject.SetActive(true);
                infoPrev.gameObject.SetActive(false);
                infoHide.gameObject.SetActive(true);

                ResetViewAndPosition(false);
                yield return new WaitForEndOfFrame();
                ResetViewAndPosition(true);

                StopAllCoroutines();
            }
        }
    }

    private void SelectController(string hand)
    {
        mainHand = hand;
        controllerSelected = true;

        if (hand.Equals("right"))
        {
            eventSystemPoint.gameObject.GetComponent<OVRInputModule>().rayTransform = rightHand.transform;
            GetComponent<Pointing_Discrete_Selection>().hand = rightHand;
            GetComponent<Joystick_Discrete_Selection_Global>().hand = rightHand;
        }
        else if (hand.Equals("left"))
        {
            eventSystemPoint.gameObject.GetComponent<OVRInputModule>().rayTransform = leftHand.transform;
            GetComponent<Pointing_Discrete_Selection>().hand = leftHand;
            GetComponent<Joystick_Discrete_Selection_Global>().hand = leftHand;
        }
        explanationTasks[0] = "Your controller has been selected. From now on the " + mainHand + " controller will be refered to as \"main controller\".\nUse the arrows below to navigate through the instructions.\nPress buttons by using the trigger button on your main controller.";
        uiHelpers.SetActive(true); 
    }

    private void ButtonPress(int i)
    {
        if (userIDText.text.Length < 4)
        {
            userIDText.text += i + "";
        }
        buttonDelete.gameObject.SetActive(true);
        if (userIDText.text.Length == 4)
        {
            buttonEnter.gameObject.SetActive(true);
        }
    }

    private IEnumerator CalibrationStop()
    {
        yield return new WaitForSeconds(4);
        calibrationScript.EndCalibration();
    }

    public void ResetViewAndPosition(bool newAxis)
    {
        transform.position = Vector3.zero;
        transform.rotation = Quaternion.Euler(0, 0, 0);
        cameraRig.transform.localPosition = Vector3.zero;
        cameraRig.transform.rotation = Quaternion.Euler(Vector3.zero);
        cameraRig.transform.rotation = Quaternion.Euler(0, 360 - pointOfReference.transform.rotation.eulerAngles.y, 0);
        cameraRig.transform.position = new Vector3(-1 * pointOfReference.transform.position.x, 0, -1 * pointOfReference.transform.position.z);
        if (newAxis)
        {
            rotationSphere.transform.position = pointOfReference.transform.position;
        }
    }

    public void SortByArray(List<string> list, int[] array)
    {
        List<string> copy = new List<string>();
        foreach (int i in array)
        {
            copy.Add(list[i]);
        }
        list.Clear();
        list.AddRange(copy);
    }

    public void SortByArray(List<int> list, int[] array)
    {
        List<int> copy = new List<int>();
        foreach (int i in array)
        {
            copy.Add(list[i]);
        }
        list.Clear();
        list.AddRange(copy);
    }
    /*
    private string Encrypt(string data, int key)
    {
        StringBuilder input = new StringBuilder(data);
        StringBuilder output = new StringBuilder(data.Length);

        char character;
        for (int i = 0; i < data.Length; i++)
        {
            character = input[i];
            character = (char)(character ^ key);
            output.Append(character);
        }

        return output.ToString();
    }
    */
}
