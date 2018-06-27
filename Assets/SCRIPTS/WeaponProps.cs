using UnityEngine;
using System;


public class WeaponProps : MonoBehaviour {
	
	[Tooltip("Add automatically a tag, to the game object, used to pass informations to the information window in UI.")]
	public bool _addPlayTimeTag = true;
    
    public bool _isUnlocked = false;

	[Tooltip ("The name of the weapon.")]
    [TextArea(1, 2)]
	public String _name;

	[Tooltip("The description of the weapon.")]
    [TextArea(5, 7)]
	public String _description;

	[Tooltip("The damage that this weapon does to enemies or things.")]
	public float _damage = 1f;

	[Tooltip("How far the bullet can go before starting to fall.")]
	public float _range = 100f;

    [Tooltip("The impact force applied to a rigidbody when shot")]
    public float _impactForce = 10;

    [Tooltip("Infinite ammos?")]
    public bool _infinteAmmos;

	// Are the remaining ammos before recharging, displayed in UI, on the left of the "x /"
	[Tooltip("The number of the bullets in the weapon charger (less or egual to _maxChargerAmmo) (This will change during the game).")]
	public int _currentChargerAmmo;

	// used only on script, when recharging
	[Tooltip("The max number of the bullets that the charger can contains.")]
	public int _maxChargerAmmo;

	// used when the player find the weapon on the ground. Tis value can be used to display the ammos in the UI, on the right of the "/ x"
	[Tooltip("The max number of the bullets that the weapon is carrying. (This will change during the game)")]
	public int _currentAmmo;

	// used when the player find ammos on the ground
	[Tooltip("The max number of the bullets that the player can carry for that weapon.")]
	public int _maxAmmo;

	[Tooltip("The identifier of the weapon. (Is used to find the weapon and the attributes of it during the game).")]
	public int _id;

	[Tooltip("Determines if the weapon will continue to shoot while the fire button is pressed, or just one time till next relase and press")]
	public bool _automatic = false;

	[Tooltip("The minimum time between the shoot of one bullet and the next one in seconds.")]
	public float _fireRate;

    [Tooltip("if is a granade.")]
    public bool _isGranade = false;

    [Tooltip("Explosive radius.")]
    public float _granadeRadius = 10F;

    [Tooltip("Time before explosion.")]
    public float _granadeDelay = 3F;

    [Tooltip("Force need to explode instantly.")]
    public float _granadeForceImpactOfContact = 50;

    public float _granadExplosionForce = 300F;


    void Start () {
		this.gameObject.layer = 0;

		if (_addPlayTimeTag)
			this.tag = "DetailedWeapon";

		/* *** CONTROLS *** */

		if (_currentChargerAmmo > _maxChargerAmmo)
			_currentChargerAmmo = _maxChargerAmmo;

		if (_maxChargerAmmo > _maxAmmo)
			_maxChargerAmmo = _maxAmmo;

		if (_currentAmmo > _maxAmmo)
			_currentAmmo = _maxAmmo;
	}

    public void Reload (int amount) {
        int reloaded = 0;

        if ((_currentAmmo + amount) >= _maxAmmo) {
            reloaded = (_maxAmmo - _currentAmmo);
            _currentAmmo = _maxAmmo;
        } else {
            reloaded = amount;
            _currentAmmo += amount;
        }

        GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>().WriteOnInfoBox("Found \"" + _name + "\" ammo (+" + reloaded + ")");
    }
}
