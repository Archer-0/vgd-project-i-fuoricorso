using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StatsAndOther : MonoBehaviour {

    #region Player Fortune
    [Serializable]
    public class Fortune {
        [Tooltip("La fortuna del giocatore in percentuale. \nLa fortuna percentuale decisa prima dell'avvio del gioco e' considerata quella di default.")]
        [Range(1, 100)]
        public float _fortune = 0;

        private float _originalFortune;

        public void SetDefaultFortune() {
            _originalFortune = _fortune;
        }

        public void ResetFortuneToDefault() {
            _fortune = _originalFortune;
        }

        public void ChangeFortunePermanently(float amount) {
            if (SceneManager.GetActiveScene().buildIndex != 6) {
                _fortune += amount;
                GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>().WriteOnInfoBox("Fortune increased (+" + amount + ")");
            }
        }

        public void ChangeFortuneForSeconds(float amount, float seconds) {
            _fortune += amount;
            //StartCoroutine(ResetFortune(seconds));
        }

        private IEnumerator ResetFortune(float seconds) {
            yield return new WaitForSecondsRealtime(seconds);
            SetDefaultFortune();
        }
    }

    public Fortune fortune = new Fortune();
    #endregion

    [HideInInspector]
    public int enemyKilled = 0;

    //public int currentLevelIndex = 2;
    //public int levelIndex = 2;

    public List<int> unlockedWeaponIds = new List<int>();

    public bool hiddenLevelReached = false;

    public void Awake() {
        DontDestroyOnLoad(this);

        if (FindObjectsOfType(GetType()).Length > 1) {
            Destroy(gameObject);
        }
    }

    void Start () {
        // initializzation
        enemyKilled = 0;
	}


    public bool IsWeaponUnlocked(int weaponId) {
        foreach (int weapId in unlockedWeaponIds) {
            if (weapId == weaponId) {
                return true;
            }
        }
        return false;
    }

    public void AddUnlockedWeapon(int weaponId) {
        unlockedWeaponIds.Add(weaponId);
    }

    public void UnlockAllWeapons() {
        for (int i = 0; i < 3; i++) {
            if (!IsWeaponUnlocked(i)) {
                unlockedWeaponIds.Add(i);
            }
        }
    }

    public void AddEnemyKilled() {
        enemyKilled++;
        fortune.ChangeFortunePermanently(.05F);
    }

}
