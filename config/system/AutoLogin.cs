//MCCScript 1.0

MCC.LoadBot(new AutoLogin());

//MCCScript Extensions

class AutoLogin : ChatBot
{
    public override void AfterGameJoined()
    {
        string path = "config/user/AutoLogin/password.txt";
        if (System.IO.File.Exists(path))
        {
            string password = System.IO.File.ReadAllText(path).Trim();
            if (!string.IsNullOrEmpty(password))
            {
                SendText("/login " + password);
                LogToConsole("§eLogin Successfully");
                SendText("/msg"); // active auto respond
            }
            else
            {
                LogToConsole("§ePassword is empty");
            }
        }
        else
        {
            LogToConsole("§eCannot find password file");
        }
    }
}
