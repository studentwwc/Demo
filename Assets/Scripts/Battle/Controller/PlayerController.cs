/****************************************************
    文件：PlayerController.cs
	作者：WWC
    邮箱: 1469359779@qq.com
    日期：2023/5/4 22:27:50
	功能：角色控制器
*****************************************************/


using System;
using Battle;
using Common;
using DarkGod.System;
using UnityEngine;

public class PlayerController : Controller
{
    private float currentBlend;
    private int targetBlend;
    private float currentMoveSpeed;
    private Vector3 cameraFollowOffset;
    [SerializeField] private GameObject skillFx1;
    [SerializeField] private GameObject skillFx2;
    [SerializeField] private GameObject skillFx3;
    [SerializeField] private GameObject skillFx4;
    [SerializeField] private GameObject skillFx5;
    [SerializeField] private GameObject skill1;
    [SerializeField] private GameObject skill2;
    [SerializeField] private GameObject skill3;
    

    public override void Init()
    {
        base.Init();
        cameraTran = Camera.main.transform;
        cameraFollowOffset = Camera.main.transform.position - transform.position;
        if(skillFx1!=null){
            fxDic.Add(skillFx1.name,skillFx1);
        }
        if(skillFx2!=null){
            fxDic.Add(skillFx2.name,skillFx2);
        }
        if(skillFx3!=null){
            fxDic.Add(skillFx3.name,skillFx3);
        }
        if(skillFx4!=null){
            fxDic.Add(skillFx4.name,skillFx4);
        }
        if(skillFx5!=null){
            fxDic.Add(skillFx5.name,skillFx5);
        }
        if(skill1!=null){
            fxDic.Add(skill1.name,skill1);
        }
        if(skill2!=null){
            fxDic.Add(skill2.name,skill2);
        } if(skill3!=null){
            fxDic.Add(skill3.name,skill3);
        }
    }

    private void Update()
    {
        
            // float h = Input.GetAxis("Horizontal");
            // float v = Input.GetAxis("Vertical");
            // Dir = new Vector2(h, v);
            // if (Dir != Vector2.zero)
            // {
            //     SetDir();
            // }
            // else
            // {
            //     targetBlend = Constant.playerIdleBlend;
            // }
         

        if (isMove)
        {
            SetDir();
            SetMove();
            SetCameraFollow();
        }

        if (isSkillMove)
        {
            SetSkillMove();
            SetCameraFollow();
        }

        if (targetBlend != currentBlend)
        {
            MixBlend();
        }
    }

    public void SetDir()
    {
        if (dir != Vector2.zero)
        {
            float angle = Vector2.SignedAngle(dir, new Vector2(0, 1));
            transform.localEulerAngles = new Vector3(0, angle, 0)+new Vector3(0,cameraTran.localEulerAngles.y,0);
        }
    }

    public void SetMove()
    {
        _cc.Move(transform.forward * Time.deltaTime * Constant.playerMoveSpeed);
        _cc.Move( -transform.up* Time.deltaTime * Constant.playerMoveSpeed);
    }
    public void SetSkillMove()
    {
        _cc.Move(transform.forward * Time.deltaTime * skillMoveSpeed);
    }

    public void SetCameraFollow()
    {
        if (cameraTran != null)
        {
            cameraTran.transform.position = transform.position + cameraFollowOffset;
        }
    }

    
    public override  void SetBlend(int tar)
    {
        targetBlend = tar;
    }

    public void MixBlend()
    {
        if (Mathf.Abs(currentBlend - targetBlend) < Constant.playerMoveAccelerateSpeed * Time.deltaTime)
        {
            currentBlend = targetBlend;
        }
        else if (currentBlend > targetBlend)
        {
            currentBlend -= Time.deltaTime * Constant.playerMoveAccelerateSpeed;
        }
        else
        {
            currentBlend += Time.deltaTime * Constant.playerMoveAccelerateSpeed;
        }

        ani.SetFloat("Blend", currentBlend);
    }
}