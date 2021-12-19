using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimatorController : MonoBehaviour
{
    [SerializeField] private Animator playerAnimator;

    private int fire = Animator.StringToHash("Fire");
    private int reload = Animator.StringToHash("Reload");

    #region ---Set Trigger---
    public void SetTrigger_reload()
    {
        playerAnimator.SetTrigger(reload);
    }
    public void ResetTrigger_reload()
    {
        playerAnimator.ResetTrigger(reload);
    }
    public void SetTrigger_fire()
    {
        playerAnimator.SetTrigger(fire);
    }
    public void ResetTrigger_fire()
    {
        playerAnimator.ResetTrigger(fire);
    }
    #endregion

}
