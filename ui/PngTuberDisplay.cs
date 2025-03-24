using System;
using GenUtilities;
using Godot;
using PNGTuber.states;

namespace PNGTuber.ui;

public partial class PngTuberDisplay : Control {

	[Export] 
    public string SenderName {
    	get => _senderName;
    	set => UpdateSenderName(value);
    }
	
	[Export] private PngTuberDescription _pngTuberDescription = null!;

	[Bind] private TextureRect _avatar = null!;
	[Bind] private Viewport _spoutViewport = null!;
	
	private string _senderName = "";
	private int _busIndex;
	private AudioEffectSpectrumAnalyzerInstance _analyzerInstance = null!;

	private bool _blinking;
	private bool _eyesOpen;

	

	private void UpdateSenderName(string value) {
		_senderName = value;
		if (IsNodeReady()) {
			_spoutViewport.Set("sender_name", value);
		}
	}

	public override void _Ready() {
		base._Ready();
		BindFields();
		_busIndex = AudioServer.GetBusIndex("MicInput");
		_analyzerInstance = (AudioEffectSpectrumAnalyzerInstance)AudioServer.GetBusEffectInstance(_busIndex, 1);
		UpdateSenderName(_senderName);
	}

	public override void _Process(double delta) {
		base._Process(delta);
		var volumeInDb = AudioServer.GetBusPeakVolumeLeftDb(_busIndex, 0);
		var volume = Mathf.DbToLinear(volumeInDb);
		var isSpeaking = volume > _pngTuberDescription.SpeakThreshold;
		var frequency = _analyzerInstance.GetMagnitudeForFrequencyRange(
			_pngTuberDescription.BlinkingMinFrequency,
			_pngTuberDescription.BlinkingMaxFrequency);
		if (frequency.X * 1000.0f > 1.0f && !_blinking) StartBlink();

		if (isSpeaking) {
			var closedEyeOpenMouth = _pngTuberDescription.GetState(DefaultStates.ClosedEyesOpenMouth);
			var openEyeOpenMouth = _pngTuberDescription.GetState(DefaultStates.OpenEyesOpenMouth);
			
			_avatar.Texture = !_blinking
				? closedEyeOpenMouth?.Texture
				: openEyeOpenMouth?.Texture;
		} else {
			var closedEyesClosedMouth = _pngTuberDescription.GetState(DefaultStates.ClosedEyesClosedMouth);
			var openEyesClosedMouth = _pngTuberDescription.GetState(DefaultStates.OpenEyesClosedMouth);
			_avatar.Texture = !_blinking
				? closedEyesClosedMouth?.Texture
				: openEyesClosedMouth?.Texture;
		}
	}

	private async void StartBlink() {
		try {
			_blinking = true;
			_eyesOpen = false;
			var timer = GetTree().CreateTimer(_pngTuberDescription.CloseTime);
			await ToSignal(timer, SceneTreeTimer.SignalName.Timeout);
			_eyesOpen = true;
			timer = GetTree().CreateTimer(_pngTuberDescription.BlinkTimeout);
			await ToSignal(timer, SceneTreeTimer.SignalName.Timeout);
			_blinking = false;
		}
		catch (Exception e) {
			GD.PrintErr(e);
		}
	}
}
