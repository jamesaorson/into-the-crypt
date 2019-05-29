extends Node

var Player = load("res://models/Player.gd")

export(float) var friction = 0.80

var numberOfPlayers = 1

var players = [
	Player.new(
		null,
		Vector2(3, 2),
		2,
		3,
		Vector2(),
		7,
		15,
		50,
		false,
		0,
		2,
		2
	)
]