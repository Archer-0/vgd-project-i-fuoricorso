/*
 * Questo script regola il gioco e contiene tutte le variabili pubbliche accessibili da tutti gli script all'interno del gioco
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using System;
using UnityStandardAssets.Characters.FirstPerson;

public class GameController : MonoBehaviour {

    [Serializable]
    public class Fortune {
        [Tooltip("La fortuna del giocatore in percentuale. \nLa fortuna percentuale decisa prima dell'avvio del gioco e' considerata quella di default.")]
        [Range(1, 100)]
        public float _fortune = 40;

        private float _originalFortune;

        public void SetDefaultFortune() {
            _originalFortune = _fortune;
        }

        public void ResetFortuneToDefault() {
            _fortune = _originalFortune;
        }

        public void ChangeFortunePermanently(float amount) {
            _fortune += amount;
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

    [Serializable]
    public class Powers //velocitaAumentata, saltoAumentato, dannoAumentato, vitaExtra, scudo, invincibilitaTemp, invisibilitaTemp, spine, veleno, lava/fuoco
    {
        private ObjectProps objectProps;

        public void VelocitaAumentata(Collision target)
        {
            RigidbodyFirstPersonController movimento = target.collider.GetComponent<RigidbodyFirstPersonController>();
            
            float countdown = 10f;

            bool hasPower = false;

            if (countdown <= 0f && !hasPower)
            {
                movimento.movementSettings.ForwardSpeed = 16;
                movimento.movementSettings.BackwardSpeed = 8;
                movimento.movementSettings.StrafeSpeed = 8;
                movimento.movementSettings.RunMultiplier = 4;

                hasPower = true;
            }
        }

        public void SaltoAumentato(Collision target)
        {
            float countdown = 10f;

            bool hasPower = false;

            RigidbodyFirstPersonController movimento = target.collider.GetComponent<RigidbodyFirstPersonController>();
                        
            countdown -= Time.deltaTime;

            if (countdown <= 0f && !hasPower)
            {
                movimento.movementSettings.JumpForce = 100;

                hasPower = true;
            }
        }

        public void DannoAumentato()
        {

        }

        void VitaExtra()
        {

        }

        void Scudo()
        {

        }

        void InvincibilitaTemp()
        {

        }
        void InvisibilitaTemp()
        {

        }
    }

    [HideInInspector]
    public Powers powers = new Powers();

    public GameObject _player;

    public AudioClip[] _soundTracks;

    private void Start() {

    }

}
