using UnityEngine;
using UnityEngine.Events;

public class PowerSystem : MonoBehaviour
{
    public bool isPowerOn = false;
    public UnityEvent onPowerRestored;

    // Esta función la llamaremos desde el evento del XR Grab Interactable
    public void RestorePower()
    {
        if (!isPowerOn)
        {
            isPowerOn = true;
            Debug.Log("Sistema de soporte vital: Energía restaurada.");
            
            // Dispara cualquier acción conectada en el Inspector (luces, sonidos, puertas)
            onPowerRestored.Invoke(); 
        }
    }
}