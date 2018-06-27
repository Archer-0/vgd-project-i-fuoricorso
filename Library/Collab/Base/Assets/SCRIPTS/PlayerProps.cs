using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PostProcessing;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerProps : MonoBehaviour {

    public float _health;
    public float _shield;
    public float shieldDamageMultiplier;
    public bool _shieldActivated;
    public RawImage damageImage;
    public PostProcessingProfile userProfile;

	public void GetDamage(float damage) {
        if (_shield <= 0) {
            this._health -= damage;

        } else {
            _shield -= damage * shieldDamageMultiplier;

        }

        if (_health <= 0) {
            // manage game over
            Die();
        } else {
            //GetComponent<AnimationController>().SetGetDamage();
        }

        if (_health <= 30) {
            //userProfile.vignette.settings settings ;
        }
    }

    public void GetDamage(float damage, float multiplier) {
        if (_shield <= 0) {
            this._health -= (damage * multiplier);
            _shieldActivated = false;

        } else {
            _shield -= damage * (shieldDamageMultiplier * multiplier);

        }

        return;
    }

    //IEnumerator ShowDamageShort() {

    //}

    public void ChargeHealth(float amount) {
        this._health += amount;
    }

    public void Die() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
