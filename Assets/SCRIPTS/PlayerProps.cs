using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PostProcessing;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerProps : MonoBehaviour {

    public float _health;
    public float _shield;
    public float shieldDamageMultiplier = 1;
    public bool _shieldActivated;
    public bool isInvincible = false;
    public CanvasGroup damageImage;
    public float damageImageMinRange; 
    public float damageImageMaxRange; 
    public PostProcessingProfile userProfile;
    public GameObject mesh;
    
	public void GetDamage(float damage) {

        if (!isInvincible) {

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
        }

        if (_health <= 30) {
            //userProfile.vignette.settings settings ;
        }
    }

    public void GetDamage(float damage, float multiplier) {

        if (!isInvincible) {

            if (_shield <= 0) {
                this._health -= (damage * multiplier);
                _shieldActivated = false;
            } else {
                _shield -= damage * (shieldDamageMultiplier * multiplier);
            }

        }
        return;
    }

    //IEnumerator ShowDamageShort() {

    //}

    public void ChargeHealth(float amount) {
        if (_health < 100 && (_health + amount) <= 100) {
            _health += amount;
        } else if (_health <= 100 && (_health + amount) > 100) {
            _health = 100;
        } else {
            _health += amount;
        }
    }

    public void ChargeShield(float amount)
    {
        if (_shield < 100 && (_shield + amount) <= 100)
        {
            _shield += amount;
        }
        else if (_shield <= 100 && (_shield + amount) > 100)
        {
            _shield = 100;
        }
        else
        {
            _shield += amount;
        }
    }

    public void Die() {
        GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>().PlayerDead();
        //SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    void Start()
    {
        if(_shield > 1)
        {
            _shieldActivated = true;
        }
    }

    private void FixedUpdate() {
        if (_health < damageImageMaxRange) {
            damageImage.alpha = Map(_health, damageImageMaxRange, damageImageMinRange, 0, 1);
        } else if (_health > damageImageMaxRange) {
            damageImage.alpha = 0;
        } else {
            damageImage.alpha = 1;
        }
    }

    public void ChangeSkin(Material material) {
        mesh.GetComponent<SkinnedMeshRenderer>().material = material;
    }

    public float Map(float value, float fromSource, float toSource, float fromTarget, float toTarget) {
        return (value - fromSource) / (toSource - fromSource) * (toTarget - fromTarget) + fromTarget;
    }
}
