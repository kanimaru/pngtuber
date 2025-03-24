using System;
using GenUtilities;
using Godot;
using PNGTuber.states;

namespace PNGTuber.ui;

public partial class State : Node {
	[Bind] private TextureButton _preview = null!;
	[Bind] private Button _loadFile = null!;
	[Bind] private LineEdit _title = null!;
	[Bind] private FileDialog _fileDialog = null!;
	[Bind] private Timer _debounce = null!;
	[Bind] private Button _delete = null!;

	[Export] public StateDescription StateDescription = null!;

	[Export] private bool _titleLocked;

	public event Action? StateUpdated;
	public event Action? StateDeleted;
	private PackedScene? _popup;

	public bool TitleLocked {
		get => _titleLocked;
		set {
			_titleLocked = value;
			if (!IsNodeReady()) return;
			_title.Editable = !value;
		}
	}

	public State() {
		_popup = GD.Load<PackedScene>("res://ui/popup.tscn");
	}

	public override void _Ready() {
		base._Ready();
		_updateStateDescription();
		TitleLocked = _titleLocked;
		_preview.Pressed += PreviewOnPressed;
		_loadFile.Pressed += LoadFileOnPressed;
		_fileDialog.FileSelected += FileDialogOnFileSelected;
		_title.TextChanged += TitleOnTextChanged;
		_debounce.Timeout += DebounceOnTimeout;
		_delete.Pressed += DeleteOnPressed;
	}

	private void _updateStateDescription() {
		_title.Text = StateDescription.Title;
		_preview.TextureNormal = StateDescription.Texture;
	}

	private void TitleOnTextChanged(string newtext) {
		StateDescription.Title = newtext;
		_debounce.Start();
	}

	private void DeleteOnPressed() {
		var popup = _popup?.Instantiate<Popup>();
		if (popup == null) return;

		popup.Title = "Are you sure?";
		popup.Description = $"Do you want to delete {StateDescription.Title}";
		AddChild(popup);
		popup.PopupCentered();
		popup.YesPressed += () => StateDeleted?.Invoke();
	}

	private void FileDialogOnFileSelected(string path) {
		var imageTexture = ResourceLoader.Load<Texture2D>(path);
		StateDescription.Texture = imageTexture;
		_preview.TextureNormal = imageTexture;
		_debounce.Start();
	}

	private void LoadFileOnPressed() {
		_fileDialog.Show();
	}

	private void PreviewOnPressed() {
		_fileDialog.Show();
	}

	private void DebounceOnTimeout() {
		StateUpdated?.Invoke();
	}
}