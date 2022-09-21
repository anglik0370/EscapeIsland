using UnityEngine;


[ExecuteInEditMode]
public class SpriteOutline : MonoBehaviour
{
    public Color color = Color.white;

    [Range(0, 16)]
    public int outlineSize = 1;

    private SpriteRenderer spriteRenderer;

    [SerializeField]
    private ColorPicker redTeamPicker;
    [SerializeField]
    private ColorPicker blueTeamPicker;

    private float defaultTime = 30f;
    private float curTime = 0f;

    private bool isOccupy = false;

    void OnEnable()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        //UpdateOutline(false);
    }

    void OnDisable()
    {
        //UpdateOutline(false);
    }

    void Update()
    {
        //UpdateOutline(true);
        if (!isOccupy) return;

        curTime -= Time.deltaTime;

        if(curTime <= 0f)
        {
            isOccupy = false;
            SetOccupy(Team.NONE);
        }
    }

    void UpdateOutline(bool outline)
    {
        MaterialPropertyBlock mpb = new MaterialPropertyBlock();
        spriteRenderer.GetPropertyBlock(mpb);
        mpb.SetFloat("_Outline", outline ? 1f : 0);
        mpb.SetColor("_OutlineColor", color);
        mpb.SetFloat("_OutlineSize", outlineSize);
        spriteRenderer.SetPropertyBlock(mpb);
    }

    public void SetOccupy(Team team)
    {
        if(Team.NONE.Equals(team))
        {
            color = UtilClass.limpidityColor;
            spriteRenderer.color = UtilClass.opacityColor;
            //UpdateOutline(false);
        }
        else
        {
            color = UtilClass.GetTeamColor(team);
            curTime = defaultTime;
            spriteRenderer.color = team.Equals(Team.RED) ? redTeamPicker.pickColor : blueTeamPicker.pickColor;

            //UpdateOutline(true);
            isOccupy = true;
        }
    }
}