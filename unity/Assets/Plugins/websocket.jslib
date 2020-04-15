mergeInto(LibraryManager.library, {

  WebSocketInit: function(url) {
      //this.socket = new WebSocket(Pointer_stringify(url));
  },
  WebSocketSend: function(message) {
      this.socket.send(Pointer_stringify(message));
  }
});