# NetCoreHerokuEnvironmentVariablesConfigurator
Working with .netcore applications on Heroku, you have to manually update appsettings.json file from Heroku's Dashboard using environment variables. This package allows automatically getting all environment variables from Heroku and injects all variables (configurable) into the IConfiguration instances.

# Usage

In program.cs file, you should use HerokuEnvVariableConfigurationSource from this package:

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseKestrel()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .ConfigureAppConfiguration((builderContext, config) =>
                {
                    //Ensure that only on local development machine this configuration is injected. There is no need for environments that work on Heroku directly.
                    if (builderContext.HostingEnvironment.EnvironmentName != "Development")
                        return;
                    var options = new HECVOptions
                    {
                        BearerToken = "d9c93bf9-ae47-431d-99cc-38d1af51b286",
                        HerokuAppNameOrId = "kale-backoffice-dev",
                        OnParseVariables = variables =>
                        {
                            //variables.Remove("test");
                        }
                    };
                    config.Add(new HerokuEnvVariableConfigurationSource(options));
                })
                .ConfigureAppConfiguration((builderContext, config) =>
                {
                    IHostingEnvironment env = builderContext.HostingEnvironment;
                    config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                        .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true);

                })
                .UseStartup<Startup>();

What you need is, you have to specify a BearerToken for Heroku APIs. You can get it using "heroku auth:token" command using Heroku CLI tool. 
