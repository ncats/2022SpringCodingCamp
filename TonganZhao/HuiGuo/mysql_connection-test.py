from configuration import healDev, opendataCmsDev
from mysql_connection import MySQLConnection  as mysqlconn

if __name__ == "__main__":
    
    db = mysqlconn (healDev)
    conn1 = db.getConnection()

    db = mysqlconn (opendataCmsDev)
    conn2 = db.getConnection()
