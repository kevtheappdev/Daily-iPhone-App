using System;
using System.Data;
using Mono.Data.Sqlite;
using SQLite;
using System.IO;
using System.Collections.Generic;

namespace Daily
{
	public static class DailyLocalFetcher
	{
		private static string path;

		static DailyLocalFetcher(){
			Console.WriteLine ("static ran");
			var documents =	Environment.GetFolderPath (Environment.SpecialFolder.Personal);
			path = Path.Combine (documents, "quote.sqlite");

			using (var conn = new SQLite.SQLiteConnection (path)) {
				conn.CreateTable<Quote> ();
				conn.CreateTable<Image> ();
				conn.CreateTable<Word> ();
			}
		}

		public static void QuoteUpdate(Quote q){
			using (var conn = new SQLite.SQLiteConnection (path)) {
				conn.DeleteAll <Quote>();
				conn.Insert (q);
			}
		}

		public static Quote TodaysQuote(){
			using (var conn = new SQLite.SQLiteConnection (path)) {
				var results =  (List<Quote>)conn.Query <Quote>("select * from Quote");
				if (results.Count > 0) {
					return results [0];
				} else {
					return null;
				}
			}
		}

		public static void ImageUpdate(Image i){
			using(var conn = new SQLite.SQLiteConnection(path)){
				conn.DeleteAll<Image> ();
				conn.Insert (i);
			}
		}

		public static void ImageDelete(){
			using(var conn = new SQLite.SQLiteConnection(path)){
				conn.DeleteAll<Image> ();

			}
		}

		public static Image TodaysImage(){
			using (var conn = new SQLite.SQLiteConnection (path)) {
				var results = (List<Image>)conn.Query<Image>("select * from Image");
					if(results.Count > 0){
						return results[0];
					} else {
						return null;
					}
			}
		}

		public static Word TodaysWord(){
			using (var conn = new SQLite.SQLiteConnection (path)) {
				var results = (List<Word>)conn.Query<Word> ("select * from Word");
				if (results.Count > 0) {
					return results [0];
				} else {
					return null;
				}
			}
		}

		public static void WordUpdate(Word w){
			using (var conn = new SQLite.SQLiteConnection (path)) {
				conn.DeleteAll <Word>();
				conn.Insert (w);
			}
		}





	}
}

