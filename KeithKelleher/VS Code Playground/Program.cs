using ETL_Playground;

CampDBConnection db = new CampDBConnection();
db.Open();
foreach (string name in db.getLigandList()){
    Console.WriteLine(name);
}
db.Close();