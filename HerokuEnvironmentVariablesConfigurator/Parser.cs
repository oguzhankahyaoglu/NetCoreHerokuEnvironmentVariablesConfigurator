using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace HerokuEnvironmentVariablesConfigurator
{
    internal class Parser
    {
        private readonly HEVCOptions _options;

        public Parser(HEVCOptions options)
        {
            _options = options;
        }

        private async Task<string> GetVariablesRaw(bool hideException)
        {
            using (var client = new HttpClient())
            {
                try
                {
                    client.DefaultRequestHeaders.Add("Accept", "application/vnd.heroku+json; version=3");
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _options.BearerToken);
                    var result = await client.GetStringAsync($"https://api.heroku.com/apps/{_options.HerokuAppNameOrId}/config-vars");
                    Debug.WriteLine("[HerokuEnvironmentVariablesConfigurator] API Result: " + result);
                    return result;
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("[HerokuEnvironmentVariablesConfigurator] API Exception: " + ex);
                    Debugger.Break();
                    //try second time...
                    if (hideException)
                        return await GetVariablesRaw(false);
                    throw new Exception("Exception occured while getting Heroku Environment Variables.", ex);
                }
            }
        }

        public async Task<IDictionary<string, string>> Parse()
        {
            var str = await GetVariablesRaw(true);
            str = str.Replace("{\"", "\"");
            str = str.Replace("\"}", "\"");
            var matches = Regex.Matches(str, @"""([^""]*)"":""([^""]*)""(,?)", RegexOptions.Multiline);

            SortedDictionary<string, string> _data = new SortedDictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            foreach (Match match in matches)
            {
                var groups = match.Groups.Cast<Group>().ToArray();
                var key = groups.ElementAtOrDefault(1)?.Value;
                var value = groups.ElementAtOrDefault(2)?.Value;
                if (string.IsNullOrEmpty(key))
                    continue;
                _data[key] = value;
            }
            Debug.WriteLine("[HerokuEnvironmentVariablesConfigurator] API Data count: " + _data.Keys.Count);
            _options?.OnParseVariables(_data);

            if (_data.Keys.Count == 0)
            {
                Debugger.Break();
                throw new Exception("Heroku api returned empty response or no environment variables are defined. Check variables in dashboard.");
            }
            return _data;
        }
    }
}