using System.Collections.Generic;
using APIRequest;
using UnityEngine;
using Newtonsoft.Json;

namespace APIRequestSample
{
    public class APIManager : BaseAPIManager<APIManager>
    {
        private Dictionary<string, string> headers;

        protected override void Awake()
        {
            base.Awake();
            headers = new Dictionary<string, string>
            {
                { "x-api-key", "KPn2QxCzQxIBRbIT0S84v0e6dAQBvGSy" }
            };
        }

        protected override string GetBaseUrl()
        {
            return "https://fcw8qzotv4.execute-api.eu-central-1.amazonaws.com";
        }

        protected override Dictionary<string, string> GetHeaders()
        {
            return headers;
        }

        public void GetInfoUser()
        {
            string json =
                "{\"user_id\": \"random-user-id\",\"adid\": \"random-adid\",\"is_test_user\": 1,\"device_model\": \"Iphone 17\",\"ram\": 6,\"os_version\": \"ios 26\",\"network_type\": 1,\"country_code\": \"VN\",\"first_time_watch_ads\": 400,\"ad_click\": 5,\"rewarded_ad_count\": 1,\"interstitial_ad_view\": 1,\"session_per_day\": 2,\"d3_retention\": 1,\"d7_retention\": 1,\"login_per_week\": 3,\"stuck_level_count\": 2,\"win_rate\": 30,\"fail_rate\": 40,\"level_retry_intent\": 5,\"vip_booster_uses\": 1,\"ads_booster_uses\": 6,\"buy_coin_pack_count\": 2,\"ad_bypass_level\": 4,\"open_shop_count\": 3,\"shop_ctr\": 34,\"first_time_watch_shop\": 1200,\"promo_count\": 1,\"tap_buy\": 2,\"iap_success\": 2,\"purchase_per_month\": 1,\"total_revenue\": 7.5,\"join_event_count\": 6,\"win_streak_count\": 3,\"level_per_week\": 22,\"current_level\": 41,\"oav\": 3.15,\"rewarded_ad_rate\": 20}";

            UserData userData = JsonConvert.DeserializeObject<UserData>(json);
            Debug.Log($"datdb - {JsonConvert.SerializeObject(userData, Formatting.Indented)}");
            POST<UserTier>("class",
                userData,
                userTier =>
                {
                    Debug.Log($"datdb - user tier: {JsonConvert.SerializeObject(userTier, Formatting.Indented)}");
                },
                error => { Debug.Log($"datdb - error {error}"); }
            );
        }
    }

    [System.Serializable]
    public class UserTier
    {
        [JsonProperty("tier")] public long Tier { get; set; }

        [JsonProperty("class")] public string Class { get; set; }

        [JsonProperty("description")] public string Description { get; set; }

        [JsonProperty("tier_point")] public TypePoint TierTypePoint { get; set; }

        [JsonProperty("class_point")] public TypePoint ClassTypePoint { get; set; }
    }

    [System.Serializable]
    public class TypePoint
    {
        [JsonProperty("low")] public long Low { get; set; }

        [JsonProperty("med")] public long Med { get; set; }

        [JsonProperty("high")] public long High { get; set; }
    }

    [System.Serializable]
    public class UserData
    {
        [JsonProperty("user_id")] public string UserId { get; set; }

        [JsonProperty("adid")] public string Adid { get; set; }

        [JsonProperty("is_test_user")] public long IsTestUser { get; set; }

        [JsonProperty("device_model")] public string DeviceModel { get; set; }

        [JsonProperty("ram")] public long Ram { get; set; }

        [JsonProperty("os_version")] public string OsVersion { get; set; }

        [JsonProperty("network_type")] public long NetworkType { get; set; }

        [JsonProperty("country_code")] public string CountryCode { get; set; }

        [JsonProperty("first_time_watch_ads")] public long FirstTimeWatchAds { get; set; }

        [JsonProperty("ad_click")] public long AdClick { get; set; }

        [JsonProperty("rewarded_ad_count")] public long RewardedAdCount { get; set; }

        [JsonProperty("interstitial_ad_view")] public long InterstitialAdView { get; set; }

        [JsonProperty("session_per_day")] public long SessionPerDay { get; set; }

        [JsonProperty("d3_retention")] public long D3Retention { get; set; }

        [JsonProperty("d7_retention")] public long D7Retention { get; set; }

        [JsonProperty("login_per_week")] public long LoginPerWeek { get; set; }

        [JsonProperty("stuck_level_count")] public long StuckLevelCount { get; set; }

        [JsonProperty("win_rate")] public long WinRate { get; set; }

        [JsonProperty("fail_rate")] public long FailRate { get; set; }

        [JsonProperty("level_retry_intent")] public long LevelRetryIntent { get; set; }

        [JsonProperty("vip_booster_uses")] public long VipBoosterUses { get; set; }

        [JsonProperty("ads_booster_uses")] public long AdsBoosterUses { get; set; }

        [JsonProperty("buy_coin_pack_count")] public long BuyCoinPackCount { get; set; }

        [JsonProperty("ad_bypass_level")] public long AdBypassLevel { get; set; }

        [JsonProperty("open_shop_count")] public long OpenShopCount { get; set; }

        [JsonProperty("shop_ctr")] public long ShopCtr { get; set; }

        [JsonProperty("first_time_watch_shop")]
        public long FirstTimeWatchShop { get; set; }

        [JsonProperty("promo_count")] public long PromoCount { get; set; }

        [JsonProperty("tap_buy")] public long TapBuy { get; set; }

        [JsonProperty("iap_success")] public long IapSuccess { get; set; }

        [JsonProperty("purchase_per_month")] public long PurchasePerMonth { get; set; }

        [JsonProperty("total_revenue")] public double TotalRevenue { get; set; }

        [JsonProperty("join_event_count")] public long JoinEventCount { get; set; }

        [JsonProperty("win_streak_count")] public long WinStreakCount { get; set; }

        [JsonProperty("level_per_week")] public long LevelPerWeek { get; set; }

        [JsonProperty("current_level")] public long CurrentLevel { get; set; }

        [JsonProperty("oav")] public double Oav { get; set; }

        [JsonProperty("rewarded_ad_rate")] public long RewardedAdRate { get; set; }
    }
}