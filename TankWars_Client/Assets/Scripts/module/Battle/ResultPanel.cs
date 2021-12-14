
using UnityEngine.UI;

public class ResultPanel : BasePanel
{
    private Image winImage;
    private Image lostImage;
    private Button okButton;

    public override void OnInit()
    {
        skinPath = "ResultPanel";
        layer = PanelManager.Layer.Tip;
    }

    public override void OnShow(params object[] args)
    {
        winImage = skin.transform.Find("WinImage").GetComponent<Image>();
        lostImage = skin.transform.Find("LostImage").GetComponent<Image>();
        okButton = skin.transform.Find("OkBtn").GetComponent<Button>();

        okButton.onClick.AddListener(OnOkClick);

        if (args.Length == 1)
        {
            bool isWin = (bool)args[0];

            if (isWin)
            {
                winImage.gameObject.SetActive(true);
                lostImage.gameObject.SetActive(false);

            }
            else
            {
                winImage.gameObject.SetActive(false);
                lostImage.gameObject.SetActive(true);
            }
        }
    }

    private void OnOkClick()
    {
        PanelManager.Open<RoomPanel>();
        this.Close();
    }

    public override void OnClose()
    {
    }
}