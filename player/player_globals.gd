extends Node

const UP = { vector = Vector2(0, -1), inputName = "player_move_up_" }
const DOWN = { vector = -UP.vector, inputName = "player_move_down_" }
const LEFT = { vector = Vector2(-1, 0), inputName = "player_move_left_" }
const RIGHT = { vector = -LEFT.vector, inputName = "player_move_right_" }

var numberOfPlayers = 2

var players = [
	{
		playerIndex = 0,
		instance = null,
		startPosition = Vector2(3, 2),
		width = 2,
		height = 3,
		velocity = Vector2(),
		walkingSpeed = 1000,
		sprintingSpeed = 3000,
		friction = 0.9,
		isSprinting = false,
		timeStart = null,
		timeElapsed = null,
		debugInfo = null,
		lightNode = null,
		up = { vector = UP.vector, inputName = UP.inputName + "0" },
		down = { vector = DOWN.vector, inputName = DOWN.inputName + "0" },
		left = { vector = LEFT.vector, inputName = LEFT.inputName + "0" },
		right = { vector = RIGHT.vector, inputName = RIGHT.inputName + "0" }
	},
	{
		playerIndex = 1,
		instance = null,
		startPosition = Vector2(6, 2),
		width = 2,
		height = 3,
		velocity = Vector2(),
		walkingSpeed = 1000,
		sprintingSpeed = 3000,
		friction = 0.9,
		isSprinting = false,
		timeStart = null,
		timeElapsed = null,
		debugInfo = null,
		lightNode = null,
		up = { vector = UP.vector, inputName = UP.inputName + "1" },
		down = { vector = DOWN.vector, inputName = DOWN.inputName + "1" },
		left = { vector = LEFT.vector, inputName = LEFT.inputName + "1" },
		right = { vector = RIGHT.vector, inputName = RIGHT.inputName + "1" }
	}
]