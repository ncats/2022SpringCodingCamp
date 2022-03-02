#Convert 96 well plate from robot integer recognition to human form & vice-versa

ROW = 8
COL = 12


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