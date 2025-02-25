using System;
using System.Runtime.InteropServices;
using UnityEngine;
using System.Collections.Generic;

public class SequenceManager : MonoBehaviour
{
    
    public GameObject[] OriginalSequence;

    public GameObject[] shape1, shape2, shape3, shape4, shape5;

    private GameObject[][] shapes;
    private float timer = 0f;
    private int index = 0;
    private bool activationStarted = false;

    void Start()
    {
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
            return;

        // Timer-based delay
        timer += Time.deltaTime;
        if (timer >= 1f)
        {
            timer = 0f;
            ActivateNextShape();
        }

        if (index >= shapes.Length)
        {
            if (index == shapes.Length)
            {
                // Print the sequence when all activations are complete
                PrintOriginalSequence();
            }
            return;
        }

    }

    void ActivateNextShape()
    {
        if (index >= shapes.Length) return;

        GameObject[] shape = shapes[index];

        if (shape.Length > 0)
        {
            

            // Select and activate a random object
            int randomIndex = UnityEngine.Random.Range(0, shape.Length);
            shape[randomIndex].SetActive(true);

            // Store the selected object in OriginalSequence
            OriginalSequence[index] = shape[randomIndex];
        }

        index++; // Move to the next shape array after processing the current one
    }

    void PrintOriginalSequence()
    {
        Debug.Log("Original Sequence of Activated Objects:");
        for (int i = 0; i < OriginalSequence.Length; i++)
        {
            if (OriginalSequence[i] != null)
            {
                Debug.Log($"Shape {i + 1}: {OriginalSequence[i].name}");
            }
            else
            {
                Debug.Log($"Shape {i + 1}: No object was activated.");
            }
        }
    }
}
