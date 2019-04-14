extends Node

const FLOOR_TILE = 0
const WALL_TILE = 1

const CRYPT_SECTION_SIZE = 8

var CRYPT_HEIGHT
const CRYPT_MAX_HEIGHT = 30
const CRYPT_MIN_HEIGHT = 10

var CRYPT_WIDTH
const CRYPT_MAX_WIDTH = 30
const CRYPT_MIN_WIDTH = 10

var HORIZONTAL_HALLWAYS = []
var VERTICAL_HALLWAYS = []

###################
# Godot Functions #
###################

func _init():
	var binaryLength = (CRYPT_SECTION_SIZE - 2) * 2
	var numberOfVariations = pow(2, binaryLength)
	
	for variation in range(numberOfVariations):
		HORIZONTAL_HALLWAYS.append([])
		HORIZONTAL_HALLWAYS[variation].resize(CRYPT_SECTION_SIZE)
		
		var binaryString = ""
		var binaryValue = variation
		for digit in range(binaryLength):
			binaryString = str(binaryValue % 2) + binaryString
			binaryValue = binaryValue >> 1
		var horizontalVariation = HORIZONTAL_HALLWAYS[variation]
		var halfBinaryLength = binaryLength / 2
		for row in range(CRYPT_SECTION_SIZE):
			horizontalVariation[row] = []
			horizontalVariation[row].resize(CRYPT_SECTION_SIZE)
			if row == 0:
				horizontalVariation[row][0] = 1
				horizontalVariation[row][CRYPT_SECTION_SIZE - 1] = 1
				for i in range(1, halfBinaryLength + 1):
					horizontalVariation[row][i] = int(binaryString[i - 1])
			elif row == CRYPT_SECTION_SIZE - 1:
				horizontalVariation[row][0] = 1
				horizontalVariation[row][CRYPT_SECTION_SIZE - 1] = 1
				for i in range(1, halfBinaryLength + 1):
					horizontalVariation[row][i] = int(binaryString[i + halfBinaryLength - 1])
			else:
				for i in range(CRYPT_SECTION_SIZE):
					horizontalVariation[row][i] = 0
					
		VERTICAL_HALLWAYS.append([])
		VERTICAL_HALLWAYS[variation].resize(CRYPT_SECTION_SIZE)

		var verticalVariation = VERTICAL_HALLWAYS[variation]
		for row in range(CRYPT_SECTION_SIZE):
			verticalVariation[row] = []
			verticalVariation[row].resize(CRYPT_SECTION_SIZE)
			if row == 0 or row == CRYPT_SECTION_SIZE - 1:
				for i in range(CRYPT_SECTION_SIZE):
					if i == 0 or i == CRYPT_SECTION_SIZE - 1:
						verticalVariation[row][i] = 1
					else:
						verticalVariation[row][i] = 0
			else:
				for i in range(CRYPT_SECTION_SIZE):
					if i != 0 and i != CRYPT_SECTION_SIZE - 1:
						verticalVariation[row][i] = 0
				verticalVariation[row][0] = int(binaryString[(row - 1) * 2])
				verticalVariation[row][CRYPT_SECTION_SIZE - 1] = int(binaryString[((row - 1) * 2) + 1])