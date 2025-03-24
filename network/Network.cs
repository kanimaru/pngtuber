using System;
using GenUtilities;
using Godot;
using PNGTuber.ui;

namespace PNGTuber.network;

public partial class Network : Node {
	[Bind] private Button _chooseClient = null!;
	[Bind] private Button _chooseServer = null!;
	[Bind] private LineEdit _address = null!;
	[Bind] private Button _connect = null!;
	[Bind] private Button _listen = null!;
	[Bind] private LineEdit _port = null!;
	[Bind] private Button _disconnect = null!;
	[Bind] private VBoxContainer _chooseNetwork = null!;
	[Bind] private GridContainer _clientSetting = null!;
	[Bind] private GridContainer _serverSetting = null!;
	[Bind] private Server _server = null!;
	[Bind] private Client _client = null!;

	public event Action? NetworkReady;

	public override void _Ready() {
		base._Ready();
		BindFields();

		_chooseClient.Pressed += ChooseClientOnPressed;
		_chooseServer.Pressed += ChooseServerOnPressed;
		_connect.Pressed += ConnectOnPressed;
		_listen.Pressed += ListenOnPressed;
		_disconnect.Pressed += DisconnectOnPressed;
		_chooseNetwork.Visible = true;
		_clientSetting.Visible = false;
		_serverSetting.Visible = false;
		_server.NetworkReady += () => NetworkReady?.Invoke();
	}

	private void ChooseServerOnPressed() {
		_chooseNetwork.Visible = false;
		_serverSetting.Visible = true;
	}

	private void ListenOnPressed() {
		var port = _port.Text;
		if (!int.TryParse(port, out var portNumber)) {
			Logs.Info($"Listening on port ({port}) is invalid");
			return;
		}

		_server.Listen(portNumber);
	}

	private void ChooseClientOnPressed() {
		_chooseNetwork.Visible = false;
		_clientSetting.Visible = true;
	}

	private void ConnectOnPressed() {
		_client.ConnectToServer(_address.Text);
	}

	private void DisconnectOnPressed() {
		_client.Close();
	}
}