from random import randint

# ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
#   Print boilerplate banner code
# ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
def printBanner():
  global stones
  print(" _____ _       _   _        _____ _       \n"+
  "|   __| |_ ___| |_|_|___   |   | |_|_____ \n"+
  "|__   |  _| .'|  _| |  _|  | | | | |     |\n"+
  "|_____|_| |__,|_| |_|___|  |_|___|_|_|_|_|\n"+
  "                                          ")
  print("COSC 419 - Lab 02 \nBen Ward \nSN: 300190739\n")
  num = 0
  print("There are %2d stones in a pile. Take turns with a friend removing 1 - 3 stones." % (stones))
  print("Whoever takes the last stone loses! Type \"cheat\" for suggested move and \"quit\""+
  " to end the game. \n")

# ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
#   If there are 0 stones return true 
#     else return false
# ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
def checkWin():
  global stones
  return bool(stones <=0)
  
# ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
#   Takes a variable and checks if it is an integer between 1 and 3 inclusive.
#   returns True if check is True
#   else returns False
# ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
def checkNum(num):
  try:
    iNum = int(num)
    return bool (iNum <= 3 and iNum > 0)
  except ValueError:
    print("Input error: Please enter 1, 2, 3 or \"cheat\"")
    return False

# ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
#   Prints the number of stones a player should take on their currrent turn.
#   Returns remainder of stones modulo 4
#   If no winning move exists tells player to take 1 stone 
#   (hopefully the other player makes a mistake)
#   
# ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
def cheat():
  move = stones % 4
  '''
    If stones modulo 4 equals 1 there is no winning state
      for the current 'cheating' player
      
    Win conditions:
    0,2,3,5
    if(n = 1)	# 1 mod 4 = 1 (remainder of 1 is a losing state)
    	lose
    if(n = 2 )	# 2 mod 4 = 2
    	take 1
    if(n = 3)	# 3 mod 4 = 3
    	take 2
    if(n = 4)	# 4 mod 4 = 0
    	take 3
    	
    If nStones mod 4 is not one then take the remainder and subtract 1-3
      if the value is -1 move = 3
      else move = value
  '''
  if (move == 1):
    print("No win unless opponent makes a mistake.")
    return 
  
  # calculate new winning move
  move = (move-1)
  if (move < 0):
    move = 3
  print("Cheat Suggest: \"Take %d stone(s)\"" % move)

# ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
#   The game logic
#   Asks current player to remove 1-3 stones 
#   Checks win state after each player move
#   Switches to opposing player after each move
#   Displays winner when stones reach 0 or below
# ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
def game():
  global stones, player, cheatEnabled0, cheatEnabled1, quit
  while(stones > 0):
    while(True):
      print("\nStones left [%d]"%stones)
      if ((cheatEnabled0 and player == 0) or (cheatEnabled1 and player == 1)):
        cheat()
        
      num = input("Player%2d, how many stones will you take (1, 2 or 3)"%(player))
      if num == "cheat":
        if player == 0:
          cheatEnabled0 = not cheatEnabled0
        else:
          cheatEnabled1 = not cheatEnabled1
      elif num == "quit":
        quit = True
        break
      elif(checkNum(num)):
        stones = stones - int(num)
        player = ((player + 1) % 2)
        break
    if quit:
      break

quit = False      
player = 0
stones = (randint(13,40))
cheatEnabled0 = False
cheatEnabled1 = False
printBanner()
while(not quit):
  game()
  if quit:
    break
  print("\n\n##### Player %d wins! #####\n\n" % player)
  stones = (randint(13,40))
  printBanner()
  
num = input("\nGoodbye")