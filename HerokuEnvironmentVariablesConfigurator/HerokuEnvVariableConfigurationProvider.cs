using Microsoft.Extensions.Configuration;

namespace HerokuEnvironmentVariablesConfigurator
{
    public class HerokuEnvVariableConfigurationProvider : ConfigurationProvider
    {
        private readonly HerokuEnvVariableConfigurationSource _source;

        public HerokuEnvVariableConfigurationProvider(HerokuEnvVariableConfigurationSource source)
        {
            _source = source;
        }

        public override async void Load()
        {
            var parser = new Parser(_source._options);
            Data = await parser.Parse();
        }
    }
}