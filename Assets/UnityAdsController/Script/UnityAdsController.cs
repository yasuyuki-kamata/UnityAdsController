/* 
 * UnityAdsController.cs
 * https://github.com/yasuyuki-kamata/UnityAdsController
 * 
 * 
 * if the errors are caused in Editor, please check the following things
 *  - Missing "UnityEngine.Advertisements"
 *    -> Turning ON the UnityAds Service from Services Window.
 *    -> Or, import UnityAds SDK from AssetStore.
 * 
 * もしUnityエディタでエラーがでる場合は、下記のことをご確認ください
 *  - "UnityEngine.Advertisements"がみつからないといわれたとき
 *    -> ServiceウィンドウからAdsサービスをONにしてください
 *    -> または、アセットストアからUnityAdsのSDKをインポートしてください
 */
using UnityEngine;
using System.Collections;
using UnityEngine.Advertisements;
using UnityEngine.Events;

public class UnityAdsController : MonoBehaviour
{
	[SerializeField]
	string zoneID = "rewardedVideo";
	[SerializeField]
	string gameID_iOS = "1050908";
	[SerializeField]
	string gameID_Android = "1050907";

	[Header("OnFinished Callback")]
	public UnityEvent OnFinishedAds;
	[Header("OnSkipped Callback")]
	public UnityEvent OnSkippedAds;
	[Header("OnFailed Callback")]
	public UnityEvent OnFailedAds;

	void Start ()
	{
		if (Advertisement.isSupported && !Advertisement.isInitialized) {
			#if UNITY_ANDROID
			Advertisement.Initialize(gameID_Android);
			#elif UNITY_IOS
			Advertisement.Initialize(gameID_iOS);
			#endif
		}
	}

	public void ShowUnityAds ()
	{
		#if UNITY_ANDROID || UNITY_IOS
		if (Advertisement.IsReady(zoneID)) {
		var options = new ShowOptions { resultCallback = HandleShowResult };
		Advertisement.Show(zoneID, options);
		}
		#endif
	}

	private void HandleShowResult(ShowResult result)
	{
		switch (result)
		{
		case ShowResult.Finished:
			Debug.Log ("The ad was successfully shown.");
			OnFinished ();
			break;
		case ShowResult.Skipped:
			Debug.Log ("The ad was skipped before reaching the end.");
			OnSkipped ();
			break;
		case ShowResult.Failed:
			Debug.LogError ("The ad failed to be shown.");
			OnFailed ();
			break;
		}
	}

	void OnFinished ()
	{
		// ここに動画視聴完了時の処理
		OnFinishedAds.Invoke();
	}

	void OnSkipped ()
	{
		// ここに動画をスキップしたときの処理
		OnSkippedAds.Invoke();
	}

	void OnFailed ()
	{
		// ここに動画視聴失敗時の処理
		OnFailedAds.Invoke();
	}
}