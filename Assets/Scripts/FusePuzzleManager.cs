using UnityEngine;
using UnityEngine.Events;

// Va en un único GameObject de la escena. Cuenta los fusibles colocados y, al completar,
// dispara el UnityEvent (y opcionalmente cambia la puerta cerrada por una abierta).
public class FusePuzzleManager : MonoBehaviour
{
    [Header("Configuración")]
    [SerializeField] private int requiredFuses = 3;

    [Header("Se dispara cuando están TODOS los fusibles")]
    [SerializeField] private UnityEvent onAllFusesPlaced;

    [Header("Opcional: cambiar puerta cerrada por abierta")]
    [Tooltip("Raíz del objeto de la puerta CERRADA. Arrástralo aquí: NO se usa .root para no borrar media escena.")]
    [SerializeField] private Transform doorRoot;
    [SerializeField] private GameObject openDoorPrefab;
    [Tooltip("Collider a desactivar al abrir (opcional).")]
    [SerializeField] private Collider doorCollider;

    private SFXManager sfxManager;
    private int placedFuses;
    private bool completed;

    private void Awake()
    {
        sfxManager = FindAnyObjectByType<SFXManager>(); // puede ser null, lo manejamos

        // Si tienes varios puzzles independientes, mejor asigna los sockets a mano
        // en cada uno en vez de buscarlos todos en la escena.
        var sockets = FindObjectsByType<FuseSocket>(FindObjectsSortMode.None);
        foreach (var socket in sockets)
            socket.SetPuzzleManager(this);
    }

    public void NotifyFusePlaced()
    {
        if (completed) return;

        placedFuses++;

        if (sfxManager != null) sfxManager.PlayCorrectSound();

        if (placedFuses >= requiredFuses)
            CompletePuzzle();
    }

    private void CompletePuzzle()
    {
        if (completed) return;
        completed = true;

        onAllFusesPlaced?.Invoke();

        if (sfxManager != null) sfxManager.PlayPuzzleSolvedSound();

        TrySwapDoor();
    }

    private void TrySwapDoor()
    {
        if (openDoorPrefab == null || doorRoot == null) return;

        Transform parent = doorRoot.parent;

        GameObject openDoor = Instantiate(openDoorPrefab, parent);
        openDoor.transform.localPosition = doorRoot.localPosition;
        openDoor.transform.localRotation = doorRoot.localRotation;
        openDoor.transform.localScale = doorRoot.localScale;

        if (doorCollider != null) doorCollider.enabled = false;

        Destroy(doorRoot.gameObject);
    }

    // Para probar desde el Inspector (clic derecho en el componente) sin VR.
    [ContextMenu("Forzar completar puzzle")]
    private void DebugComplete()
    {
        CompletePuzzle();
    }
}
