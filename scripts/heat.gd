extends Node
## Can handle heat subscription events.
## [code]{
##	 type: "click",      // Message type, currently either "click" or "system".
##	 id: "U97032862",    // User ID for viewer, which may be Anonymous, Opaque or a real Twitch user ID.
##	 x: "0.354",         // Normalized X coordinate.
##	 y: "0.736"          // Normalized Y coordinate.
## }[/code]

class_name HeatService

signal clicked(position: Vector2);
@export var cooldown: float = 0;
@export var viewport: Viewport;

var cooldowns: Dictionary;
var client = WebsocketClient.new()


func _ready() -> void:
	add_child(client)
	client.message_received.connect(_on_message);
	client.connection_url = 'wss://heat-api.j38.net/channel/132799162'
	client.open_connection()
	viewport = get_tree().root.get_viewport()


func _click(position: Vector2):
	if viewport == null: return;

	var event: InputEventMouseButton = InputEventMouseButton.new()
	event.position = position;
	event.global_position = position;
	event.button_index = MOUSE_BUTTON_LEFT;
	event.pressed = true;
	event.set_meta("heat", true)
	viewport.push_input(event, true)

	event = InputEventMouseButton.new()
	event.position = position;
	event.global_position = position;
	event.button_index = MOUSE_BUTTON_LEFT;
	event.pressed = false;
	event.set_meta("heat", true)
	viewport.push_input(event, true)


func _on_message(data: PackedByteArray):
	var msg = JSON.parse_string(data.get_string_from_utf8());
	if msg.type == 'system':
		print(msg)
		return
	print(msg)
	if cooldowns.has(msg.id) && cooldowns[msg.id] > Time.get_unix_time_from_system(): return;
	cooldowns[msg.id] = Time.get_unix_time_from_system() + cooldown;

	var relative_position := Vector2(float(msg.x), float(msg.y))
	_click(relative_position);
	clicked.emit(relative_position);
