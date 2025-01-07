using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Advertisements;
using System.Collections;


public class AdsManager : MonoBehaviour, IUnityAdsLoadListener, IUnityAdsShowListener,IUnityAdsInitializationListener
{
    public static AdsManager Instance;
    
    [Header("初始化广告")]
    // [SerializeField]private GameObject canvas;
    // [SerializeField]private GameObject GameOver;
    [SerializeField] string _androidGameId="5726188";
    [SerializeField] string _iOSGameId="5726189";
    [SerializeField] bool _testMode = true;
    private string _gameId;
    [Header("加载奖励广告")]
    [SerializeField] Button _showAdButton;//按钮
    [SerializeField] string _androidAdUnitId = "Rewarded_Android";
    [SerializeField] string _iOSAdUnitId = "Rewarded_iOS";

    string _adUnitId = null; // 对于不支持的平台，此值将保持为空
    void Start()
    {   
        if (!Advertisement.isInitialized)
        {
            Debug.Log("初始化广告");
            InitializeAds();//初始化广告
        }
        GetAdUnitIdForCurrentPlatform();//获取广告位ID

         // 配置按钮，使其在点击时调用 ShowAd() 方法:
            _showAdButton.onClick.AddListener(ShowAd);
    }
    public void InitializeAds()
    {
     #if UNITY_IOS
            _gameId = _iOSGameId;
    #elif UNITY_ANDROID
            _gameId = _androidGameId;
    #elif UNITY_EDITOR
            _gameId = _androidGameId; //Only for testing the functionality in the Editor
    #endif
        if (!Advertisement.isInitialized && Advertisement.isSupported)
        {
            Advertisement.Initialize(_gameId, _testMode, this);
            _showAdButton.interactable = true;
        }
    }
    public void GetAdUnitIdForCurrentPlatform(){
        // Get the Ad Unit ID for the current platform:
    #if UNITY_IOS
            _adUnitId = _iOSAdUnitId;
    #elif UNITY_ANDROID
            _adUnitId = _androidAdUnitId;
    #endif
        // Disable the button until the ad is ready to show:
        // _showAdButton.interactable = false;
    }
    
    // 当你想准备一个广告显示时调用这个公共方法。
    public void LoadAd()
    {
        // 重要！仅在初始化之后加载内容（在此示例中，初始化在其他脚本中进行）。
            Debug.Log("正在加载广告: " + _adUnitId);
            Advertisement.Load(_adUnitId, this);
    }   
 
    // 如果广告成功加载，给按钮添加一个监听器并启用按钮:
    public void OnUnityAdsAdLoaded(string adUnitId)
    {
        Debug.Log("广告已加载: " + adUnitId);
 
        // if (adUnitId.Equals(_adUnitId))
        // {
           
            // 启用按钮供用户点击:
            //  _showAdButton.interactable = true;
        // }
    }
 
    // 实现一个方法，当用户点击按钮时执行:
    public void ShowAd()
    {
        Debug.Log("播放Unity Ads");
        // 禁用按钮:
        //  _showAdButton.interactable = false;
        // 然后显示广告:
        Advertisement.Show(_adUnitId, this);
    }
 
    // 实现 Show 监听器的 OnUnityAdsShowComplete 回调方法，以确定用户是否可以获得奖励:
    public void OnUnityAdsShowComplete(string adUnitId, UnityAdsShowCompletionState showCompletionState)
    {
        if (adUnitId.Equals(_adUnitId) && showCompletionState.Equals(UnityAdsShowCompletionState.COMPLETED))
        {
            Debug.Log("Unity Ads 奖励广告已完成");
            // 授予奖励。
            FindObjectOfType<playerControl>().Healthy=3;
            FindAnyObjectByType<playerControl>().isDead=false;
            UIManger.Instance.changeHp(FindObjectOfType<playerControl>().Healthy);
            LoadAd(); // 加载下一个广告
        }
    }
 
    // 实现 Load 和 Show 监听器的错误回调方法:
    public void OnUnityAdsFailedToLoad(string adUnitId, UnityAdsLoadError error, string message)
    {
        Debug.Log($"加载广告单元 {adUnitId} 出错: {error.ToString()} - {message}");
        // 使用错误信息来决定是否尝试加载另一个广告。
    }
 
    public void OnUnityAdsShowFailure(string adUnitId, UnityAdsShowError error, string message)
    {
        Debug.Log($"显示广告单元 {adUnitId} 出错: {error.ToString()} - {message}");
        // 使用错误信息来决定是否尝试加载另一个广告。
    }
 
    public void OnUnityAdsShowStart(string adUnitId) { }
    public void OnUnityAdsShowClick(string adUnitId) { }
 
    void OnDestroy()
    {
        // 清除按钮的监听器:
        _showAdButton.onClick.RemoveAllListeners();
    }

    public void OnInitializationComplete()//初始化成功回调
    {
        Debug.Log("初始化广告成功");
        LoadAd();
    }

    public void OnInitializationFailed(UnityAdsInitializationError error, string message)
    {
        Debug.Log($"初始化广告失败: {error.ToString()} - {message}");
        
       InitializeAds(); // 加载下一个广告
    }
}
