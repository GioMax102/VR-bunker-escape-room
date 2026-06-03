using UnityEngine;

public class PuzzleButton : MonoBehaviour
{
    [Header("Índice de este botón (0=btn1, 1=btn2, 2=btn3)")]
    public int buttonIndex;
    public ButtonPuzzle puzzle;

    [Header("Materiales")]
    public Renderer buttonRenderer;
    public Material matDefault;
    public Material matPressed;
    public Material matSolved;

    private bool isSolved = false; // Bloqueo para cuando ganen

    void Start()
    {
        // Asignar el material por defecto al inicio
        if (buttonRenderer && matDefault)
            buttonRenderer.material = matDefault;
    }

    // AQUI ESTA LA MAGIA DE TU CAÑON
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Hand") && !isSolved)
        {
            Debug.Log("Mano tocó el botón " + buttonIndex);
            
            // Cambiamos el color
            SetPressed(true);
            
            // Le avisamos al cerebro del puzzle
            if (puzzle != null)
            {
                puzzle.RegisterPress(buttonIndex);
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        // 👈 LÍNEA DE DIAGNÓSTICO TEMPORAL
        Debug.Log("FÍSICAS ACTIVAS: Algo entró al botón: " + other.gameObject.name + " | Tag actual: " + other.gameObject.tag);
        // Opcional: Si quieres que el botón regrese a su color normal 
        // al sacar la mano (y si aún no han ganado)
        if (other.CompareTag("Hand") && !isSolved)
        {
            SetPressed(false);
        }
    }

    public void SetPressed(bool pressed)
    {
        if (buttonRenderer && !isSolved)
            buttonRenderer.material = pressed ? matPressed : matDefault;
    }

    public void SetSolved()
    {
        isSolved = true;
        if (buttonRenderer)
            buttonRenderer.material = matSolved;
    }
}