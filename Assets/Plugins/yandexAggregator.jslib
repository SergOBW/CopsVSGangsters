mergeInto(LibraryManager.library, {
  TryToAuth: function () {
    console.log('TRY TO AUTH');
    auth();
  },

  TryToLogin: function () {
    login();
  },

  GameReady: function () {
    YandexGameReady();
  },

  SaveExtern: function (_data) {
    console.log('Try save');
    var dataString = UTF8ToString(_data);
    var myObj = JSON.parse(dataString);
    console.log(myObj);
    player.setData(myObj).then(() => {
      console.log('Data saved')
    }).catch(() => {
      console.log('Error with saving data')
    });
  },

  LoadExtern: function () {
    initPlayer().then(_player => {
      player = _player;
      player.getData().then(_date => {
        const myJson = JSON.stringify(_date);
        console.log('From yandex saved')
        myGameInstance.SendMessage('YandexAggregator', 'SendPlayerSaves', myJson)
      })
    });
  },

  BuyItem: function (_id) {
    _id = UTF8ToString(_id);
    if(payments != null){
      console.log(_id + ' Whanna be byed');
      payments.purchase({ id: _id }).then(purchase => {
        // Покупка успешно совершена!
        console.log('Success buy id = ' + _id)
        myGameInstance.SendMessage('YandexAggregator', 'PurchaseSuccess', _id);
      }).catch(err => {
        console.log(err);
        myGameInstance.SendMessage('YandexAggregator', 'PurchaseError', _id);
      })
    } 
    else {
      console.log('Payments was null, initializing ' + _id);
      YaSDK.getPayments({ signed: true }).then(_payments => {
      // Покупки доступны.
      payments = _payments;
      console.log(_id + ' Whanna be byed');
      payments.purchase({ id: _id }).then(purchase => {
        // Покупка успешно совершена!
        console.log('Success buy id = ' + _id)
        myGameInstance.SendMessage('YandexAggregator', 'PurchaseSuccess', _id);
      }).catch(err => {
        console.log(err);
        myGameInstance.SendMessage('YandexAggregator', 'PurchaseError', _id);
      })}).catch(err => {
      console.log(err);
    })
    }
},

  GetPurchases: function () {
    console.log('Getting purchases');
    if(payments != null){
      payments.getPurchases().then(purchases => {
        var purchasesString = JSON.stringify(purchases);
        myGameInstance.SendMessage('YandexAggregator', 'SetPurchases', purchasesString);
      }).catch(err => {
        console.log(err);
      })
    } else 
    YaSDK.getPayments({ signed: true }).then(_payments => {
    payments = _payments;
    payments.getPurchases().then(purchases => {
      var purchasesString = JSON.stringify(purchases);
      myGameInstance.SendMessage('YandexAggregator', 'SetPurchases', purchasesString);
    }).catch(err => {console.log(err);})})
  },

  GetLang: function () {
    var lang = YaSDK.environment.i18n.lang;
    var bufferSize = lengthBytesUTF8(lang) + 1;
    var buffer = _malloc(bufferSize);
    stringToUTF8(lang, buffer, bufferSize);
    console.log(lang);
    return buffer;
  },

  IsSDKInitialized: function () {
    var isInitialized = false;
    isInitialized = YaSDK != null;
    console.log(isInitialized);
    if(!isInitialized){
      initYASDK();
    }
    return isInitialized;
  },

  GetDeviceTypeFromYandex: function () {
    if(YaSDK === undefined){
      console.log("ERROR DEVICE TYPE YASDK IS UNDEFINED");
      return "error";
    }
    var lang = YaSDK.deviceInfo.type;
    var bufferSize = lengthBytesUTF8(lang) + 1;
    var buffer = _malloc(bufferSize);
    stringToUTF8(lang, buffer, bufferSize);
    return buffer;
  },

});