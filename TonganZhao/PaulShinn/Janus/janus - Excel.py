# This program will convert a plate map exported from the compound store database to
# a worklist for the Janus or FX liquid handler.

from turtle import colormode
import pandas as pd
import csv
import re
import math

#future parameters
	#instrument--Janus or FX
	#dilution points--7, 11, 22
	#transfer volume--10
	#start at well--A03
	#transfer type--dilution or 1-to-1

#if it's 7pt, then cols 3, 10, and 17
#if it's 11pt, then cols 3 and 14
#if it's 22pt, then col 3

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


def getParams():
	input_file = (input("Enter filename: ") or "map.csv")
	instrument = (input("[Janus] or FX?: ") or "Janus")
	dil_points = int((input("How many dilution points? 7, [11], 22 ") or 11))
	volume = int((input("What volume? [10] ") or 10))

	return input_file, instrument, dil_points, volume


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
column_counter = {
	7:3,
	11:2,
	22:1
}

skip_row_janus = {
	0:1,
	1:3,
	2:5,
	3:7,
	4:9,
	5:11,
	6:13,
	7:15,
	8:2,
	9:4,
	10:6,
	11:8,
	12:10,
	13:12,
	14:14,
	15:16
}

#converts a 96-well row/column to robot well location
def robotize96(well, instrument):
	(row_name, col_num) = (well[0], well[1:3])

	#print(column_num)
	if instrument=='FX':
		#FX number
		ascii_value = ord(row_name) - 65
		return ascii_value*12 + int(col_num)
	elif instrument=='Janus':
		#Janus number
		return int(col_num)*8 - row96_to_num_for_janus[row_name]


#Convert 384-well machine form to human readable form
def humanize384(well, instrument):
	if instrument=='FX':
		#convert an FX 384-well to RowColumn format
		row_num=int(math.ceil(well/16))
		row = chr(row_num + 64)
		col_num = well % 24
		new_well = row + f"{col_num:02}"
		return new_well
	elif instrument=='Janus':
		#convert a Janus 384-well to RowColumn format
		col_num=int(math.ceil(well/16))
		row_num=col_num*16-well
		new_well=row_num384_to_row_for_janus[row_num] + f"{col_num:02}"
		return new_well

#converts a Janus well number to an FX well number
def convertJanus384toFX(well):
	col_num=int(math.ceil(well/16))
	if (well % 16 == 0): #is it the bottom row?
		row_num=16
	else:
		row_num=well % 16

	return ((row_num-1)*24 + col_num)	#this is the FX well number


#def readCSVFile(FileName):
	
#	with open(FileName) as csv_file:
#		csv_reader = csv.reader(csv_file, delimiter=',')
#		line_count=0
#		for row in csv_reader:
#			if line_count==0:	#first row in the file contains the column header names
#				print(f'Column names are {", ".join(row)}')
#				line_count+=1
#			else:				#the columns are reorganized
#				print(f'{row[1]}, {row[2]}, {row[4]}, {row[5]}, {row[3]}')
#				line_count+=1
#		print(f'Processed {line_count} lines')

#read the CSV using Pandas
def readCSVFile2(FileName, instrument, dil_points, volume, worklist, platemap):
	df=pd.read_csv(FileName)

	print("File Length: " + str(len(df)))
	print("Dil Points: " + str(dil_points))

	print(column_counter[dil_points])

	#create the worklist and write the headers
	worklist_to_write = []

	#create the platemap list and write the headers
	platemap_to_write = []

	dest_plate_count=0
	column_number=3

	#reorganizes the columns in the CSV and
	#extracts just the number from the concentration column
	#before writing them to a file
	for i, row in df.iterrows():
		Conc=re.findall("\d+", row['Concentration'])[0]
		#for the future, add an IF statement when no concentration is listed

		if (i % (16*column_counter[dil_points]))==0:
			dest_plate_count += 1

		#if the row count is less than the MOD of total columns, then skip rows
		if (int(math.ceil(i/16)) <= int(math.floor(len(df)/16))):
			well=(column_number-1)*16 + skip_row_janus[i % 16]
		else:
			well += 1

		if (i != 0):	#ignore the first one
			if ((i+1) % 16) == 0:	#at the bottom of a column
				if ((column_number+dil_points-1) < 22):
					column_number=column_number+dil_points
				else:
					column_number=3

		source_well=str(robotize96(str(row['Well']), instrument))

		if (instrument=="Janus"):
			worklist_to_write.append([str(row['Plate']), source_well, "384-" + str(dest_plate_count), str(well), str(volume)])
			platemap_to_write.append([f"{row['Barcode']:010}", str(row['Sample ID']), str(Conc) + "mM", "384-" + str(dest_plate_count), str(well), str(humanize384(well, "Janus"))])
		else:
			worklist_to_write.append([str(row['Plate']), source_well, "384-" + str(dest_plate_count), str(convertJanus384toFX(well)), str(volume)])
			platemap_to_write.append([f"{row['Barcode']:010}", str(row['Sample ID']), str(Conc) + "mM" , "384-" + str(dest_plate_count), str(well), str(humanize384(convertJanus384toFX(well), "FX"))])			
	
	df_worklist=pd.DataFrame(worklist_to_write, columns=['Source','Well','Dest','DestWell','Volume'], dtype = float)
#	df_worklist.columns=df_worklist[0]
#	df_worklist=df_worklist[1:]

	df_platemap=pd.DataFrame(platemap_to_write)
#	df_platemap.columns=df_platemap[0]
#	df_platemap=df_platemap[2:]

	with pd.ExcelWriter(worklist) as writer:
		df_worklist.to_excel(writer, sheet_name='Worklist', header=False, index=False)
		df_platemap.to_excel(writer, sheet_name='Platemap', header=False, index=False)


def Main():
	print("Started")

	# calling the getFileName function and
	# storing its returned value in the output variable
	FileName, instrument, dil_points, volume = getParams()

	#extracts the root file name and appends "worklist" or "platemap" onto it
	worklist=re.findall("(\S+).csv", FileName)[0] + "-worklist.xlsx"
	platemap=re.findall("(\S+).csv", FileName)[0] + "-platemap.xlsx"

	print(worklist)
	print(platemap)

	#read a CSV through regular file reader
	#readCSVFile(FileName)

	#read a CSV using pandas
	readCSVFile2(FileName, instrument, dil_points, volume, worklist, platemap)

# now we are required to tell Python
# for 'Main' function existence
#if __name__=="__main__":
Main()
