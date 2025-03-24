using GenUtilities;
using Godot;
using PNGTuber.network;

namespace PNGTuber;

public partial class Bootstrap : Node {
	[Export] private PackedScene? _onlineScene;

	[Bind] private Network _selectRole = null!;

	public override void _Ready() {
		base._Ready();
		BindFields();
		_selectRole.NetworkReady += NetworkReady;
	}

	private void NetworkReady() {
		if (_onlineScene != null) GD.PrintErr("Online scene not set");

		GetTree().ChangeSceneToPacked(_onlineScene);
	}
}