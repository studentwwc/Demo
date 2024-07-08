using Server.Frame;
using MySql.Data.MySqlClient;
using Protocal;
using System;
using System.Globalization;
using System.Data.SqlClient;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using Server._01Service._03Timer;
using System.Collections.Generic;
using PENet;
using Server.Data;
using System.Reflection;

namespace Server._04DB
{
    public class DBManager : Singleton<DBManager>
    {
        private MySqlConnection mySqlConnection;
        public void Init()
        {
            mySqlConnection = new MySqlConnection("server=127.0.0.1;database=darkgod;username=root;password=146935;charset=utf8");
            NetCommon.Log("DBManager Init Done");
        }
        public PlayerData GetPlayerData(string account, string password)
        {
            mySqlConnection.Open();
            PlayerData playerData = null;
            MySqlDataReader dataReader = null;
            bool isExist = false;
            MySqlCommand command = new MySqlCommand("select * from user where account=@account", mySqlConnection);
            try
            {
                command.Parameters.AddWithValue("account", account);
                dataReader = command.ExecuteReader();
                //存在该用户
                if (dataReader.Read())
                {
                    isExist = true;
                    if (password.Equals(dataReader.GetString("password")))
                    {
                        playerData = new PlayerData()
                        {
                            id = dataReader.GetInt32("id"),
                            name = dataReader.GetString("nick"),
                            power = dataReader.GetInt32("power"),
                            coin = dataReader.GetInt32("coin"),
                            diamond = dataReader.GetInt32("diamond"),
                            crystal = dataReader.GetInt32("crystal"),
                            lv = dataReader.GetInt32("lv"),
                            exp = dataReader.GetInt32("exp"),
                            hp = dataReader.GetInt32("hp"),
                            ad = dataReader.GetInt32("ad"),
                            ap = dataReader.GetInt32("ap"),
                            addef = dataReader.GetInt32("addef"),
                            apdef = dataReader.GetInt32("apdef"),
                            dodge = dataReader.GetInt32("dodge"),
                            pierce = dataReader.GetInt32("pierce"),
                            critical = dataReader.GetInt32("critical"),
                            guideid = dataReader.GetInt32("guideid"),
                            repower = dataReader.GetInt32("repower"),
                            levelpass = dataReader.GetInt32("levelpass"),
                        };
                        //strongArr
                        string[] temps = dataReader.GetString("strongarr").Split('#');
                        playerData.strongArr = new int[temps.Length];
                        for (int i = 0; i < temps.Length; i++) {
                            playerData.strongArr[i] = int.Parse(temps[i]);
                        }
                        //taskRewardArr
                        playerData.taskReward = dataReader.GetString("taskreward").Split('#');
                    }
                }
            }
            catch (Exception e)
            {
                NetCommon.Log(e.ToString(), NetLogType.Error);
            }
            finally
            {
                dataReader.Close();
                DBUtility.CloseConnect(mySqlConnection);
                if (!isExist)
                {
                    playerData = new PlayerData()
                    {
                        id = -1,
                        name = "",
                        power = 150,
                        coin = 5000,
                        diamond = 2000,
                        crystal = 500,
                        lv = 1,
                        exp = 0,
                        hp = 2000,
                        ad = 275,
                        ap = 265,
                        addef = 67,
                        apdef = 43,
                        dodge = 7,
                        pierce = 5,
                        critical = 2,
                        guideid = 1001,
                        strongArr = new int[6],
                        repower = (long)TimerService.Instance.pt.GetMillisecondsTime(),
                        taskReward = new string[6],
                        levelpass=10001,
                       
                    };
                    //初始化taskReward
                    for (int i = 0; i < playerData.taskReward.Length; i++) {
                        playerData.taskReward[i] = i+1+"|0|0";
                    }
                    int id = (int)InsertPlayerData(account, password, playerData);
                    playerData.id = id;
                }
            }
            return playerData;
        }
        public long InsertPlayerData(string account, string password, PlayerData playerData)
        {
            mySqlConnection.Open();
            MySqlCommand cmd = new MySqlCommand("insert into user" +
                "(account,password,nick,power,lv,exp,coin,diamond,crystal,hp,ad,ap,addef,apdef,dodge,pierce,critical,guideid,strongarr,repower,taskreward,levelpass)values" +
                "(@account,@password,@nick,@power,@lv,@exp,@coin,@diamond,@crystal,@hp,@ad,@ap,@addef,@apdef,@dodge,@pierce,@critical,@guideid,@strongarr,@repower,@taskreward,@levelpass)", mySqlConnection);
            cmd.Parameters.AddWithValue("account", account);
            cmd.Parameters.AddWithValue("password", password);
            cmd.Parameters.AddWithValue("nick", playerData.name);
            cmd.Parameters.AddWithValue("power", playerData.power);
            cmd.Parameters.AddWithValue("lv", playerData.lv);
            cmd.Parameters.AddWithValue("exp", playerData.exp);
            cmd.Parameters.AddWithValue("coin", playerData.coin);
            cmd.Parameters.AddWithValue("diamond", playerData.diamond);
            cmd.Parameters.AddWithValue("crystal", playerData.crystal);
            cmd.Parameters.AddWithValue("hp", playerData.hp);
            cmd.Parameters.AddWithValue("ad", playerData.ad);
            cmd.Parameters.AddWithValue("ap", playerData.ap);
            cmd.Parameters.AddWithValue("addef", playerData.addef);
            cmd.Parameters.AddWithValue("apdef", playerData.apdef);
            cmd.Parameters.AddWithValue("dodge", playerData.dodge);
            cmd.Parameters.AddWithValue("pierce", playerData.pierce);
            cmd.Parameters.AddWithValue("critical", playerData.critical);
            cmd.Parameters.AddWithValue("guideid", playerData.guideid);
            cmd.Parameters.AddWithValue("repower", (int)playerData.repower);
            cmd.Parameters.AddWithValue("levelpass", (int)playerData.levelpass);
            //strongArr
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < playerData.strongArr.Length; i++) {
                if (i != playerData.strongArr.Length - 1)
                {
                    sb.Append(playerData.strongArr[i] + "#");
                }
                else {
                    sb.Append(playerData.strongArr[i]);
                }
            }
            cmd.Parameters.AddWithValue("strongarr", sb.ToString());
            //taskRewardArr
            sb.Clear();
            for (int i = 0; i < playerData.taskReward.Length; i++) {
                if (i != playerData.strongArr.Length - 1)
                {
                    sb.Append(playerData.taskReward[i] + "#");
                }
                else
                {
                    sb.Append(playerData.taskReward[i]);
                }
            }
            cmd.Parameters.AddWithValue("taskreward", sb.ToString());
            try
            {

                cmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                NetCommon.Log(e.ToString(), NetLogType.Error);
                NetCommon.Log("插入user错误", NetLogType.Error);
            }
            finally
            {
                DBUtility.CloseConnect(mySqlConnection);
            }

            return cmd.LastInsertedId;
        }

        public bool IsExistName(string name)
        {
            bool isExist = false;
            mySqlConnection.Open();
            MySqlDataReader mySqlDataReader = null;
            MySqlCommand command = new MySqlCommand("select * from user where nick=@nick", mySqlConnection);
            try
            {
                command.Parameters.AddWithValue("nick", name);
                mySqlDataReader = command.ExecuteReader();
                if (mySqlDataReader.Read())
                {
                    isExist = true;
                }
            }
            catch (Exception e)
            {
                NetCommon.Log(e.ToString(), NetLogType.Error);
            }
            finally
            {
                mySqlDataReader.Close();
                DBUtility.CloseConnect(mySqlConnection);
            }
            return isExist;
        }

        public bool UpdatePlayerData(PlayerData playerData)
        {
            mySqlConnection.Open();
            try
            {
                MySqlCommand cmd = new MySqlCommand("update user set nick=@nick,power=@power,lv=@lv,exp=@exp,coin=@coin,diamond=@diamond,crystal=@crystal,hp=@hp,ad=@ad,ap=@ap,addef=@addef,apdef=@apdef,dodge=@dodge,pierce=@pierce,critical=@critical,guideid=@guideid,strongarr=@strongarr,repower=@repower,taskreward=@taskreward,levelpass=@levelpass where id=@id", mySqlConnection);
                cmd.Parameters.AddWithValue("id", playerData.id);
                cmd.Parameters.AddWithValue("nick", playerData.name);
                cmd.Parameters.AddWithValue("power", playerData.power);
                cmd.Parameters.AddWithValue("lv", playerData.lv);
                cmd.Parameters.AddWithValue("exp", playerData.exp);
                cmd.Parameters.AddWithValue("coin", playerData.coin);
                cmd.Parameters.AddWithValue("diamond", playerData.diamond);
                cmd.Parameters.AddWithValue("crystal", playerData.crystal);
                cmd.Parameters.AddWithValue("hp", playerData.hp);
                cmd.Parameters.AddWithValue("ad", playerData.ad);
                cmd.Parameters.AddWithValue("ap", playerData.ap);
                cmd.Parameters.AddWithValue("addef", playerData.addef);
                cmd.Parameters.AddWithValue("apdef", playerData.apdef);
                cmd.Parameters.AddWithValue("dodge", playerData.dodge);
                cmd.Parameters.AddWithValue("pierce", playerData.pierce);
                cmd.Parameters.AddWithValue("critical", playerData.critical);
                cmd.Parameters.AddWithValue("guideid", playerData.guideid);
                cmd.Parameters.AddWithValue("repower", (int)playerData.repower);
                cmd.Parameters.AddWithValue("levelpass", (int)playerData.levelpass);
                StringBuilder sb = new StringBuilder();
                //strongArr
                for (int i = 0; i < playerData.strongArr.Length; i++)
                {
                    if (i != playerData.strongArr.Length - 1)
                    {
                        sb.Append(playerData.strongArr[i] + "#");
                    }
                    else
                    {
                        sb.Append(playerData.strongArr[i]);
                    }
                }
                cmd.Parameters.AddWithValue("strongarr", sb.ToString());
                sb.Clear();
                // taskRewardArr
                for (int i = 0; i < playerData.taskReward.Length; i++)
                {
                    if (i != playerData.taskReward.Length - 1)
                    {
                        sb.Append(playerData.taskReward[i]+ "#");
                    }
                    else
                    {
                        sb.Append(playerData.taskReward[i]);
                    }
                }
                cmd.Parameters.AddWithValue("taskreward", sb.ToString());
                cmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                NetCommon.Log(e.ToString(), NetLogType.Error);
                return false;
            }
            finally
            {
                DBUtility.CloseConnect(mySqlConnection);
            }
            return true;
        }

        public List<PlayerPack> GetPlayerPack(int userid) {
            mySqlConnection.Open();
            List<PlayerPack> packs=new List<PlayerPack>();
            MySqlDataReader dataReader = null;
            try
            {
                MySqlCommand command = new MySqlCommand("Select * from pack where userid=@userid", mySqlConnection);
                command.Parameters.AddWithValue("userid", userid);
                dataReader = command.ExecuteReader();
                while (dataReader.Read())
                {
                    packs.Add(new PlayerPack()
                    {
                        id = dataReader.GetInt32("id"),
                        userid = dataReader.GetInt32("userid"),
                        goodsid = dataReader.GetInt32("goodsid"),
                        count = dataReader.GetInt32("count"),
                        packindex = dataReader.GetInt32("packindex"),
                    });
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally {
                dataReader.Close();
                DBUtility.CloseConnect(mySqlConnection);
            }
            return packs;
        }

        public bool ChangePackIndex(int id,int index) {
            mySqlConnection.Open();
            bool res=false;
            try
            {
                res = DBUtility.NoQuery("update pack set packindex=@index where id=@id",
                 new SqlParam[] { new SqlParam("index", index),
                    new SqlParam("id", id) }
                 , mySqlConnection);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            finally {
                DBUtility.CloseConnect(mySqlConnection);
            }
            
            return res;
        }
        public int AddPack(int userId, int goodsId,int count,int packIndex)
        {
            mySqlConnection.Open();
            int id = -1;
            MySqlDataReader dataReader = null;
  
            try
            {
                 DBUtility.NoQuery("insert into pack (userid,goodsid,count,packindex)values(@userid,@goodsid,@count,@packindex)",
                 new SqlParam[] {
                    new SqlParam("userid", userId),
                    new SqlParam("goodsid", goodsId),
                    new SqlParam("count", count),
                    new SqlParam("packindex", packIndex),
                 }
                 , mySqlConnection);
                MySqlCommand command = new MySqlCommand("Select id from pack where userid=@userid and goodsid=@goodsid and packindex=@packindex", mySqlConnection);
                command.Parameters.AddWithValue("userid", userId);
                command.Parameters.AddWithValue("goodsid", goodsId);
                command.Parameters.AddWithValue("packindex", packIndex);
                dataReader = command.ExecuteReader();
                while (dataReader.Read())
                {
                    id = dataReader.GetInt32("id");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                id = -1;
            }
            finally
            {
                DBUtility.CloseConnect(mySqlConnection);
            }

            return id;
        }

        public bool UpdatePackCount(int userId,int goodsId,int count) {
            mySqlConnection.Open();
            bool res = false;
            try
            {
                res = DBUtility.NoQuery("update pack set count=@count where userid=@userid and goodsid=@goodsid",
                 new SqlParam[] { new SqlParam("count", count),
                    new SqlParam("userid", userId),
                    new SqlParam("goodsid", goodsId),
                 }
                 , mySqlConnection);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            finally
            {
                DBUtility.CloseConnect(mySqlConnection);
            }

            return res;

        }
        public bool AddUserRole(int userid,int roleId) {
            mySqlConnection.Open();
            bool res = false;
            try
            {
                res = DBUtility.NoQuery("insert into role (userid,roleid)values(@userid,@roleid)",
                 new SqlParam[] { new SqlParam("userid", userid), new SqlParam("roleid", roleId) }, mySqlConnection);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            finally {
                DBUtility.CloseConnect(mySqlConnection);
            }
           
            return res;
        }
        public bool SetUserProp(int id,int hp,int ad,int ap,int addef,int dodge,int critical) {
            mySqlConnection.Open();
            bool res = false;
            try
            {
                res = DBUtility.NoQuery("update user set hp=@hp,ad=@ad,ap=@ap,addef=@addef,dodge=@dodge,critical=@critical where id=@id",
                   new SqlParam[] {
                    new SqlParam("hp", hp),
                    new SqlParam("ad", ad),
                    new SqlParam("ap",ap),
                    new SqlParam("addef",addef),
                    new SqlParam("dodge",dodge),
                    new SqlParam("critical",critical),
                    new SqlParam("id",id),
                   },
                   mySqlConnection);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            finally {
                DBUtility.CloseConnect(mySqlConnection);
            }
            
            return res;
        }

        public int QueryRoleId(int userId) {
            mySqlConnection.Open();
            int roleid = -1;
            IntPack intPack = new IntPack();
            try
            {
                roleid=DBUtility.Query("select roleid from role where userid=@userid",
                new SqlParam[] { new SqlParam("userid", userId) },
                mySqlConnection);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            finally {
                DBUtility.CloseConnect(mySqlConnection);
            }
            return roleid;
        }

        public bool UpdateUserProp(int id,int ad,int ap,int addef,int hp,int dodge,int critical) {
            mySqlConnection.Open();
            bool res=false;
            try {
              res= DBUtility.NoQuery("Update user set ad=@ad,ap=@ap,addef=@addef,hp=@hp,dodge=@dodge,critical=@critical where id=@id",
                    new SqlParam[] {
                        new SqlParam("ad",ad),
                        new SqlParam("ap",ap),
                        new SqlParam("hp",hp),
                        new SqlParam("addef",addef),
                        new SqlParam("dodge",dodge),
                        new SqlParam("critical",critical),
                        new SqlParam("id",id) },
                    mySqlConnection);
            } catch (Exception e) {
                Console.WriteLine(e);
            }
            finally {
                mySqlConnection.Dispose();
                mySqlConnection.Close();
            }
            return res;
        }

        

    }
}
