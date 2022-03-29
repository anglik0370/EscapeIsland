using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameOverCase
{
    KillAllCitizen,
    FindAllKidnapper,
    CollectAllItem,
}

public class GameOverPanel : Panel
{
    public CanvasGroup citizenCg;
    public CanvasGroup kidnapperCg;

    //0 - kidnapper, 1 - citizen
    [SerializeField]
    private Transform[] kidnapperImgParent;
    [SerializeField]
    private Transform[] citizenImgParent;

    private CanvasGroup curCg = null;

    private WaitForSeconds closeSec;

    protected override void Awake()
    {
        base.Awake();

        closeSec = new WaitForSeconds(1.5f);
    }

    private void Start()
    {
        EventManager.SubGameOver(gameOverCase =>
        {
            Open(gameOverCase);
        });
    }

    public void Open(GameOverCase gameOverCase)
    {
        switch (gameOverCase)
        {
            case GameOverCase.KillAllCitizen:
                {
                    CanvasGroupOpenAndClose(kidnapperCg, true);
                    print("��� �ù� ���");
                }
                break;
            case GameOverCase.FindAllKidnapper:
                {
                    CanvasGroupOpenAndClose(citizenCg, true);
                    print("��� ��ġ�� �˰�");
                }
                break;
            case GameOverCase.CollectAllItem:
                {
                    print("��� ��� ����");
                    CanvasGroupOpenAndClose(citizenCg, true);
                }
                break;
            default:
                {
                    print("��ȿ���� ���� Case�Դϴ�");
                }
                break;
        }

        base.Open();

        NetworkManager.instance.GameEnd();
        //StartCoroutine(ClosePanel());
    }

    //�ϴ� �̷��� ���� ���߿� Ʈ������ ������ �ϸ� �� ��
    public void CanvasGroupOpenAndClose(CanvasGroup cg,bool isOpen)
    {
        if (cg == null) return;
        curCg = cg;

        curCg.alpha = isOpen ? 1f : 0f;
        curCg.interactable = isOpen;
        curCg.blocksRaycasts = isOpen;
    }

    public void CloseGameOverPanel()
    {
        CanvasGroupOpenAndClose(curCg, false);
        base.Close();
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
