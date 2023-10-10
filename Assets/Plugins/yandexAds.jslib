mergeInto(LibraryManager.library, {
  ShowAdv: function () {
    YaSDK.adv.showFullscreenAdv({
      callbacks: {
        onClose: function (wasShown) {
          myGameInstance.SendMessage('Ads', 'InterstitialAddClosed');
        },
        onError: function (error) {
          myGameInstance.SendMessage('Ads', 'InterstitialAddClosedWithError');
        }
      }
    })
  },

  ShowAdvReward: function () {
    YaSDK.adv.showRewardedVideo({
      callbacks: {
        onOpen: () => {
          console.log("Reward add open");
        },
        onRewarded: () => {
          console.log("Rewarded");
        },
        onClose: () => {
          myGameInstance.SendMessage('Ads', 'RewardAddClosed');
        },
        onError: (e) => {
          myGameInstance.SendMessage('Ads', 'RewardAddClosedWithError');
          console.log("ERROR");
        }
      }
    })
  },

});