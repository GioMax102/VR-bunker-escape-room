using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

// Va en un GameObject vacío colocado/orientado EXACTAMENTE donde debe quedar el fusible.
// El collider se usa como zona de detección (se fuerza a trigger en Awake).
[RequireComponent(typeof(Collider))]
public class FuseSocket : MonoBehaviour
{
    [Tooltip("Solo aceptará el fusible cuyo fuseId coincida con este valor.")]
    [SerializeField] private int socketId;

    [SerializeField] private FusePuzzleManager puzzleManager;

    [Header("Pose final del fusible")]
    [Tooltip("Si lo asignas, el fusible se coloca aquí. Si lo dejas vacío, usa el propio socket.")]
    [SerializeField] private Transform snapPoint;

    public bool IsOccupied => occupied;
    private bool occupied;

    private void Awake()
    {
        GetComponent<Collider>().isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (occupied) return;

        // El collider puede estar en un hijo del fusible, por eso GetComponentInParent.
        PuzzleFuse fuse = other.GetComponentInParent<PuzzleFuse>();
        if (fuse == null || fuse.fuseId != socketId) return;

        // Soltar de la mano: al desactivar el grab, XRI cancela la interacción en curso.
        XRGrabInteractable grab = fuse.GetComponent<XRGrabInteractable>();
        if (grab != null) grab.enabled = false;

        // Congelar el rigidbody para que se quede fijo en el socket.
        Rigidbody rb = fuse.GetComponent<Rigidbody>();
        if (rb != null)
        {
            // Solo se puede tocar la velocidad si NO es kinematic (evita un warning).
            if (!rb.isKinematic)
            {
                rb.linearVelocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
            }
            rb.isKinematic = true;
            rb.useGravity = false;
        }

        // Snap a la posición/rotación del socket y emparentar.
        Transform anchor = snapPoint != null ? snapPoint : transform;
        fuse.transform.SetPositionAndRotation(anchor.position, anchor.rotation);
        fuse.transform.SetParent(anchor, true);

        occupied = true;

        if (puzzleManager != null) puzzleManager.NotifyFusePlaced();
    }

    public void SetPuzzleManager(FusePuzzleManager manager)
    {
        puzzleManager = manager;
    }
}
