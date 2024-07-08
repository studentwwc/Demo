using Protocal;
using Server._04DB;
using Server.Frame;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server._02System
{
    public class InventorySystem:Singleton<InventorySystem>
    {
        public void HandleGetAllPackReq(MsgPack msgPack) {
            AllPackReq allPackReq=msgPack.netMsg.allPackReq;
            List<PlayerPack>packs= DBManager.Instance.GetPlayerPack(allPackReq.userid);
            msgPack.session.SendMsg(new NetMsg() { 
              cmd=(int)TransCode.AllPackRes,
              allPackRes=new AllPackRes(packs)
            });
        }
        public void HandleUpdatePackStateReq(MsgPack msgPack) {
            UpdatePackStateReq updatePackStateReq= msgPack.netMsg.updatePackStateReq;
            switch (updatePackStateReq.opt) {
                case 1://改变下标
                    DBManager.Instance.ChangePackIndex(updatePackStateReq.packId, updatePackStateReq.changeIndex);
                    break;
                case 2://售卖
                    break;
                case 3://使用消耗品
                    break;
            }
        }
        public void HandlePropUpdateReq(MsgPack msgPack) {
            UpdateUserPropReq updateUserPropReq = msgPack.netMsg.updateUserPropReq;
            DBManager.Instance.UpdateUserProp(updateUserPropReq.userid, updateUserPropReq.ad, updateUserPropReq.ap,
                updateUserPropReq.addef, updateUserPropReq.hp, updateUserPropReq.dodge, updateUserPropReq.critical);
        }
    }
}
