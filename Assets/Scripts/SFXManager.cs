using UnityEngine;

public class SFXManager : MonoBehaviour
{
    public static SFXManager instance;

    [Header("Canales de Audio")]
    [Tooltip("AudioSource para música de fondo (cancion.mp3)")]
    public AudioSource musicSource; 
    [Tooltip("AudioSource para voces globales (bien, malhecho, peligro, alivio)")]
    public AudioSource globalSFXSource;

    void Awake()
    {
        // Configuración del Singleton
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    // Llama esto para voces, alarmas o retroalimentación UI (Audio 2D)
    public void PlayGlobalSFX(AudioClip clip)
    {
        if (clip != null)
            globalSFXSource.PlayOneShot(clip);
    }

    // Llama esto para sonidos mecánicos en VR (Audio 3D optimizado)
    public void PlaySpatialSFX(AudioClip clip, Vector3 position)
    {
        if (clip != null)
            AudioSource.PlayClipAtPoint(clip, position);
    }
}