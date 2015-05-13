using UnityEngine;
using System.Collections.Generic;
using System;
using LitJson;
using CloudGoods.SDK.Models;
using CloudGoods.Services;

namespace CloudGoods.CurrencyPurchase
{
    public class AndroidPremiumCurrencyPurchaser : MonoBehaviour, IPlatformPurchaser
    {
        public int currentBundleID = 0;
        public string currentProductID = "";

#if UNITY_ANDROID
        public AndroidJavaClass jc;
#endif

        public event Action<PurchasePremiumCurrencyBundleResponse> RecievedPurchaseResponse;
        public event Action<PurchasePremiumCurrencyBundleResponse> OnPurchaseErrorEvent;

        void Start()
        {
            gameObject.name = "AndroidCreditPurchaser";
            initStore();
        }

        void initStore()
        {
#if UNITY_ANDROID
        if (string.IsNullOrEmpty(CloudGoodsSettings.AndroidKey))
        {
            Debug.LogError("No Android key has been set, cannot initialize premium bundle store");
            return;
        }

        jc = new AndroidJavaClass("com.example.unityandroidpremiumpurchase.AndroidPurchaser");

        using (AndroidJavaClass cls = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
        {
            using (AndroidJavaObject obj_Activity = cls.GetStatic<AndroidJavaObject>("currentActivity"))
            {

                Debug.Log("Calling androidpurchas init");
                jc.CallStatic("InitAndroidPurchaser", obj_Activity, CloudGoodsSettings.AndroidKey);

            }
        }
#endif
        }

        public void Purchase(PremiumBundle bundleItem, int amount, string userID)
        {
#if UNITY_ANDROID
        if (string.IsNullOrEmpty(CloudGoodsSettings.AndroidKey))
        {
            Debug.LogError("No Android key has been set, cannot purchase from premium store");
            return;
        }

        currentBundleID = int.Parse(bundleItem.BundleID);
        currentProductID = bundleItem.ProductID;

        Debug.Log("Current product id: " + currentProductID);

        using (AndroidJavaClass cls = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
        {
            using (AndroidJavaObject obj_Activity = cls.GetStatic<AndroidJavaObject>("currentActivity"))
            {
                jc.CallStatic("PurchasePremiumCurrencyBundle", obj_Activity, currentProductID);

            }
        }
#endif
        }

        void ErrorFromAndroid(string responseCode)
        {
#if UNITY_ANDROID
        if (OnPurchaseErrorEvent != null)
        {
            PurchasePremiumCurrencyBundleResponse response = new PurchasePremiumCurrencyBundleResponse();
            response.StatusCode = 0;
            response.Message = "Error Occured, Response Code: " + responseCode;
            OnPurchaseErrorEvent(response);
        }

        if (responseCode.Remove(1, responseCode.Length - 1) == "7")
        {
            ConsumeOwneditem();
        }
#endif
        }
        private void ConsumeOwneditem()
        {
#if UNITY_ANDROID
        using (AndroidJavaClass cls = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
        {
            using (AndroidJavaObject obj_Activity = cls.GetStatic<AndroidJavaObject>("currentActivity"))
            {
                //cls_StorePurchaser.CallStatic("consumeitem", obj_Activity, currentProductID);
            }
        }
#endif
        }

        private void ConsumeCurrentPurchase()
        {
#if UNITY_ANDROID
        using (AndroidJavaClass cls = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
        {
            using (AndroidJavaObject obj_Activity = cls.GetStatic<AndroidJavaObject>("currentActivity"))
            {
                //cls_StorePurchaser.CallStatic("ConsumeCurrentPurchase", obj_Activity);
            }
        }
#endif
        }

        void RecieveFromJava(string message)
        {
#if UNITY_ANDROID
        Debug.Log("Received from java message: " + message);

        if (message != "Fail")
        {
            BundlePurchaseRequest bundlePurchaseRequest = new BundlePurchaseRequest();
            bundlePurchaseRequest.BundleID = currentBundleID;
           // bundlePurchaseRequest.UserID = CloudGoods.user.userID.ToString();
            bundlePurchaseRequest.ReceiptToken = message;

            //TODO implement platform check for platform premium currency bundle purchase
            bundlePurchaseRequest.PaymentPlatform = 3;

          //  string bundleJsonString = JsonConvert.SerializeObject(bundlePurchaseRequest);

        
          //  CloudGoods.PurchaseCreditBundles(bundleJsonString, OnReceivedPurchaseResponse);
        }
        else
        {
            PurchasePremiumCurrencyBundleResponse response = new PurchasePremiumCurrencyBundleResponse();
            response.StatusCode = 0;
            response.Message = message;

            OnPurchaseErrorEvent(response);
        }
#endif
        }

        void DebugFromJava(string message)
        {
            Debug.Log("Debug from Java: " + message);
        }

        public void OnReceivedPurchaseResponse(PurchasePremiumCurrencyBundleResponse data)
        {
            if (data.StatusCode == 1)
            {
                ConsumeCurrentPurchase();

                if (RecievedPurchaseResponse != null)
                    RecievedPurchaseResponse(data);
            }
            else
            {
                Debug.Log("Purchase was not authentic, consuming Item");

                if (OnPurchaseErrorEvent != null)
                    OnPurchaseErrorEvent(data);

            }
        }

    }
}