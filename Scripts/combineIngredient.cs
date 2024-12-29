using System.Collections.Generic;
using UnityEngine;

public class CombineIngredient : MonoBehaviour
{
    [Header("GameObjects")]
    public GameObject[] combinationObjects;
    [Header("AudioClips")]
    public AudioClip[] combinationSounds;
    private Dictionary<(int, int), (GameObject, AudioClip)> combinationDictionary;

    private void Start()
    {
        InitializeCombinations();
    }

    private void InitializeCombinations()
    {
        // Initialize combinations
        combinationDictionary = new Dictionary<(int, int), (GameObject, AudioClip)>
        {
            {(1, 2), (combinationObjects[0], combinationSounds[0])},
            {(2, 1), (combinationObjects[0], combinationSounds[0])}
        };
    }

    public void TryCombine()
    {
        moveIngredient moveIngredient = FindFirstObjectByType<moveIngredient>();
        if (moveIngredient == null || moveIngredient.lastMovedObject == null)
        {
            Debug.LogWarning("Kein gültiges bewegtes Objekt gefunden.");
            return;
        }

        combineTracker lastMovedTracker = moveIngredient.lastMovedObject.GetComponent<combineTracker>();
        combineTracker collidedTracker = lastMovedTracker?.lastCollided?.GetComponent<combineTracker>();

        if (lastMovedTracker != null && collidedTracker != null && !moveIngredient.isDragging)
        {
            int objNumber1 = lastMovedTracker.objectNumber;
            int objNumber2 = collidedTracker.objectNumber;

            // Check if combination exists
            if (combinationDictionary.TryGetValue((objNumber1, objNumber2), out var result))
            {
                GameObject obj = result.Item1;  // Access GameObject
                AudioClip sound = result.Item2; // Access AudioClip

                // Instantiate new object
                Instantiate(obj, collidedTracker.gameObject.transform.position, Quaternion.identity);

                // Optionally play the sound
                AudioSource.PlayClipAtPoint(sound, collidedTracker.gameObject.transform.position);

                // Destroy original objects
                Destroy(lastMovedTracker.gameObject);
                Destroy(collidedTracker.gameObject);
            }
            else
            {
                // No valid combination, move objects apart
                lastMovedTracker.MoveAway();
            }
        }
        else
        {
            Debug.LogWarning("Fehlerhafte Tracker-Komponenten.");
        }
    }
}
