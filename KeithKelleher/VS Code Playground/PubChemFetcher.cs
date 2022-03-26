using System;
using System.Collections.Generic;
using System.IO;
using System.Net;

namespace ETL_Playground
{
	class PubChemFetcher
	{
		// URL Format based on the tutorial here https://pubchemdocs.ncbi.nlm.nih.gov/pug-rest-tutorial$_toc458584410
		string prolog = "https://pubchem.ncbi.nlm.nih.gov/rest/pug";
		string input = "compound/name";
		string property = "property";
		string[] fields = new string[] { "MolecularFormula", "MolecularWeight", "InChI", "CanonicalSMILES" };
		string format = "JSON";

		public string getUrl(string name)
		{
			List<string> pieces = new List<string>();
			pieces.Add(this.prolog);
			pieces.Add(this.input);
			pieces.Add(name);
			pieces.Add(this.property);
			pieces.Add(String.Join(",", this.fields));
			pieces.Add(this.format);
			return String.Join("/", pieces.ToArray());
		}

		public dynamic fetchDetails(string name)
		{
			dynamic details = null;
			WebRequest request = null;
			WebResponse response = null;
			Stream dataStream = null;
			StreamReader reader = null;
			try
			{
				request = WebRequest.Create(this.getUrl(name));
				response = request.GetResponse();
				dataStream = response.GetResponseStream();
				reader = new StreamReader(dataStream);
				details =   Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(reader.ReadToEnd());
			}
			catch(Exception ex)
			{
				Console.WriteLine(ex.Message);
			}
			finally
			{
				if (reader != null) reader.Close();
				if (response != null) response.Close();
				if (dataStream != null) dataStream.Close();
				if (reader != null) reader.Close();
			}
			return details;
		}
	}
}