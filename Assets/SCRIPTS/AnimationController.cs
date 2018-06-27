using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController : MonoBehaviour {

    public Animator anim;

    // Use this for initialization
    void Start() {
        //anim = GetComponent<Animator>();
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

    public void SetIsNearAttacking() {
        anim.SetTrigger("isNearAttackingTrigger");
    }

    public void SetIsShooting(bool isShooting) {
        anim.SetBool("isShooting", isShooting);
    }

    public void SetIsStanding(bool isStanding) {
        anim.SetBool("isStanding", isStanding);
    }

    public void SetIsLongStanding(bool isLongStanding) {
        anim.SetBool("isLongStanding", isLongStanding);
    }

    public void SetIsJumping(bool isJumping) {
        anim.SetBool("isJumping", isJumping);
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

    public Animator GetAnimator() {
        return this.anim;
    }
}

