using Hardware.Info;

namespace Monke.Metrics.Caches
{
	public interface ICache<T> where T : class
	{
		/// <summary>
		/// Gets the cached value and the last updated time. If the cached value is null, it means that the cache has not been initialized yet.
		/// </summary>
		/// <returns>A tuple of the cached data and the last time it was updated.</returns>
		(IReadOnlyList<CPU> Cpus, DateTime LastUpdated) Get();

		/// <summary>
		/// Updates the cached value and the last updated time. This method should be called when the cached value is updated.
		/// </summary>
		/// <param name="value"> The new value of the cache.</param>
		void Set(T value);
	}
}
