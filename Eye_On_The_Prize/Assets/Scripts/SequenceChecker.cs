using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Globalization;
using System.Runtime.InteropServices;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using System.Linq;




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
    private SequenceManager sequenceManager;
    private bool isSequenceReady = false;
    //private bool canTakeInput = false;
    private bool[] canTakeInput2;
    private bool[] hasHiddenOriginalSequence;
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
        playerTries = new int[GameManager.Instance.playerCount, 2];
        shapeX = new int[GameManager.Instance.playerCount, 2];
        canTakeInput2 = new bool[GameManager.Instance.playerCount];
        hasCheckedSequence = new bool[GameManager.Instance.playerCount];
        roundTime = 25f - ((float)GameManager.Instance.difficultyLevel * 5f);
        hasHiddenOriginalSequence = new bool[GameManager.Instance.playerCount];

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
        if (/*isSequenceReady && !hasHiddenOriginalSequence &&*/ GameManager.Instance.gameTimer <= (roundTime - GameManager.Instance.hideTime))
        {
            //Debug.Log("hidden orig sequence");
            for (int i = 0; i < GameManager.Instance.playerCount; i++)
            {
                if (!hasHiddenOriginalSequence[i])
                {
                    HideOriginalSequence(i);
                }

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
        if (Input.GetButtonDown("2") && canTakeInput2[0] && playerTries[0, 1] < 3)
        {
            if (userIndex[0, 1] == 0) HideOriginalSequence(0);
            AddTouserSequences(0, 0);
        }
        else if (Input.GetButtonDown("1") && canTakeInput2[0] && playerTries[0, 1] < 3)
        {
            if (userIndex[0, 1] == 0) HideOriginalSequence(0);
            AddTouserSequences(0, 1);
        }
        else if (Input.GetButtonDown("3") && canTakeInput2[0] && playerTries[0, 1] < 3)
        {
            if (userIndex[0, 1] == 0) HideOriginalSequence(0);
            AddTouserSequences(0, 2);
        }
        else if (Input.GetButtonDown("4") && canTakeInput2[0] && playerTries[0, 1] < 3)
        {
            if (userIndex[0, 1] == 0) HideOriginalSequence(0);
            AddTouserSequences(0, 3);
        }
        else if (Input.GetButtonDown("con1") && canTakeInput2[1] && playerTries[1, 1] < 3)
        {
            if (userIndex[1, 1] == 0) HideOriginalSequence(1);
            AddTouserSequences(1, 0);
        }
        else if (Input.GetButtonDown("con2") && canTakeInput2[1] && playerTries[1, 1] < 3)
        {
            if (userIndex[1, 1] == 0) HideOriginalSequence(1);
            AddTouserSequences(1, 1);
        }
        else if (Input.GetButtonDown("con3") && canTakeInput2[1] && playerTries[1, 1] < 3)
        {
            if (userIndex[1, 1] == 0) HideOriginalSequence(1);
            AddTouserSequences(1, 2);
        }
        else if (Input.GetButtonDown("con4") && canTakeInput2[1] && playerTries[1, 1] < 3)
        {
            if (userIndex[1, 1] == 0) HideOriginalSequence(1);
            AddTouserSequences(1, 3);
        }
        else if (Input.GetButtonDown("leftarrow") && canTakeInput2[2] && playerTries[2, 1] < 3)
        {
            if (userIndex[2, 1] == 0) HideOriginalSequence(2);
            AddTouserSequences(2, 0);
        }
        else if (Input.GetButtonDown("uparrow") && canTakeInput2[2] && playerTries[2, 1] < 3)
        {
            if (userIndex[2, 1] == 0) HideOriginalSequence(2);
            AddTouserSequences(2, 1);
        }
        else if (Input.GetButtonDown("rightarrow") && canTakeInput2[2] && playerTries[2, 1] < 3)
        {
            if (userIndex[2, 1] == 0) HideOriginalSequence(2);
            AddTouserSequences(2, 2);
        }
        else if (Input.GetButtonDown("downarrow") && canTakeInput2[2] && playerTries[2, 1] < 3)
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
        //Debug.Log("Hiding Original Sequence!");
        for (int s = 0; s < sequenceManager.shapeCount; s++)
        {
            sequenceManager.OriginalSequences[playerInt, s].SetActive(false);
        }
        hasHiddenOriginalSequence[playerInt] = true;
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
            //Debug.Log("Correct Sequence!");
            Result.text = "Correct!";
            userPoints[playerInt, 1] += (1000 + (GameManager.Instance.difficultyLevel * 500)); // Base points for correctness

            // Calculate bonus points based on remaining time
            if (GameManager.Instance.gameTimer > 0)
            {
                float bonusMultiplier = Mathf.Clamp(GameManager.Instance.gameTimer / (roundTime - 3f), 0f, 1f);
                int bonusPoints = Mathf.RoundToInt(1000 * bonusMultiplier / 10) * 10; // Round to nearest 10
                if (GameManager.Instance.activeModifier == "2x Points")
                {
                    bonusPoints *= 2;
                }
                else if (GameManager.Instance.activeModifier == "1.5x Points")
                {
                    bonusPoints *= (int)1.5;
                }
                else if (GameManager.Instance.activeModifier == "Bonus Points")
                {
                    bonusPoints += 1000;
                }
                userPoints[playerInt, 1] += bonusPoints;
                GameManager.Instance.PlayerFinished(playerInt, userPoints[playerInt, 1]);
                for (int i = 0; i >= sequenceManager.shapeCount; i++)
                {
                    Destroy(sequenceManager.OriginalSequences[playerInt, i]);
                }
            }
            //Debug.Log(userPoints[playerInt, 1]);

        }
        else
        {
            Result.text = "Incorrect!";
            //Debug.Log("Incorrect Sequence. Try Again!");

            playerTries[playerInt, 1]++;
            userIndex[playerInt, 1] = 0;

            GameObject[] feedbackObjects = new GameObject[sequenceManager.shapeCount];
            if (playerTries[playerInt, 1] == 3)
            {
                GameManager.Instance.PlayerFinished(playerInt, userPoints[playerInt, 1]);
            }
            StartCoroutine(ShowFeedbackSequence(playerInt));


        }


        IEnumerator ShowFeedbackSequence(int playerInt)
        {
            for (int i = 0; i < sequenceManager.shapeCount; i++)
            {
                // Destroy previous user input shape if it exists
                if (userSequences[playerInt, i] != null)
                {
                    Destroy(userSequences[playerInt, i]);
                }

                // Get the position of the original shape
                Vector3 originalPos = sequenceManager.OriginalSequences[playerInt, i].transform.position;

                // Debugging: Log the shape names for both user and original
                string playerShapeName = userSequences[playerInt, i] != null
                    ? userSequences[playerInt, i].name.Split('(')[0].Trim()
                    : "null";
                string originalShapeName = sequenceManager.OriginalSequences[playerInt, i].name.Split('(')[0].Trim();

                Debug.Log($"Index {i}: Player Shape: {playerShapeName}, Original Shape: {originalShapeName}");

                // Determine which shape to show
                GameObject feedbackShape = null;
                bool isCorrectShape = false;

                if (userSequences[playerInt, i] != null)
                {
                    // Check if the player's input matches the original shape
                    isCorrectShape = playerShapeName == originalShapeName;
                }

                if (isCorrectShape)
                {
                    // If the shape is correct, instantiate the original shape
                    //Debug.Log($"Correct shape for index {i}: {originalShapeName}");
                    feedbackShape = Instantiate(sequenceManager.OriginalSequences[playerInt, i], originalPos, Quaternion.identity);
                    feedbackShape.SetActive(true);
                }
                else if (userSequences[playerInt, i] != null)
                {
                    // If the shape is incorrect, instantiate the X version of the user's shape
                    string baseName = userSequences[playerInt, i].name.Split('(')[0].Trim();
                    GameObject xVersion = sequenceManager.shapesWithX
                        .FirstOrDefault(go => go.name.StartsWith(baseName));

                    if (xVersion != null)
                    {
                        //Debug.Log($"Incorrect shape for index {i}, replacing with X version of: {baseName}");
                        feedbackShape = Instantiate(xVersion, originalPos, Quaternion.identity);
                    }
                    else
                    {
                        //Debug.LogWarning($"No X version found for shape {baseName}, using original");
                        feedbackShape = sequenceManager.OriginalSequences[playerInt, i]; // Fallback to the original shape
                    }
                }

                int seqNum = 0;
                // Set the parent and store the shape
                if (feedbackShape != null)
                {
                    if(GameManager.Instance.playerCount == 1)
                    {
                        seqNum = 0;
                    }
                    else if(GameManager.Instance.playerCount == 2)
                    {
                        if(playerInt == 0)
                        {
                            seqNum = 1;
                        }
                        else if(playerInt == 1)
                        {
                            seqNum = 2;
                        }
                    }
                    else if(GameManager.Instance.playerCount == 3)
                    {
                        if (playerInt == 0)
                        {
                            seqNum = 3;
                        }
                        else if (playerInt == 1)
                        {
                            seqNum = 4;
                        }
                        else if(playerInt == 2)
                        {
                            seqNum = 5;
                        }
                    }
                    feedbackShape.transform.SetParent(sequenceManager.seqs[seqNum], true);
                    userSequences[playerInt, i] = feedbackShape; // Store feedback shape in the sequence
                }
                else
                {
                    //Debug.LogError($"Failed to instantiate feedback shape at index {i}");
                }
            }

            // Wait for 1 second before shaking and destroying feedback shapes
            yield return new WaitForSeconds(1f);

            // Shake and destroy all feedback shapes
            for (int i = 0; i < sequenceManager.shapeCount; i++)
            {
                if (userSequences[playerInt, i] != null)
                {
                    StartCoroutine(ShakeAndDestroy(userSequences[playerInt, i]));
                }
            }

            // Wait for 0.5 seconds before resetting the sequence check
            yield return new WaitForSeconds(0.5f);
            hasCheckedSequence[playerInt] = false;
            userIndex[playerInt, 1] = 0;
            canTakeInput2[playerInt] = true;
        }






        IEnumerator ShakeAndDestroy(GameObject obj)
        {
            Vector3 originalPos = obj.transform.localPosition;
            float shakeAmount = 10f;
            float duration = 0.3f;

            float elapsed = 0f;
            while (elapsed < duration)
            {
                float x = Random.Range(-1f, 1f) * shakeAmount;
                float y = Random.Range(-1f, 1f) * shakeAmount;

                obj.transform.localPosition = originalPos + new Vector3(x, y, 0);
                elapsed += Time.deltaTime;
                yield return null;
            }

            Destroy(obj);
        }


    }
}