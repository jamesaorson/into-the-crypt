extends TileMap

onready var doorScene = load("res://crypt/door/door.tscn")

const ROOM_HEIGHT = 18
const ROOM_WIDTH = 32

const PLAYER_START_POSITION = Vector2(1, 1)
const PLAYER_HEIGHT = 3
const PLAYER_WIDTH = 2

var DOOR_POSITION = Vector2(ROOM_WIDTH - 1, ROOM_HEIGHT / 2)
const DOOR_HEIGHT = 2

var room = []

# Called when the node enters the scene tree for the first time.
func _ready():
	randomize()
	for y in range(ROOM_HEIGHT):
		room.append([])
		room[y].resize(ROOM_WIDTH)
		for x in range(ROOM_WIDTH):
			if x >= PLAYER_START_POSITION.x and x < PLAYER_START_POSITION.x + PLAYER_WIDTH and y >= PLAYER_START_POSITION.y and y < PLAYER_START_POSITION.y + PLAYER_HEIGHT:
				room[y][x] = 0
			elif x >= DOOR_POSITION.x - PLAYER_WIDTH and x <= DOOR_POSITION.x and y >= DOOR_POSITION.y and y < DOOR_POSITION.y + DOOR_HEIGHT:
				if x == DOOR_POSITION.x and y == DOOR_POSITION.y:
					var doors = get_tree().get_nodes_in_group("door")
					if len(doors) == 0:
						print("x, y: ", x, " ", y)
						var doorPosition = map_to_world(Vector2(x, y))
						print("Door: ", doorPosition)
						var door = doorScene.instance()
						door.position.x = doorPosition.x
						door.position.y = doorPosition.y
						get_tree().root.add_child(door)
				room[y][x] = 0
			elif x == 0 or x == ROOM_WIDTH - 1 or y == 0 or y == ROOM_HEIGHT - 1:
				room[y][x] = 1
			else:
				var choice = rand_range(1, 10)
				if choice > 2:
					room[y][x] = 0
				else:
					room[y][x] = 1
	draw_room()


func draw_room():
	for y in range(len(room)):
		for x in range(len(room[y])):
			set_cell(x, y, room[y][x])