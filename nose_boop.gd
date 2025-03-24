extends Node2D

@onready var click_area: Area2D = %ClickArea

@export var resolution: Vector2
@export var offset: Vector2

var clicks: Array[Vector2] = []

func _ready() -> void:
	click_area.input_event.connect(_on_click)

func _process(delta: float) -> void:
	queue_redraw()

func _unhandled_input(event: InputEvent) -> void:
	if event.has_meta("heat"):
		clicks.append(event.global_position)
		queue_redraw()

func _on_click(viewport: Node, event: InputEvent, shape_idx: int):
	print("HIT")

func _draw() -> void:
	for click in clicks:
		click *= resolution
		click -= offset
		draw_circle(click, 5, Color.RED)
