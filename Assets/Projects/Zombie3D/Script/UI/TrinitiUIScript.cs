using UnityEngine;
using System.Collections;
using System.IO;
using Zombie3D;
using UnityEngine.SceneManagement;

class TrinitiUIPosition
{
    public Rect Background = new Rect(0, 0, 960, 640);

}
	
public class TrinitiUIScript : MonoBehaviour
{
    protected Rect[] buttonRect;

    public UIManager m_UIManager = null;
    public string m_ui_material_path;  //最后带反斜杠
    protected Material trinitiMaterial;


    protected UIImage background;

    private TrinitiUIPosition uiPos;
    //private StartMenuTexturePosition texPos;

    protected float screenRatioX;
    protected float screenRatioY;
    protected float startTime;


    protected Timer fadeTimer = new Timer();
    // Use this for initialization
    void Start()
    {


        FlurryTAd.ShowTAd(true);

        startTime = Time.time;

        uiPos = new TrinitiUIPosition();
        //texPos = new StartMenuTexturePosition();


        m_UIManager = gameObject.AddComponent<UIManager>() as UIManager;
        m_UIManager.SetParameter(8, 1, false);
        m_UIManager.CLEAR = true;

        trinitiMaterial = UIResourceMgr.GetInstance().GetMaterial("yuyue");
        background = new UIImage();
		background.SetTexture(trinitiMaterial, StartMenuTexturePosition.Background, new Vector2(Screen.width, Screen.height));
        background.Rect = AutoRect.AutoPos(uiPos.Background);
	
		//! background.Rect = new Rect(0, 0, Screen.width, Screen.height);


        m_UIManager.Add(background);
        
        GameCenterInterface.Login();

    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time - startTime > 3.0f)
        {
			SceneManager.LoadScene (SceneName.START_MENU);    //! 跳转 开始菜单
        }

    }

       

}
