using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickAmmoOnTouch : MonoBehaviour {
    private FPSController fpsController = null;
    private WeaponProps weapon = null;


    private void OnCollisionEnter(Collision other) {

        // se si collide con il player
        if (other.transform.CompareTag("Player")) {

            // se e' presente un componente di tipo FPSController
            if (other.transform.GetComponent<FPSController>() != null) {

                // assegno la variabile 
                fpsController = other.transform.GetComponent<FPSController>();

                // se il gameobject a cui e' attaccato lo script contiene un componente di tipo WeaponProps
                if (GetComponent<WeaponProps>() != null) {

                    // assegno la variabile
                    weapon = GetComponent<WeaponProps>();

                    // se l'arma e' la granata utilizzo il suo metodo per ricaricare
                    if (weapon._name == "Granade") {
                        ExplosiveLauncher granadeHolderManager = other.transform.GetComponent<FPSController>().granadeHolder.GetComponent<ExplosiveLauncher>();

                        if (!granadeHolderManager.IsWeaponFull(weapon._name)) {
                            // ricarica la granata
                            granadeHolderManager.RefillAmmoToWeapon(weapon._name, weapon._currentChargerAmmo);
                            // distruggi l'oggetto
                            Destroy(gameObject);
                        }
                    
                    }     
                    // se l'arma e' un'arma generica utilizzo il metodo generico per la ricarica
                    else {
                        WeaponHolderManager weaponHolderManager = other.transform.GetComponent<FPSController>().weaponHolder.GetComponent<WeaponHolderManager>();

                        if (weaponHolderManager.IsUnlocked(weapon._name) && !weaponHolderManager.IsWeaponFull(weapon._name)) {
                            // ricarica l'arma
                            weaponHolderManager.RefillAmmoToWeapon(weapon._name, weapon._currentChargerAmmo);
                            // distruggi l'oggetto
                            Destroy(gameObject);

                        }
                    }
                }
            }   
        }
    }
}
