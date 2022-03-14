using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverPanel : Panel
{
    public CanvasGroup citizenCg;
    public CanvasGroup kidnapperCg;

    private CanvasGroup curCg = null;

    private WaitForSeconds closeSec;

    protected override void Awake()
    {
        base.Awake();

        closeSec = new WaitForSeconds(1.5f);
    }

    private void Start()
    {
        EventManager.SubGameOver(isKidnapperWin =>
        {
            Open(isKidnapperWin);
        });
    }


    //�ϴ� �ӽ÷� �̷��� �ص� - ���߿� �� ���ҽ� ������ enum���� �ϴ� ���� �ϴ� �ٲٸ� �ɵ�
    public override void Open(bool isKidnapperWin)
    {
        CanvasGroupOpenAndClose(isKidnapperWin ? kidnapperCg : citizenCg, true);

        base.Open(false);

        StartCoroutine(ClosePanel());
    }

    //�ϴ� �̷��� ���� ���߿� Ʈ������ ������ �ϸ� �� ��
    public void CanvasGroupOpenAndClose(CanvasGroup cg,bool isOpen)
    {
        if (cg == null) return;
        curCg = cg;

        curCg.alpha = isOpen ? 1f : 0f;
        curCg.interactable = isOpen;
        //curCg.blocksRaycasts = isOpen;
    }

    IEnumerator ClosePanel()
    {
        //yield return new WaitUntil(() => cvs.interactable);

        yield return closeSec;

        CanvasGroupOpenAndClose(curCg, false);
        base.Close();
        NetworkManager.instance.GameEnd();
    }
}
