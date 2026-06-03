using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactors;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class SocketSwapper : MonoBehaviour
{
    public GameObject manivelaReal;
    public Transform spawnPoint; // Asigna el Transform del socket como referencia

    public void EjecutarCambiazo()
    {
        // 1. Obtener el objeto ANTES de destruir
        XRSocketInteractor socket = GetComponent<XRSocketInteractor>();
        IXRSelectInteractable objetoEnSocket = socket.GetOldestInteractableSelected();

        if (objetoEnSocket == null)
        {
            Debug.LogWarning("Socket vacío al ejecutar cambiazo");
            return;
        }

        // 2. Aparecer la real en la posición del socket
        manivelaReal.transform.position = spawnPoint != null 
            ? spawnPoint.position 
            : transform.position;
        manivelaReal.transform.rotation = spawnPoint != null 
            ? spawnPoint.rotation 
            : transform.rotation;
        manivelaReal.SetActive(true);

        // 3. Destruir el temporal
        Destroy(objetoEnSocket.transform.gameObject);

        Debug.Log("Cambiazo ejecutado: palanca real activada");
    }
}