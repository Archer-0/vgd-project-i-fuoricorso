/*
 * Interfaccia per la gestione del trigger dei power up
 */

using UnityEngine;

public interface IOnPowerUpTrigger {
    void OnPowerUpEnter(Collider target);
}