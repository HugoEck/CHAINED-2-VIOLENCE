using System;
using System.Collections;
using UnityEngine;

/// <summary>
/// This script handles all the ultimate abilites (updating and synchronizing player inputs to use them)
/// </summary>

[RequireComponent(typeof(LaserChain))]
[RequireComponent(typeof(ElectricChain))]
[RequireComponent(typeof(FireChain))]
[RequireComponent(typeof(GhostChain))]
public class UltimateAbilityManager : MonoBehaviour
{
    public enum UltimateAbilities
    {
        LaserChain,
        ElectricChain,
        FireChain,
        GhostChain
    }

    public UltimateAbilities _currentUltimateAbility = UltimateAbilities.ElectricChain;

    [Header("Ultimate attributes")]
    [SerializeField] private int _timeForOtherPlayerToUseUltimate = 3;

    public static UltimateAbilityManager instance { get; private set; }    
    
    [HideInInspector]
    public bool _bIsBothPlayersUsingUltimate = false;

    private bool _bIsPlayer1UsingUltimateAbility = false;
    private bool _bIsPlayer2UsingUltimateAbility = false;
    private bool _bIsUltimateUsed = false;

    public static bool bISSceneChanged = false;

    private Coroutine _player1WaitCoroutine;
    private Coroutine _player2WaitCoroutine;

    #region Ultimate ability components

    private LaserChain _laserChain;
    private ElectricChain _electricChain;
    private FireChain _fireChain;
    private GhostChain _ghostChain;

    #endregion

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        _laserChain = GetComponent<LaserChain>();
        _electricChain = GetComponent<ElectricChain>();
        _fireChain = GetComponent<FireChain>();
        _ghostChain = GetComponent<GhostChain>();

        Chained2ViolenceGameManager.Instance.OnSceneStateChanged += Chained2VilenceGameManagerOnSceneStateChanged;
    }
    private void OnDestroy()
    {
        Chained2ViolenceGameManager.Instance.OnSceneStateChanged -= Chained2VilenceGameManagerOnSceneStateChanged;
    }
    private void Chained2VilenceGameManagerOnSceneStateChanged(Chained2ViolenceGameManager.SceneState state)
    {
        DeactivateUltimateChains();
    }
    
    public void DeactivateUltimateChains()
    {
        _laserChain.Deactivate();
        _laserChain._cooldownTimer = 0;
        _electricChain.Deactivate();
        _electricChain._cooldownTimer = 0;
        _fireChain.Deactivate();
        _fireChain._cooldownTimer = 0;
        _ghostChain.Deactivate();
        _ghostChain._cooldownTimer = 0;
    }

    /// <summary>
    /// Call this from UI logic that lets players change their current ultimate ability
    /// </summary>
    /// <param name="newUltimateAbiltiy"></param>
    public void SetCurrentUltimateAbility(UltimateAbilities newUltimateAbiltiy)
    {
        _currentUltimateAbility = newUltimateAbiltiy;
    }

    /// <summary>
    /// Update the current Ultimate ability that is being used
    /// </summary>
    private void Update()
    {
        switch (_currentUltimateAbility)
        {
            case UltimateAbilities.LaserChain:
                _laserChain.UpdateUltimateAttack();

                break;

            case UltimateAbilities.ElectricChain:
                _electricChain.UpdateUltimateAttack();

                break;

            case UltimateAbilities.FireChain:

                _fireChain.UpdateUltimateAttack();

                break;

            case UltimateAbilities.GhostChain:
                _ghostChain.UpdateUltimateAttack();

                break;
                            
        }
    }

    /// <summary>
    /// Activate the current Ultimate ability that is being used
    /// </summary>
    public void UseUltimateAbility()
    {
        if (UIUltimateBar.instance.CanActivateUltimate())
        {
            UIUltimateBar.instance.ActivateUltimate(); // Keeps track of when ultimates are activated for the UI bar.
        }

        switch (_currentUltimateAbility)
        {
            case UltimateAbilities.LaserChain:
                _laserChain.UseUltimate();
                break;

            case UltimateAbilities.ElectricChain:
                _electricChain.UseUltimate();
                break;

            case UltimateAbilities.FireChain:
                _fireChain.UseUltimate();
                break;

            case UltimateAbilities.GhostChain:
                _ghostChain.UseUltimate();
                break;
        }
    }

    /// <summary>
    /// Use the current Ultimate ability for the players (Called from Player script)
    /// </summary>
    public void UseUltimateAbilityPlayer1()
    {
        _bIsPlayer1UsingUltimateAbility = true;
        UIUltimateBar.instance.Player1Activate();

        if (_bIsPlayer2UsingUltimateAbility && !_bIsUltimateUsed)
        {
            // Both players have activated, use the ultimate
            _bIsUltimateUsed = true;
            UseUltimateAbility();

            ResetUltimateStates();
        }
        else
        {
            // Start the timer for Player 2 to activate
            if (_player1WaitCoroutine != null) StopCoroutine(_player1WaitCoroutine);
            _player1WaitCoroutine = StartCoroutine(WaitForPlayer2ToUseAbility());
        }
    }

    /// <summary>
    /// Use the current Ultimate ability for the players (Called from Player script)
    /// </summary>
    public void UseUltimateAbilityPlayer2()
    {
        _bIsPlayer2UsingUltimateAbility = true;
        UIUltimateBar.instance.Player2Activate();
    
        if (_bIsPlayer1UsingUltimateAbility && !_bIsUltimateUsed)
        {
            // Both players have activated, use the ultimate
            _bIsUltimateUsed = true;
            UseUltimateAbility();

            ResetUltimateStates();
        }
        else
        {
            // Start the timer for Player 1 to activate
            if (_player2WaitCoroutine != null) StopCoroutine(_player2WaitCoroutine);
            _player2WaitCoroutine = StartCoroutine(WaitForPlayer1ToUseAbility());
        }
    }

    /// <summary>
    /// Waits x seconds for player 1 to activate ultimate, otherwise it won't activate
    /// </summary>
    /// <returns></returns>
    private IEnumerator WaitForPlayer1ToUseAbility()
    {
        yield return new WaitForSeconds(_timeForOtherPlayerToUseUltimate);

        // If Player 1 has not activated, reset
        if (!_bIsPlayer1UsingUltimateAbility)
        {
            ResetUltimateStates();
        }
    }

    /// <summary>
    /// Waits x seconds for player 2 to activate ultimate, otherwise it won't activate
    /// </summary>
    /// <returns></returns>
    private IEnumerator WaitForPlayer2ToUseAbility()
    {
        yield return new WaitForSeconds(_timeForOtherPlayerToUseUltimate);

        // If Player 2 has not activated, reset
        if (!_bIsPlayer2UsingUltimateAbility)
        {
            ResetUltimateStates();
        }
    }

    /// <summary>
    /// Reset ultimate ability state for both players
    /// </summary>
    private void ResetUltimateStates()
    {
        _bIsPlayer1UsingUltimateAbility = false;
        _bIsPlayer2UsingUltimateAbility = false;
        _bIsUltimateUsed = false;

        // Stop waiting coroutines if they are still running
        if (_player1WaitCoroutine != null)
        {
            StopCoroutine(_player1WaitCoroutine);
            _player1WaitCoroutine = null;
        }

        if (_player2WaitCoroutine != null)
        {
            StopCoroutine(_player2WaitCoroutine);
            _player2WaitCoroutine = null;
        }
    }
}

