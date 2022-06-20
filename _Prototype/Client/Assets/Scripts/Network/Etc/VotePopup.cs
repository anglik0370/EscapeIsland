using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class VotePopup : Popup
{
    public Button skipBtn;
    public Transform skipUserParent;

    [SerializeField]
    private Text timeInfoText;

    public Transform voteParent;
    public VoteTimeBar voteTimeBar;

    //public Text middleText;

    public List<VoteUI> voteUIList = new List<VoteUI>();

    private void Start()
    {
        voteUIList = voteParent.GetComponentsInChildren<VoteUI>().ToList();

        voteUIList.ForEach(x => x.OnOff(false));

        EventManager.SubGameOver(p =>
        {
            InitSkipUser();
        });
    }

    //public void ChangeMiddleText(string msg)
    //{
    //    if (VoteManager.Instance.isTextChange) return;
    //    middleText.text = msg;
    //}

    public void CanvasOpenAndClose(CanvasGroup cg, bool on)
    {
        cg.alpha = on ? 1f : 0f;
        cg.interactable = on;
        cg.blocksRaycasts = on;
    }

    public void SetTimeInfoText(string msg)
    {
        timeInfoText.text = msg;
    }

    public void SetVoteUI(int socId,string name, Sprite charSprite,bool isKidnapper = false)
    {
        VoteUI ui = voteUIList.Find(x => !x.gameObject.activeSelf);

        if(ui == null)
        {
            Debug.LogError("¾øÀ½");
            return;
        }

        ui.SetVoteUI(socId,name, charSprite,isKidnapper);
    }

    public VoteUI FindVoteUI(int socId)
    {
        return voteUIList.Find(x => x.socId == socId && x.gameObject.activeSelf);
    }

    public void CompleteVote()
    {
        //voteUIList.ForEach(x =>
        //{
        //    x.ToggleOnOff(false);
        //});
        //skipToggle.gameObject.SetActive(false);
    }

    public void VoteUIDisable()
    {
        voteUIList.ForEach(x => x.OnOff(false));
    }

    public void VoteBtnDisable()
    {
        VoteEnable(false);
    }

    public void VoteEnable(bool enabled)
    {
        skipBtn.enabled = enabled;
        voteUIList.ForEach(x =>
        {
            if(NetworkManager.instance.GetPlayerDic().TryGetValue(x.socId,out Player p))
            {
                if(!p.isDie || !enabled)
                {
                    x.BtnEnabled(enabled);
                }
            }
            else if(x.socId == NetworkManager.instance.socketId)
            {
                x.BtnEnabled(enabled);
            }
        });
    }

    public void AddSkipUser()
    {
        Transform userImg = PoolManager.GetItem<UserImg>().transform;

        userImg.SetParent(skipUserParent);

        userImg.localScale = Vector3.one;
    }

    public void InitSkipUser()
    {
        for (int i = 0; i < skipUserParent.childCount; i++)
        {
            GameObject userImg = skipUserParent.GetChild(i).gameObject;

            if (userImg.activeSelf)
            {
                userImg.SetActive(false);
            }
        }
    }
}
