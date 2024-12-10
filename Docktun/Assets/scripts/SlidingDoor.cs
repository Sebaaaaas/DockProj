using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlidingDoor : MonoBehaviour
{
    Animator animator;
    bool open;
    // Guardamos los nombres de las animaciones, por eficiencia asi
    private static readonly int DoorOpenHash = Animator.StringToHash("DoorOpen");
    private static readonly int DoorCloseHash = Animator.StringToHash("DoorClose");

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Si la puerta estaba abierta, se cierra, y viceversa
    public void changeDoorOpen()
    {
        // Evitamos que abra/cierre mientras ya esta en proceso de abrir/cerrar
        if (IsAnimationPlaying("DoorOpen") || IsAnimationPlaying("DoorClose"))
        {
            Debug.Log("Cannot open door, animation already playing.");
            return;
        }

        if (open)
        {            
            animator.Play("DoorClose");
            StartCoroutine(WaitForAnimation(DoorCloseHash, () => open = false));
        }
        else
        {
            animator.Play("DoorOpen");
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


