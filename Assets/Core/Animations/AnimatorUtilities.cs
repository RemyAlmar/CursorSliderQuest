using System.Collections;
using UnityEngine;

public class AnimatorUtilities : MonoBehaviour
{
    private Animator animator;
    [SerializeField] private bool keepAnimatorStateOnDisable = false;

    void Awake()
    {
        animator = GetComponent<Animator>();
        animator.keepAnimatorStateOnDisable = keepAnimatorStateOnDisable;
    }

    public void DeactivateAnimator()
    {
        StartCoroutine(DisableAnimatorNextFrame());
    }

    public void DeactivateGameObject()
    {
        gameObject.SetActive(false);
    }

    private IEnumerator DisableAnimatorNextFrame()
    {
        yield return null;
        animator.enabled = false;
    }
}
