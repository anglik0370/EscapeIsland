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
                    print("모든 시민 사망");
                }
                break;
            case GameOverCase.FindAllKidnapper:
                {
                    CanvasGroupOpenAndClose(citizenCg, true);
                    print("모든 납치자 검거");
                }
                break;
            case GameOverCase.CollectAllItem:
                {
                    print("모든 재료 수집");
                    CanvasGroupOpenAndClose(citizenCg, true);
                }
                break;
            default:
                {
                    print("유효하지 않은 Case입니다");
                }
                break;
        }

        base.Open();

        NetworkManager.instance.GameEnd();
        //StartCoroutine(ClosePanel());
    }

    //일단 이렇게 쓰고 나중에 트위닝을 쓰던가 하면 될 듯
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
