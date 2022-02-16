# Python program to illustrate
# function with main
def getFileName():
	result = (input("Enter filename: "))
	return result

#def PrintStep():
#    for step in range(5):    
#        print(step)

def Main():
	print("Started")

	# calling the getFileName function and
	# storing its returned value in the output variable
	FileName = getFileName()
	f = open(FileName, "rt")
	print(f.read())

# now we are required to tell Python
# for 'Main' function existence
#if __name__=="__main__":
Main()
