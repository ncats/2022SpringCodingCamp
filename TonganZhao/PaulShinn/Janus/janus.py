# This program will convert a plate map exported from the compound store database to
# a worklist for the Janus or FX liquid handler.

import pandas as pd
import csv
import re

#future parameters
	#instrument--Janus or FX
	#dilution points--7, 11, 22
	#transfer volume--10
	#start at well--A03
	#transfer type--dilution or 1-to-1

#Janus
#A01    A02 A03 A04 A05 A06 A07 A08 A09 A10 A11 A12     1   9   17  25  33  41  49  57  65  73  81  89
#B01    B02 B03 B04 B05 B06 B07 B08 B09 B10 B11 B12     2   10  18  26  34  42  50  58  66  74  82  90
#C01    C02 C03 C04 C05 C06 C07 C08 C09 C10 C11 C12     3   11  19  27  35  43  51  59  67  75  83  91
#D01    D02 D03 D04 D05 D06 D07 D08 D09 D10 D11 D12     4   12  20  28  36  44  52  60  68  76  84  92
#E01    E02 E03 E04 E05 E06 E07 E08 E09 E10 E11 E12     5   13  21  29  37  45  53  61  69  77  85  93
#F01    F02 F03 F04 F05 F06 F07 F08 F09 F10 F11 F12     6   14  22  30  38  46  54  62  70  78  86  94
#G01    G02 G03 G04 G05 G06 G07 G08 G09 G10 G11 G12     7   15  23  31  39  47  55  63  71  79  87  95
#H01    H02 H03 H04 H05 H06 H07 H08 H09 H10 H11 H12     8   16  24  32  40  48  56  64  72  80  88  96

#FX
#A01    A02 A03 A04 A05 A06 A07 A08 A09 A10 A11 A12     1   2   3   4   5   6   7   8   9   10  11  12
#B01    B02 B03 B04 B05 B06 B07 B08 B09 B10 B11 B12     13  14  15  16  17  18  19  20  21  22  23  24
#C01    C02 C03 C04 C05 C06 C07 C08 C09 C10 C11 C12     25  26  27  28  29  30  31  32  33  34  35  36
#D01    D02 D03 D04 D05 D06 D07 D08 D09 D10 D11 D12     37  38  39  40  41  42  43  44  45  46  47  48
#E01    E02 E03 E04 E05 E06 E07 E08 E09 E10 E11 E12     49  50  51  52  53  54  55  56  57  58  59  60
#F01    F02 F03 F04 F05 F06 F07 F08 F09 F10 F11 F12     61  62  63  64  65  66  67  68  69  70  71  72
#G01    G02 G03 G04 G05 G06 G07 G08 G09 G10 G11 G12     73  74  75  76  77  78  79  80  81  82  83  84
#H01    H02 H03 H04 H05 H06 H07 H08 H09 H10 H11 H12     85  86  87  88  89  90  91  92  93  94  95  96

#Convert 96 well plate from robot integer recognition to human form & vice-versa

row96_to_num_for_janus = {
	"A": 7,
	"B": 6,
	"C": 5,
	"D": 4,
	"E": 3,
	"F": 2,
	"G": 1,
	"H": 0
}

row_num384_to_row_for_janus = {
	15: "A",
	14: "B",
	13: "C",
	12: "D",
	11: "E",
	10: "F",
	9: "G",
	8: "H",
	7: "I",
	6: "J",
	5: "K",
	4: "L",
	3: "M",
	2: "N",
	1: "O",
	0: "P"
}

#the number of columns that will be filled in a 384-well plate depending on the number of dilution points
plate_counter = {
	7: 3,
	11: 2,
	22: 1
}

def robotize(well, instrument):
	(row_name, column_num) = (well[0], well[1:3])

	#print(column_num)
	if instrument=='FX':
		#FX number
		ascii_value = ord(row_name) - 65
		return ascii_value*12 + int(column_num)
	elif instrument=='Janus':
		#Janus number
		return int(column_num)*8 - row96_to_num_for_janus[row_name]


#Convert 384-well machine form to human readable form
def humanize(well, instrument):
	if instrument=='FX':
		#convert an FX 384-well to RowColumn format
		row_num=int(math.ceil(well/16))
		row = chr(row_num + 64)
		column = well % 24
		new_well = row + str(column)
		return new_well
	elif instrument=='Janus':
		#convert a Janus 384-well to RowColumn format
		column=int(math.ceil(well/16))
		row_num=column*16-well
		new_well=row_num384_to_row_for_janus[row_num] + str(column)
		return new_well


def getParams():
	input_file = (input("Enter filename: ") or "map.csv")
	instrument = (input("[Janus] or FX?: ") or "Janus")
	dil_points = (input("How many dilution points? 7, [11], 22 ") or 11)
	volume = (input("What volume? [10] ") or 10)

	return input_file, instrument, dil_points, volume



def readCSVFile(FileName):
	
	with open(FileName) as csv_file:
		csv_reader = csv.reader(csv_file, delimiter=',')
		line_count=0
		for row in csv_reader:
			if line_count==0:	#first row in the file contains the column header names
				print(f'Column names are {", ".join(row)}')
				line_count+=1
			else:				#the columns are reorganized
				print(f'{row[1]}, {row[2]}, {row[4]}, {row[5]}, {row[3]}')
				line_count+=1
		print(f'Processed {line_count} lines')

#read the CSV using Pandas
def readCSVFile2(FileName, instrument, dil_points, worklist, platemap):
	df=pd.read_csv(FileName)

	print (len(df))
	print(plate_counter[dil_points])
	w=open(worklist, "w")	#opens the worklist file for writing
	p=open(platemap, "w")	#opens the platemap file for writing

	dest_plate_count=0

	#reorganizes the columns in the CSV and
	#extracts just the number from the concentration column
	#before writing them to a file
	for i, row in df.iterrows():
		Conc=re.findall("\d+", row['Concentration'])[0]

		source_well=str(robotize(str(row['Well']), instrument))

		if (i % (16*plate_counter[dil_points]))==0:
			dest_plate_count += 1

		worklist_to_write= str(i) + "," + str(row['Plate']) + "," + str(row['Well']) + "," + source_well + ",384-" + str(dest_plate_count) + "\n"
		platemap_to_write= str(i) + "," + str(row['Barcode']) + "," + str(row['Sample ID']) + "," + str(Conc) + "\n"

		#print(row_to_write)
		w.write(worklist_to_write)
		p.write(platemap_to_write)

	w.close()

#do a mod 16 on the number of rows in the plate map.
#if it's 7pt, then cols 3, 10, and 17
#if it's 11pt, then cols 3 and 14
#if it's 22pt, then col 3


#TO-DOs
#--need to count the total rows and do a MOD operation so the correct number of plates and wells are used
#--loop through the list in sets of 16 and then change to single tip dispense
#--create the platemap file with 384-well locations displayed

#def convertRowtoNum():


def Main():
	print("Started")

	# calling the getFileName function and
	# storing its returned value in the output variable
	FileName, instrument, dil_points, volume = getParams()

	#extracts the root file name and appends "worklist" or "platemap" onto it
	worklist=re.findall("(\S+).csv", FileName)[0] + "-worklist.csv"
	platemap=re.findall("(\S+).csv", FileName)[0] + "-platemap.csv"

	print(worklist)
	print(platemap)

	#read a CSV through regular file reader
	#readCSVFile(FileName)

	#read a CSV using pandas
	readCSVFile2(FileName, instrument, dil_points, worklist, platemap)

# now we are required to tell Python
# for 'Main' function existence
#if __name__=="__main__":
Main()
