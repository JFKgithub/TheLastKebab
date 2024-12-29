using UnityEngine;
using TMPro;

public class DayCounter : MonoBehaviour
{
    [Header("Day Counter")]
    [Tooltip("Counting the days")]
    public int dayCount = 0;

    [Header("Text Object")]
    [Tooltip("Displaying the day")]
    public TextMeshProUGUI displayingDayNumberTextFront;
    public TextMeshProUGUI displayingDayNumberTextBack;

    [Header("Calendar in the background")]
    [Tooltip("Prefab for cloning")]
    public GameObject xPrefab;
    public GameObject bossPrefab;
    public GameObject calanderText;

    [Tooltip("Container GameObject for spawned X's")]
    public GameObject prefabContainer;

    [Tooltip("Point for the X's to spawn")]
    public Vector3 spawnPoint;

    [Header("Positioning Settings")]
    [Tooltip("Spacing between X's")]
    public float horizontalSpacing = 4.5f;

    [Tooltip("Horizontal offset for X's")]
    public float horizontalOffset = 6.3f;

    [Tooltip("Vertical positions for rows")]
    public float[] rowYPositions = { 2.81f, 2.1f, 1.22f };

    [Header("Animation")]
    public Animator animator;
    public string animationTrigger;

    [Header("Sound")]
    public AudioSource audioSource;
    public AudioClip flyBy;

    private GameObject currentBossEntry;
    private GameObject currentCalanderDayText;
    private TextMeshProUGUI currentCalanderText;

    private float verticalPosition = 0f;

    public void StartNewDay()
    {
        UpdateCalendar();
        dayCount++;

        // Update Text Displays
        displayingDayNumberTextFront.text = dayCount.ToString();
        displayingDayNumberTextBack.text = dayCount.ToString();

        // Trigger animation and play sound
        animator.SetTrigger(animationTrigger);
        audioSource.PlayOneShot(flyBy);
    }

    public void UpdateCalendar()
    {
        if (dayCount == 0)
        {
            AddDayNumbersToTheCalander();
        }

        if (dayCount > 0 && dayCount <= 15)
        {
            SetCalanderEntry();
        }

        if (dayCount == 3 || dayCount == 8 || dayCount == 13)
        {
            UpdateBossEntry(dayCount);
        }
    }

    private void SetCalanderEntry()
    {
        float horizontalPosition = dayCount * horizontalSpacing;
        int rowIndex = Mathf.FloorToInt((dayCount - 1) / 5); // Determine the row index (0, 1, 2, ...)

        // Ensure row index is valid
        if (rowIndex >= 0 && rowIndex < rowYPositions.Length)
        {
            horizontalPosition -= rowIndex * 5 * horizontalSpacing; // Adjust horizontal position based on row
            verticalPosition = rowYPositions[rowIndex]; // Get vertical position for the row
        }
        else
        {
            Debug.LogError("Unexpected rowIndex: " + rowIndex);
            return; // Exit early if rowIndex is invalid
        }

        spawnPoint = new Vector3(horizontalPosition - horizontalOffset, verticalPosition, 0);

        // Spawn X as a child of PrefabContainer if available
        Transform parent = prefabContainer != null ? prefabContainer.transform : null;
        Instantiate(xPrefab, spawnPoint, Quaternion.identity, parent);
        Debug.Log(spawnPoint);
    }

    private void UpdateBossEntry(int day)
    {
        // Determine the row index based on day
        int rowIndex = (day - 3) / 5;

        // Destroy existing boss entry if it exists
        if (currentBossEntry != null)
        {
            Destroy(currentBossEntry);
        }

        // Calculate the spawn position for the boss entry
        float horizontalPosition = 5 * horizontalSpacing;
        verticalPosition = rowYPositions[rowIndex];
        spawnPoint = new Vector3(horizontalPosition - horizontalOffset, verticalPosition, 0);

        // Instantiate the boss prefab
        currentBossEntry = Instantiate(bossPrefab, spawnPoint, Quaternion.identity, prefabContainer.transform);
    }

    private void AddDayNumbersToTheCalander()
    {
        for (int i = 1; i < 16; i++) // Start from 1 as intended
        {
            // Determine the row index and calculate positions
            int rowIndex = Mathf.FloorToInt((i - 1) / 5); // Current row (0, 1, 2)
            float horizontalPosition = i * horizontalSpacing;
            horizontalPosition -= (rowIndex * 5 * horizontalSpacing); // Reset horizontal position for each row
            verticalPosition = rowYPositions[rowIndex]; // Get vertical position for the row

            // Determine the spawn point for this calendar day
            spawnPoint = new Vector3(horizontalPosition - horizontalOffset, verticalPosition, 0);

            // Instantiate the calendar text prefab
            if (prefabContainer != null)
            {
                currentCalanderDayText = Instantiate(calanderText, spawnPoint, Quaternion.identity, prefabContainer.transform);
            }
            else
            {
                currentCalanderDayText = Instantiate(calanderText, spawnPoint, Quaternion.identity);
            }

            // Configure the TextMeshPro component
            currentCalanderText = currentCalanderDayText.GetComponent<TextMeshProUGUI>();
            if (currentCalanderText != null)
            {
                currentCalanderText.text = i.ToString(); // Set the day number
            }
            else
            {
                Debug.LogError("Missing TextMeshPro component on the calendar text prefab.");
            }
        }
    }
}
