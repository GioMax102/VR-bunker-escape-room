using UnityEngine;
using UnityEngine.Events;

public class PowerSystem : MonoBehaviour
{
    public bool isPowerOn = false;
    public AudioClip sfxPeligro;
    public UnityEvent onPowerRestored;

    // Esta función la llamaremos desde el evento del XR Grab Interactable
    public void RestorePower()
    {
        if (!isPowerOn)
        {
            isPowerOn = true;
            Debug.Log("Sistema de soporte vital: Energía restaurada.");
            if (SFXManager.instance != null) 
                SFXManager.instance.PlayGlobalSFX(sfxPeligro);
            if (ObjectiveManager.instance != null)
                ObjectiveManager.instance.AdvanceStep(0, "OBJETIVO 2:\n¡Alerta de Temperatura! Encuentra la celda de enfriamiento e insértala en el reactor.");
            // Dispara cualquier acción conectada en el Inspector (luces, sonidos, puertas)
            onPowerRestored.Invoke(); 
        }
    }
}