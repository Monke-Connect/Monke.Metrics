using Serilog.Core;
using Serilog.Events;

namespace Monke.Metrics.Extensions
{
	public class ShortSourceContextEnricher : ILogEventEnricher
	{
		public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
		{
			if (logEvent.Properties.TryGetValue("SourceContext", out LogEventPropertyValue? value))
			{
				if (value is ScalarValue { Value: string sourceContext })
				{
					string shortContext = sourceContext.Split('.').Last();
					logEvent.AddOrUpdateProperty(
						property: propertyFactory.CreateProperty("ShortSourceContext", shortContext));
				}
			}
		}
	}
}
