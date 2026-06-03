using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;

public class LeverPuzzle : MonoBehaviour
{
    [Header("Secuencia correcta (0=lev1, 1=lev2, 2=lev3)")]
    public int[] correctSequence = { 2, 0, 1 }; // 3-1-2

    [Header("Palancas (mismo orden: lev1, lev2, lev3)")]
    public LeverController[] levers;

    [Header("Luces")]
    public Light[] lights;

    [Header("Puertas")]
    public Transform doorLeft;
    public Transform doorRight;
    public float doorSlideDistance = 2f;
    public float doorSpeed = 2f;

    [Header("Eventos")]
    public UnityEvent onPuzzleSolved;

    private List<int> inputSequence = new List<int>();
    private bool solved = false;
    private bool locked = false;

    private Vector3 doorLeftOpen;
    private Vector3 doorRightOpen;
    [Header("Audios")]
    public AudioClip sfxBien;
    public AudioClip sfxMalHecho;

    void Start()
    {
        if (doorLeft)  doorLeftOpen  = doorLeft.position  + Vector3.left  * doorSlideDistance;
        if (doorRight) doorRightOpen = doorRight.position + Vector3.right * doorSlideDistance;
    }

    public void RegisterLever(int leverIndex)
    {
        if (solved || locked) return;

        inputSequence.Add(leverIndex);

        int step = inputSequence.Count - 1;
        if (inputSequence[step] != correctSequence[step])
        {
            StartCoroutine(HandleError());
            return;
        }

        if (inputSequence.Count == correctSequence.Length)
            StartCoroutine(HandleSuccess());
    }

    IEnumerator HandleError()
    {
        locked = true;
        if (SFXManager.instance != null) SFXManager.instance.PlayGlobalSFX(sfxMalHecho);
        yield return new WaitForSeconds(0.5f); // pequeña pausa antes de resetear

        // Levantar todas las palancas que bajaron
        foreach (var lever in levers)
            lever.ResetLever();

        inputSequence.Clear();
        locked = false;
    }

    IEnumerator HandleSuccess()
    {
        solved = true;
        if (SFXManager.instance != null) SFXManager.instance.PlayGlobalSFX(sfxBien);

        foreach (Light l in lights) l.color = Color.green;

        StartCoroutine(SlideDoor(doorLeft, doorLeftOpen));
        StartCoroutine(SlideDoor(doorRight, doorRightOpen));

        onPuzzleSolved.Invoke();
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