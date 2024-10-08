using UnityEngine;

/// <summary>
/// This script handles updating all the player actions (i.e, movement, combat etc)
/// </summary>
public class Player : MonoBehaviour 
{
    // References for all player functionality (components) here
    #region Player components

    private PlayerMovement _playerMovement;

    #endregion

    #region Player attributes

    [Header("Player attributes")]
    [SerializeField] private float _maxHealth = 10.0f;
    public float _currentHealth { get; private set; }

    #endregion
    void Start()
    {
        #region Instantiate components

        _playerMovement = GetComponent<PlayerMovement>();

        #endregion

        #region Set attributes

        _currentHealth = _maxHealth;

        #endregion
    }
    private void FixedUpdate()
    {  
        UpdatePlayerMovement();
        UpdatePlayerCombat();
    }

    #region Player Movement
    private void UpdatePlayerMovement()
    {
        if (_playerMovement == null) return;

        _playerMovement.MovePlayer();
    }

    #endregion

    #region Player Combat

    private void UpdatePlayerCombat()
    {
        
    }

    #endregion

    #region Player HP

    public void SetHealth(float damage)
    {
        _currentHealth -= damage;

        Debug.Log(gameObject.tag + " took: " +  damage + " damage" + ", current health = " + _currentHealth);
    }

    //Used for upgrades
    public void SetMaxHealth(float newMaxHealth)
    {
        _maxHealth = newMaxHealth;
        _currentHealth = _maxHealth;// heal to full when upgrading health

        if (_currentHealth > _maxHealth)
        {
            _currentHealth = _maxHealth;
        }

        Debug.Log("Player max health set to: " + _maxHealth);
    }

    #endregion

}



