using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETL_Playground
{
	class Program
	{
		static void Main(string[] args)
		{
			CampDBConnection connection = new CampDBConnection();
			List<string> list = null;
			try
			{
				connection.Open();
				list = connection.getLigandList();
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
			}
			finally
			{
				connection.Close();
			}
			if (list != null)
			{
				PubChemFetcher pubChemFetcher = new PubChemFetcher();
				foreach (string name in list)
				{
					Console.WriteLine(pubChemFetcher.fetchDetails(name));
				}
			}
			Console.ReadKey(true);
		}
	}
}