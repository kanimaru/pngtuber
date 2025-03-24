using System;
using GenUtilities;
using Godot;

namespace PNGTuber.ui;

public partial class Popup : Window {
	[Export]
	public string Description {
		get => _descriptionText;
		set {
			_descriptionText = value;
			if (IsNodeReady()) UpdateDescription(value);
		}
	}

	[Bind] private Label _description = null!;
	[Bind] private Button _no = null!;
	[Bind] private Button _yes = null!;

	private string _descriptionText = null!;

	public event Action? YesPressed;
	public event Action? NoPressed;

	public override void _Ready() {
		base._Ready();
		UpdateDescription(_descriptionText);
		_yes.Pressed += YesOnPressed;
		_no.Pressed += NoOnPressed;
		CloseRequested += OnCloseRequested;
	}

	private void OnCloseRequested() {
		QueueFree();
	}

	private void UpdateDescription(string value) {
		_descriptionText = value;
		_description.Text = _descriptionText;
	}

	private void YesOnPressed() {
		YesPressed?.Invoke();
	}

	private void NoOnPressed() {
		NoPressed?.Invoke();
	}
}