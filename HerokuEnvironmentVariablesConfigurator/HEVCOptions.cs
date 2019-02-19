using System;
using System.Collections.Generic;

namespace HerokuEnvironmentVariablesConfigurator
{
    public class HEVCOptions
    {
        /// <summary>
        /// A lambda function representing whether any found setting pair should be used or not. This allows configuring each setting also.
        /// You can modify, add/remove any environment variable value on this event.
        /// First element is Key, second is Value
        /// </summary>
        public Action<SortedDictionary<string, string>> OnParseVariables { get; set; }
        /// <summary>
        /// Heroku APP ID or Name
        /// </summary>
        public string HerokuAppNameOrId { get; set; }
        /// <summary>
        /// Bearer token for heroku api. Can be get using "heroku auth:token" command from Heroku CLI
        /// </summary>
        public string BearerToken { get; set; }
    }
}