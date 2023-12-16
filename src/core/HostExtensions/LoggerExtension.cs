using System.Globalization;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Configuration;
using Serilog.Events;

namespace Kandooj.Core.HostExtensions;

public static class LoggerExtension
{
	public static void ConfigureReloadableLogger(HostBuilderContext context, IServiceProvider services, LoggerConfiguration configuration)
    {
        ArgumentNullException.ThrowIfNull(configuration);
        ArgumentNullException.ThrowIfNull(context);
        configuration
	        .Enrich.WithProperty("Application", context.HostingEnvironment.ApplicationName)
	        .Enrich.WithProperty("Environment", context.HostingEnvironment.EnvironmentName)
	        .WriteTo.Conditional(x
		        => context.HostingEnvironment.IsDevelopment(), x
		        => x.Debug(formatProvider: CultureInfo.InvariantCulture)
	        ).WriteTo.File("logs/log.txt",
		        context.HostingEnvironment.IsDevelopment() ? LogEventLevel.Verbose : LogEventLevel.Information,
		        retainedFileCountLimit: 12,
		        fileSizeLimitBytes: 52428800L,
		        flushToDiskInterval: TimeSpan.FromSeconds(1.0),
		        formatProvider: CultureInfo.InvariantCulture,
		        levelSwitch: null,
		        buffered: true,
		        shared: false,
		        rollOnFileSizeLimit: true,
		        rollingInterval: RollingInterval.Month);
    }
}
