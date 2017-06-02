using System;
using System.Data;
using Mono.Data.Sqlite;
using SQLite;

namespace Daily
{

	public class Daily
	{
		public string TimeStamp { get; set;}
		public Quote q { get; set;}
	}
}

