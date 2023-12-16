#pragma warning disable CA1506
using System.Globalization;
using Kandooj.Core.Constants;
using Kandooj.Core.HostExtensions;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;

Log.Logger = new LoggerConfiguration()
	.WriteTo.Console(LogEventLevel.Verbose, LoggerConstants.BootstrapLogTemplate, CultureInfo.InvariantCulture)
	.WriteTo.Debug(LogEventLevel.Verbose, LoggerConstants.BootstrapLogTemplate, CultureInfo.InvariantCulture)
	.CreateBootstrapLogger();
try
{
	Log.Information("Kandooj Db Manager Starting ...");
	var builder = new HostBuilder();
	_ = builder.UseContentRoot(Directory.GetCurrentDirectory())
		.ConfigureAppConfiguration(
			(hostingContext, configurationBuilder) =>
			{
				_ = configurationBuilder.AddSettingSource(hostingContext.HostingEnvironment, args);
			})
		.UseSerilog(LoggerExtension.ConfigureReloadableLogger)
		.UseDefaultServiceProvider(
			(context, options) =>
			{
				var isDevelopment = context.HostingEnvironment.IsDevelopment();
				options.ValidateScopes = isDevelopment;
				options.ValidateOnBuild = isDevelopment;
			})
		.ConfigureServices(x
			=> x.AddKandoojServices()
			)
		.UseConsoleLifetime();
	var host = builder.Build();
	await host.RunAsync().ConfigureAwait(false);

}
#pragma warning disable CA1031
catch (Exception exception)
#pragma warning restore CA1031
{
	Log.Fatal(exception,"Kandooj stopped unexpectedly");
}
finally
{
	await Log.CloseAndFlushAsync().ConfigureAwait(false);
}
