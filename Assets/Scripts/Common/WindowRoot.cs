using System;
using Service;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Object = System.Object;

namespace Common
{
    public class WindowRoot:MonoBehaviour
    {
        protected ResService _resService;
        protected AudioService _audioService;
        protected NetService _netService;
        public void SetWndState(bool isActive=true)
        {
            if (gameObject.activeSelf != isActive)
            {
               SetActive(gameObject,isActive);
            }

            if (isActive)
            {
                InitWnd();
            }
            else
            {
                ClearWnd();
            }
        }

        protected virtual void InitWnd()
        {
            _resService=ResService.Instance;
            _audioService=AudioService.Instance;
            _netService=NetService.Instance;
        }

        protected virtual void ClearWnd()
        {
            _resService = null;
            _audioService = null;
        }

        public bool GetState()
        {
            return this.gameObject.activeSelf;
        }

        #region ToolFunction

        protected void SetActive(GameObject go,bool isActive=true)
        {
            go.SetActive(isActive);
        }
        protected void SetActive(Transform trans,bool state=true)
        {
            trans.gameObject.SetActive(state);
        }
        protected void SetActive(RectTransform rectTrans,bool state=true)
        {
            rectTrans.gameObject.SetActive(state);
        }
        protected void SetActive(TMP_Text text,bool state=true)
        {
            text.transform.gameObject.SetActive(state);
        }
        protected void SetActive(Image img,bool state=true)
        {
            img.transform.gameObject.SetActive(state);
        }

        protected void SetText(TMP_Text txt,string context="")
        {
            txt.text = context;
        }
        protected void SetText(TMP_Text txt,int num=0)
        {
            txt.text = num.ToString();
        }
        protected void SetText(Text txt,string context="")
        {
            txt.text = context;
        }
        protected void SetText(Text txt,int num=0)
        {
            txt.text = num.ToString();
        }
        protected void SetText(Transform txt,string context="")
        {
            txt.GetComponent<TMP_Text>().text = context;
        }
        protected void SetText(Transform txt,int num=0)
        {
            txt.GetComponent<TMP_Text>().text = num.ToString();
        }

        protected void SetSprite(Image image, string path)
        {
            Sprite sprite = _resService.GetSprite(path);
            if (sprite != null)
            {
                image.sprite = sprite;
            }
        }

        #endregion

        #region EventsSyetem

        protected T GetOrAddComponent<T>(GameObject go)where T:Component
        {
            T t = go.GetComponent<T>();
            if (t == null)
            {
                t = go.AddComponent<T>();
            }
            return t;
        }

        protected void OnClickDownPEListener(GameObject go,Action<PointerEventData>ped)
        {
            PEListener peListener = GetOrAddComponent<PEListener>(go);
            peListener.onClickDown += ped;
        }
        protected void OnClickUpPEListener(GameObject go,Action<PointerEventData>ped)
        {
            PEListener peListener = GetOrAddComponent<PEListener>(go);
            peListener.onClickUp += ped;
        }
        protected void OnDragPEListener(GameObject go,Action<PointerEventData>ped)
        {
            PEListener peListener = GetOrAddComponent<PEListener>(go);
            peListener.onDrag += ped;
        }
        protected void OnClickPEListener(GameObject go,Action<Object>ped,Object i)
        {
            PEListener peListener = GetOrAddComponent<PEListener>(go);
            peListener.args = i;
            peListener.onClick += ped;
        }

        #endregion
    }
}