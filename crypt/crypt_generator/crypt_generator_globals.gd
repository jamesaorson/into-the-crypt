extends Node

#warning-ignore-all:unused_class_variable
const FLOOR_TILES : Array = [2, 3, 4]
const WALL_TILES : Array = [1]

const FLOOR : int = 0
const WALL : int = 1
const EXIT : int = 2

const CRYPT_SECTION_SIZE : int = 8

var CRYPT_HEIGHT : int
const CRYPT_MAX_HEIGHT : int = 30
const CRYPT_MIN_HEIGHT : int = 10

var CRYPT_WIDTH : int
const CRYPT_MAX_WIDTH : int = 30
const CRYPT_MIN_WIDTH : int = 10

var HORIZONTAL_HALLWAYS : Array = []
var VERTICAL_HALLWAYS : Array = []

###################
# Godot Functions #
###################

func _init() -> void:
	var binaryLength : int = (CRYPT_SECTION_SIZE - 2) * 2
	var numberOfVariations : int = pow(2, binaryLength)
	
	for variation in range(numberOfVariations):
		HORIZONTAL_HALLWAYS.append([])
		HORIZONTAL_HALLWAYS[variation].resize(CRYPT_SECTION_SIZE)
		
		var binaryString : String = ''
		var binaryValue : int = variation
		for digit in range(binaryLength):
			binaryString = str(binaryValue % 2) + binaryString
			binaryValue = binaryValue >> 1
		
		var horizontalVariation : Array = HORIZONTAL_HALLWAYS[variation]
		var halfBinaryLength : int = binaryLength / 2
		for row in range(CRYPT_SECTION_SIZE):
			horizontalVariation[row] = []
			horizontalVariation[row].resize(CRYPT_SECTION_SIZE)
			
			if row == 0:
				horizontalVariation[row][0] = FLOOR
				horizontalVariation[row][CRYPT_SECTION_SIZE - 1] = WALL
				for i in range(1, halfBinaryLength + 1):
					horizontalVariation[row][i] = int(binaryString[i - 1])
			elif row == CRYPT_SECTION_SIZE - 1:
				horizontalVariation[row][0] = WALL
				horizontalVariation[row][CRYPT_SECTION_SIZE - 1] = WALL
				for i in range(1, halfBinaryLength + 1):
					horizontalVariation[row][i] = int(binaryString[i + halfBinaryLength - 1])
			else:
				for i in range(CRYPT_SECTION_SIZE):
					horizontalVariation[row][i] = FLOOR
					
		VERTICAL_HALLWAYS.append([])
		VERTICAL_HALLWAYS[variation].resize(CRYPT_SECTION_SIZE)

		var verticalVariation : Array = VERTICAL_HALLWAYS[variation]
		for row in range(CRYPT_SECTION_SIZE):
			verticalVariation[row] = []
			verticalVariation[row].resize(CRYPT_SECTION_SIZE)
			if row == 0 or row == CRYPT_SECTION_SIZE - 1:
				for i in range(CRYPT_SECTION_SIZE):
					if i == 0 or i == CRYPT_SECTION_SIZE - 1:
						verticalVariation[row][i] = WALL
					else:
						verticalVariation[row][i] = FLOOR
			else:
				for i in range(CRYPT_SECTION_SIZE):
					if i != 0 and i != CRYPT_SECTION_SIZE - 1:
						verticalVariation[row][i] = FLOOR
				verticalVariation[row][0] = int(binaryString[(row - 1) * 2])
				verticalVariation[row][CRYPT_SECTION_SIZE - 1] = int(binaryString[((row - 1) * 2) + 1])