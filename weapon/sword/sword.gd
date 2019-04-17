extends Area2D

onready var animationPlayer = $AnimationPlayer
export var canAttack = false
var currentAttack = 0

var damage = 1

var nextAnimation

func attack():
	if canAttack:
		self.canAttack = false
		var animation = "attack" + str(self.currentAttack)
		if self.animationPlayer.current_animation == "idle":
			self.animationPlayer.play(animation)
		else:
			self.animationPlayer.queue(animation)
		self.currentAttack = (self.currentAttack + 1) % 2

func _on_SwordNode2D_body_entered(body):
	body.damage(damage)

func _on_AnimationPlayer_animation_finished(animationName):
	match animationName:
		"attack0":
			if self.animationPlayer.current_animation.empty():
				self.currentAttack = 0
				self.animationPlayer.queue("idle")
		"attack1":
			if self.animationPlayer.current_animation.empty():
				self.currentAttack = 0
				self.animationPlayer.queue("idle")
