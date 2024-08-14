using System.Collections;
using System.Collections.Generic;
using Common;
using DarkGod;
using Protocal;
using TMPro;
using UnityEngine;
using Utility;

public class BagWnd : WindowRoot
{
    public BoxItem WeaponBox;
    public BoxItem JacketBox;
    public BoxItem TrouserBox;
    public BoxItem NecklaceBox;
    public BoxItem BeltBox;
    public BoxItem ShoesBox;

    public TMP_Text tmp_Ad;
    public TMP_Text tmp_Ap;
    public TMP_Text tmp_Hp;
    public TMP_Text tmp_Dodge;
    public TMP_Text tmp_Defense;
    public TMP_Text tmp_Critical;
    

    public Transform packsTransform;
    private bool isFirst=true;
    protected override void InitWnd()
    {
        base.InitWnd();
        RefreshUI();
    }

    public void RefreshUI()
    {
        tmp_Ad.text = "物理攻击:"+GameRoot.Instance.PlayerData.ad;
        tmp_Ap.text = "魔法攻击:" + GameRoot.Instance.PlayerData.ap;
        tmp_Hp.text = "生命力:"+GameRoot.Instance.PlayerData.hp;
        tmp_Dodge.text = "闪避率:"+GameRoot.Instance.PlayerData.dodge+"%";
        tmp_Defense.text = "防御力:"+GameRoot.Instance.PlayerData.addef;
        tmp_Critical.text ="暴击率:"+ GameRoot.Instance.PlayerData.critical+"%";
    }
}
