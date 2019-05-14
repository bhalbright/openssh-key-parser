using System.IO;
using System.Reflection;

namespace OpenSshKey.Parser.Tests
{
    static class TestHelpers
    {
        public static string GetResourceData(string folder, string resourceName)
        {
            string resourceText = null;
            using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(
                $"OpenSshKey.Parser.Tests.Data.{folder}.{resourceName}"))
            {
                using(var streamReader = new StreamReader(stream))
                {
                    resourceText = streamReader.ReadToEnd();
                }
            }
            return resourceText;
        }
    }
}
