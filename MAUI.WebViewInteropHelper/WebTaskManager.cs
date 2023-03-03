namespace MAUI.WebViewInteropHelper
{
    internal static class WebTaskManager
    {
        private static long _lastTaskCompletionSourceId;
        private static readonly Dictionary<long, TaskCompletionSource<string>> _taskCompletionSources = new();

        internal static long GenerateTaskCompletionSource(out TaskCompletionSource<string> taskCompletionSource)
        {
            lock (_taskCompletionSources)
            {
                _lastTaskCompletionSourceId++;
                taskCompletionSource = new TaskCompletionSource<string>();
                _taskCompletionSources[_lastTaskCompletionSourceId] = taskCompletionSource;
                return _lastTaskCompletionSourceId;
            }
        }

        internal static TaskCompletionSource<string> GetAndRemoveTaskCompletionSource(long id)
        {
            TaskCompletionSource<string> taskCompletionSource;
            lock (_taskCompletionSources)
            {
                if (!_taskCompletionSources.TryGetValue(id, out taskCompletionSource))
                {
                    Console.WriteLine($"Unexpected TCS ID {id}");
                    return null;
                }

                _taskCompletionSources.Remove(id);
            }

            return taskCompletionSource;
        }
    }
}