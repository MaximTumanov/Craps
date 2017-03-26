using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameStateManager : MonoBehaviour
{
    
    public NetworkPlayerManager PlayerManager;
    public enum States
    {
        PutBets,
        ThrowDices,
        Payouts
    }

    public EventManager<States> EventManager = new EventManager<States>();

    [SerializeField] private float PayoutTime = 5f;
    [SerializeField] private int BetsTime = 30;

    public States CurrentState
    {
        get {
            return _currentState;
        }
        set {
            _currentState = value;

            StateChanged();
            EventManager.Broadcast(_currentState);
        }
    }
    private States _currentState = States.PutBets;

    //trigger
    public void DiceThrown()
    {
        if (CurrentState == States.ThrowDices)
            CurrentState = States.Payouts;
    }

    public void PayoutsComplete()
    {
        
    }

    private IEnumerator WaitPayouts()
    {
        yield return new WaitForSeconds(PayoutTime);
        if (CurrentState == States.Payouts) //double check current state
            CurrentState = States.PutBets;
    }

    private IEnumerator WaitBets()
    {
        for (int i = 0; i < BetsTime; i++)
        {
            yield return new WaitForSeconds(1f);
            bool allReady = true;
            foreach (KeyValuePair<int, NetworkPlayer> item in PlayerManager.Players)
            {
                if (item.Value.PositionInRoom == -1)
                    continue;

                if (!item.Value.BetsReady)
                    allReady = false;
            }
            if (allReady)
                break;
        }

        if (_currentState == States.PutBets) 
            _currentState = States.ThrowDices;
    }

    private void StateChanged()
    {
        foreach (KeyValuePair<int, NetworkPlayer> item in PlayerManager.Players)
        {
            if (item.Value.IsShooter)
            {
                //logic for shooter in NetworkPlayerGameControls
                continue;
            }
            if (_currentState == States.Payouts || _currentState == States.ThrowDices)
            {
                item.Value.RpcNotAvailable();
                continue;
            }
            if (_currentState == States.PutBets)
            {
                item.Value.RpcBetsAvailable(new List<int>{0, 1, 2, 3}); //here will be bet check
            }
        }

        if (_currentState == States.PutBets)
            StartCoroutine(WaitBets());

        if (_currentState == States.Payouts)
            StartCoroutine(WaitPayouts());
    }

    private void Start()
    {
        CurrentState = _currentState; //apply state
    }


}

