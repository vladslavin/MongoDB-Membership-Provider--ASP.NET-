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
				checkError(mongo);
			}
		}

		public void Save(object item)
		{
			using (var mongo = connect) {
				var col = collection(mongo, item);
				col.Save(item);
				checkError(mongo);
			}
		}

		public void Save<T>(T item) where T : class
		{
			using (var mongo = connect) {
				var col = mongo.GetCollection<T>();
				col.Save(item);
				checkError(mongo);
			}
		}

		public void Delete(object item)
		{
			using (var mongo = connect) {
				collection(mongo, item).Delete(item);
				checkError(mongo);
			}
		}

		public IEnumerable<T> All<T>(string collection_name = null) where T : class
		{
			using (var mongo = connect) {
				var col = collection_name == null ? mongo.GetCollection<T>() : mongo.Database.GetCollection<T>(collection_name);
				checkError(mongo);
				return col.AsQueryable().ToList();
			}
		}

		public T SingleOrDef<T>(Expression<Func<T, bool>> expression) where T : class
		{
			using (var mongo = connect) {
				var res = mongo.GetCollection<T>().AsQueryable().Where(expression).SingleOrDefault();
				checkError(mongo);
				return res;
			}
		}

		string connection;
		IMongo connect { get { return Norm.Mongo.Create(connection); } }
		IMongoCollection collection(IMongo mongo, object byObjectType)
		{
			return mongo.Database.GetCollection(byObjectType.GetType().Name);

		}

		void checkError(IMongo mongo)
		{
			var e = mongo.LastError().Error;
			if (e != null)
				throw new Exception(e);
		}
	}
}
