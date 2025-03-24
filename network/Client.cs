using System;
using Godot;
using PNGTuber.ui;

namespace PNGTuber.network;

public partial class Client : Node {
	private const int BufferSize = 1024 * 1024 * 10;

	private WebSocketMultiplayerPeer _peer = new();

	public event Action? NetworkReady;

	public void ConnectToServer(string address) {
		_peer.InboundBufferSize = BufferSize;
		_peer.OutboundBufferSize = BufferSize;
		var err = _peer.CreateClient(address);
		if (err != Error.Ok) {
			Logs.Info(err.ToString());
			return;
		}

		Multiplayer.MultiplayerPeer = _peer;
		Logs.Info($"Connecting to {address}");
		NetworkReady?.Invoke();
	}

	public void Close() {
		_peer.Close();
		Multiplayer.MultiplayerPeer = null;
		Logs.Info("Disconnecting");
	}
}