# Python program to illustrate
# function with main

import pandas as pd
import csv
import re

def getFileName():
	input_file = (input("Enter filename: "))
	return input_file

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
<<<<<<< HEAD
def readCSVFile2(FileName):
=======
def readCSVFile2(FileName, outfile):
>>>>>>> tongan
	df=pd.read_csv(FileName)

	print (len(df))
	f=open(outfile, "w")

	#reorganizes the columns in the CSV and
	#extracts just the number from the concentration column
	#before writing them to a file
	for i, row in df.iterrows():
		Conc=re.findall("\d+", row['Concentration'])[0]
		#row_to_write= (i, row['Plate'], row['Well'], row['Sample ID'], Conc, row['Barcode'])
		row_to_write= str(i) + "," + str(row['Plate']) + "," + str(row['Well']) + "," + str(row['Sample ID']) + "," + str(Conc) + "," + str(row['Barcode']) + "\n"
		#print(row_to_write)
		f.write(row_to_write)

<<<<<<< HEAD
=======

	f.close()
>>>>>>> tongan

def Main():
	print("Started")

	# calling the getFileName function and
	# storing its returned value in the output variable
	FileName = getFileName()

	#this open FileName was part of the first homework
	#f = open(FileName, "rt")	
	#print(f.read())

	#extracts the root file name and appends a 2 onto it
	outfile=re.findall("(\S+).csv", FileName)[0] + "2.csv"
	
	print(outfile)

	#read a CSV through regular file reader
	#readCSVFile(FileName)

	#read a CSV using pandas
	readCSVFile2(FileName, outfile)

# now we are required to tell Python
# for 'Main' function existence
#if __name__=="__main__":
Main()
