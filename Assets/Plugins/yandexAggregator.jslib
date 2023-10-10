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
    _id = Pointer_stringify(_id);
    console.log(_id + ' Whanna be byed');
    payments.purchase({ id: _id }).then(purchase => {
      // Покупка успешно совершена!
      myGameInstance.SendMessage('Ads', 'PurchaseSuccess', _id);
    }).catch(err => {
      // Покупка не удалась: в консоли разработчика не добавлен товар с таким id,
      // пользователь не авторизовался, передумал и закрыл окно оплаты,
      // истекло отведенное на покупку время, не хватило денег и т. д.
      console.log(_id);
      console.log(err);
      myGameInstance.SendMessage('Ads', 'PurchaseClose', _id);
    })
  },

  CheckForPayments: function (_id) {
    _id = Pointer_stringify(_id);
    payments.getPurchases().then(purchases => {
      if (purchases.some(purchase => purchase.productID === _id)) {
        console.log(_id);
        myGameInstance.SendMessage('Ads', 'PurchaseSuccess', _id);
      }
    }).catch(err => {
      // Выбрасывает исключение USER_NOT_AUTHORIZED для неавторизованных пользователей.
    })
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
    console.log(lang);
    return buffer;
  },

});