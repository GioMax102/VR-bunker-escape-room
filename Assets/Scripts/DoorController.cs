using System.Collections;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    [Header("El pivot de la puerta (el objeto que rota)")]
    public Transform doorPivot;

    [Header("Rotación final al abrir (en grados locales)")]
    public Vector3 openRotation = new Vector3(0, 90, 0);

    [Header("Velocidad de apertura")]
    public float openSpeed = 1.5f;

    public AudioSource openSound;

    private bool isOpen = false;

    public void OpenDoor()
    {
        if (isOpen) return;
        isOpen = true;
        if (openSound) openSound.Play();
        StartCoroutine(RotateDoor());
    }

    IEnumerator RotateDoor()
    {
        Quaternion startRot = doorPivot.localRotation;
        Quaternion targetRot = Quaternion.Euler(openRotation);
        float t = 0f;

        while (t < 1f)
        {
            t += Time.deltaTime * openSpeed;
            doorPivot.localRotation = Quaternion.Lerp(startRot, targetRot, t);
            yield return null;
        }

        doorPivot.localRotation = targetRot;
    }
}