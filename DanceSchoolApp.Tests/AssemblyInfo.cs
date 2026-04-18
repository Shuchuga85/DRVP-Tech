using Xunit;

// Disable parallel execution across test collections.
// Integration tests share process-wide SQLite state (connection registry)
// and must run sequentially to avoid concurrent Dictionary corruption.
[assembly: CollectionBehavior(DisableTestParallelization = true)]
