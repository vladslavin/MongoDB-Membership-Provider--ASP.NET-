using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Norm;
using Norm.Collections;
using System.Linq.Expressions;

namespace Ludopoli.MongoMember
{
	public class Mongo
	{
		public static Mongo Create(string connection)
		{
			var res = new Mongo();
			res.connection = connection;
			return res;
		}

		public void Add(object item)
		{
			using (var mongo = connect) {
				collection(mongo, item).Insert(item);
			}
		}

		public void Save(object item)
		{
			using (var mongo = connect) {
				var col = collection(mongo, item);
				col.Save(item);
			}
		}

		public void Save<T>(T item) where T : class
		{
			using (var mongo = connect) {
				var col = mongo.GetCollection<T>();
				col.Save(item);

				var e = mongo.LastError().Error;
				if (e != null)
					throw new Exception(e);
			}
		}

		public IEnumerable<T> All<T>(string collection_name = null) where T : class
		{
			using (var mongo = connect) {
				var col = collection_name == null ? mongo.GetCollection<T>() : mongo.Database.GetCollection<T>(collection_name);
				return col.AsQueryable().ToList();
			}
		}

		public T SingleOrDef<T>(Expression<Func<T, bool>> expression) where T : class
		{
			using (var mongo = connect) {
				return mongo.GetCollection<T>().AsQueryable().Where(expression).SingleOrDefault();
			}
		}

		IMongo connect { get { return Norm.Mongo.Create(connection); } }
		IMongoCollection collection(IMongo mongo, object byObjectType)
		{
			return mongo.Database.GetCollection(byObjectType.GetType().Name);

		}
		string connection;
	}
}
