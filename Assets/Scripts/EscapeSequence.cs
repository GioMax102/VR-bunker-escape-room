using UnityEngine;
using UnityEngine.Events;

public class EscapeSequence : MonoBehaviour
{
    [Header("Estado del Búnker")]
    public bool hasPower = false;
    public bool isCooled = false;

    [Header("Eventos de la Puerta")]
    public UnityEvent onEscapeSuccessful; // Abre la puerta
    public UnityEvent onDoorLockedError;  // Sonido de error

    // Llama a esto desde el HingeJoint de la palanca
    public void RestorePower()
    {
        hasPower = true;
        Debug.Log("Energía restaurada. Falta enfriamiento.");
    }

    // Llama a esto desde el XR Socket Interactor del Reactor
    public void InsertCoolingCell()
    {
        isCooled = true;
        Debug.Log("Reactor estabilizado.");
    }

    // Llama a esto cuando el jugador intente agarrar la manija de la puerta principal
    public void TryOpenMainDoor()
    {
        if (hasPower && isCooled)
        {
            Debug.Log("¡Secuencia completada! Abriendo puerta...");
            onEscapeSuccessful.Invoke();
        }
        else
        {
            Debug.Log("Acceso denegado. Faltan sistemas por activar.");
            onDoorLockedError.Invoke();
        }
    }
}