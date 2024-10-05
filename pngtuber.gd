extends Node

## How long the eyes should close
@export var close_time: float = 1

## Time between 2 blinks
@export var blink_timeout: float = 1

## When should the mouth open
@export var speak_threshold: float = .1

## Minimal frequece that should be taken for blinking
@export var blinking_min_frequence: int = 6000

## Maximal frequece that should be taken for blinking
@export var blinking_max_frequence: int = 8000

## Different textures
@export var states: States

@onready var display: Sprite2D = %Display

var bus_index: int
var analizer: AudioEffectSpectrumAnalyzerInstance

var blinking: bool
var eyes_open: bool = true


func _ready() -> void:
	bus_index = AudioServer.get_bus_index("MicInput")
	analizer = AudioServer.get_bus_effect_instance(bus_index, 1)


func _process(_delta: float) -> void:
	var power := AudioServer.get_bus_peak_volume_left_db(bus_index, 0)
	var cur = db_to_linear(power)
	var is_speaking := db_to_linear(power) > speak_threshold

	var db = analizer.get_magnitude_for_frequency_range(blinking_min_frequence, blinking_max_frequence, AudioEffectSpectrumAnalyzerInstance.MAGNITUDE_MAX)
	if db.x * 1000 > 1 && !blinking: _start_blink()

	if is_speaking:
		if not eyes_open:
			display.texture = states.closed_eye_open_mouth
		else:
			display.texture = states.open_eye_open_mouth
	else:
		if not eyes_open:
			display.texture = states.closed_eye_closed_mouth
		else:
			display.texture = states.open_eye_closed_mouth


func _start_blink():
	blinking = true
	eyes_open = false
	await get_tree().create_timer(.5).timeout
	eyes_open = true
	await get_tree().create_timer(1).timeout
	blinking = false
