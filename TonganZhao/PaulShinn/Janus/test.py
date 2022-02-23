# Python program to illustrate
# function with main

import pandas as pd
import csv
import re

def getFileName():
	result = (input("Enter filename: "))
	return result

#def PrintStep():
#    for step in range(5):    
#        print(step)

def readCSVFile(FileName):
	
	with open(FileName) as csv_file:
		csv_reader = csv.reader(csv_file, delimiter=',')
		line_count=0
		for row in csv_reader:
			if line_count==0:
				print(f'Column names are {", ".join(row)}')
				line_count+=1
			else:
				print(f'{row[1]}, {row[2]}, {row[4]}, {row[5]}, {row[3]}')
				line_count+=1
		print(f'Processed {line_count} lines')

#read the CSV using Pandas
def readCSVFile2(FileName):
	df=pd.read_csv(FileName)

	print (len(df))

	for i, row in df.iterrows():
		Conc=re.findall("\d+", row['Concentration'])
		print(i, row['Plate'], row['Well'], row['Sample ID'], Conc, row['Barcode'])


def Main():
	print("Started")

	# calling the getFileName function and
	# storing its returned value in the output variable
	FileName = getFileName()

	#this open FileName was part of the first homework
	#f = open(FileName, "rt")	
	#print(f.read())

	#read a CSV through regular file reader
	#readCSVFile(FileName)

	#read a CSV using pandas
	readCSVFile2(FileName)

# now we are required to tell Python
# for 'Main' function existence
#if __name__=="__main__":
Main()
