/*
 * Grazie di nuovo a Brackeys per il tutorial e per lo spunto di come implemetare questo script
 */

using System.Collections;
using System.Threading;
using UnityEngine;

public class WeaponHolderManager : MonoBehaviour {

    public int selectedWeapon = 0;
    public float timeBeforeChange = .1F;

    public Camera playerCamera;
    public PlayerAnimationController playerAnimation;
    public Material invisibleMaterial;
    public WeaponProps[] weapons;


    [HideInInspector]
    public WeaponManager weaponManager;

    [HideInInspector]
    public float damageMultiplayer = 1;

    // blocco sul cambio d'arma
    // quando si sta cambiando l'arma si deve attendere l'animazione di cambio, e viene gestito tramite questo booleano
    private bool isChangingWeapon = false;

    private GameController gameController;
    private StatsAndOther stats;

    private void Start() {
        weaponManager = GetComponentInChildren<WeaponManager>(false);
        isChangingWeapon = false;
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();

        // sincronizzazione delle eventuali armi sbloccate nei livelli precedenti
        stats = GameObject.FindGameObjectWithTag("StatsAndOther").GetComponent<StatsAndOther>();
        for (int i = 0; i < weapons.Length; i++) {
            weapons[i]._isUnlocked = stats.IsWeaponUnlocked(weapons[i]._id);
            UnlockWeapon(weapons[i].name, false);
        }
    }

    private void Update() {
        
        if (!gameController.isGamePaused) {

            // se non sta ricaricando e non si sta gia cambiando l'arma
            if (!weaponManager.isReloading && !isChangingWeapon) {
            
                int previousSelectedWeapon = selectedWeapon;

                // input da rotella del mouse SI ROMPE

                //if (Input.GetAxis("Mouse ScrollWheel") > 0f) {
                //    if (selectedWeapon <= 0) {
                //        selectedWeapon = transform.childCount - 1;
                //    } else {
                //        selectedWeapon--;
                //    }
                //}
                //if (Input.GetAxis("Mouse ScrollWheel") < 0f) {
                //    if (selectedWeapon >= transform.childCount - 1) {
                //        selectedWeapon = 0;
                //    } else {
                //        selectedWeapon++;
                //    }
                //}

                // input da tasti numerici della tastiera
                if (Input.GetKeyDown(KeyCode.Alpha1)) {
                    selectedWeapon = 0;
                }

                if (Input.GetKeyDown(KeyCode.Alpha2) && transform.childCount >= 2) {
                    selectedWeapon = 1;
                }

                if (Input.GetKeyDown(KeyCode.Alpha3) && transform.childCount >= 3) {
                    selectedWeapon = 2;
                }

                if (Input.GetKeyDown(KeyCode.Alpha4) && transform.childCount >= 4) {
                    selectedWeapon = 3;
                }

                // controllo sulle armi per verificate che siano sbloccate
                // in caso negativo l'arma non vinene cambiata
                if (selectedWeapon == 0 && !weapons[0]._isUnlocked) {
                    selectedWeapon = previousSelectedWeapon;
                } else if (selectedWeapon == 1 && !weapons[1]._isUnlocked) {
                    selectedWeapon = previousSelectedWeapon;
                } else if (selectedWeapon == 2 && !weapons[2]._isUnlocked) {
                    selectedWeapon = previousSelectedWeapon;
                } else if (selectedWeapon == 3 && !weapons[3]._isUnlocked) {
                    selectedWeapon = previousSelectedWeapon;
                }

                if (previousSelectedWeapon != selectedWeapon) {
                    // imposta il blocco sul cambio d'arma
                    isChangingWeapon = true;

                    StartCoroutine(ChangeWeapon());
                }
            }
        }
    }

    // cambia l'arma
    private IEnumerator ChangeWeapon() {

        int i = 0;

        playerAnimation.SetIsChangingWeapon();

        yield return new WaitForSeconds(timeBeforeChange);

        foreach (Transform weapon in transform) {
            if (i == selectedWeapon) {
                weapon.gameObject.SetActive(true);

                if (gameController.isPlayerInvisible) {
                    weapon.GetComponent<WeaponManager>().ChangeMaterial(invisibleMaterial);
                }

            } else {
                weapon.gameObject.SetActive(false);
            }

            i++;
        }

        weaponManager = GetComponentInChildren<WeaponManager>(false);

        // lascia il blocco sul cambio d'arma
        isChangingWeapon = false;
    }

    public void UnlockWeapon(string name) {
        int i = 0;

        foreach (Transform weapon in transform) {
            if (weapon.GetComponent<WeaponProps>()._name == name) {
                weapon.GetComponent<WeaponProps>()._isUnlocked = true;
                weapons[i]._isUnlocked = true;
                stats.AddUnlockedWeapon(weapons[i]._id);

                // cambia automaticamente l'arma attuale con la nuova
                selectedWeapon = GetIndexOfWeapon(name);

                GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>().WriteOnInfoBox("New weapon found (" + weapons[i]._name + ")");

                //Debug.Log("Found new weapon");
                return;
            }

            i++;
        }
    }

    public void UnlockWeapon(string name, bool equipLastUnlocked) {
        int i = 0;

        foreach (Transform weapon in transform) {
            if (weapon.GetComponent<WeaponProps>()._name == name) {
                weapon.GetComponent<WeaponProps>()._isUnlocked = true;
                weapons[i]._isUnlocked = true;
                stats.AddUnlockedWeapon(weapons[i]._id);

                // cambia automaticamente l'arma attuale con la nuova
                if (equipLastUnlocked)
                    selectedWeapon = GetIndexOfWeapon(name);

                GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>().WriteOnInfoBox("New weapon found (" + weapons[i]._name + ")");

                //Debug.Log("Found new weapon");
                return;
            }

            i++;
        }
    }

    public int GetIndexOfWeapon(string name) {

        int i = -1;

        foreach (WeaponProps wp in weapons) {
            if (wp._name == name) {
                return i;
            }

            i++;
        }

        return -1;
    }

    public bool IsUnlocked(string name) {

        foreach (WeaponProps wp in weapons) {
            if (wp._name == name && wp._isUnlocked) {
                return true;
            }
        }

        return false;
    }

    public int GetAmmos(string name) {

        foreach (WeaponProps wp in weapons) {
            if (wp._name == name) {
                return wp._currentAmmo;
            }
        }

        return -1;
    }

    public WeaponProps GetCurrentWeaponProps() {
        return weapons[selectedWeapon];
    }

    public WeaponProps GetWeaponProps(string name) {
        foreach (WeaponProps wp in weapons) {
            if (wp._name == name) {
                return wp;
            }
        }

        return null;
    }

    public void RefillAmmoToWeapon(string name, int amount) {
        foreach (WeaponProps wp in weapons) {
            if (wp._name == name) {
                if (!IsWeaponFull(name)) {
                    wp.Reload(amount);
                }
                return;
            }
        }
    }

    public bool IsWeaponFull(string name) {
        foreach (WeaponProps wp in weapons) {
            if (wp._name == name) {
                if (wp._currentAmmo >= wp._maxAmmo) {
                    return true;
                }
            }
        }

        return false;
    }
}
