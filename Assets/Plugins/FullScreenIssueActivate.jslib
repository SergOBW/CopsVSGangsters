mergeInto(LibraryManager.library, {

    GoFullscreen: function()
    {
          Module['dynCall_vi'](true,document.pointerLockElement == Module["canvas"]);
    },

  });