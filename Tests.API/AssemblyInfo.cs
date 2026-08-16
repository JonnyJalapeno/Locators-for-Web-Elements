// Enables NUnit's parallelization mechanism for this assembly: fixtures
// (test classes) run in parallel with each other, up to 4 at a time. Each
// ApiTestsBase-derived fixture builds and disposes its own DI
// container/RestClient per test, so fixtures do not share mutable state and
// are safe to run concurrently.
[assembly: Parallelizable(ParallelScope.Fixtures)]
[assembly: LevelOfParallelism(4)]
