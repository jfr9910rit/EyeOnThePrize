using System;
using System.Runtime.InteropServices;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;

public class SequenceManager : MonoBehaviour
{
    public GameObject[,] OriginalSequences;
    public GameObject[] shapes;
    public GameObject[] shapesWithX;

    private float[] probabilities;
    private int lastSelected = -1;
    private float timer = 0f;
    private float delayTime = 0.5f;
    private int index = 0;
    private bool activationStarted = false;
    private int playerCnt;
    private int diffLevel;
    public int shapeCount = 5;
    private int shapeX = -500;
    private int shapeGap = 250;
    public Transform[] seqs;
    public AudioSource shapeSounds;

    void Start()
    {
        playerCnt = GameManager.Instance.playerCount;
        diffLevel = GameManager.Instance.difficultyLevel;
        probabilities = new float[] { 25f, 25f, 25f, 25f };
        shapeCount = 5 + diffLevel;
        shapeGap = 250 - (diffLevel * 30);
        OriginalSequences = new GameObject[playerCnt, shapeCount];
        activationStarted = true;
        ActivateNextShape();
    }

    void Awake()
    {
        GameManager.Instance.playersFinished = 0;
    }

    void Update()
    {
        if (!activationStarted || index >= shapeCount)
        {
            activationStarted = false;
        }

        timer += Time.deltaTime;

        if (timer >= delayTime && activationStarted)
        {
            timer = 0f;
            ActivateNextShape();
        }
    }

    void ActivateNextShape()
    {
        if (index >= shapeCount) return;

        for (int i = 0; i < playerCnt; i++)
        {
            int seqIndex = GetSequenceIndex(i);
            int randomIndex = GetWeightedRandomIndex(shapes.Length);

            GameObject shape = Instantiate(shapes[randomIndex], new Vector3(shapeX, 0, -5), Quaternion.identity);
            shape.transform.SetParent(seqs[seqIndex], false);
            AnimateShape(shape);
            OriginalSequences[i, index] = shape;

            UpdateProbabilities(randomIndex, shapes.Length);
        }

        shapeX += shapeGap;
        index++;
        shapeSounds.Play();
    }

    int GetSequenceIndex(int playerSlot)
    {
        if (playerCnt == 1)
        {
            string role = GameManager.Instance.pRoles[playerSlot];
            if (role == "eppee") return 0;
            if (role == "teebee") return 1;
            if (role == "heartly") return 2;
        }
        else if (playerCnt == 2)
        {
            string role = GameManager.Instance.pRoles[playerSlot];
            if (playerSlot == 0)
            {
                if (role == "eppee") return 3;
                if (role == "teebee") return 4;
            }
            else if (playerSlot == 1)
            {
                if (role == "teebee") return 5;
                if (role == "heartly") return 6;
            }
        }
        else if (playerCnt == 3)
        {
            return 7 + playerSlot;
        }
        return 0; // fallback
    }

    void AnimateShape(GameObject shape)
    {
        shape.transform.localScale = Vector3.zero;
        shape.AddComponent<ShapeAnimator>();
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

        return 0;
    }

    void UpdateProbabilities(int selected, int shapeCount)
    {
        if (lastSelected == selected)
        {
            probabilities[selected] /= 2f;
        }
        else
        {
            if (lastSelected != -1)
            {
                probabilities[lastSelected] = 12.5f;
            }

            probabilities[selected] /= 2f;
        }

        float remaining = 100f - probabilities[selected] - (lastSelected != -1 ? probabilities[lastSelected] : 0);
        int countOther = shapeCount - (lastSelected == -1 ? 1 : 2);

        if (countOther <= 0) countOther = 1;

        for (int i = 0; i < Mathf.Min(shapeCount, probabilities.Length); i++)
        {
            if (i != selected && i != lastSelected)
            {
                probabilities[i] = remaining / countOther;
            }
        }

        lastSelected = selected;
    }
}

public class ShapeAnimator : MonoBehaviour
{
    private float scaleSpeed = 30f;
    private float rotationSpeed = 1080f;
    private bool finishedScaling = false;

    void Update()
    {
        if (!finishedScaling)
        {
            transform.localScale = Vector3.Lerp(transform.localScale, Vector3.one, Time.deltaTime * scaleSpeed);
            transform.Rotate(Vector3.forward, rotationSpeed * Time.deltaTime);

            if (Vector3.Distance(transform.localScale, Vector3.one) < 0.01f)
            {
                transform.localScale = Vector3.one;
                transform.rotation = Quaternion.Euler(0f, 0f, 0f);
                finishedScaling = true;
            }
        }
    }
}
