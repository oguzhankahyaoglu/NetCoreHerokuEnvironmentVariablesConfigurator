using Microsoft.Extensions.Configuration;

namespace HerokuEnvironmentVariablesConfigurator
{
    public class HerokuEnvVariableConfigurationSource : IConfigurationSource
    {
        internal readonly HEVCOptions _options;

        public HerokuEnvVariableConfigurationSource(HEVCOptions options)
        {
            _options = options;
        }
        public IConfigurationProvider Build(IConfigurationBuilder builder) 
            => new HerokuEnvVariableConfigurationProvider(this);
    }
}
