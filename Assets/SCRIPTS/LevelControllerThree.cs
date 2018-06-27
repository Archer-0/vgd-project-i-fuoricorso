using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelControllerThree : MonoBehaviour {

    public GameController gameController;
    public GameObject boundaryTrigger;
    public GameObject portals;

    public GameObject boss;
    public int godsLife = 80;

    public ParticleSystem bossParticleSystem;

    public IdolManager[] idols;

    public Slider enemyHealthBar;

    private float bossLifeFraction;

    List<IdolManager> idolsToComplete = new List<IdolManager>();

	void Start () {
        bossLifeFraction = godsLife / idols.Length;
        enemyHealthBar.maxValue = godsLife;
        enemyHealthBar.minValue = 0;
        enemyHealthBar.value = godsLife;
        Debug.Log("idols" + idols.Length);

        for (int i = 0; i < idols.Length; i++) {
            idolsToComplete.Add(idols[i]);
        }

        ActivateNextIdol();
    }

    public void OnIdolInteracted(IdolManager idol) {
        idolsToComplete.Remove(idol);
        //idol.completed = true;
        //idol.DisableIdol();
        enemyHealthBar.value = enemyHealthBar.value - bossLifeFraction;
        //bossParticleSystem.emissionRate = (bossParticleSystem.emissionRate / idols.Length) * idolsToComplete.Count;
        var noise = bossParticleSystem.noise;
        noise.strengthXMultiplier += 1;
        noise.strengthYMultiplier += 1;
        noise.strengthZMultiplier += 1;

        ActivateNextIdol();
    }

    void ActivateNextIdol() {
 
        if (idolsToComplete.Count > 0) {
            // estrazione di un indice dalla lista temp
            int candidate = Random.Range(0, idolsToComplete.Count - 1);
            // attivazione dell'idolo
            idolsToComplete[candidate].EnableIdol();
            Debug.Log("Candidate: " + idolsToComplete[candidate].name);
        } else {
            StartCoroutine(HideBossLifeCanvas());
            EndGame();
        }
    }

    IEnumerator HideBossLifeCanvas() {
        CanvasGroup canvas = transform.GetChild(0).GetComponentInChildren<CanvasGroup>();
        
        while (canvas.alpha > 0) {
            canvas.alpha -= 6 * Time.deltaTime;
            yield return null;
        }
    }

    private void EndGame() {
        //TODO: da fare
        Debug.Log("End Game");
        bossParticleSystem.transform.parent = null;
        Destroy(boss);
        bossParticleSystem.enableEmission = false;

        boundaryTrigger.AddComponent<EnemyDeadOnTrigger>();
        gameController.WriteOnInfoBox("Boss defeated!");

        portals.SetActive(true);
    }

    private void SaveGameFinished() {
        PlayerPrefs.SetInt("isGameFinished", 1);
    }

    private void EnableIdol(IdolManager idol) {
        idol.EnableIdol();
    }
}
