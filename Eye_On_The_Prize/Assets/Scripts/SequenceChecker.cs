using UnityEngine;

public class SequenceChecker : MonoBehaviour
{
    public GameObject[] shape1, shape2, shape3, shape4, shape5; // Arrays for the shapes
    private GameObject[][] shapes;
    private GameObject[] OriginalSequence; // From the previous script
    private GameObject[] userSequence;
    private int userIndex = 0;

    private SequenceManager sequenceManager;
    private bool hasStarted = false; // To track if user input has started

    void Start()
    {
        sequenceManager = FindObjectOfType<SequenceManager>();

        // Initialize the shapes array (same as the previous script)
        shapes = new GameObject[][] { shape1, shape2, shape3, shape4, shape5 };
        OriginalSequence = new GameObject[shapes.Length]; // This will be filled by sequenceManager

        // Initialize the user sequence array
        userSequence = new GameObject[shapes.Length];
    }

    void Update()
    {
        // Ensure OriginalSequence is populated by sequenceManager before accepting user input
        if (sequenceManager != null && sequenceManager.OriginalSequence != null)
        {
            // Wait until sequenceManager has finished populating the OriginalSequence
            if (OriginalSequence.Length == 0 || OriginalSequence[0] == null)
            {
                // Get the populated OriginalSequence from sequenceManager
                OriginalSequence = sequenceManager.OriginalSequence;
            }
        }

        // Listen for the key presses (1-4)
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

        // Check if user has completed their sequence
        if (userIndex >= shapes.Length)
        {
            CheckSequence();
        }
    }

    void AddToUserSequence(string shapeName)
    {
        if (userIndex >= shapes.Length)
        {
            return; // Stop adding if sequence is full
        }

        // Only deactivate all shapes once, on the first input
        if (!hasStarted)
        {
            DeactivateAllShapes();
            hasStarted = true;
        }

        // Add the selected shape to the user sequence array
        GameObject selectedShape = GetShapeByName(shapeName, userIndex);
        if (selectedShape != null)
        {
            userSequence[userIndex] = selectedShape;
            selectedShape.SetActive(true); // Activate the selected shape
            userIndex++;
        }
    }

    GameObject GetShapeByName(string shapeName, int index)
    {
        // Return the corresponding shape from the correct shape array based on user input
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
        return null; // If the shape is not found
    }

    void DeactivateAllShapes()
    {
        // Deactivate all objects in all shape arrays once at the start
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
        bool isCorrect = true;

        // Compare the user sequence with the original sequence
        for (int i = 0; i < shapes.Length; i++)
        {
            // Check if the user sequence shape matches the original sequence shape
            if (userSequence[i].name != OriginalSequence[i].name)
            {
                isCorrect = false;
                break;
            }
        }

        // Output the result
        if (isCorrect)
        {
            Debug.Log("Correct Sequence!");
        }
        else
        {
            Debug.Log("Incorrect Sequence. Try Again!");
        }

        // Reset for next round
        userIndex = 0;
        System.Array.Clear(userSequence, 0, userSequence.Length); // Clear the user sequence
        hasStarted = false; // Reset the start flag
    }
}
