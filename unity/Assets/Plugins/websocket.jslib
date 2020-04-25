mergeInto(LibraryManager.library, {
  WebSocketInit: function(url) {
    this.socket = new WebSocket(Pointer_stringify(url));
    this.socket.onmessage = function(e) {
			gameInstance.SendMessage('Controller', 'ParseMessage', e.data);
	};
    this.socket.onopen = function(e) {
        this.send('{"type": "request_state", "content": ""}');
    };
  },
  WebSocketSend: function(content) {
    this.socket.send(Pointer_stringify(content));
  }
});