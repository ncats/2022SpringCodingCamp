using System.Collections.Generic;
using MySql.Data.MySqlClient;
using System.IO;

namespace ETL_Playground
{
	class CampDBConnection
	{
		DBConnectionSettings settings;
		public MySqlConnection connection;

		public CampDBConnection()
		{
			using (StreamReader r = new StreamReader("db_credentials.json"))
			{
				string json = r.ReadToEnd();
				this.settings = Newtonsoft.Json.JsonConvert.DeserializeObject<DBConnectionSettings>(json);
			}	
			this.connection = new MySqlConnection(this.settings.getConnectionString());
		}

		public List<string> getLigandList()
		{
			string query = "SELECT `Ligand Name` FROM ligands";
			MySqlCommand command = new MySqlCommand(query, this.connection);
			MySqlDataReader dataReader = command.ExecuteReader();

			List<string> list = new List<string>();

			//Read the data and store them in the list
			while (dataReader.Read())
			{
				if (dataReader["Ligand Name"] != null) {
					list.Add(dataReader["Ligand Name"].ToString());
				}
			}

			//close Data Reader
			dataReader.Close();
			return list;
		}

		public void Open()
		{
			this.connection.Open();
		}
		public void Close()
		{
			this.connection.Close();
		}
	}

	class DBConnectionSettings
	{
		public string server;
		public string database;
		public string uid;
		public string password;

		public string getConnectionString()
		{
			return $"SERVER={this.server};DATABASE={this.database};" +
				$"UID={this.uid};PASSWORD={this.password};";
		}
	}

}