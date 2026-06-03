using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

[RequireComponent(typeof(HingeJoint))]
public class LeverController : MonoBehaviour
{
    [Header("Configuración")]
    [Tooltip("Ángulo que se considera 'abajo' (palanca bajada)")]
    public float downAngle = -40f;

    [Header("Eventos")]
    public UnityEvent onLeverPulledDown;

    private HingeJoint hinge;
    private Rigidbody rb;
    private XRGrabInteractable grab;
    private bool triggered = false;
    private bool isBeingHeld = false;

    void Start()
    {
        hinge = GetComponent<HingeJoint>();
        rb = GetComponent<Rigidbody>();
        grab = GetComponent<XRGrabInteractable>();

        // Forzar posición inicial: arriba
        ForceUpPosition();

        // Escuchar cuando el jugador agarra/suelta
        grab.selectEntered.AddListener(_ => isBeingHeld = true);
        grab.selectExited.AddListener(_ => OnReleased());
    }

    void ForceUpPosition()
    {
        // Bloquear físicas temporalmente y forzar rotación "arriba"
        rb.isKinematic = true;
        transform.localRotation = Quaternion.identity; // ajusta si tu "arriba" es otra rotación
        rb.isKinematic = false;

        // Usar spring del hinge para mantenerla arriba si nadie la toca
        JointSpring spring = hinge.spring;
        spring.spring = 50f;
        spring.damper = 5f;
        spring.targetPosition = 0f; // 0 = posición inicial (arriba)
        hinge.spring = spring;
        hinge.useSpring = true;
    }

    void OnReleased()
    {
        isBeingHeld = false;

        if (!triggered)
        {
            // Si no ha sido activada aún, el spring la regresa arriba
            hinge.useSpring = true;
        }
        else
        {
            // Ya fue bajada: desactivar spring para que se quede abajo
            hinge.useSpring = false;
        }
    }

    void Update()
    {
        if (triggered) return;

        // Mientras la sostiene, desactivar spring para que pueda moverla
        if (isBeingHeld)
            hinge.useSpring = false;

        if (isBeingHeld && hinge.angle <= downAngle)
        {
            triggered = true;
            hinge.useSpring = false; // se queda abajo para siempre
            Debug.Log("Palanca bajada. Activando sistema...");
            onLeverPulledDown.Invoke();
        }
    }
}