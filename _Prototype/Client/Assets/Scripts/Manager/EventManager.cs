using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class EventManager
{
    private static Action<Player> EnterRoom = p => { };
    private static Action GameInit = () => { };
    private static Action<Player> GameStart = p => { };
    private static Action PlayerDead = () => { };
    private static Action<bool> TimeChange = isLight => { };
    private static Action BackToRoom = () => { };
    private static Action ExitRoom = () => { };
    private static Action<GameOverCase> GameOver = overCase => { };

    public static void SubGameInit(Action Callback)
    {
        GameInit += Callback;
    }

    public static void OccurGameInit()
    {
        GameInit?.Invoke();
    }

    public static void SubPlayerDead(Action Callback)
    {
        PlayerDead += Callback;
    }

    public static void OccurPlayerDead()
    {
        PlayerDead?.Invoke();
    }

    public static void SubTimeChange(Action<bool> Callback)
    {
        TimeChange += Callback;
    }

    public static void OccurTimeChange(bool isLight)
    {
        TimeChange?.Invoke(isLight);
    }

    public static void SubGameOver(Action<GameOverCase> Callback)
    {
        GameOver += Callback;
    }

    public static void OccurGameOver(GameOverCase type)
    {
        GameOver?.Invoke(type);
    }

    public static void SubBackToRoom(Action Callback)
    {
        BackToRoom += Callback;
    }

    public static void OccurBackToRoom()
    {
        BackToRoom?.Invoke();
    }

    public static void SubExitRoom(Action Callback)
    {
        ExitRoom += Callback;
    }

    public static void OccurExitRoom()
    {
        ExitRoom?.Invoke();
    }

    public static void SubEnterRoom(Action<Player> Callback)
    {
        EnterRoom += Callback;
    }

    public static void OccurEnterRoom(Player p)
    {
        EnterRoom?.Invoke(p);
    }

    public static void SubGameStart(Action<Player> Callback)
    {
        GameStart += Callback;
    }

    public static void OccurGameStart(Player p)
    {
        GameStart?.Invoke(p);
    }
}
