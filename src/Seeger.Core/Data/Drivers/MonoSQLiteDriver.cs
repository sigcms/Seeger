using System;

namespace Seeger.Data.Drivers
{
	public class MonoSQLiteDriver : NHibernate.Driver.ReflectionBasedDriver
	{
		public MonoSQLiteDriver() 
			: base("Mono.Data.Sqlite", "Mono.Data.Sqlite", "Mono.Data.SqliteConnection", "Mono.Data.SqliteCommand")
		{
		}

		public override bool UseNamedPrefixInSql
		{
			get
			{
				return true;
			}
		}

		public override bool UseNamedPrefixInParameter
		{
			get
			{
				return false;
			}
		}

		public override string NamedPrefix
		{
			get
			{
				return "@";
			}
		}

		public override bool SupportsMultipleOpenReaders
		{
			get
			{
				return false;
			}
		}
	}
}

