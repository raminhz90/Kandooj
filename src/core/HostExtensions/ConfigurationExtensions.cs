using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace Kandooj.Core.HostExtensions;

public static class ConfigurationExtensions
{
	public static IConfigurationBuilder AddSettingSource(
		this IConfigurationBuilder configurationBuilder,
		IHostEnvironment hostEnvironment,
		string[]? args) =>
		configurationBuilder
			// Add configuration from the appsettings.json file.
			.AddJsonFile("appsettings.json", optional: true, reloadOnChange: false)
			// Add configuration from an optional appsettings.development.json, appsettings.staging.json or
			// appsettings.production.json file, depending on the environment. These settings override the ones in
			// the appsettings.json file.
			.AddJsonFile($"appsettings.{hostEnvironment.EnvironmentName}.json", optional: true, reloadOnChange: false)
			.AddEnvironmentVariables()
			// Add command line options. These take the highest priority.
			.AddCommandLine(args ?? Array.Empty<string>());
}
