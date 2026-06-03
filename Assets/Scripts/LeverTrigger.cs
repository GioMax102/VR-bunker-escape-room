using UnityEngine;
using UnityEngine.Events;

public class LeverTrigger : MonoBehaviour
{
    private HingeJoint hinge;
    public float targetAngle = 40f; 
    public UnityEvent onLeverPulled;
    private bool hasBeenPulled = false;

    void Start()
    {
        // Obtenemos la referencia a la bisagra automáticamente
        hinge = GetComponent<HingeJoint>();
    }

    void Update()
    {
        if (!hasBeenPulled && hinge.angle >= targetAngle)
        {
            hasBeenPulled = true;
            Debug.Log("¡Palanca activada hasta el fondo!");
            onLeverPulled.Invoke();
        }
    }
}