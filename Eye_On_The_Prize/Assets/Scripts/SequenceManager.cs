using System;
using System.Runtime.InteropServices;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;



//go nuclear, make into instantiate
public class SequenceManager : MonoBehaviour
{
    
    public GameObject[,] OriginalSequences;

    //public GameObject[] shape1, shape2, shape3, shape4, shape5;

    private float[] probabilities;
    private int lastSelected = -1;
    public GameObject[] shapes;
    //private GameObject[][] shapes;
    private float timer = 0f;
    private float delayTime = 0.5f;
    private int index = 0;
    private bool activationStarted = false;
    private int playerCnt;
    private int diffLevel;
    public int shapeCount = 5;
    private int shapeX = -600;
    public Transform[] seqs;

    void Start()
    {
        playerCnt = GameManager.Instance.playerCount;
        diffLevel = GameManager.Instance.difficultyLevel;
        probabilities = new float[] { 25f, 25f, 25f, 25f };
        //oldCode
        //for (int s = 1; s < 6; s++)
        //{
        //    List<GameObject[]> shapes = new List<GameObject[]> { shape1, shape2, shape3, shape4, shape5 };

        //    foreach (GameObject[] shape in shapes)
        //    {
        //        foreach (GameObject obj in shape)
        //        {
        //            obj.SetActive(false);
        //        }
        //    }
        //}
        //shapes = new GameObject[][] { shape1, shape2, shape3, shape4, shape5 };

        OriginalSequences = new GameObject[playerCnt,shapeCount];

            activationStarted = true;
        ActivateNextShape();
    }

    void Update()
    {

        if (!activationStarted || index >= shapeCount)
        {
            activationStarted = false;
        }

        // Timer-based delay
        timer += Time.deltaTime;
        

        if (timer >= delayTime && activationStarted == true) //experiment with order/speed
        {
            timer = 0f;
            ActivateNextShape();
        }


    }

    //actually creates the sequence
    void ActivateNextShape()
    {
        if (index >= shapeCount) return;
        if (playerCnt == 1)
        { 
           int randomIndex = GetWeightedRandomIndex(index);
           OriginalSequences[0,index] = Instantiate(shapes[randomIndex], new Vector3(shapeX, 0, -5), Quaternion.identity);
           OriginalSequences[0,index].transform.SetParent(seqs[0], false);
           shapeX += 300;
           UpdateProbabilities(randomIndex, shapeCount);
        }
        else if(playerCnt == 2)
        {
            int randomIndex = GetWeightedRandomIndex(index);
            OriginalSequences[0,index] = Instantiate(shapes[randomIndex], new Vector3(shapeX, 0, -5), Quaternion.identity);
            OriginalSequences[0,index].transform.SetParent(seqs[1], false);
            randomIndex = GetWeightedRandomIndex(index);
            OriginalSequences[1, index] = Instantiate(shapes[randomIndex], new Vector3(shapeX, 0, -5), Quaternion.identity);
            OriginalSequences[1, index].transform.SetParent(seqs[2], false);
            shapeX += 300;
            UpdateProbabilities(randomIndex, shapeCount);
        }
        else if(playerCnt == 3)
        {
            int randomIndex = GetWeightedRandomIndex(index);
            OriginalSequences[0, index] = Instantiate(shapes[randomIndex], new Vector3(shapeX, 0, -5), Quaternion.identity);
            OriginalSequences[0, index].transform.SetParent(seqs[3], false);
            randomIndex = GetWeightedRandomIndex(index);
            OriginalSequences[1, index] = Instantiate(shapes[randomIndex], new Vector3(shapeX, 0, -5), Quaternion.identity);
            OriginalSequences[1, index].transform.SetParent(seqs[4], false);
            randomIndex = GetWeightedRandomIndex(index);
            OriginalSequences[2, index] = Instantiate(shapes[randomIndex], new Vector3(shapeX, 0, -5), Quaternion.identity);
            OriginalSequences[2, index].transform.SetParent(seqs[5], false);
            shapeX += 300;
            UpdateProbabilities(randomIndex, shapeCount);
        }

        index++;
        
    }

    int GetWeightedRandomIndex(int shapeCount)
    {
        float total = 0;
        foreach (float p in probabilities)
            total += p;

        float randomPoint = UnityEngine.Random.Range(0, total);
        float cumulative = 0f;

        for (int i = 0; i < shapeCount; i++)
        {
            cumulative += probabilities[i];
            if (randomPoint <= cumulative)
            {
                return i;
            }
        }

        return 0; // Failsafe
    }

    void UpdateProbabilities(int selected, int shapeCount)
    {
        if (lastSelected == selected) // If the same shape is selected again
        {
            probabilities[selected] /= 2f;
        }
        else
        {
            if (lastSelected != -1)
            {
                probabilities[lastSelected] = 12.5f; // Restore the previous selection to half of its original 25%
            }

            probabilities[selected] /= 2f; // Halve the new selection
        }

        // Recalculate the remaining probability pool
        float remaining = 100f - probabilities[selected] - (lastSelected != -1 ? probabilities[lastSelected] : 0);
        int countOther = shapeCount - (lastSelected == -1 ? 1 : 2); // Remaining non-selected options

        if (countOther <= 0)
        {
            countOther = 1;  // Prevent division by zero
        }

        // Only loop within bounds of probabilities
        for (int i = 0; i < Mathf.Min(shapeCount, probabilities.Length); i++)
        {
            if (i != selected && i != lastSelected)
            {
                probabilities[i] = remaining / countOther;
            }
        }

        lastSelected = selected; // Store last choice
    }

}
