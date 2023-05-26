using UnityEngine;
using System.Net.Http;
using System;
using Mono.CSharp;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Net;
using System.Text;
using Newtonsoft.Json;
using System.Collections.Generic;

public class API
{
    static HttpClient _client;
    //string _url = "https://localhost:7109";
    string _url = "https://lonewald.ru";

    public API(string token)
    {
        _client = new HttpClient();
        _client.BaseAddress = new Uri(_url);
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
    }

    public async Task<T> SendAsync<T>(Endpoints.EndpointInfo endpointinfo)
    {
        return await SendAsync<T>(endpointinfo.Method, endpointinfo.Path);
    }
    public async Task<T> SendAsync<T>(Endpoints.EndpointInfo endpointinfo, IRequest requestBody = default)
    {
        return await SendAsync<T>(endpointinfo.Method, endpointinfo.Path, requestBody);
    }
    public async Task<T> SendAsync<T>(Endpoints.EndpointInfo endpointinfo, IDictionary<string, string> urlParams = default)
    {
        return await SendAsync<T>(endpointinfo.Method, endpointinfo.Path, urlParams);
    }
    public async Task<T> SendAsync<T>(HttpMethod type, string endPoint)
    {
        if (type == HttpMethod.Post || type == HttpMethod.Put)
            return await SendAsync<T>(type: type, endPoint: endPoint, requestBody: null);
        else
            return await SendAsync<T>(type: type, endPoint: endPoint, urlParams: null);
    }
    public async Task<T> SendAsync<T>(HttpMethod type, string endPoint, IRequest requestBody = default)
    {
        var request = new HttpRequestMessage(type, endPoint);
        if (requestBody != null)
        {
            var json = JsonConvert.SerializeObject(requestBody);
            request.Content = new StringContent(json, Encoding.UTF8, "application/json");
        }
        return await SendAsync<T>(request);
    }
    public async Task<T> SendAsync<T>(HttpMethod type, string endPoint, IDictionary<string, string> urlParams = default)
    {
        StringBuilder str = new StringBuilder("?");
        if (urlParams != null)
            foreach (var param in urlParams)
                str.Append($"{param.Key}={param.Value}&");

        var request = new HttpRequestMessage(type, endPoint + str.ToString());

        return await SendAsync<T>(request);
    }
    public async Task<T> SendAsync<T>(HttpRequestMessage message)
    {
        var response = await _client.SendAsync(message);
        //if (!response.IsSuccessStatusCode)
            //throw new Exception(response.ReasonPhrase);
        var str = await response.Content.ReadAsStringAsync();
        Response<T> resultObject;
        if (!TryDeserealize<Response<T>>(str, out resultObject))
            throw new Exception(response.ReasonPhrase);
        if (!resultObject.IsValid)
            throw new Exception(resultObject.Error.Message);
        return resultObject.Data;
    }

    bool TryDeserealize<T>(string str, out T result)
    {
        try
        {
            result = JsonConvert.DeserializeObject<T>(str);
            return true;
        }
        catch
        {
            result = default;
            return false;
        }
    }
}
public class Endpoints
{
    public static EndpointInfo WHO_I_AM = new EndpointInfo() { Method = HttpMethod.Get, Path = "/Authorization/WhoIAm" };
    
    public static EndpointInfo SEARCH_SESSION = new EndpointInfo() { Method = HttpMethod.Post, Path = "/GamePlay/Sessions/Search" };
    public static EndpointInfo STATUS_SESSION = new EndpointInfo() { Method = HttpMethod.Post, Path = "/GamePlay/Sessions/Status" };
    public static EndpointInfo STOP_SEARCH_SESSION = new EndpointInfo() { Method = HttpMethod.Post, Path = "/GamePlay/Sessions/StopSearch" };
    public static EndpointInfo PROCESS_SESSION = new EndpointInfo() { Method = HttpMethod.Post, Path = "/GamePlay/Sessions/Process" };
    public static EndpointInfo GET_MAPS = new EndpointInfo() { Method = HttpMethod.Get, Path = "/GamePlay/Maps/Get" };

    public static EndpointInfo GET_AVAILABLE_STUFFS = new EndpointInfo() { Method = HttpMethod.Get, Path = "/Stuff/Get" };
    public static EndpointInfo BUY_STUFF = new EndpointInfo() { Method= HttpMethod.Post, Path = "/Stuff/Buy" };


    public static EndpointInfo SESSION_UPDATE = new EndpointInfo() { Method = HttpMethod.Put, Path = "/Session/Update" };    

    public static EndpointInfo LAST_SESSION = new EndpointInfo() { Method = HttpMethod.Get, Path = "/Session/LastSession" };    
    public static EndpointInfo SESSION_RATING_TABLE = new EndpointInfo() { Method = HttpMethod.Get, Path = "/Session/RatingTable" };    
    
    public struct EndpointInfo
    {
        public string Path;
        public HttpMethod Method;
    }
}