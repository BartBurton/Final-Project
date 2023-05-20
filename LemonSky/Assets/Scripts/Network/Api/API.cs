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
    string _url = "https://localhost:7109";
    //string _url = "https://lonewald.ru";
    public API(string token)
    {
        Debug.Log("Api");
        Debug.Log(token);
        _client = new HttpClient();
        _client.BaseAddress = new Uri(_url);
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
    }

    public async Task<Account> WhoIAm()
    {
        return await SendAsync<Account>(HttpMethod.Get, Endpoints.WHO_I_AM);
    }
    public async Task<T> SendAsync<T>(HttpMethod type, string endPoint)
    {
        if(type == HttpMethod.Post || type == HttpMethod.Put)
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
            //var json = JsonUtility.ToJson(requestBody);
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
        if (!response.IsSuccessStatusCode)
            throw new Exception(response.ReasonPhrase);
        var str = await response.Content.ReadAsStringAsync();
        var str1 = "";
        var resultObject = JsonConvert.DeserializeObject<Response<T>>(str);
        //var resultObject = JsonUtility.FromJson<T>(str);
        return resultObject.Data;
    }
}
public class Endpoints
{
    public static string WHO_I_AM = "/Authorization/WhoIAm";
}