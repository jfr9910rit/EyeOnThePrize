using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Globalization;
using System.Runtime.InteropServices;
using UnityEngine.SceneManagement;
using System.Collections.Generic;



public class SequenceChecker : MonoBehaviour
{
    public static SequenceChecker Instance;

    public TextMeshProUGUI Result;
    //public TextMeshProUGUI Timer;
    //private float mainTimer = 25f;
    //private float hideTime = 10f;
    //private GameObject[] shape1, shape2, shape3, shape4, shape5;
    //private GameObject[][] shapes;
    //private GameObject[] OriginalSequence;
    private float roundTime = 25f;
    private GameObject[,] userSequences;
    public int[,] userIndex;
    private int[,] userPoints;
    private Dictionary<string, string[]> userInputs;
    private SequenceManager sequenceManager;
    private bool isSequenceReady = false;
    //private bool canTakeInput = false;
    private bool[] canTakeInput2;
    private bool hasHiddenOriginalSequence = false;
    private bool isTimerRunning = false;  // Track if timer is still running
    bool[] hasCheckedSequence;
    private int[,] shapeX;
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
        userInputs = GameManager.Instance.playerInputs;
        playerTries = new int[GameManager.Instance.playerCount, 2];
        shapeX = new int[GameManager.Instance.playerCount, 2];
        canTakeInput2 = new bool[GameManager.Instance.playerCount];
        hasCheckedSequence = new bool[GameManager.Instance.playerCount];
        roundTime = 25f - ((float)GameManager.Instance.difficultyLevel * 5f);
        //if (GameManger.Instance.difficultyLevel == 1)
        //{
        //    roundTime = 25f;
        //}
        //else
        //{
        //    roundTime = 25f - ((float)GameManger.Instance.difficultyLevel * 5f);
        //}
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
                    if (sequenceManager.OriginalSequences[0, i] == null)
                    {
                        sequencePopulated = false;
                        break;
                    }
                }

                if (sequencePopulated)
                {


                    isSequenceReady = true;


                    for (int i = 0; i < GameManager.Instance.playerCount; i++)
                    {
                        canTakeInput2[i] = true;
                        Debug.Log(canTakeInput2[i]);
                    }
                    //canTakeInput = true;

                    GameManager.Instance.isTimerRunning = true;
                }
            }
        }

        // Hide the original sequence after 10 seconds
        if (/*isSequenceReady &&*/ !hasHiddenOriginalSequence && GameManager.Instance.gameTimer <= (roundTime - GameManager.Instance.hideTime))
        {
            //Debug.Log("hidden orig sequence");
            for (int i = 0; i < GameManager.Instance.playerCount; i++)
            {
                HideOriginalSequence(i);
                //Debug.Log("hidden orig sequence");
            }


        }

        // Timer countdown and stopping at 0
        if (isTimerRunning)
        {
            //GameManager.Instance.gameTimer -= Time.deltaTime;
            if (GameManager.Instance.gameTimer <= 0)
            {

                GameManager.Instance.isTimerRunning = false;

                //check all 3
                for (int i = 0; i >= GameManager.Instance.playerCount; i++)
                {
                    canTakeInput2[i] = false;
                    CheckSequence(i);
                }

                SceneManager.LoadScene("EndScene");

            }

        }

        //need to make player dependant
        // Block input if sequence isn't ready or if userSequences is full 
        //seperate
        //for (int i = 0; i >= GameManager.Instance.playerCount; i++)
        //{
        //    if (!canTakeInput2[i] || userIndex[i, 1] >= sequenceManager.shapeCount) return;
        //}






        //prob can turn into a function later
        // Listen for input
        if (Input.GetButtonDown(userInputs["Player_1"][0]) && canTakeInput2[0] && playerTries[0, 1] < 3)
        {
            if (userIndex[0, 1] == 0) HideOriginalSequence(0);
            AddTouserSequences(0, 0);
        }
        else if (Input.GetButtonDown(userInputs["Player_1"][1]) && canTakeInput2[0] && playerTries[0, 1] < 3)
        {
            if (userIndex[0, 1] == 0) HideOriginalSequence(0);
            AddTouserSequences(0, 1);
        }
        else if (Input.GetButtonDown(userInputs["Player_1"][2]) && canTakeInput2[0] && playerTries[0, 1] < 3)
        {
            if (userIndex[0, 1] == 0) HideOriginalSequence(0);
            AddTouserSequences(0, 2);
        }
        else if (Input.GetButtonDown(userInputs["Player_1"][3]) && canTakeInput2[0] && playerTries[0, 1] < 3)
        {
            if (userIndex[0, 1] == 0) HideOriginalSequence(0);
            AddTouserSequences(0, 3);
        }
        else if (Input.GetButtonDown(userInputs["Player_2"][0]) && canTakeInput2[1] && playerTries[1, 1] < 3)
        {
            if (userIndex[1, 1] == 0) HideOriginalSequence(1);
            AddTouserSequences(1, 0);
        }
        else if (Input.GetButtonDown(userInputs["Player_2"][1]) && canTakeInput2[1] && playerTries[1, 1] < 3)
        {
            if (userIndex[1, 1] == 0) HideOriginalSequence(1);
            AddTouserSequences(1, 1);
        }
        else if (Input.GetButtonDown(userInputs["Player_2"][2]) && canTakeInput2[1] && playerTries[1, 1] < 3)
        {
            if (userIndex[1, 1] == 0) HideOriginalSequence(1);
            AddTouserSequences(1, 2);
        }
        else if (Input.GetButtonDown(userInputs["Player_2"][3]) && canTakeInput2[1] && playerTries[1, 1] < 3)
        {
            if (userIndex[1, 1] == 0) HideOriginalSequence(1);
            AddTouserSequences(1, 3);
        }
        else if (Input.GetButtonDown(userInputs["Player_3"][0]) && canTakeInput2[2] && playerTries[2, 1] < 3)
        {
            if (userIndex[2, 1] == 0) HideOriginalSequence(2);
            AddTouserSequences(2, 0);
        }
        else if (Input.GetButtonDown(userInputs["Player_3"][1]) && canTakeInput2[2] && playerTries[2, 1] < 3)
        {
            if (userIndex[2, 1] == 0) HideOriginalSequence(2);
            AddTouserSequences(2, 1);
        }
        else if (Input.GetButtonDown(userInputs["Player_3"][2]) && canTakeInput2[2] && playerTries[2, 1] < 3)
        {
            if (userIndex[2, 1] == 0) HideOriginalSequence(2);
            AddTouserSequences(2, 2);
        }
        else if (Input.GetButtonDown(userInputs["Player_3"][3]) && canTakeInput2[2] && playerTries[2, 1] < 3)
        {
            if (userIndex[2, 1] == 0) HideOriginalSequence(2);
            AddTouserSequences(2, 3);
        }

        for (int i = 0; i >= GameManager.Instance.playerCount; i++)
        {
            // Stop taking input immediately once user finishes their sequence
            if (userIndex[i, 1] >= sequenceManager.shapeCount)
            {
                canTakeInput2[i] = false;
                //isTimerRunning = false; // change to make it so that player time is tracked unless all players finish
                CheckSequence(i);
            }
        }



    }


    void AddTouserSequences(int playerInt, int shapeInt)
    {
        if (userIndex[playerInt, 1] >= sequenceManager.shapeCount || !canTakeInput2[playerInt])
        {
            return;
        }

        if (userIndex[playerInt, 1] == 0) shapeX[playerInt, 1] = -600;
        GameObject selectedShape = Instantiate(sequenceManager.shapes[shapeInt], new Vector3(shapeX[playerInt, 1], 0, -5), Quaternion.identity);

        if (GameManager.Instance.playerCount == 1)
        {
            selectedShape.transform.SetParent(sequenceManager.seqs[0], false);
        }
        else if (GameManager.Instance.playerCount == 2)
        {
            if (playerInt == 0)
            {
                selectedShape.transform.SetParent(sequenceManager.seqs[1], false);
            }
            else if (playerInt == 1)
            {
                selectedShape.transform.SetParent(sequenceManager.seqs[2], false);
            }
        }
        else if (GameManager.Instance.playerCount == 3)
        {
            if (playerInt == 0)
            {
                selectedShape.transform.SetParent(sequenceManager.seqs[3], false);
            }
            else if (playerInt == 1)
            {
                selectedShape.transform.SetParent(sequenceManager.seqs[4], false);
            }
            else if (playerInt == 2)
            {
                selectedShape.transform.SetParent(sequenceManager.seqs[5], false);
            }
        }



        if (selectedShape != null)
        {
            userSequences[playerInt, userIndex[playerInt, 1]] = selectedShape;
            //selectedShape.SetActive(true);
            userIndex[playerInt, 1]++;
            //Debug.Log(userIndex[playerInt, 1]);

            shapeX[playerInt, 1] += (300 - GameManager.Instance.difficultyLevel * 50); //turn 300 to variable

            if (userIndex[playerInt, 1] >= sequenceManager.shapeCount)
            {
                canTakeInput2[playerInt] = false;
                //isTimerRunning = false; // change to make it so that player time is tracked unless all players finish
                CheckSequence(playerInt);
            }

        }

    }


    void HideOriginalSequence(int playerInt)
    {
        Debug.Log("Hiding Original Sequence!");
        for (int s = 0; s < sequenceManager.shapeCount; s++)
        {
            sequenceManager.OriginalSequences[playerInt, s].SetActive(false);
        }
        hasHiddenOriginalSequence = true;
    }




    //make per player and also assign points to player
    void CheckSequence(int playerInt)
    {
        if (hasCheckedSequence[playerInt]) return;
        hasCheckedSequence[playerInt] = true;  // Mark as checked
        bool isCorrect = true;

        for (int i = 0; i < sequenceManager.shapeCount; i++)
        {
            if (userSequences[playerInt, i] == null || sequenceManager.OriginalSequences[playerInt, i] == null || userSequences[playerInt, i].name != sequenceManager.OriginalSequences[playerInt, i].name)
            {
                isCorrect = false;
                break;
            }
        }

        if (isCorrect)
        {
            Debug.Log("Correct Sequence!");
            Result.text = "Correct!";
            userPoints[playerInt, 1] += 1000; // Base points for correctness turn into variable later

            // Calculate bonus points based on remaining time
            if (GameManager.Instance.gameTimer > 0)
            {
                float bonusMultiplier = Mathf.Clamp(GameManager.Instance.gameTimer / (roundTime - 3f), 0f, 1f);
                int bonusPoints = Mathf.RoundToInt(1000 * bonusMultiplier / 10) * 10; // Round to nearest 10
                userPoints[playerInt, 1] += bonusPoints;
                GameManager.Instance.PlayerFinished(playerInt, userPoints[playerInt, 1]);
                for (int i = 0; i >= sequenceManager.shapeCount; i++)
                {
                    Destroy(sequenceManager.OriginalSequences[playerInt, i]);
                }
            }
            Debug.Log(userPoints[playerInt, 1]);

        }
        else
        {
            Result.text = "Incorrect!";
            Debug.Log("Incorrect Sequence. Try Again!");
            //add retry steps here
            playerTries[playerInt, 1]++;
            userIndex[playerInt, 1] = 0;
            // say whats wrong would be here
            for (int i = 0; i < sequenceManager.shapeCount; i++)
            {

                Destroy(userSequences[playerInt, i]);

            }

            //userSequences[playerInt,]
            canTakeInput2[playerInt] = true;
            hasCheckedSequence[playerInt] = false;
            if (playerTries[playerInt, 1] == 2)
            {
                GameManager.Instance.PlayerFinished(playerInt, userPoints[playerInt, 1]);
            }
        }

        PlayerPrefs.SetInt($"Player{playerInt}Score", userPoints[playerInt, 1]);
        PlayerPrefs.Save();


    }

}

