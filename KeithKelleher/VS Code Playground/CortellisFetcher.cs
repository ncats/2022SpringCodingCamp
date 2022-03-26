using System;
using System.Collections.Generic;
using System.IO;
using System.Net;

namespace ETL_Playground
{
	class CortellisFetcher
	{
		string username = "";
		string password = "";
		public CortellisFetcher() {
			
			using (StreamReader r = new StreamReader("cortelliscredentials.json"))
			{
				string json = r.ReadToEnd();
				dynamic credentials = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(json);
				this.username = credentials.username;
				this.password = credentials.password;
			}	
		}

		public string drugSearchURL(string name)
		{
			return $"https://api.cortellis.com/api-ws/ws/rs/drugdesign-v4/drug/search?query=nameMain:{name}&hits=100&offset=0&filtersEnabled=false&fmt=json";
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
				request = WebRequest.Create(this.drugSearchURL(name));
				request.Credentials = new NetworkCredential(this.username, this.password);
				response = request.GetResponse();
				dataStream = response.GetResponseStream();
				reader = new StreamReader(dataStream);
				details = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(reader.ReadToEnd());;
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