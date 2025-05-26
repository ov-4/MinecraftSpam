//MCCScript 1.0

MCC.LoadBot(new SpamBot());

//MCCScript Extensions

class SpamBot : ChatBot
{
    List<string> spamLines = new List<string>();
    double baseDelay = 5.0;
    double randomDelay = 0.0;
    int bypassLength = 0;
    Random rnd = new Random();
    DateTime nextSpamTime = DateTime.MinValue;

    public override void Initialize()
    {
        LoadConfig();
        LogToConsole("§e Script Loaded");
    }

    public override void Update()
    {
        if (DateTime.Now >= nextSpamTime)
        {
            string message = GetRandomMessage();
            LogToConsole($"§eSelected message '{message}'");

            if (!string.IsNullOrEmpty(message))
            {
                SendText($" {message}");
            }

            double delay = baseDelay + (rnd.NextDouble() * 2 * randomDelay) - randomDelay;
            if (delay < 0.1) delay = 0.1;
            nextSpamTime = DateTime.Now.AddSeconds(delay);
        }
    }

    void LoadConfig()
    {
        try
        {
            string basePath = "config/user/SpamBot/";
            string spamPath = basePath + "spam.txt";
            string delayPath = basePath + "delay.txt";
            string randomPath = basePath + "randomDelay.txt";
            string bypassPath = basePath + "bypass.txt";

            if (File.Exists(spamPath))
                spamLines.AddRange(File.ReadAllLines(spamPath));

            if (File.Exists(delayPath))
                baseDelay = double.Parse(File.ReadAllText(delayPath).Trim());

            if (File.Exists(randomPath))
                randomDelay = double.Parse(File.ReadAllText(randomPath).Trim());

            if (File.Exists(bypassPath))
                bypassLength = int.Parse(File.ReadAllText(bypassPath).Trim());
        }
        catch (Exception ex)
        {
            LogToConsole("§e " + ex.Message);
        }
    }

    string GetRandomMessage()
    {
        if (spamLines.Count == 0) return "";

        string line = spamLines[rnd.Next(spamLines.Count)].Trim();
        string suffix = GenerateRandomSuffix(bypassLength);
        return line + suffix;
    }

    string GenerateRandomSuffix(int length) // anti cheat bypass
    {
        if (length <= 0) return "";

        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
        char[] buffer = new char[length];
        for (int i = 0; i < length; i++)
            buffer[i] = chars[rnd.Next(chars.Length)];
        return " " + new string(buffer);
    }
}
