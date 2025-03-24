using GenUtilities;
using Godot;
using PNGTuber.states;

namespace PNGTuber.ui;

public partial class AvatarListEntry : Control {
	[Bind] private TextureRect _preview = null!;
	[Bind] private Label _title = null!;

	[Export] public PngTuberDescription PngTuberDescription = null!;

	public override void _Ready() {
		base._Ready();
		BindFields();
		_preview.Texture = PngTuberDescription.GetState(DefaultStates.OpenEyesOpenMouth)!.Texture;
		_title.Text = PngTuberDescription.Title;
	}
}