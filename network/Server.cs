using System;
using Godot;
using Godot.Collections;
using PNGTuber.ui;

namespace PNGTuber.network;

public partial class Server : Node {
	private const int BufferSize = 1024 * 1024 * 10;

	private WebSocketMultiplayerPeer _peer = new();

	private Dictionary<long, User> _users = new();

	public event Action? NetworkReady;

	public void Listen(int port) {
		var multiplayer = GetTree().GetMultiplayer();
		_peer.InboundBufferSize = BufferSize;
		_peer.OutboundBufferSize = BufferSize;
		_peer.PeerConnected += PeerOnPeerConnected;
		_peer.PeerDisconnected += PeerOnPeerDisconnected;
		var err = _peer.CreateServer(port);
		if (err != Error.Ok) {
			Logs.Info(err.ToString());
			return;
		}

		multiplayer.MultiplayerPeer = _peer;
		Logs.Info($"Listen on {port}");
		NetworkReady?.Invoke();
	}

	private void PeerOnPeerConnected(long id) {
		var user = new User(id);
		AddChild(user);
		_users.Add(id, user);
		var peer = _peer.GetPeer((int)id);
		peer.InboundBufferSize = BufferSize;
		peer.OutboundBufferSize = BufferSize;
		Logs.Info($"Peer {id} is connected");
	}

	private void PeerOnPeerDisconnected(long id) {
		_users[id].QueueFree();
		_users.Remove(id);
		Logs.Info($"Peer {id} is disconnected");
	}
}