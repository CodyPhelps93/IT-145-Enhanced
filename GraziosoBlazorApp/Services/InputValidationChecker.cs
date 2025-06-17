
namespace GraziosoBlazorApp.Services
{
    public class InputValidationChecker
    {
        // list of common patterns for NoSQL injection attacks
        private static readonly string[] NoSQLInjectionPatterns = { "$where", "$ne", "$gt", "$lt", "$in", "{", "}", "[", "]", ":", "," };

        public bool SafeInput(string userInput)
        {
            if (string.IsNullOrEmpty(userInput)) return false;

            foreach (var pattern in NoSQLInjectionPatterns)
            {
                if (userInput.Contains(pattern, StringComparison.OrdinalIgnoreCase))
                {
                    return false;
                }
            }
            return true;
        }
    }
}
