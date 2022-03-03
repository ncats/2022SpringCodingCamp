#Convert 96 well plate from robot integer recognition to human form & vice-versa

ROW = 8
COL = 12

#A01	A02	A03	A04	A05	A06	A07	A08	A09	A10	A11	A12		1	9	17	25	33	41	49	57	65	73	81	89
#B01	B02	B03	B04	B05	B06	B07	B08	B09	B10	B11	B12		2	10	18	26	34	42	50	58	66	74	82	90
#C01	C02	C03	C04	C05	C06	C07	C08	C09	C10	C11	C12		3	11	19	27	35	43	51	59	67	75	83	91
#D01	D02	D03	D04	D05	D06	D07	D08	D09	D10	D11	D12		4	12	20	28	36	44	52	60	68	76	84	92
#E01	E02	E03	E04	E05	E06	E07	E08	E09	E10	E11	E12		5	13	21	29	37	45	53	61	69	77	85	93
#F01	F02	F03	F04	F05	F06	F07	F08	F09	F10	F11	F12		6	14	22	30	38	46	54	62	70	78	86	94
#G01	G02	G03	G04	G05	G06	G07	G08	G09	G10	G11	G12		7	15	23	31	39	47	55	63	71	79	87	95
#H01	H02	H03	H04	H05	H06	H07	H08	H09	H10	H11	H12		8	16	24	32	40	48	56	64	72	80	88	96

#A01	A02	A03	A04	A05	A06	A07	A08	A09	A10	A11	A12		1	2	3	4	5	6	7	8	9	10	11	12
#B01	B02	B03	B04	B05	B06	B07	B08	B09	B10	B11	B12		13	14	15	16	17	18	19	20	21	22	23	24
#C01	C02	C03	C04	C05	C06	C07	C08	C09	C10	C11	C12		25	26	27	28	29	30	31	32	33	34	35	36
#D01	D02	D03	D04	D05	D06	D07	D08	D09	D10	D11	D12		37	38	39	40	41	42	43	44	45	46	47	48
#E01	E02	E03	E04	E05	E06	E07	E08	E09	E10	E11	E12		49	50	51	52	53	54	55	56	57	58	59	60
#F01	F02	F03	F04	F05	F06	F07	F08	F09	F10	F11	F12		61	62	63	64	65	66	67	68	69	70	71	72
#G01	G02	G03	G04	G05	G06	G07	G08	G09	G10	G11	G12		73	74	75	76	77	78	79	80	81	82	83	84
#H01	H02	H03	H04	H05	H06	H07	H08	H09	H10	H11	H12		85	86	87	88	89	90	91	92	93	94	95	96


#Convert human form to machine form
def robotize(well):

    """
    Given well in matrix human form, return its robot form
    >>> robotize("A1")
    1
    >>> robotize('A10')
    10
    >>> robotize('A11')
    11
    >>> robotize('A12')
    12
    >>> robotize('B11')
    23
    >>> robotize('C10')
    34
    >>> robotize('G12')
    84
    >>> robotize('H1')
    85
    >>> robotize('H12')
    96
    """
    
    (rowIndex, colIndex) = (0,0)

    for i in range(0, len(well)):
        (left, right) = (well[:i], well[i:i+1])
        if right.isdigit():
            (rowIndex, colIndex) = (left, well[i:])
            break
        
    ascii_value = ord(rowIndex) - 65
    
    return ascii_value*(ROW+(4*i)) + int(colIndex)


#Convert machine form to human form
def humanize(well):
    """
    Given a number, return its human form
    >>> humanize(1)
    'A1'
    >>> humanize(10)
    'A10'
    >>> humanize(11)
    'A11'
    >>> humanize(12)
    'A12'
    >>> humanize(23)
    'B11'
    >>> humanize(34)
    'C10'
    >>> humanize(84)
    'G12'
    >>> humanize(85)
    'H1'
    >>> humanize(96)
    'H12'
    """
    
    i=0
    i+=1
    offset = (well-1)/(COL)*i
    rowIndex = chr(65 + offset)
    
    colIndex = well - (offset * (COL)*i)
    return rowIndex + str(colIndex)

if __name__ == "__main__":
    import doctest
    doctest.testmod(verbose=True, optionflags=doctest.NORMALIZE_WHITESPACE)


print "This program is written by Cyrus Khajvandi."