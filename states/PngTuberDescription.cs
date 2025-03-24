using System;
using System.Linq;
using Godot;
using Godot.Collections;
using PNGTuber.ui;

namespace PNGTuber.states;

[GlobalClass]
public partial class PngTuberDescription : Resource {
	/// <summary>
	/// Title to save the file.
	/// </summary>
	[Export] public string Title = "Untitled";

	/// <summary>
	/// How long the eyes should close
	/// </summary>
	[Export] public float CloseTime = 1;

	/// <summary>
	/// Time between 2 blinks
	/// </summary>
	[Export] public float BlinkTimeout = 1;

	/// <summary>
	/// When should the mouth open
	/// </summary>
	[Export] public float SpeakThreshold = .1f;

	/// <summary>
	/// Minimal frequency that should be taken for blinking
	/// </summary>
	[Export] public float BlinkingMinFrequency = 6000f;

	/// <summary>
	/// Maximal frequency that should be taken for blinking
	/// </summary>
	[Export] public float BlinkingMaxFrequency = 8000f;

	/// <summary>
	/// All possible states
	/// </summary>
	[Export] private Array<StateDescription> _states = new Array<StateDescription>();

	public StateDescription? GetState(string name) {
		try {
			return _states.First(state => state.Title.Equals(name));
		}
		catch (InvalidOperationException e) {
			Logs.Error($"Couldn't find state {name}.");
			return null;
		}
	}

	public void AddState(StateDescription state) {
		_states.Add(state);
	}
	
	public void CreateState(string stateName) {
		var state = new StateDescription();
		state.Title = stateName;
		_states.Add(state);
	}
	
	public void Save(string path) {
		ResourceSaver.Save(this, path);
	}

	public void RemoveState(StateDescription state) {
		_states.Remove(state);
	}
}