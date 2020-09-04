using KbartAvailability.Export.Service.SystemTests.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace KbartAvailability.Export.Service.SystemTests.Holdings
{
    public class KbartHoldingsRepository
    {
        //private const string AVAIALABILITY_AMBASSADOR_ROUTE = "http://books-availability-webservice.bobafett-dev.eks.ehost-devqa.eislz.com/";
        //private const string SUBSCRIPTION_AMBASSADOR_ROUTE = "http://books-subscription-webservice.bobafett-dev.eks.ehost-devqa.eislz.com/";

        private const string AVAIALABILITY_AMBASSADOR_ROUTE = "http://books-availability-webservice.bobafett-live.eks.ehost-live.eislz.com/";
        private const string SUBSCRIPTION_AMBASSADOR_ROUTE = "http://books-subscription-webservice.bobafett-live.eks.ehost-live.eislz.com/";

        
        private const string AVILABILITY_ENDPOINT_PATH = "api/v1/availability/site/{0}/kbart";
        private const string SUBSCRIPTION_ENDPOINT_PATH = "api/v1/subscriptions/customer/{0}/subscriptions/holdings";


        //public KbartHoldingsRepository(IHttpClientFactory httpClientFactory)
        //{
        //    _httpClient = httpClientFactory?.CreateClient("WebserviceClient")
        //                  ?? throw new ArgumentNullException(nameof(httpClientFactory));
        //}
        public KbartHoldingsRepository()
        {
        }


        public async Task<IList<KbartHolding>> GetAllHoldings(string customerId,
            CancellationToken cx = default)
        {
            var availabilityPath = string.Format(AVILABILITY_ENDPOINT_PATH, customerId);
            var subscriptionPath = string.Format(SUBSCRIPTION_ENDPOINT_PATH, customerId);
            List<KbartHolding> kbartHoldings = new List<KbartHolding>();

            using (var client = new HttpClient())
            {
                try
                {
                    var result = await client.GetAsync(AVAIALABILITY_AMBASSADOR_ROUTE + availabilityPath);
                    string json = result.Content.ReadAsStringAsync().Result;
                    List<KbartHoldingFromAvailabilityWebService> holdingsAvailability = JsonConvert.DeserializeObject<List<KbartHoldingFromAvailabilityWebService>>(json);
                    kbartHoldings.AddRange(holdingsAvailability.Select(h => new KbartHolding(h.TitleId, h.ContentClass, h.LicenseType)).ToList());
                    //string uncompressedHoldings = string.Join(Environment.NewLine, kbartHoldings.Select(h => $"{h.ProductCode}/{h.CollectionId}"));

                    var subscriptionResult = await client.GetAsync(SUBSCRIPTION_AMBASSADOR_ROUTE + subscriptionPath);
                    json = subscriptionResult.Content.ReadAsStringAsync().Result;
                    List<int> subscriptionHoldings = JsonConvert.DeserializeObject<List<int>>(json);
                    kbartHoldings.AddRange(subscriptionHoldings.Select(h => new KbartHolding(h)).ToList());
                    //string uncompressedHoldingsSub = string.Join(Environment.NewLine, kbartHoldingsSubscription.Select(h => $"{h.ProductCode}/{h.CollectionId}"));

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }

            }

            return kbartHoldings;

            //HttpResponseMessage responseMessage =
            //    await _httpClient.GetAsync($"{AVAIALABILITY_AMBASSADOR_ROUTE}{path}", cx);
            //responseMessage.EnsureSuccessStatusCode();

            //var holdings = await responseMessage.Content.ReadAsAsync<List<KbartHoldingFromAvailabilityWebService>>(cx);
            //return holdings.Select(h => new KbartHolding(h.TitleId, h.ContentClass, h.LicenseType)).ToList();
        }

        //public async Task<IList<KbartHolding>> GetSubscriptionHoldingsByCustomerId(string customerId, CancellationToken cx = default)
        //{
        //    var path = string.Format(SUBSCRIPTION_ENDPOINT_PATH, customerId);

        //    HttpResponseMessage responseMessage =
        //        await _httpClient.GetAsync($"{SUBSCRIPTION_AMBASSADOR_ROUTE}{path}", cx);
        //    responseMessage.EnsureSuccessStatusCode();
        //    var holdings = await responseMessage.Content.ReadAsAsync<List<int>>(cx);
        //    return holdings.Select(h => new KbartHolding(h)).ToList();
        //}

    }
}
