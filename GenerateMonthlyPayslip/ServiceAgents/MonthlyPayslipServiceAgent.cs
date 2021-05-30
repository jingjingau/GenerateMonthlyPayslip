using GenerateMonthlyPayslip.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace GenerateMonthlyPayslip.ServiceAgents
{
    public class MonthlyPayslipServiceAgent
    {
        protected Uri _baseAddress;

        public MonthlyPayslipServiceAgent(string baseAddress)
        {
            if (baseAddress == null) throw new ArgumentNullException(nameof(baseAddress));
            _baseAddress = new Uri(baseAddress);
        }

        public async virtual Task<MonthlyPayslip> PostAsync(string uri, MonthlyPayslipRequestModel model)
        {
            using var client = new HttpClient();
            client.BaseAddress = _baseAddress;
            string jsonModel = JsonConvert.SerializeObject(model);
            HttpContent content = new StringContent(jsonModel, Encoding.UTF8, "application/json");
            HttpResponseMessage resp = await client.PostAsync(uri, content);
            Console.WriteLine($"status from POST: {resp.StatusCode}");
            resp.EnsureSuccessStatusCode();

            string jsonContent = await resp.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<MonthlyPayslip>(jsonContent);
        }


    }
}
