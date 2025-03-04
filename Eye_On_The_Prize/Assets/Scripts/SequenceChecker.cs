using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Globalization;


public class SequenceChecker : MonoBehaviour
{
    
    public TextMeshProUGUI Result;
    //public TextMeshProUGUI Timer;
    //private float mainTimer = 25f;
    private float hideTime = 10f;
    //private GameObject[] shape1, shape2, shape3, shape4, shape5;
    //private GameObject[][] shapes;
    //private GameObject[] OriginalSequence;
    private GameObject[,] userSequences;
    private int[,] userIndex;
    private int[,] userPoints;
    private SequenceManager sequenceManager;
    private bool isSequenceReady = false;
    private bool canTakeInput = false;
    private bool hasHiddenOriginalSequence = false;
    private bool isTimerRunning = false;  // Track if timer is still running
    private bool hasCheckedSequence = false;
    private int shapeX = -600;
    private int shapeY = 0;
    private int[,] playerTries;
    //make references and add in tries
    void Start()
    {
        sequenceManager = FindObjectOfType<SequenceManager>();
        
        //shape1 = sequenceManager.shape1;
        //shape2 = sequenceManager.shape2;
        //shape3 = sequenceManager.shape3;
        //shape4 = sequenceManager.shape4;
        //shape5 = sequenceManager.shape5;
        //shapes = new GameObject[][] { shape1, shape2, shape3, shape4, shape5 };
        //OriginalSequences = new GameObject[sequenceManager.shapeCount];
        userSequences = new GameObject[GameManager.Instance.playerCount, sequenceManager.shapeCount];
        userIndex = new int[GameManager.Instance.playerCount, 2];
        userPoints = new int[GameManager.Instance.playerCount, 2];
        playerTries = new int[GameManager.Instance.playerCount, 2];
    }

    void Update()
    {
        if (!isSequenceReady)
        {
            if (sequenceManager != null && sequenceManager.OriginalSequences != null)
            {
                bool sequencePopulated = true;

                for (int i = 0; i < sequenceManager.shapeCount; i++)
                {
                    if (sequenceManager.OriginalSequences[0,i] == null)
                    {
                        sequencePopulated = false;
                        break;
                    }
                }

                if (sequencePopulated)
                {
                    
                    
                    isSequenceReady = true;
                    canTakeInput = true;
                    GameManager.Instance.isTimerRunning = true;
                }
            }
        }

        // Hide the original sequence after 10 seconds
        if (isSequenceReady && !hasHiddenOriginalSequence && (25f - GameManager.Instance.gameTimer) >= hideTime)
        {
            for(int i = 0; i >= GameManager.Instance.playerCount; i++)
            {
                HideOriginalSequence(i);
            }
            
            
        }

        // Timer countdown and stopping at 0
        if (isTimerRunning)
        {
            GameManager.Instance.gameTimer -= Time.deltaTime;
            if (GameManager.Instance.gameTimer <= 0)
            {

                GameManager.Instance.isTimerRunning = false;
                canTakeInput = false;  // Stop user input
                //call 3x
                CheckSequence();  // Immediately check sequence
            }
            
        }
        //need to make player dependant
        // Block input if sequence isn't ready or if userSequences is full
        if (!canTakeInput || userIndex[0, 1] >= sequenceManager.shapeCount)
        {
            return;
        }
        //may need to change dependant on players
        
        //prob can turn into a function later
        // Listen for input
        if (Input.GetButtonDown("1"))
        {
            if (userIndex[0, 1] == 0) HideOriginalSequence(0);
            AddTouserSequences(0, 0);
        }
        else if (Input.GetButtonDown("2"))
        {
            if (userIndex[0, 1] == 0) HideOriginalSequence(0);
            AddTouserSequences(0, 1);
        }
        else if (Input.GetButtonDown("3"))
        {
            if (userIndex[0, 1] == 0) HideOriginalSequence(0);
            AddTouserSequences(0, 2);
        }
        else if (Input.GetButtonDown("4"))
        {
            if (userIndex[0, 1] == 0) HideOriginalSequence(0);
            AddTouserSequences(0, 3);
        }
        else if (Input.GetButtonDown("con1"))
        {
            if (userIndex[1, 1] == 0) HideOriginalSequence(1);
            AddTouserSequences(1, 0);
        }
        else if (Input.GetButtonDown("con2"))
        {
            if (userIndex[1, 1] == 0) HideOriginalSequence(1);
            AddTouserSequences(1, 1);
        }
        else if (Input.GetButtonDown("con3"))
        {
            if (userIndex[1, 1] == 0) HideOriginalSequence(1);
            AddTouserSequences(1, 2);
        }
        else if (Input.GetButtonDown("con4"))
        {
            if (userIndex[1, 1] == 0) HideOriginalSequence(1);
            AddTouserSequences(1, 3);
        }
        else if (Input.GetButtonDown("leftarrow"))
        {
            if (userIndex[2, 1] == 0) HideOriginalSequence(2);
            AddTouserSequences(2, 0);
        }
        else if (Input.GetButtonDown("uparrow"))
        {
            if (userIndex[2, 1] == 0) HideOriginalSequence(2);
            AddTouserSequences(2, 1);
        }
        else if (Input.GetButtonDown("rightarrow"))
        {
            if (userIndex[2, 1] == 0) HideOriginalSequence(2);
            AddTouserSequences(2, 2);
        }
        else if (Input.GetButtonDown("downarrow"))
        {
            if (userIndex[2, 1] == 0) HideOriginalSequence(2);
            AddTouserSequences(2, 3);
        }

        // Stop taking input immediately once user finishes their sequence
        if (userIndex[0, 1] >= sequenceManager.shapeCount)
        {
            canTakeInput = false;
            //isTimerRunning = false; // change to make so that its dependant on player
            CheckSequence();
        }
    }

    //add arguement for which player so it adds to individual arrays
    void AddTouserSequences(int playerInt, int shapeInt)
    {
        if (userIndex[playerInt, 1] >= sequenceManager.shapeCount || !canTakeInput)
        {
            return;
        }

        

        if (playerInt == 0 && GameManager.Instance.playerCount == 1)
        {
            if (userIndex[playerInt, 1] == 0) shapeX = -600;
            shapeY = 0;
            GameObject selectedShape = Instantiate(sequenceManager.shapes[shapeInt], new Vector3(shapeX, shapeY, -5), Quaternion.identity);
            selectedShape.transform.SetParent(sequenceManager.seqs[0], false);
            if (selectedShape != null)
            {
                userSequences[playerInt, userIndex[playerInt, 1]] = selectedShape;
                //selectedShape.SetActive(true);
                userIndex[playerInt,1]++;
                shapeX += 300;

                if (userIndex[playerInt, 1] >= sequenceManager.shapeCount)
                {
                    canTakeInput = false;
                    //isTimerRunning = false; // change to make it so that player time is tracked unless all players finish
                    CheckSequence();
                }
            }
        }
        else if (playerInt == 1)
        {

        }
        else if (playerInt == 2)
        {

        }


        //GameObject selectedShape = Instantiate(sequenceManager.shapes[shapeInt],);
        //if (selectedShape != null)
        //{
        //    userSequences[playerInt,userIndex] = selectedShape;
        //    //selectedShape.SetActive(true);
        //    userIndex++;

        //    if (userIndex >= sequenceManager.shapeCount)
        //    {
        //        canTakeInput = false;
        //        isTimerRunning = false; // change to make it so that player time is tracked unless all players finish
        //        CheckSequence();
        //    }
        //}
    }
    //same as above, also fix shapes
    //GameObject GetShapeByName(string shapeName, int index)
    //{
    //    if (index >= 0 && index < sequenceManager.shapeCount)
    //    {
    //        foreach (GameObject shape in shapes[index])
    //        {
    //            if (shape.name.ToLower() == shapeName.ToLower())
    //            {
    //                return shape;
    //            }
    //        }
    //    }
    //    return null;
    //}


    //make independant for player
    void HideOriginalSequence(int playerInt)
    {
        Debug.Log("Hiding Original Sequence!");
            for(int s = 0; s < sequenceManager.shapeCount; s++)
            {
                sequenceManager.OriginalSequences[playerInt, s].SetActive(false);
            }
        hasHiddenOriginalSequence = true;
    }

    //void DeactivateAllShapes()
    //{
    //    foreach (GameObject[] shapeArray in shapes)
    //    {
    //        foreach (GameObject shape in shapeArray)
    //        {
    //            shape.SetActive(false);
    //        }
    //    }
    //}
    //make per player and also assign points to player
    void CheckSequence()
    {
        if (hasCheckedSequence) return;  // Prevent multiple calls

        hasCheckedSequence = true;  // Mark as checked
        bool isCorrect = true;
        //if(GameManager.Instance.playerCount == 1)
        //{
        //    
        //}
        //else if (GameManager.Instance.playerCount == 2)
        //{
        //    
        //}
        //else if (GameManager.Instance.playerCount == 3)
        //{
        //    
        //}
        for (int i = 0; i < sequenceManager.shapeCount; i++)
        {
            if (userSequences[0,i] == null || sequenceManager.OriginalSequences[0,i] == null || userSequences[0,i].name != sequenceManager.OriginalSequences[0,i].name)
            {
                isCorrect = false;
                break;
            }
        }

        if (isCorrect)
        {
            Debug.Log("Correct Sequence!");
            Result.text = "Correct!";
            userPoints[0, 1] += 1000; // Base points for correctness

            // Calculate bonus points based on remaining time
            if (GameManager.Instance.gameTimer > 0)
            {
                float bonusMultiplier = Mathf.Clamp(GameManager.Instance.gameTimer / 22f, 0f, 1f);
                int bonusPoints = Mathf.RoundToInt(1000 * bonusMultiplier / 10) * 10; // Round to nearest 10
                userPoints[0, 1] += bonusPoints;
                 // Debug log for testing
            }
            Debug.Log(userPoints);
        }
        else
        {
            Result.text = "Incorrect!";
            Debug.Log("Incorrect Sequence. Try Again!");
            //add retry steps here
        }
    }


}
