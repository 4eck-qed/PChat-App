using System;
using System.Collections.Generic;
using System.Linq;
using PChat.Log;
using static PChat.GUI.Config;

namespace PChat.GUI;

public class LoginHistoryService
{
    public IEnumerable<DateTime> GetLoginHistory()
    {
        return PLogger.Singleton.ReadLines(F_LOGIN_HISTORY).Select(DateTime.Parse);
    }

    public void AddToLoginHistory(DateTime login)
    {
        PLogger.Singleton.AppendLine(F_LOGIN_HISTORY, DateTime.Now.ToUniversalTime().ToString());
    }
}