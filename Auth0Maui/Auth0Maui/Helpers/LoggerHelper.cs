namespace Auth0Maui.Helpers
{
    public class LoggerHelper
    {
        public static void LogError(string message) 
        {
            // Here, you can integrate with any logging library you prefer or simply use Console.WriteLine for now.
            Console.WriteLine($"ERROR: {message}");
        }

    }
}
