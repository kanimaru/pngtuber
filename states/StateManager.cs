using GenUtilities;
using Godot;
using PNGTuber.ui;

namespace PNGTuber.states;

public partial class StateManager : Node {
	[Bind] private Node2D _pngTuber = null!;
	[Bind] private VBoxContainer _stateContainer = null!;
	[Bind] private Button _addState = null!;

	[Export] private PngTuberDescription? _pngTuberDescription;

	public override void _Ready() {
		base._Ready();
		BindFields();
		_setupState();
		_addState.Pressed += AddStateOnPressed;
	}

	private void AddStateOnPressed() {
		var stateDescription = new StateDescription();
		_pngTuberDescription!.AddState(stateDescription);
		AddState(stateDescription);
	}

	private void AddState(StateDescription stateDescription) {
		var state = new State();
		state.StateDescription = stateDescription;
		state.StateUpdated += StateOnStateUpdated;
		state.StateDeleted += () => StateOnStateDeleted(state);
	}

	private void StateOnStateDeleted(State state) {
		_pngTuberDescription!.RemoveState(state.StateDescription);
		state.QueueFree();
		Save();
	}

	private void StateOnStateUpdated() {
		Save();
	}

	private void _setupState() {
		if (_pngTuberDescription != null) return;
		_pngTuberDescription = new PngTuberDescription();
		// _pngTuberDescription.AddState(DefaultStates.CECM);
		// _pngTuberDescription.AddState(DefaultStates.CEOM);
		// _pngTuberDescription.AddState(DefaultStates.OECM);
		// _pngTuberDescription.AddState(DefaultStates.OEOM);
	}

	private void Save() {
		_pngTuberDescription!.Save("user://current.tres");
	}
}