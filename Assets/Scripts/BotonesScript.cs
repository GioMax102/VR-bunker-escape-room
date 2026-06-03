using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;

public class ButtonPuzzle : MonoBehaviour
{
    [Header("Secuencia correcta (índices 0=btn1, 1=btn2, 2=btn3)")]
    public int[] correctSequence = { 2, 0, 1 }; // 3-1-2

    [Header("Luces")]
    public Light[] lights;

    [Header("Puertas")]
    public Transform doorLeft;
    public Transform doorRight;
    public float doorSlideDistance = 2f;
    public float doorSpeed = 2f;

    [Header("Panel de error")]
    public GameObject errorPanel;
    public float errorDuration = 2f;

    [Header("Botones (mismo orden: btn1, btn2, btn3)")]
    public PuzzleButton[] buttons;

    [Header("Eventos")]
    public UnityEvent onPuzzleSolved;

    private List<int> inputSequence = new List<int>();
    private bool solved = false;
    private bool locked = false; // bloqueado durante error

    private Vector3 doorLeftOpen;
    private Vector3 doorRightOpen;

    void Start()
    {
        if (errorPanel) errorPanel.SetActive(false);

        if (doorLeft) doorLeftOpen  = doorLeft.position  + Vector3.left  * doorSlideDistance;
        if (doorRight) doorRightOpen = doorRight.position + Vector3.right * doorSlideDistance;
    }

    public void RegisterPress(int buttonIndex)
    {
        if (solved || locked) return;

        inputSequence.Add(buttonIndex);
        buttons[buttonIndex].SetPressed(true);

        // Verificar parcialmente
        int step = inputSequence.Count - 1;
        if (inputSequence[step] != correctSequence[step])
        {
            StartCoroutine(HandleError());
            return;
        }

        // Secuencia completa y correcta
        if (inputSequence.Count == correctSequence.Length)
            StartCoroutine(HandleSuccess());
    }

    IEnumerator HandleError()
    {
        locked = true;

        // Mostrar panel rojo
        if (errorPanel) errorPanel.SetActive(true);

        yield return new WaitForSeconds(errorDuration);

        // Reset
        if (errorPanel) errorPanel.SetActive(false);
        foreach (var btn in buttons) btn.SetPressed(false);
        inputSequence.Clear();
        locked = false;
    }

    IEnumerator HandleSuccess()
    {
        solved = true;

        // Luces verdes
        foreach (Light l in lights) l.color = Color.green;

        // Botones verdes
        foreach (var btn in buttons) btn.SetSolved();

        onPuzzleSolved.Invoke();

        // Abrir puertas
        StartCoroutine(SlideDoor(doorLeft, doorLeftOpen));
        StartCoroutine(SlideDoor(doorRight, doorRightOpen));

        yield return null;
    }

    IEnumerator SlideDoor(Transform door, Vector3 target)
    {
        if (door == null) yield break;
        while (Vector3.Distance(door.position, target) > 0.01f)
        {
            door.position = Vector3.MoveTowards(door.position, target, doorSpeed * Time.deltaTime);
            yield return null;
        }
    }
}
