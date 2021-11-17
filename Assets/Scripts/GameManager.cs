using System;
using System.Collections;
using System.Collections.Generic;
using GameStates;
using Models;
using Photon.Pun;
using Photon.Realtime;
using ScriptableObjects;
using UniRx;
using UnityEngine;

public class GameManager : MonoBehaviourPunCallbacks
{
    public enum State
    {
        None,
        Initial,
        Title,
        Login,
        LoginFailed
    }

    private readonly Dictionary<State, GameState> _states = new Dictionary<State, GameState>
    {
        {
            State.None, new GameStateNone()
        },
        {
            State.Initial, new GameStateInitial()
        },
        {
            State.Title, new GameStateTitle()
        },
        {
            State.Login, new GameStateLogin()
        }
    };
    
    private GameState _state = new GameStateNone();
    private void Awake()
    {
        GameModel.State.Subscribe(state =>
        {
            _state.OnExit();
            _state = _states[state];
            _state.OnEnter();
        }).AddTo(gameObject);
    }

    private void Update()
    {
        _state.OnUpdate();
    }

    private void FixedUpdate()
    {
        _state.OnFixedUpdate();
    }

    public override void OnConnectedToMaster()
    {
        _state.OnConnectedToMaster();
    }

    public override void OnJoinedLobby()
    {
        _state.OnJoinedLobby();
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        _state.OnDisconnected(cause);
    }
}
