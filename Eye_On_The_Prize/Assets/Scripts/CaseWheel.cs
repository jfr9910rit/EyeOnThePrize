using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class CaseOpening : MonoBehaviour
{
    [Header("UI References")]
    public RectTransform itemRow;              // Row containing the items
    public Button spinButton;                  // Spin button
    public TextMeshProUGUI rewardText;         // Display reward
    public ParticleSystem winEffect;           // Particle effect for winning

    [Header("Spin Settings")]
    public float spinDuration = 4f;            // Time for the full spin
    public float initialSpeed = 4000f;         // Starting speed
    public float finalSpeed = 100f;            // Slow speed at the end
    public float decelerationRate = 400f;      // Rate of deceleration
    public float itemWidth = 400f;             // Width of each item

    [Header("Sound Effects")]
    public AudioSource spinSound;              // Sound for spinning
    public AudioSource winSound;               // Sound when you win

    private bool isSpinning = false;
    private List<RectTransform> items = new List<RectTransform>();
    private RectTransform duplicateRow;        // Duplicate for seamless looping

    void Start()
    {
        spinButton.onClick.AddListener(StartSpin);

        // Cache and randomize the items
        CacheItems();
        DuplicateRow();
        RandomizeItems();
    }

    private void CacheItems()
    {
        // Store the original items
        items.Clear();
        for (int i = 0; i < itemRow.childCount; i++)
        {
            items.Add(itemRow.GetChild(i).GetComponent<RectTransform>());
        }

        // Set the row width based on the number of items
        itemRow.sizeDelta = new Vector2(itemWidth * items.Count, itemRow.sizeDelta.y);
    }

    private void DuplicateRow()
    {
        // Duplicate the row for seamless looping
        duplicateRow = Instantiate(itemRow, itemRow.parent);

        // Position the duplicate row exactly adjacent to the original row
        duplicateRow.anchoredPosition = new Vector2(itemRow.anchoredPosition.x + itemRow.rect.width, itemRow.anchoredPosition.y);

        // Ensure the duplicate row is visible and has the same items
        for (int i = 0; i < items.Count; i++)
        {
            RectTransform duplicateItem = duplicateRow.GetChild(i).GetComponent<RectTransform>();
            duplicateItem.anchoredPosition = items[i].anchoredPosition;
            duplicateItem.gameObject.SetActive(true);
        }
    }

    private void RandomizeItems()
    {
        // Shuffle the item order before spinning
        for (int i = 0; i < items.Count; i++)
        {
            int randomIndex = Random.Range(i, items.Count);

            // Swap item positions
            Vector3 tempPos = items[i].anchoredPosition;
            items[i].anchoredPosition = items[randomIndex].anchoredPosition;
            items[randomIndex].anchoredPosition = tempPos;

            // Swap the items in the list as well
            RectTransform tempItem = items[i];
            items[i] = items[randomIndex];
            items[randomIndex] = tempItem;
        }

        // Apply the same randomization to the duplicate row
        for (int i = 0; i < items.Count; i++)
        {
            duplicateRow.GetChild(i).GetComponent<RectTransform>().anchoredPosition = items[i].anchoredPosition;
        }
    }

    public void StartSpin()
    {
        if (isSpinning) return;

        isSpinning = true;

        // Randomize the items before every spin
        RandomizeItems();

        StartCoroutine(SpinWheel());
    }

    private IEnumerator SpinWheel()
    {
        float speed = initialSpeed;
        float time = 0f;

        if (spinSound) spinSound.Play();

        while (time < spinDuration)
        {
            float t = time / spinDuration;
            speed = Mathf.Lerp(initialSpeed, finalSpeed, t);

            // Move both rows together
            itemRow.anchoredPosition += new Vector2(-speed * Time.deltaTime, 0);
            duplicateRow.anchoredPosition += new Vector2(-speed * Time.deltaTime, 0);

            // Perfect seamless looping
            float rowWidth = itemRow.rect.width;
            if (itemRow.anchoredPosition.x <= -rowWidth)
            {
                itemRow.anchoredPosition = new Vector2(rowWidth, itemRow.anchoredPosition.y);
            }
            if (duplicateRow.anchoredPosition.x <= -rowWidth)
            {
                duplicateRow.anchoredPosition = new Vector2(rowWidth, duplicateRow.anchoredPosition.y);
            }

            time += Time.deltaTime;
            yield return null;
        }

        // Natural deceleration
        yield return StartCoroutine(NaturalDeceleration());

        // Display the reward
        DisplayReward();
        isSpinning = false;
    }

    private IEnumerator NaturalDeceleration()
    {
        float speed = finalSpeed;

        while (speed > 10f)
        {
            itemRow.anchoredPosition += new Vector2(-speed * Time.deltaTime, 0);
            duplicateRow.anchoredPosition += new Vector2(-speed * Time.deltaTime, 0);

            // Gradual deceleration
            speed -= decelerationRate * Time.deltaTime;

            // Ensure perfect seamless looping during deceleration
            float rowWidth = itemRow.rect.width;
            if (itemRow.anchoredPosition.x <= -rowWidth)
            {
                itemRow.anchoredPosition = new Vector2(rowWidth, itemRow.anchoredPosition.y);
            }
            if (duplicateRow.anchoredPosition.x <= -rowWidth)
            {
                duplicateRow.anchoredPosition = new Vector2(rowWidth, duplicateRow.anchoredPosition.y);
            }

            yield return null;
        }
    }

    private void DisplayReward()
    {
        // Determine the winning item by the center position
        float centerX = Mathf.Abs(itemRow.anchoredPosition.x) + (itemRow.rect.width / 2f);
        int winningIndex = Mathf.FloorToInt(centerX / itemWidth) % items.Count;

        GameObject winningItem = items[winningIndex].gameObject;
        string itemName = winningItem.name;

        rewardText.text = $"You won: {itemName}";
        GameManager.Instance.activeModifier = itemName;
        Debug.Log($"You won: {itemName}");

        // Play win sound and effect
        if (winSound) winSound.Play();
        if (winEffect)
        {
            //winEffect.transform.position = winningItem.transform.position;
            winEffect.Play();
        }
    }
}
