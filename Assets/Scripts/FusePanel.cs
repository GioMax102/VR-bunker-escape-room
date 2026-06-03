using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class FusePanel : MonoBehaviour
{
    [Header("Los 3 sockets del panel")]
    public XRSocketInteractor[] fuseSockets;

    [Header("Se dispara cuando los 3 están llenos")]
    public UnityEvent onAllFusesPlaced;

    private bool completed = false;

    void Start()
    {
        foreach (var socket in fuseSockets)
        {
            socket.selectEntered.AddListener(_ => CheckPanel());
            socket.selectExited.AddListener(_ => CheckPanel());
        }
    }

    void CheckPanel()
    {
        if (completed) return;

        foreach (var socket in fuseSockets)
            if (!socket.hasSelection) return;

        completed = true;
        onAllFusesPlaced.Invoke();
    }
}