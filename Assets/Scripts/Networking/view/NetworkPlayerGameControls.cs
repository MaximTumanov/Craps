using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class NetworkPlayerGameControls : MonoBehaviour
{
//    [SerializeField] GameStateManager StateManager;
    [SerializeField] private GameObject MainControllers;
    [SerializeField] private GameObject[] BetsButtons;
    [SerializeField] private GameObject ShooterControllers;
    [SerializeField] private GameObject ShooterThrowControllers;
    [SerializeField] private Text MoneyField;
    [SerializeField] private Text BetField;
//    [SerializeField] private GameObject[] BetControllers;

    private List<int> Bets = new List<int> {
        10, 25, 50, 100, 200, 500, 1000
    };

    private int CurrentBetId = 3;

    public void PutBet(string betId)
    {
        NetworkPlayer.CurrentPlayer.PutBet(betId, Bets[CurrentBetId]);
        UpdateMoney();
    }

    public void BetPlus()
    {
        CurrentBetId++;
        if (CurrentBetId >= Bets.Count)
            CurrentBetId = Bets.Count - 1;

        BetField.text = Bets[CurrentBetId].ToString();
    }

    public void BetMinus()
    {
        CurrentBetId--;
        if (CurrentBetId < 0)
            CurrentBetId = 0;

        BetField.text = Bets[CurrentBetId].ToString();
    }

    public void BetsReady()
    {
        NetworkPlayer.CurrentPlayer.CmdBetsReady();
    }

    private void Awake()
    {
        MainControllers.SetActive(false);
        if (NetworkingMainSingletone.Instance.HostMode)
        {
//            StateManager.EventManager.AddListener(GameStateManager.States.Payouts, OnDeactivateShooter);
//            StateManager.EventManager.AddListener(GameStateManager.States.Payouts, OnDeactivateShooter);
//            StateManager.EventManager.AddListener(GameStateManager.States.ThrowDices, OnActivateShooter);
            ShooterControllers.SetActive(true);
//            ShooterThrowControllers.SetActive(StateManager.CurrentState == GameStateManager.States.ThrowDices);
        }
        else
            ShooterControllers.SetActive(false);


        NetworkingMainSingletone.Instance.NetworkEventManager.AddListener<List<int>>(NetworkingEvents.ClientControlsEnabled, OnControlsEnabled);
        NetworkingMainSingletone.Instance.NetworkEventManager.AddListener(NetworkingEvents.PlacedToTable, OnTableEnabled);
        if (NetworkPlayer.CurrentPlayer != null && NetworkPlayer.CurrentPlayer.PositionInRoom != -1)
            OnTableEnabled();
    }

    private void OnDestroy()
    {
        if (NetworkingMainSingletone.Instance.NetworkEventManager.HasListener<List<int>>(NetworkingEvents.ClientControlsEnabled, OnControlsEnabled))
            NetworkingMainSingletone.Instance.NetworkEventManager.RemoveListener<List<int>>(NetworkingEvents.ClientControlsEnabled, OnControlsEnabled);

        if (NetworkingMainSingletone.Instance.NetworkEventManager.HasListener(NetworkingEvents.PlacedToTable, OnTableEnabled))
            NetworkingMainSingletone.Instance.NetworkEventManager.RemoveListener(NetworkingEvents.PlacedToTable, OnTableEnabled);
    }

    private void UpdateMoney()
    {
        MoneyField.text = NetworkPlayer.CurrentPlayer.Coins.ToString("N0");
    }

    private void OnTableEnabled()
    {
        UpdateMoney();
        MainControllers.SetActive(true);
    }

    private void OnControlsEnabled(List<int> bets)
    {
        for (int i = 0; i < BetsButtons.Length; i++)
        {
            BetsButtons[i].SetActive(bets.Contains(i));
        }
        UpdateMoney();
    }

    #region shooter
    private void OnDeactivateShooter()
    {
        ShooterThrowControllers.SetActive(false);
    }

    private void OnActivateShooter()
    {
        ShooterThrowControllers.SetActive(true);
    }

    #endregion

}

