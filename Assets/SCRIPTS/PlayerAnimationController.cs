using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationController : MonoBehaviour {

    public Animator anim;

    // Use this for initialization
    void Start() {
        //anim = GetComponent<Animator>();
    }

    public void SetHorizontalSpeed(float horizontalSpeed) {
        anim.SetFloat("Horizontal", horizontalSpeed);
    }

    public void SetVerticalSpeed(float verticalSpeed) {
        anim.SetFloat("Vertical", verticalSpeed);
    }

    public void SetIsWalking(bool isWalking) {
        anim.SetBool("isWalking", isWalking);
    }

    public void SetGetDamage() {
        anim.SetTrigger("getDamage");
    }

    public void SetIsRunning(bool isRunning) {
        anim.SetBool("isRunning", isRunning);
    }

    public void SetIsNearAttacking(bool isNearAttacking) {
        anim.SetBool("isNearAttacking", isNearAttacking);
    }

    public void SetIsShooting(bool isShooting) {
        anim.SetBool("isShooting", isShooting);
    }

    public void SetIsShooting() {
        anim.SetTrigger("isShootingTrigger");
    }

    public void SetIsReloading() {
        anim.SetTrigger("isReloadingTrigger");
        anim.ResetTrigger("isShootingTrigger");
    }

    public void SetIsZooming(bool isZooming) {
        anim.SetBool("isZooming", isZooming);
    }

    public bool GetIsZooming() {
        return anim.GetBool("isZooming");
    }
    
    public void SetIsNearAttacking() {
        anim.SetTrigger("isNearAttackingTrigger"); 
    }

    public void SetIsLaunchingGranade() {
        anim.SetTrigger("isLaunchingGranadeTrigger");
    }

    public void SetIsChangingWeapon() {
        anim.SetTrigger("isChangingWeaponTrigger");
    }

    public void SetIsStanding(bool isStanding) {
        anim.SetBool("isStanding", isStanding);
    }

    public void SetIsLongStanding(bool isLongStanding) {
        anim.SetBool("isLongStanding", isLongStanding);
    }

    public void SetIsLongStanding() {
        anim.SetTrigger("isLongStandingTrigger");
    }

    public void SetIsJumping(bool isJumping) {
        anim.SetBool("isJumping", isJumping);
        anim.ResetTrigger("isLanding");
    }

    public void SetIsJumping() {
        anim.SetTrigger("isJumping");
        anim.ResetTrigger("isLanding");
    }

    public void SetIsFalling() {
        anim.SetTrigger("isFalling");
        anim.ResetTrigger("isLanding");
    }

    public void SetIsLanding() {
        anim.SetTrigger("isLanding");
        anim.ResetTrigger("isFalling");
    }

    public void SetWalkSpeedX(float walkSpeedX) {
        anim.SetFloat("WalkSpeedX", walkSpeedX);
    }

    public void SetWalkSpeedY(float walkSpeedY) {
        anim.SetFloat("WalkSpeedY", walkSpeedY);
    }

    public void SetIsDead() {
        anim.SetTrigger("isDead");
    }

    public void StopLongStandingIdle() {
        anim.SetTrigger("StopLongStandingIdle");
    }


    public void ResetAllTriggers() {
        // trova tutti i parametri e resettali
        int i = 0;
        while (true) {
            AnimatorControllerParameter parameter = null;

            if (anim.GetParameter(i) != null)
                parameter = anim.GetParameter(i);
            else 
                break;

            if (anim.GetParameter(i) != null) {
                if (anim.GetParameter(i).type == AnimatorControllerParameterType.Trigger) {
                    anim.ResetTrigger(i);
                    Debug.Log("Trigger " + i + "resettato");
                }
            }
            Debug.Log("Index" + i);
            i++;
        }
    }

    public Animator GetAnimator() {
        return this.anim;
    }
}

