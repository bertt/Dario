using Dario.Formatters;
using NUnit.Framework;

namespace Dario.Tests
{
    public class PngMediaTypeFormatterTests
    {
        [Test]
        public void ShouldNotReadAnyType()
        {
            var formatter = new PngMediaTypeFormatter();
            var canRead = formatter.CanReadType(typeof(object));
            Assert.False(canRead);
        }
    }
}
