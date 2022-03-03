# Python program to illustrate
# function with main

import pandas as pd
import csv
import re

#future parameters
	#instrument--Janus or FX
	#dilution points--7, 11, 12, 22, 24
	#transfer volume--10
	#start at well--A03
	#transfer type--dilution or 1-to-1

#A01    A02 A03 A04 A05 A06 A07 A08 A09 A10 A11 A12     1   9   17  25  33  41  49  57  65  73  81  89
#B01    B02 B03 B04 B05 B06 B07 B08 B09 B10 B11 B12     2   10  18  26  34  42  50  58  66  74  82  90
#C01    C02 C03 C04 C05 C06 C07 C08 C09 C10 C11 C12     3   11  19  27  35  43  51  59  67  75  83  91
#D01    D02 D03 D04 D05 D06 D07 D08 D09 D10 D11 D12     4   12  20  28  36  44  52  60  68  76  84  92
#E01    E02 E03 E04 E05 E06 E07 E08 E09 E10 E11 E12     5   13  21  29  37  45  53  61  69  77  85  93
#F01    F02 F03 F04 F05 F06 F07 F08 F09 F10 F11 F12     6   14  22  30  38  46  54  62  70  78  86  94
#G01    G02 G03 G04 G05 G06 G07 G08 G09 G10 G11 G12     7   15  23  31  39  47  55  63  71  79  87  95
#H01    H02 H03 H04 H05 H06 H07 H08 H09 H10 H11 H12     8   16  24  32  40  48  56  64  72  80  88  96

#A01    A02 A03 A04 A05 A06 A07 A08 A09 A10 A11 A12     1   2   3   4   5   6   7   8   9   10  11  12
#B01    B02 B03 B04 B05 B06 B07 B08 B09 B10 B11 B12     13  14  15  16  17  18  19  20  21  22  23  24
#C01    C02 C03 C04 C05 C06 C07 C08 C09 C10 C11 C12     25  26  27  28  29  30  31  32  33  34  35  36
#D01    D02 D03 D04 D05 D06 D07 D08 D09 D10 D11 D12     37  38  39  40  41  42  43  44  45  46  47  48
#E01    E02 E03 E04 E05 E06 E07 E08 E09 E10 E11 E12     49  50  51  52  53  54  55  56  57  58  59  60
#F01    F02 F03 F04 F05 F06 F07 F08 F09 F10 F11 F12     61  62  63  64  65  66  67  68  69  70  71  72
#G01    G02 G03 G04 G05 G06 G07 G08 G09 G10 G11 G12     73  74  75  76  77  78  79  80  81  82  83  84
#H01    H02 H03 H04 H05 H06 H07 H08 H09 H10 H11 H12     85  86  87  88  89  90  91  92  93  94  95  96

#Matrix Conversion robotize and humanize written by Cyrus Khajvandi
#Convert 96 well plate from robot integer recognition to human form & vice-versa

def robotize(well, instrument):
	(row_name, column_num) = (well[0], well[1:2])


	ascii_value = ord(row_name) - 65
    
	#FX number
	return ascii_value*8 + int(column_num)

	#Janus number
	return 


#Convert machine form to human form
def humanize(well):
	i=0
	i+=1
	offset = (well-1)/(COL)*i
	rowIndex = chr(65 + offset)

	colIndex = well - (offset * (COL)*i)


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
			if line_count==0:	#first row in the file contains the column header names
				print(f'Column names are {", ".join(row)}')
				line_count+=1
			else:				#the columns are reoganized
				print(f'{row[1]}, {row[2]}, {row[4]}, {row[5]}, {row[3]}')
				line_count+=1
		print(f'Processed {line_count} lines')

#read the CSV using Pandas
def readCSVFile2(FileName, outfile):
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

	f.close()

#TO-DOs
#--need to count the total rows and do a MOD operation so the correct number of plates and wells are used
#--convert a letter row to a number for calculating the well number needed by the Janus
#--loop through the list in sets of 16 and then change to single tip dispense
#--create the platemap file with 384-well locations displayed

#def convertRowtoNum():


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
