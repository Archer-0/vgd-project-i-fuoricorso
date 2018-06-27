using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProps : MonoBehaviour {

    //public Renderer rend = null;

    public float health = 100F;
    public float shield = 0F;
    public float shieldDamageMultiplier = 1;
    public float attackDamage = 1F;
    public float secondsBeforeDisapperaAfterDeath = 30;
    public bool disappearAfterDeath = true;

    public float animationDuration = 0;

    public bool ohNo = false;       // se false disabilita lo spawn delle cose dopo la morte

    public bool wasAlive = true;
    public bool shieldActivated = true;

    public void GetDamage(float damage) {
        if (shield <= 0) {
            this.health -= damage;
        }
        else
            shield -= damage * shieldDamageMultiplier;

        if (health <= 0) {
            Die();
        }
        else {
            GetComponent<AnimationController>().SetGetDamage();
        }
    }

    public void GetDamage(float damage, float multiplier) {
        if (shield <= 0) {
            this.health -= (damage * multiplier);
            shieldActivated = false;
        }
        else
            shield -= damage * (shieldDamageMultiplier * multiplier);

        return;
    }

    public bool IsDead() {
        if (health <= 0)
            return true;
        else
            return false;
    }

    public void ChargeHealth(float amount) {
        this.health += amount;
    }

    public void Die() {
        //GetComponent<AnimationController>().SetIsDead();
        //StartCoroutine(Disappear());

        // per non continuare a richiamare cose dopo che il nemico e' gia' morto
        if (wasAlive || ohNo) {

            IOnDeath[] onDeaths = GetComponents<IOnDeath>();
            foreach(IOnDeath dies in onDeaths) {
                dies.OnDeath();
            }

            ActivateRagdoll();
            wasAlive = false;        
            GameObject.FindGameObjectWithTag("StatsAndOther").GetComponent<StatsAndOther>().AddEnemyKilled();
            GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>().enemyCount -= 1;

            if (disappearAfterDeath)
                StartCoroutine(Disappear());
        }
    }

    private void ActivateRagdoll() {
        Animator anim = GetComponent<Animator>();
        RagdollOnOff ragdoll = GetComponent<RagdollOnOff>();
        Collider collider = GetComponent<Collider>();
        Rigidbody body = GetComponent<Rigidbody>();

        if (anim != null) {
            anim.enabled = false;
        }

        if (ragdoll != null) {
            ragdoll.OnDeath();
        }

        if (collider != null) {
            collider.enabled = false;
        }

        if (body != null) {
            body.isKinematic = true;
            body.detectCollisions = false;
        }
    }

    void Update() {

        if (shield <= 0 && shieldActivated) {
            Debug.Log("No Enemy Shield");
            shieldActivated = false;
        }
    }

    private IEnumerator Disappear() {
        yield return new WaitForSeconds(animationDuration + secondsBeforeDisapperaAfterDeath);
        Destroy(gameObject);
    }
}