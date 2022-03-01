using ETL_Playground;

CampDBConnection db = new CampDBConnection();
db.Open();
PubChemFetcher fetcher = new PubChemFetcher();
string[] ligands = new string[] {"nimodipine"};
foreach (string name in ligands){
    Console.WriteLine(fetcher.fetchDetails(name));
}
db.Close();