using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace UsersService.Infrastructure.Helpers
{
    public interface IHttpHelper
    {
        Task<HttpResponseMessage> GetCallAssetAPIAsync(string apiUrl, string token);
        // Task<HttpResponseMessage> GetCallAssetWithBodyAPIAsync(string apiUrl, string contentBody);
        Task<HttpResponseMessage> GetCallMockAPIAsync(string apiUrl);
    }
    public class HttpHelper : IHttpHelper
    {

        public async Task<HttpResponseMessage> GetCallAssetAPIAsync(string apiUrl, string token)
        {
            return await Helper.GetCallAssetAuthAPIAsync(apiUrl, token);
        }

        public async Task<HttpResponseMessage> GetCallMockAPIAsync(string apiUrl)
        {
            return await Helper.GetCallMockAPIAsync(apiUrl);
        }
    }
    public static class Helper
    {
        //private static readonly string AssetBaseAssetAPIAddress = "https://localhost:7200/api/";
        //private static readonly string AssetBaseAssetAPIAddress = "https://notification-appservices.azurewebsites.net/api/v1/";
        private static readonly string AssetBaseAssetAPIAddress = Environment.GetEnvironmentVariable("NotificationAPI");
        public static async Task<HttpResponseMessage> GetCallAssetAuthAPIAsync(string apiUrl, string token)
        {

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(AssetBaseAssetAPIAddress);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = (new AuthenticationHeaderValue("Bearer", token));
                HttpResponseMessage response = await client.GetAsync(SiteURL(client.BaseAddress.ToString(), apiUrl));
                if (response.IsSuccessStatusCode)
                {
                    return response;
                }
                return response;
            }
        }

        public static async Task<HttpResponseMessage> GetCallAssetWithBody1APIAsync(string apiUrl, StringContent content)
        {
            using (var client = new HttpClient())
            {
                var response = client.PostAsync(SiteNotificationURL(apiUrl), content).Result;
                if (response.IsSuccessStatusCode)
                {
                    return response;
                }
                return response;
            }
        }
        public static async Task<HttpResponseMessage> GetCallAssetWithBodyAuthAPIAsync(string apiUrl, StringContent content, string token)
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = (new AuthenticationHeaderValue("Bearer", token));
                var response = client.PostAsync(SiteNotificationURL(apiUrl), content).Result;
                if (response.IsSuccessStatusCode)
                {
                    return response;
                }
                return response;
            }
        }
        public static async Task<HttpResponseMessage> GetCallMockAPIAsync(string apiUrl)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(AssetBaseAssetAPIAddress);
                client.DefaultRequestHeaders.Accept.Clear();
                //client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response = await client.GetAsync(apiUrl);
                if (response.IsSuccessStatusCode)
                {
                    return response;
                }
                return response;
            }
        }


        private static string SiteURL(string assetBaseAPIAddress, string urlLocation)
        {
            return assetBaseAPIAddress + urlLocation;
        }

        private static string SiteNotificationURL(string urlLocation)
        {
            return AssetBaseAssetAPIAddress + urlLocation;
        }

    }
}
