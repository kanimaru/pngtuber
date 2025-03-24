using System;
using GenUtilities;
using Godot;
using PNGTuber.states;

namespace PNGTuber.ui;

public partial class AvatarList : GridContainer {
	
	[Export(PropertyHint.File)] private string _location = "user://avatars/";

	private PngTuberDescription? _selectedAvatar;

	private void LoadAvatars() {
		var dirAccess = DirAccess.Open(_location);
		var err = dirAccess.ListDirBegin();
		if (err != Error.Ok) {
			Logs.Error($"Can't open avatars at {_location}");
			return;
		}

		for (var file = dirAccess.GetNext(); file != ""; file = dirAccess.GetNext()) AddAvatar(file);
	}

	private void AddAvatar(string file) {
		try {
			var avatar = ResourceLoader.Load<PngTuberDescription>(file);
			var entry = Scenes.NewAvatarListEntry<AvatarListEntry>();
			entry.PngTuberDescription = avatar;
		}
		catch (InvalidCastException) {
			// ignore
		}
	}

	private void NewAvatar() {
		var description = new PngTuberDescription();
		description.CreateState(DefaultStates.OpenEyesClosedMouth);
		description.CreateState(DefaultStates.ClosedEyesOpenMouth);
		description.CreateState(DefaultStates.ClosedEyesClosedMouth);
		description.CreateState(DefaultStates.OpenEyesOpenMouth);
		_selectedAvatar = description;
	}
}