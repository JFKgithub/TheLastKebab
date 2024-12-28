using UnityEngine;
using TMPro;
using System.Collections;

public class CallDialogManager : MonoBehaviour
{
    [Header("AudioSource")]
    [Tooltip("AudioSource zum Abspielen von Sounds.")]
    public AudioSource audioSource;
    public AudioSource staticSource;

    [Header("Text Object")]
    [Tooltip("Text Mesh Pro für den Dialogtext.")]
    public TextMeshProUGUI dialogTextGameObject;
    [Tooltip("Text Mesh Pro für den Namen des Absenders.")]
    public TextMeshProUGUI messengerNameTextGameObject;

    [Header("Text Box")]
    [Tooltip("UI-Element für die Dialogbox.")]
    public GameObject textBoxGameObject;

    [Header("Buttons")]
    [Tooltip("Button für Weiter.")]
    public GameObject ContinueButton;
    [Tooltip("Button für Schließen.")]
    public GameObject CloseButton;

    [Header("AudioClips")]
    [Tooltip("Sound für das Klingeln des Telefons.")]
    public AudioClip phoneCalling;
    public AudioClip phoneHangUp;
    public AudioClip phoneHangUpBiep;
    [Tooltip("Schreibmaschinen-Sounds.")]
    public AudioClip typewriterA, typewriterB, typewriterC;
    [Tooltip("Klicksound für Button-Press.")]
    public AudioClip buttonClickSound;

    [Header("Phone Static")]
    [Space]
    public AudioClip PhoneStatic;

    [Header("Einstellungen")]
    [Tooltip("Standard-Geschwindigkeit, mit der die Zeichen angezeigt werden.")]
    public float defaultTextDisplayingSpeed = 0.05f;

    private string callerName; // Speichert den Namen des Anrufers.
    private string[] messages; // Speichert die Nachrichten.
    private float[] typingSpeeds; // Speichert die Tippgeschwindigkeit für jede Nachricht.
    private int currentMessageIndex; // Aktuelle Nachricht im Array.

    // Initialisiert den Dialog mit Name, Nachrichten und Tippgeschwindigkeiten.
    public void StartDialog(string name, string[] dialogMessages, float[] speeds = null)
    {       
        messengerNameTextGameObject.text = ""; // Namen anzeigen.
        dialogTextGameObject.text = ""; // Dialogtext leeren.
        textBoxGameObject.SetActive(true);

        callerName = name;
        messages = dialogMessages;
        typingSpeeds = speeds ?? new float[dialogMessages.Length]; // Fallback auf Standardgeschwindigkeit.

        // Sicherstellen, dass alle Tippgeschwindigkeiten gültig sind.
        for (int i = 0; i < typingSpeeds.Length; i++)
        {
            if (typingSpeeds[i] <= 0) typingSpeeds[i] = defaultTextDisplayingSpeed; // Standardgeschwindigkeit verwenden.
        }

        currentMessageIndex = 0;
        audioSource.PlayOneShot(phoneCalling); // Telefonklingeln abspielen.
        StartCoroutine(WaitAndStartTyping(8.77f)); // Warten, bevor der Tipp-Effekt startet.
        StartCoroutine(WaitForPhonePickUpSound(7.77f)); // Warten, bevor der Tipp-Effekt startet.
    }
    IEnumerator WaitForPhonePickUpSound(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        staticSource.loop = true;
        audioSource.PlayOneShot(PhoneStatic); // Direktes Abspielen des Telefon-Statik-Sounds
    }
    // Coroutine, die nach der Wartezeit den Typing-Effekt startet.
    IEnumerator WaitAndStartTyping(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        ContinueButton.SetActive(false);  // Buttons initial deaktivieren.
        CloseButton.SetActive(false);
        StartCoroutine(DisplayDialog()); // Dialog anzeigen.
    }

    // Zeigt den Text mit Typing-Effekt an.
    IEnumerator DisplayDialog()
    {
        messengerNameTextGameObject.text = callerName; // Namen anzeigen.
        dialogTextGameObject.text = ""; // Dialogtext leeren.

        string message = messages[currentMessageIndex];
        float speed = typingSpeeds[currentMessageIndex]; // Geschwindigkeit für diese Nachricht.

        foreach (char letter in message) // Text buchstabenweise anzeigen.
        {
            dialogTextGameObject.text += letter;
            PlayTypewriterSound(); // Schreibmaschinen-Sound abspielen.
            yield return new WaitForSeconds(speed);
        }

        // Buttons nach der vollständigen Anzeige des Texts aktivieren.
        if (currentMessageIndex < messages.Length - 1)
        {
            ContinueButton.SetActive(true); // Weiter-Button aktivieren.
        }
        else
        {
            CloseButton.SetActive(true); // Schließen-Button aktivieren.
        }
    }

    // Wechselt zur nächsten Nachricht.
    public void OnContinueButtonPressed()
    {
        audioSource.PlayOneShot(buttonClickSound); // Klicksound für Weiter-Button.      
        ContinueButton.SetActive(false); // Deaktivieren, bis die nächste Nachricht fertig ist.
        currentMessageIndex++;

        if (currentMessageIndex < messages.Length)
        {
            StartCoroutine(DisplayDialog()); // Nächste Nachricht anzeigen.
        }
    }

    // Schließt die Dialogbox.
    public void OnCloseButtonPressed()
    {
        audioSource.PlayOneShot(buttonClickSound); // Klicksound für Schließen-Button.
        EndDialog(); // Dialog beenden.
    }

    // Versteckt die Dialogbox und stoppt den Dialog.
    public void EndDialog()
    {
        audioSource.Stop(); // Stoppt den Sound.
        audioSource.PlayOneShot(phoneHangUpBiep);
        StartCoroutine(WaitForPhoneBiepSound(4f));
        staticSource.loop = false;
        textBoxGameObject.SetActive(false);
        dialogTextGameObject.text = "";
        messengerNameTextGameObject.text = "";
        ContinueButton.SetActive(false);
        CloseButton.SetActive(false);
    }
    IEnumerator WaitForPhoneBiepSound(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        audioSource.PlayOneShot(phoneHangUp);
    }
    // Spielt einen zufälligen Schreibmaschinen-Sound.
    void PlayTypewriterSound()
    {
        AudioClip[] sounds = { typewriterA, typewriterB, typewriterC };
        int randomIndex = Random.Range(0, sounds.Length);
        audioSource.PlayOneShot(sounds[randomIndex]);
    }
}
