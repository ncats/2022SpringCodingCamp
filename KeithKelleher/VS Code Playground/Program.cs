using ETL_Playground;

CampDBConnection db = new CampDBConnection();
db.Open();
PubChemFetcher fetcher = new PubChemFetcher();
foreach (string name in db.getLigandList()){
    Console.WriteLine(fetcher.fetchDetails(name));
}
db.Close();