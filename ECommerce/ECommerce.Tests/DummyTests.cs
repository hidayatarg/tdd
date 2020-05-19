using NUnit.Framework;

/*
  Packages Installed:

  NUnit
  Moq
  AutoFixture
  FluentAssertions
*/ 


namespace ECommerce.Tests
{
  [TestFixture]
  public class DummyTests
  {
    /// <summary>
    /// method to be called immediately before each test is run
    /// </summary>
    [SetUp]
    public void SetUp()
    {
    }

    /// <summary>
    /// method to be called immediately after each test is run
    /// </summary>
    [TearDown]
    public void TearDown()
    {
    }

    [Test(Description = "This method make sure testing environment working correctly")]
    public void It_should_configure_test_environment()
    {
      Assert.IsTrue(true);
    }
  }
}