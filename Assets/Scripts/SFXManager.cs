using UnityEngine;

// Va en un GameObject con un AudioSource. Si no existe en la escena, el puzzle
// igual funciona: simplemente no suena nada.
[RequireComponent(typeof(AudioSource))]
public class SFXManager : MonoBehaviour
{
    [SerializeField] private AudioClip correctSound;        // al colocar un fusible correcto
    [SerializeField] private AudioClip puzzleSolvedSound;   // al completar el puzzle

    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayCorrectSound() => Play(correctSound);

    public void PlayPuzzleSolvedSound() => Play(puzzleSolvedSound);

    private void Play(AudioClip clip)
    {
        if (clip != null) audioSource.PlayOneShot(clip);
    }
}
