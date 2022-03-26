using ETL_Playground;

CortellisFetcher cortellisFetcher = new CortellisFetcher();
PubChemFetcher pubChemFetcher = new PubChemFetcher();

string[] ligands = new string[] {"nifedipine"};
foreach (string name in ligands){
    Console.WriteLine($"cortellis data for {name}");
    Console.WriteLine(cortellisFetcher.fetchDetails(name));
    
    Console.WriteLine($"pubchem data for {name}");
    Console.WriteLine(pubChemFetcher.fetchDetails(name));
}
