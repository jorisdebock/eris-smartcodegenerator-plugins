using SmartCodeGenerator.TestKit;
using Xunit;
using static Eris.SmartCodeGenerator.ImmutableGenerator.Tests.ImmutableConstructorTestCases;

namespace Eris.SmartCodeGenerator.ImmutableGenerator.Tests
{
    public class ImmutableConstructorTests
    {
        private const string _ignoreGeneratorVersionPattern = /*lang=regex*/ @"\d+\.\d+\.\d+\.\d+";

        [Fact]
        public void Should_Create_Constructor_For_GetOnlyProperties()
        {
            var generatorFixture = new SmartCodeGeneratorFixture(typeof(OnBuildImmutableConstructorGenerator), new[]
            {
                ReferenceSource.FromType<ImmutableConstructorAttribute>()
            });

            generatorFixture.AssertGeneratedCode(GetOnlyProperties, GetOnlyProperties_Transformed, _ignoreGeneratorVersionPattern);
        }
    }
}
