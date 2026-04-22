using Xunit;

// Disable parallel execution across test collections.
// Integration tests share process-wide env vars and SQLite state.
// Sequential execution avoids race conditions between test classes.
[assembly: CollectionBehavior(DisableTestParallelization = true)]
