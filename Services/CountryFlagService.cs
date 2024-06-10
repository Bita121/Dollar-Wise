using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Dollar_Wise.Services
{
    public class CountryFlagService
    {
        private const string FlagsApiUrl = "https://flagcdn.com/en/codes.json";

        public async Task<Dictionary<string, string>> FetchCountryFlags()
        {
            Dictionary<string, string> countryFlags = new Dictionary<string, string>();

            using (HttpClient client = new HttpClient())
            {
                var response = await client.GetStringAsync(FlagsApiUrl);
                var flags = JsonConvert.DeserializeObject<Dictionary<string, string>>(response);

                foreach (var flag in flags)
                {
                    string countryCode = flag.Key.ToLower();
                    string flagUrl = $"https://flagcdn.com/64x48/{countryCode}.png";
                    countryFlags[countryCode] = flagUrl;
                }
            }

            return countryFlags;
        }
    }
}
