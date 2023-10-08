using UnityEngine;

public class EnemyAnimationManager : MonoBehaviour
{
    private Animator animator;
    private const string HIT_TRIGGER = "Hit";
    private const string SHOOT_TRIGGER = "Shoot";
    private const string DIE_TRIGGER = "Die";
    private const string PUNCH_TRIGGER = "Punch";
    private const string DIE_INDEX = "DieAnimationIndex";
    
    

    public void Initialize(AnimatorOverrideController animatorOverrideController)
    {
        animator = GetComponentInChildren<Animator>();
        if (animatorOverrideController != null)
        {
            animator.runtimeAnimatorController = animatorOverrideController;
        }
    }

    public void SetSpeed(float speed)
    {
        if (animator!= null)
        {
            animator.SetFloat("Speed",speed);
        }
        
    }

    public void HitReaction()
    {
        animator.SetTrigger(HIT_TRIGGER);
    }

    public void ShootReaction()
    {
        animator.SetTrigger(SHOOT_TRIGGER);
    }

    public void DieReaction()
    {
        //animator.SetTrigger(DIE_TRIGGER);
        EnemyVisualsController enemyVisualsController = animator.gameObject.GetComponent<EnemyVisualsController>();
        if (enemyVisualsController != null)
        {
            animator.enabled = false;
            enemyVisualsController.EnableRagdoll();
        }
    }

    public void PunchReaction()
    {
        animator.SetTrigger(PUNCH_TRIGGER);
    }

    public void SetNewAnimatorController(AnimatorOverrideController overrideController)
    {
        if (animator != null)
        {
            animator.runtimeAnimatorController = overrideController;
        }
    }

    public float GetSpeed()
    {
        return animator.GetFloat("Speed");
    }
}
