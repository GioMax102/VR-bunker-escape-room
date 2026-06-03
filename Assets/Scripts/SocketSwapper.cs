using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactors;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class SocketSwapper : MonoBehaviour
{
    public GameObject manivelaReal;

    public void EjecutarCambiazo()
    {
        // 1. Aparecemos la real primero
        manivelaReal.SetActive(true);

        // 2. Obtenemos el objeto que el socket acaba de agarrar
        IXRSelectInteractable objetoEnSocket = GetComponent<XRSocketInteractor>().GetOldestInteractableSelected();
        
        // 3. Destruimos el temporal
        if (objetoEnSocket != null)
        {
            Destroy(objetoEnSocket.transform.gameObject);
        }
    }
}