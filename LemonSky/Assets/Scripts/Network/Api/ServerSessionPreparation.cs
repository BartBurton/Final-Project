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
        Debug.Log("Подготовка сессии");
        while (CurrentSession is null)
        {
            try
            {
                CurrentSession = await APIRequests.ProcessSession(Host);
                Debug.Log($"Одобрена сесссия - {CurrentSession.Id} на адресе: {CurrentSession.GameKey}");
                break;
            }
            catch(System.Exception e)
            {
                Debug.Log( $"Нет доступных сессий. Следующая попытка через {Repeat / 1000} сек");
                await Task.Delay(Repeat);
            }
        }
    }

}
