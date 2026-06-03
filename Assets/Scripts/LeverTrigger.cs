using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

[RequireComponent(typeof(HingeJoint))]
[RequireComponent(typeof(Rigidbody))]
public class LeverController : MonoBehaviour
{
    [Header("Configuración")]
    public float downAngle = -40f;

    [Header("Puzzle")]
    public LeverPuzzle puzzle;
    public int leverIndex;

    [Header("Fallback (si no hay puzzle asignado)")]
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

        ForceUpPosition();

        grab.selectEntered.AddListener(_ => isBeingHeld = true);
        grab.selectExited.AddListener(_ => OnReleased());
    }

    void ForceUpPosition()
    {
        rb.isKinematic = true;
        transform.localRotation = Quaternion.identity;
        rb.isKinematic = false;

        JointSpring spring = hinge.spring;
        spring.spring = 50f;
        spring.damper = 5f;
        spring.targetPosition = 0f;
        hinge.spring = spring;
        hinge.useSpring = true;
    }

    void OnReleased()
    {
        isBeingHeld = false;

        if (!triggered)
            hinge.useSpring = true; // regresa arriba
        else
            hinge.useSpring = false; // se queda abajo
    }

    void Update()
    {
        if (triggered) return;

        if (isBeingHeld)
            hinge.useSpring = false;

        if (isBeingHeld && hinge.angle <= downAngle)
        {
            triggered = true;
            hinge.useSpring = false;
            NotifyPuzzle();
        }
    }

    void NotifyPuzzle()
    {
        if (puzzle != null)
            puzzle.RegisterLever(leverIndex);
        else
            onLeverPulledDown.Invoke();
    }

    public void ResetLever()
    {
        triggered = false;
        hinge.useSpring = true;
        // el spring regresa la palanca arriba automáticamente
    }
}