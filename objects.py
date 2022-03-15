import math




class Line:
    #This is a special method called a "constructor" which tells the
    #program how to make a new instance of this class
    def __init__(self,start, end):
        self.__start=start
        self.__end=end
        self.__length = self.__calculateLength__()

    def __calculateLength__(self):
        length= math.sqrt(
            pow(self.__start[0]-self.__end[0],2)
            +
            pow(self.__start[1]-self.__end[1],2)
        )
        return length

    def getLength(self):
        return self.__length

    def setStart(self,start):
        self.__start = start

    def setEnd(self,end):
        self.__end=end

class Quadrilateral:
    def  __init__(self,sides):
        if len(sides) != 2:
            print("Incorrect number of sides!")
        else:
            self.sides = sides

    def getPerimter(self):
        perimeter=0
        for side in self.sides:
            perimeter = perimeter + side.getLength()*2
        return perimeter
    def getArea(self):
        sides = self.getSides()
        area = sides[0].getLength() * sides[1].getLength()
        return area

    def getSides(self):
        return self.sides
    def __do_not_touch_this_method(self):
        print("Or do.  I'm a method, not a cop")

class Square(Quadrilateral):
    def __init__(self, side):
        super().__init__([side,side])

    def area(self):
        area=pow(self.getSides[0], 2)
        return area


def main():
    our_line = Line([1,2],[1,5])
    print(our_line.getLength())

    quad_sides =[
        Line([0,0],[0,10]),
        Line([0,0],[5,0]),
    ]
    our_quad=Quadrilateral(quad_sides)
    print(our_quad.getPerimter())
    print(our_quad.getArea())



if __name__== '__main__':
    main()