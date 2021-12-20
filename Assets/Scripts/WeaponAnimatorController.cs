using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponAnimatorController : MonoBehaviour
{

    [SerializeField] private Animator currentAnimator;

    private int fire = Animator.StringToHash("Fire");
    private int reload = Animator.StringToHash("Reload");

    public void Setup(Animator newAnimator)
    {
        currentAnimator = newAnimator;
    }

    #region ---Set Trigger---
    public void SetTrigger_reload()
    {
        currentAnimator.SetTrigger(reload);
    }
    public void ResetTrigger_reload()
    {
        currentAnimator.ResetTrigger(reload);
    }
    public void SetTrigger_fire()
    {
        currentAnimator.SetTrigger(fire);
    }
    public void ResetTrigger_fire()
    {
        currentAnimator.ResetTrigger(fire);
    }
    #endregion

}
