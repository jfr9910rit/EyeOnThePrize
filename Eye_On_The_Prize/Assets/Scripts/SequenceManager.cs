using System;
using System.Runtime.InteropServices;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;



//go nuclear, make into instantiate
public class SequenceManager : MonoBehaviour
{
    
    public GameObject[] OriginalSequence;

    public GameObject[] shape1, shape2, shape3, shape4, shape5;

    private float[] probabilities;
    private int lastSelected = -1;

    private GameObject[][] shapes;
    private float timer = 0f;
    
    private int index = 0;
    private bool activationStarted = false;

    void Start()
    {
        probabilities = new float[] { 25f, 25f, 25f, 25f };
        for (int s = 1; s < 6; s++)
        {
            List<GameObject[]> shapes = new List<GameObject[]> { shape1, shape2, shape3, shape4, shape5 };

            foreach (GameObject[] shape in shapes)
            {
                foreach (GameObject obj in shape)
                {
                    obj.SetActive(false);
                }
            }
        }
        shapes = new GameObject[][] { shape1, shape2, shape3, shape4, shape5 };
        OriginalSequence = new GameObject[shapes.Length];
        activationStarted = true;
        ActivateNextShape();
    }

    void Update()
    {

        if (!activationStarted || index >= shapes.Length)
        {
            activationStarted = false;
        }

        // Timer-based delay
        timer += Time.deltaTime;
        

        if (timer >= 0.5f && activationStarted == true) //experiment with order/speed
        {
            timer = 0f;
            ActivateNextShape();
        }


    }

    //actually creates the sequence
    void ActivateNextShape()
    {
        if (index >= shapes.Length) return;

        GameObject[] shapeGroup = shapes[index];

        if (shapeGroup.Length == 0) return;

        // Select a random index based on weighted probability
        int randomIndex = GetWeightedRandomIndex(shapeGroup.Length);

        // Activate the selected object
        shapeGroup[randomIndex].SetActive(true);

        // Store in OriginalSequence
        OriginalSequence[index] = shapeGroup[randomIndex];

        // Update probabilities based on selection
        UpdateProbabilities(randomIndex, shapeGroup.Length);

        index++; // Move to the next shape group
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

        for (int i = 0; i < shapeCount; i++)
        {
            if (i != selected && i != lastSelected)
            {
                probabilities[i] = remaining / countOther;
            }
        }

        lastSelected = selected; // Store last choice
    }
}
