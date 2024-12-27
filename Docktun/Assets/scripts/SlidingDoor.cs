using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlidingDoor : MonoBehaviour
{
    Animator animator;
    AnimationClip clip;
    bool open;

    static string doorOpenAnimationName = "Cube|OpenDoor";
    static string doorCloseAnimationName = "Cube|CloseDoor";
    // Guardamos los nombres de las animaciones, por eficiencia asi
    private static readonly int DoorOpenHash = Animator.StringToHash(doorOpenAnimationName);
    private static readonly int DoorCloseHash = Animator.StringToHash(doorCloseAnimationName);

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Si la puerta estaba abierta, se cierra, y viceversa
    public void changeDoorOpen()
    {
        // Evitamos que abra/cierre mientras ya esta en proceso de abrir/cerrar
        if (IsAnimationPlaying(doorOpenAnimationName) || IsAnimationPlaying(doorCloseAnimationName))
        {
            Debug.Log("Cannot move door, animation already playing.");
            return;
        }

        if (open)
        {
            Debug.Log("close");
            animator.Play(DoorCloseHash);
            StartCoroutine(WaitForAnimation(DoorCloseHash, () => open = false));
        }
        else
        {
            animator.Play(DoorOpenHash);
            StartCoroutine(WaitForAnimation(DoorOpenHash, () => open = true));
        }
    }

    private bool IsAnimationPlaying(string animationName)
    {
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        return stateInfo.IsName(animationName) && stateInfo.normalizedTime < 1.0f;
    }

    private IEnumerator WaitForAnimation(int animationHash, System.Action onComplete)
    {
        while (true)
        {
            AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
            if (stateInfo.shortNameHash == animationHash && stateInfo.normalizedTime >= 1.0f)
            {
                break;
            }
            yield return null; // Wait for the next frame
        }

        onComplete?.Invoke();
    }
}


