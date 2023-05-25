using System.Net.Http;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;

public static class APIRequests
{
    static API _api;
    static APIRequests()
    {
        _api = new(User.Token);
    }

    public static async Task<Account> WhoIAm()
    {
        return await _api.SendAsync<Account>(Endpoints.WHO_I_AM);
    }

    public static async Task<Session> SearchSession(Guid mapId, int duration)
    {
        var req = new SearchSessionData() { Duration = duration, MapId = mapId };
        return await _api.SendAsync<Session>(Endpoints.SEARCH_SESSION, req);
    }
    public static async Task<SessionStatusResponse> StatusSession(Guid sessionId)
    {
        var dict = new Dictionary<string, string>() { { "id", sessionId.ToString() } };
        return await _api.SendAsync<SessionStatusResponse>(Endpoints.STATUS_SESSION, dict);
    }
    public static async Task<string> StopSearchSession(Guid sessionId)
    {
        var dict = new Dictionary<string, string>() { { "id", sessionId.ToString() } };
        return await _api.SendAsync<string>(Endpoints.STOP_SEARCH_SESSION, dict);
    }
    public static async Task<Session> ProcessSession(string host)
    {
        var req = new ProcessingSessionData() { Host = host };
        return await _api.SendAsync<Session>(Endpoints.PROCESS_SESSION, req);
    }

    public static async Task<Session> UpdateSession(SessionUpdateData data)
    {
        return await _api.SendAsync<Session>(Endpoints.SESSION_UPDATE, data);
    }

    public static async Task<Session> LastSession()
    {
        return await _api.SendAsync<Session>(Endpoints.LAST_SESSION);
    }

    public static async Task<IEnumerable<RatingTableItem>> RatingTable(Guid sessionId)
    {
        var urlParams = new Dictionary<string, string>() { { "sessionId", sessionId.ToString() } };
        return await _api.SendAsync<IEnumerable<RatingTableItem>>(Endpoints.SESSION_RATING_TABLE, urlParams);
    }


    public static async Task<IEnumerable<Map>> GetMaps()
    {
        return await _api.SendAsync<IEnumerable<Map>>(Endpoints.GET_MAPS);
    }

    public static async Task<IEnumerable<Stuff>> GetStuffs()
    {
        return await _api.SendAsync<IEnumerable<Stuff>>(Endpoints.GET_AVAILABLE_STUFFS);
    }
    public static async Task<Account> BuyStuff(Guid stuffId)
    {
        var urlParams = new Dictionary<string, string>() { { "id", stuffId.ToString() } };
        return await _api.SendAsync<Account>(Endpoints.BUY_STUFF, urlParams);
    }
}