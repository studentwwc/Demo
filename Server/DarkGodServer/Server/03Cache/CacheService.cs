using Protocal;
using Server._04DB;
using Server.Frame;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server._03Cache
{
    public class CacheService:Singleton<CacheService>
    {
        private Dictionary<string, ServerSession> accOnLineDic;
        private Dictionary<ServerSession, PlayerData> onLineSessionDic;
        public void Init() {
            accOnLineDic = new Dictionary<string, ServerSession>();
            onLineSessionDic = new Dictionary<ServerSession, PlayerData>();
            NetCommon.Log("Cache Service Init Done");
        }
        public bool isAccOnLine(string account) {
            return accOnLineDic.ContainsKey(account);
        }
        public PlayerData GetPlayerData(string account,string password) {
            //从数据库判断账户密码判断是否正确
            return DBManager.Instance.GetPlayerData(account,password);
        }
        public PlayerData GetPlayerDataBySession(ServerSession session)
        {
            return onLineSessionDic[session];
        }
        public void AccOnLine(string account, ServerSession session, PlayerData playerData) {
            accOnLineDic.Add(account,session);
            onLineSessionDic.Add(session, playerData);
        }
        public void AccOutLine(ServerSession session) {
            foreach (var v in accOnLineDic) {
                if (v.Value.Equals(session)) {
                    accOnLineDic.Remove(v.Key);
                    break;
                }
            }
            onLineSessionDic.Remove(session);
            NetCommon.Log(session+":out line done");
        }
        public List<ServerSession> GetAllOnLineSession() {
            List<ServerSession> sessions = new List<ServerSession>();
            foreach (var v  in accOnLineDic ) {
                sessions.Add(v.Value);
            }
            return sessions;
        }
        public Dictionary<ServerSession, PlayerData> GetOnLineSessoonDic() {
            return onLineSessionDic;
        }
        public bool UpdateName(ServerSession session,string name) {
            onLineSessionDic[session].name = name;
            return UpdatePlayerData(session);
        }
        public bool UpdatePlayerData(ServerSession session) {
            return DBManager.Instance.UpdatePlayerData(onLineSessionDic[session]);
        }
        public bool IsExistName(string name)
        {
            return DBManager.Instance.IsExistName(name);
        }
     
    }
}
