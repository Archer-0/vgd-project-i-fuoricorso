using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveWeaponsPermanently : MonoBehaviour {

    //List<WeaponProps> unlockedWeapons = new List<WeaponProps>();

	public void SaveWeapons(GameObject player) {
        StatsAndOther stats = GameObject.FindGameObjectWithTag("StatsAndOther").GetComponent<StatsAndOther>();

        WeaponProps[] weapons = player.GetComponent<FPSController>().weaponHolder.GetComponent<WeaponHolderManager>().weapons;

        foreach(WeaponProps wp in weapons) {
            if (wp._isUnlocked)
                stats.AddUnlockedWeapon(wp._id);
        }

    }
}
