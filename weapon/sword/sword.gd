extends Area2D

var Weapon = load("res://models/Weapon.gd")

onready var animationPlayer = $AnimationPlayer
export var canAttack = false

var damage = 1
var numberOfAttacksInCombo = 2
var direction = "right"

onready var weapon

func _ready():
	self.weapon = Weapon.new(self.animationPlayer, 1, self.damage, self.numberOfAttacksInCombo)

func attack():
	if self.canAttack:
		self.canAttack = false
		self.weapon.attack(self.direction)

func flip_h():
	$Sprite.set_flip_h($Sprite.flip_h)
	if self.direction == "right":
		self.direction = "left"
	else:
		self.direction = "right"
	self.weapon.update_direction(direction)

func _on_Area2D_body_entered(body):
	self.weapon.make_contact(body)

func _on_AnimationPlayer_animation_finished(animationName):
	self.weapon.animation_finished(animationName, self.direction)
