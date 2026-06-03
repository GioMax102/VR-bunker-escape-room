using UnityEngine;
using UnityEngine.Events;

public class CoolingSystem : MonoBehaviour
{
    [Header("Lámparas")]
    public Light[] lights;
    
    [Header("Puerta")]
    public GameObject door;
    public float doorOpenDistance = 2f; // cuántos metros sube
    public float doorSpeed = 2f;

    private Vector3 openPosition;
    private bool moveDoor = false;

    [Header("Audio")]
    public AudioClip sfxAlivio;

    [Header("Eventos")]
    public UnityEvent onCoolingComplete;

    private bool cooled = false;
    void Start()
    {
        // Calcula destino relativo a su posición actual
        if (door != null)
            openPosition = door.transform.position + Vector3.up * doorOpenDistance;
    }
    public void OnCoolingCellInserted()
    {
        if (cooled) return;
        cooled = true;
        if (SFXManager.instance != null) 
            SFXManager.instance.PlayGlobalSFX(sfxAlivio);

        if (ObjectiveManager.instance != null)
            ObjectiveManager.instance.AdvanceStep(1, "OBJETIVO 3:\nReactor estable. Libera los candados magnéticos jalando las 3 palancas en el orden correcto.");

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