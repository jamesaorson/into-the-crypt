extends Node

func construct_scene_path(scene : String) -> String:
	return 'res://scenes/{scene}/{scene}.tscn'.format({'scene': scene})
