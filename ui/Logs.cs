using GenUtilities;
using Godot;

namespace PNGTuber.ui;

public partial class Logs : Control {
	private static Logs? Instance { get; set; }

	[Bind] private RichTextLabel _log = null!;

	public override void _Ready() {
		base._Ready();
		BindFields();
	}

	public override void _EnterTree() {
		if (Instance is null)
			Instance = this;
		else
			QueueFree();
	}

	public override void _ExitTree() {
		if (Instance == this)
			Instance = null;
	}

	public static void Info(string message) {
		Instance?._log.AddText(message + "\n");
	}

	public static void Error(string message) {
		Instance?._log.PushColor(Colors.Red);
		Instance?._log.AddText(message + "\n");
		Instance?._log.Pop();
	}
}