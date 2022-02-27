import mysql.connector
from mysql.connector import errorcode, Error


# define a MySQLConnection class
class MySQLConnection:

    def __init__(self,config):
        self.config = config                                                                                                                                                                                                                                                                                                                        

# define a connection function
    def getConnection(self):
        try:
            conn = mysql.connector.connect(user=self.config['username'], password=self.config['password'],
                              host=self.config['host'],database=self.config['username'])
            print("\nConnected to:", self.config['host']+'\n')   
            return conn
        except mysql.connector.Error as err:
            if err.errno == errorcode.ER_ACCESS_DENIED_ERROR:
                print("Something is wrong with your user name or password")
            elif err.errno == errorcode.ER_BAD_DB_ERROR:
               print("Database does not exist")
            else:
               print(err)