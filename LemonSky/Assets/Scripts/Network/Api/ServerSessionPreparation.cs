using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Unity.Netcode;
using UnityEngine;

public class ServerSessionPreparation : NetworkBehaviour
{
    [SerializeField]
    string Host = "0.0.0.0:5000";

    [SerializeField]
    int Repeat = 5000;

    CancellationTokenSource _tokenSourse;

    public static Session CurrentSession;
    public async void Start()

    {
        if (!IsServer) return;
        CurrentSession = null;
        _tokenSourse = new CancellationTokenSource();
        await PrepareSession(_tokenSourse.Token);
    }

    async Task PrepareSession(CancellationToken token)
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
            catch
            {
                Debug.Log($"��� ��������� ������. ��������� ������� ����� {Repeat / 1000} ���");
                if (token.IsCancellationRequested)
                {
                    Debug.Log($"������ ������ ������");
                    break;
                }
                await Task.Delay(Repeat);
            }
        }
    }
    void OnApplicationQuit()
    {
        _tokenSourse.Cancel();
    }
}
