using Godot;

namespace PNGTuber.states;

[GlobalClass]
public partial class StateDescription : Resource {
	[Export] private Texture2D _texture = null!;
	[Export] private string _title = null!;

	public Texture2D Texture {
		get => _texture;
		set {
			_texture = value;
			EmitChanged();
		}
	}

	public string Title {
		get => _title;
		set {
			_title = value;
			EmitChanged();
		}
	}
}