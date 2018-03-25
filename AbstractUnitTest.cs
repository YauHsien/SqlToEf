using System.Text.RegularExpressions;

namespace SqlToEfUnitTestProject
{
    public class AbstractUnitTest
    {
        protected string Normalize(string value)
        {
            return Regex.Replace(value, "[ ][ ]+", "");
        }
    }
}