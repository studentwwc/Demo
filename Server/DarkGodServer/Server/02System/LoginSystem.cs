using PENet;
using Protocal;
using Server._03Cache;
using Server._04DB;
using Server.Frame;
namespace Server._02System
{
    internal class LoginSystem : Singleton<LoginSystem>
    {
        public void Init()
        {
            NetCommon.Log("LoginSystem Init Done", NetLogType.Normal);
        }
        public void HandleLoginRequest(MsgPack msgPack)
        {
            CacheService cacheService = CacheService.Instance;
            LoginReq loginReq = msgPack.netMsg.loginReq;
            NetMsg netMsg = new NetMsg();
            //判断是否已登录
            if (cacheService.isAccOnLine(loginReq.account))
            {
                //netMsg.loginRes.playerData
                netMsg.err = (int)NetErrorCode.AccOnLine;
            }
            else
            {
                PlayerData playerData = cacheService.GetPlayerData(loginReq.account, loginReq.password);
                //判断是否存在
                if (playerData == null)
                {
                    netMsg.err = (int)NetErrorCode.ErrorPassword;
                }
                else
                {
                    int roleid= DBManager.Instance.QueryRoleId(playerData.id);
                    //返回角色信息
                    netMsg.loginRes = new LoginRes()
                    {
                        playerData = playerData,
                        roleid=roleid,

                    };
                    netMsg.cmd = (int)TransCode.LoginRes;
                    //把信息加入到缓冲区
                    cacheService.AccOnLine(loginReq.account, msgPack.session, playerData);

                }

            }
            //响应客户端结果
            msgPack.session.SendMsg(netMsg);
            PowerSystem.Instance.HandleOutLineRepower(msgPack);
        }

        public void HandleRenameRequest(MsgPack msgPack)
        {
            NetMsg netMsg = new NetMsg();
            bool isExist = CacheService.Instance.IsExistName(msgPack.netMsg.renameReq.name);
            int id = CacheService.Instance.GetPlayerDataBySession(msgPack.session).id;
            if (isExist)
            {
                //存在就返回已存在错误码
                netMsg.err = (int)NetErrorCode.NameExisted;
            }
            else
            {
                //不存在，更改Cache缓存
                bool isSucc = CacheService.Instance.UpdateName(msgPack.session, msgPack.netMsg.renameReq.name);
                if (isSucc) {
                    CacheService.Instance.GetPlayerDataBySession(msgPack.session).name = msgPack.netMsg.renameReq.name;
                    DBManager.Instance.AddUserRole(id, msgPack.netMsg.renameReq.roleid);
                    string prop = msgPack.netMsg.renameReq.prop;
                    string[]props= prop.Split('|');
                    DBManager.Instance.SetUserProp(id, int.Parse(props[2]),
                        int.Parse(props[0]), 
                        int.Parse(props[1]),
                        int.Parse(props[4]),
                        int.Parse(props[3]),
                        int.Parse(props[5])
                        );
                }
                if (!isSucc)
                {
                    netMsg.err = (int)NetErrorCode.UpdateFail;
                }
                else
                {
                    netMsg.cmd = (int)TransCode.RenameRes;
                }
            }
            msgPack.session.SendMsg(netMsg);
        }

        public void AccOutLine(ServerSession session) {
            CacheService.Instance.AccOutLine(session);
        }
    }
}
