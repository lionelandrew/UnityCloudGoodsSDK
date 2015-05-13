﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using CloudGoods.SDK.Item;

namespace CloudGoods.Services
{
    [System.Serializable]
    public class CloudGoodsSettings : ScriptableObject
    {
        public enum ScreenType
        {
            Settings,
            About,
            _LastDoNotUse,
        }

        static public string VERSION = "1.0";

        static public string mainPath = "Assets/CloudGoods/Textures/";

        public string appID;
        public string appSecret;
        public ScreenType screen;
        public string url = "http://webservice.socialplay.com/cloudgoods/cloudgoodsservice.svc/";
        public string bundlesUrl = "https://socialplay.blob.core.windows.net/unityassetbundles/";
        public string androidKey = "";
        public Texture2D defaultTexture;
        public GameObject defaultItemDrop;
        public GameObject defaultUIItem;
        private string domainURL = "";

        public List<ItemPrefabInitilizer.DropPrefab> itemInitializerPrefabs = new List<ItemPrefabInitilizer.DropPrefab>();

        static CloudGoodsSettings mInst;

        static public CloudGoodsSettings instance
        {
            get
            {
                if (mInst == null) mInst = (CloudGoodsSettings)Resources.Load("CloudGoodsSettings", typeof(CloudGoodsSettings));
                return mInst;
            }
        }

        static public GameObject DefaultItemDrop
        {
            get
            {
                return instance.defaultItemDrop;
            }
        }

        static public GameObject DefaultUIItem
        {
            get
            {
                return instance.defaultUIItem;
            }
        }

        static public string AppSecret
        {
            get
            {
                return instance.appSecret.Trim();
            }
        }

        static public string AppID
        {
            get
            {
                if (string.IsNullOrEmpty(instance.appID))
                {
                    Debug.Log("Clous Goods App Id is not set correctly");
                }

                return instance.appID.Trim();
            }
        }

        static public string Url
        {
            get
            {
                return instance.url.Trim();
            }
        }

        static public Texture2D DefaultTexture
        {
            get
            {
                return instance.defaultTexture;
            }
        }

        static public string BundlesUrl
        {
            get
            {
                return instance.bundlesUrl;
            }
        }

        static public string AndroidKey
        {
            get
            {
                return instance.androidKey.Trim();
            }
        }
        static public List<ItemPrefabInitilizer.DropPrefab> ExtraItemPrefabs
        {
            get
            {
                return instance.itemInitializerPrefabs;
            }
        }


    }
}
