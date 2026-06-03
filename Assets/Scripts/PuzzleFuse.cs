using UnityEngine;

// Va en el GameObject raíz del fusible (el mismo que tiene el Rigidbody y el XRGrabInteractable).
public class PuzzleFuse : MonoBehaviour
{
    [Tooltip("Debe coincidir con el socketId del FuseSocket donde encaja este fusible.")]
    public int fuseId;
}
