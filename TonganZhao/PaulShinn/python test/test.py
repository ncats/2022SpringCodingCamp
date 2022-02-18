# Python program to illustrate
# function with main
def getFileName():
	result = (input("Enter filename: "))
	return result

#def PrintStep():
#    for step in range(5):    
#        print(step)

def readCSVFile(FileName):
	import csv

	with open(FileName) as csv_file:
		csv_reader = csv.reader(csv_file, delimiter=',')
		line_count=0
		for row in csv_reader:
			if line_count==0:
				print(f'Column names are {", ".join(row)}')
				line_count+=1
			else:
				print(f'\t{row[1]}')
				line_count+=1
		print(f'Processed {line_count} lines')

def Main():
	print("Started")

	# calling the getFileName function and
	# storing its returned value in the output variable
	FileName = getFileName()

	#this open FileName was part of the first homework
	#f = open(FileName, "rt")	
	#print(f.read())

	readCSVFile(FileName)

# now we are required to tell Python
# for 'Main' function existence
#if __name__=="__main__":
Main()
