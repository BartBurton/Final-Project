using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Netcode;
using UnityEngine;

public class ServerSessionPreparation : NetworkBehaviour
{
    [SerializeField]
    string Host = "0.0.0.0:5000";

    [SerializeField]
    int Repeat = 5000;

    public static Session CurrentSession;
    public async void Start()
    {
        if (!IsServer) return;
        CurrentSession = null;
        await PrepareSession();
    }

    async Task PrepareSession()
    {
        Debug.Log("���������� ������");
        while (CurrentSession is null)
        {
            try
            {
                CurrentSession = await APIRequests.ProcessSession(Host);
                GameManager.GamePlayingTimerMax = (float)CurrentSession.Duration;
                Debug.Log($"�������� ������� - {CurrentSession.Id} �� ������: {CurrentSession.GameKey}");
                break;
            }
            catch(System.Exception e)
            {
                Debug.Log( $"��� ��������� ������. ��������� ������� ����� {Repeat / 1000} ���");
                await Task.Delay(Repeat);
            }
        }
    }

}
