using System.Collections;
using UnityEngine;

public class Timer : MonoBehaviour
{
    [Header("Audio Source")]
    public AudioSource audioSource;

    [Header("Time Length")]
    [Range(0.0f, 60.0f)]
    public float timerLength;
    [Range(0.0f, 2.0f)]
    public float tickLength;

    [Header("Pointer Object")]
    public GameObject pointer;

    [Header("Audio Clips")]
    public AudioClip[] tickSounds;

    private Quaternion rotation;
    private float rotationPercent;
    public float elapsedTime = 0f;
    private int lastTickSound = -1;

    public void ResetTimer()
    {
        elapsedTime = 0f;
        audioSource.volume = 0.25f;
        rotation = Quaternion.Euler(0, 0, 0);
        pointer.transform.rotation = rotation;
        audioSource.Stop();
    }

    public void StartTimer()
    {
        ResetTimer();
        StartCoroutine(Tick());
    }

    private IEnumerator Tick()
    {      
        while (elapsedTime <= timerLength)
        {
            // Check conditions
            if (elapsedTime >= timerLength * 0.5f && elapsedTime <= timerLength * 0.75f)
            {
                audioSource.volume = 0.5f;
                Debug.Log("Changed Audio Volume: " + audioSource.volume);
            }
            else if (elapsedTime > timerLength * 0.75f)
            {
                audioSource.volume = 1f;
                Debug.Log("Changed Audio Volume: " + audioSource.volume);
            }
            else
            {
                Debug.Log(audioSource.volume);
            }
            // Calculate rotation progress
            rotationPercent = (360f * (elapsedTime / timerLength));  // This gives us the current rotation based on elapsed time
            rotation = Quaternion.Euler(0f, 0f, rotationPercent);
            pointer.transform.rotation = rotation;

            // Play a random tick sound, avoiding the last one
            int randomIndex = Random.Range(0, tickSounds.Length);
            while (randomIndex == lastTickSound) // Ensure the sound is different from the last one
            {
                randomIndex = Random.Range(0, tickSounds.Length);
            }
            lastTickSound = randomIndex;

            audioSource.PlayOneShot(tickSounds[randomIndex]);

            // Wait for the specified tick length
            yield return new WaitForSeconds(tickLength);

            // Increment elapsed time by tick length
            elapsedTime += tickLength;
        }
    }
}
