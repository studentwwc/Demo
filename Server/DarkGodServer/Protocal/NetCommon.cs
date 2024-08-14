using PENet;
using Protocal;

public class NetCommon
{
    public static int repowerTime = 5;
    public static int resumePower = 2;
    public static void Log(string msg, NetLogType type=NetLogType.Normal)
    {
        PETool.LogMsg(msg, (LogLevel)type);
    }
    public static int GetFight(PlayerData pd) {
        int fight=pd.lv*100+pd.ad+pd.ap+pd.addef+pd.apdef+pd.pierce+pd.dodge+pd.critical;
        return fight;
    }
    public static int GetPowerLimit(int lv) {
        return (lv - 1) / 10 * 150 + 150;
    }
    public static int GetExpLimit(int lv) {
        return lv*100;
    }
}
public enum NetLogType
{
    Normal = 0,
    Warn = 1,
    Error = 2,
    Info = 4
}
