using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Chained2ViolenceGameManager;

public class ClassManager : MonoBehaviour
{
    public static ClassManager Instance { get; private set; }

    [Header("Players")]
    [SerializeField] private PlayerCombat _player1;
    [SerializeField] private PlayerCombat _player2;

    public static PlayerCombat.PlayerClass _currentPlayer1Class;
    public static PlayerCombat.PlayerClass _currentPlayer2Class;

    private void Awake()
    {
        _currentPlayer1Class = _player1.currentPlayerClass;
        _currentPlayer2Class = _player2.currentPlayerClass;

        _player1.OnPlayerClassChanged += PlayerCombatOnPlayer1ClassChanged;
        _player1.OnPlayerClassChanged += PlayerCombatOnPlayer2ClassChanged;

        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        if(_player1 == null)
        {

        }
        if(_player2 == null)
        {

        }
    }

    private void OnDestroy()
    {
        _player1.OnPlayerClassChanged -= PlayerCombatOnPlayer1ClassChanged;
        _player1.OnPlayerClassChanged -= PlayerCombatOnPlayer2ClassChanged;
    }

    private void PlayerCombatOnPlayer1ClassChanged(PlayerCombat.PlayerClass newClass)
    {
        _currentPlayer1Class = newClass;
    }

    private void PlayerCombatOnPlayer2ClassChanged(PlayerCombat.PlayerClass newClass)
    {
        _currentPlayer2Class = newClass;
    }
}
