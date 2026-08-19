// Enables NUnit's parallelization mechanism for this assembly: fixtures
// (test classes) run in parallel with each other, up to 4 at a time. Each
// ApiTestsBase-derived fixture builds and disposes its own DI container
// once, in [OneTimeSetUp]/[OneTimeTearDown] (see ApiTestsBase.cs) - fixtures
// never share a container, so they remain safe to run concurrently.
[assembly: Parallelizable(ParallelScope.Fixtures)]
[assembly: LevelOfParallelism(4)]
