using UnityEngine;
using UnityEngine.Events;

public class CoolingSystem : MonoBehaviour
{
    [Header("Lámparas")]
    public Light[] lights;
    
    [Header("Puerta")]
    public GameObject door;
    public Vector3 openPosition;      // posición destino de la puerta abierta
    public float doorSpeed = 2f;

    [Header("Eventos")]
    public UnityEvent onCoolingComplete;

    private bool cooled = false;
    private bool moveDoor = false;

    public void OnCoolingCellInserted()
    {
        if (cooled) return;
        cooled = true;

        // Cambiar lámparas a blanco
        foreach (Light l in lights)
        {
            l.color = Color.white;
        }

        // Iniciar movimiento de puerta
        moveDoor = true;

        onCoolingComplete.Invoke();
    }

    void Update()
    {
        if (moveDoor && door != null)
        {
            door.transform.position = Vector3.MoveTowards(
                door.transform.position,
                openPosition,
                doorSpeed * Time.deltaTime
            );

            if (Vector3.Distance(door.transform.position, openPosition) < 0.01f)
                moveDoor = false;
        }
    }
}