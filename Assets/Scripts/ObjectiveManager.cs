using UnityEngine;
using TMPro; // Necesario para textos nítidos en VR

public class ObjectiveManager : MonoBehaviour
{
    public static ObjectiveManager instance;

    [Header("UI y Feedback")]
    public TextMeshProUGUI objectiveText;
    public AudioClip newObjectiveSFX; // Sonido de "Nueva Misión"

    [Header("Estado Actual")]
    public int currentStep = 0; 
    // 0 = Energía
    // 1 = Enfriamiento
    // 2 = Palancas
    // 3 = Botones finales
    // 4 = Escape

    void Awake()
    {
        if (instance == null) instance = this;
    }

    void Start()
    {
        // Instrucción inicial al cargar el juego
        UpdateObjective("OBJETIVO 1:\nEncuentra la palanca principal y restaura la energía del búnker.");
    }

    // Esta función la llamaremos desde tus otros scripts cuando terminen un puzzle
    public void AdvanceStep(int completedStep, string nextObjectiveText)
    {
        // Solo avanza si el jugador completó el paso que le correspondía
        if (currentStep == completedStep)
        {
            currentStep++;
            UpdateObjective(nextObjectiveText);

            if (SFXManager.instance != null && newObjectiveSFX != null)
                SFXManager.instance.PlayGlobalSFX(newObjectiveSFX);
        }
    }

    private void UpdateObjective(string text)
    {
        if (objectiveText != null)
        {
            objectiveText.text = text;
        }
        Debug.Log("Nuevo Objetivo: " + text);
    }
}