using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Globalization;


public class SequenceChecker : MonoBehaviour
{
    public string Button1 = "";
    public string Button2 = "";
    public string Button3 = "";
    public string Button4 = "";
    public TextMeshProUGUI Result;
    public TextMeshProUGUI Timer;
    private float mainTimer = 25f;
    private float hideTime = 10f;
    private GameObject[] shape1, shape2, shape3, shape4, shape5;
    private GameObject[][] shapes;
    private GameObject[] OriginalSequence;
    private GameObject[] userSequence;
    private int userIndex = 0;
    private int userPoints = 0;
    private SequenceManager sequenceManager;
    private bool isSequenceReady = false;
    private bool canTakeInput = false;
    private bool hasHiddenOriginalSequence = false;
    private bool isTimerRunning = false;  // Track if timer is still running
    private bool hasCheckedSequence = false;

    void Start()
    {
        sequenceManager = FindObjectOfType<SequenceManager>();
        
        shape1 = sequenceManager.shape1;
        shape2 = sequenceManager.shape2;
        shape3 = sequenceManager.shape3;
        shape4 = sequenceManager.shape4;
        shape5 = sequenceManager.shape5;
        shapes = new GameObject[][] { shape1, shape2, shape3, shape4, shape5 };
        OriginalSequence = new GameObject[shapes.Length];
        userSequence = new GameObject[shapes.Length];
    }

    void Update()
    {
        if (!isSequenceReady)
        {
            if (sequenceManager != null && sequenceManager.OriginalSequence != null)
            {
                bool sequencePopulated = true;

                for (int i = 0; i < sequenceManager.OriginalSequence.Length; i++)
                {
                    if (sequenceManager.OriginalSequence[i] == null)
                    {
                        sequencePopulated = false;
                        break;
                    }
                }

                if (sequencePopulated)
                {
                    OriginalSequence = sequenceManager.OriginalSequence;
                    isSequenceReady = true;
                    canTakeInput = true;
                    isTimerRunning = true;
                }
            }
        }

        // Hide the original sequence after 10 seconds
        if (isSequenceReady && !hasHiddenOriginalSequence && (30f - mainTimer) >= hideTime)
        {
            HideOriginalSequence();
            hasHiddenOriginalSequence = true;
        }

        // Timer countdown and stopping at 0
        if (isTimerRunning)
        {
            mainTimer -= Time.deltaTime;
            if (mainTimer <= 0)
            {
                mainTimer = 0;
                isTimerRunning = false;
                canTakeInput = false;  // Stop user input
                CheckSequence();  // Immediately check sequence
            }
            Timer.text = Mathf.RoundToInt(mainTimer).ToString();
        }

        // Block input if sequence isn't ready or if userSequence is full
        if (!canTakeInput || userIndex >= shapes.Length)
        {
            return;
        }

        // Listen for input
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            AddToUserSequence("Circle");
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            AddToUserSequence("Square");
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            AddToUserSequence("Triangle");
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            AddToUserSequence("Star");
        }

        // Stop taking input immediately once user finishes their sequence
        if (userIndex >= shapes.Length)
        {
            canTakeInput = false;
            isTimerRunning = false; // Stop timer if user finishes early
            CheckSequence();
        }
    }

    void AddToUserSequence(string shapeName)
    {
        if (userIndex >= shapes.Length || !canTakeInput)
        {
            return;
        }

        if (userIndex == 0)
        {
            DeactivateAllShapes();
        }

        GameObject selectedShape = GetShapeByName(shapeName, userIndex);
        if (selectedShape != null)
        {
            userSequence[userIndex] = selectedShape;
            selectedShape.SetActive(true);
            userIndex++;

            if (userIndex >= shapes.Length)
            {
                canTakeInput = false;
                isTimerRunning = false; // Stop timer early if user finishes
                CheckSequence();
            }
        }
    }

    GameObject GetShapeByName(string shapeName, int index)
    {
        if (index >= 0 && index < shapes.Length)
        {
            foreach (GameObject shape in shapes[index])
            {
                if (shape.name.ToLower() == shapeName.ToLower())
                {
                    return shape;
                }
            }
        }
        return null;
    }

    void HideOriginalSequence()
    {
        Debug.Log("Hiding Original Sequence!");
        foreach (GameObject shape in OriginalSequence)
        {
            if (shape != null)
            {
                shape.SetActive(false);
            }
        }
    }

    void DeactivateAllShapes()
    {
        foreach (GameObject[] shapeArray in shapes)
        {
            foreach (GameObject shape in shapeArray)
            {
                shape.SetActive(false);
            }
        }
    }

    void CheckSequence()
    {
        if (hasCheckedSequence) return;  // Prevent multiple calls

        hasCheckedSequence = true;  // Mark as checked
        bool isCorrect = true;

        for (int i = 0; i < shapes.Length; i++)
        {
            if (userSequence[i] == null || OriginalSequence[i] == null || userSequence[i].name != OriginalSequence[i].name)
            {
                isCorrect = false;
                break;
            }
        }

        if (isCorrect)
        {
            Debug.Log("Correct Sequence!");
            Result.text = "Correct!";
            userPoints += 1000; // Base points for correctness

            // Calculate bonus points based on remaining time
            if (mainTimer > 0)
            {
                float bonusMultiplier = Mathf.Clamp(mainTimer / 27f, 0f, 1f);
                int bonusPoints = Mathf.RoundToInt(1000 * bonusMultiplier / 10) * 10; // Round to nearest 10
                userPoints += bonusPoints;
                 // Debug log for testing
            }
            Debug.Log(userPoints);
        }
        else
        {
            Result.text = "Incorrect!";
            Debug.Log("Incorrect Sequence. Try Again!");
        }
    }


}
