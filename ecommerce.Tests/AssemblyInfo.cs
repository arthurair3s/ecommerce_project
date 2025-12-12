using NUnit.Framework;

[assembly: Parallelizable(ParallelScope.None)]
[assembly: LevelOfParallelism(1)]

[TestFixtureSetUp]
public class TestEnvironmentConfig
{
    [OneTimeSetUp]
    public void SetEnvironment()
    {
        Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", "Test");
    }
}