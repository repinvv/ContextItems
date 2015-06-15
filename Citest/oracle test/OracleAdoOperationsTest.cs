namespace oracle_test
{
    using System;
    using System.Data.Common;
    using System.Diagnostics;
    using System.Linq;
    using System.Transactions;

    using Citest.AppConfigModel;

    using Dapper;

    using EntityFramework.Extensions;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class OracleAdoOperationsTest
    {
        private OracleContextItems contextItems;
        private AppConfigContext context;
        readonly TransactionOptions options = new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted };

        [TestInitialize]
        public void Prepare()
        {
            contextItems = new OracleContextItems();
            context = new AppConfigContext();
        }

        [TestMethod]
        public void TestDeletion()
        {
            ActualTestDelete(100);
            ActualTestDelete(1000);
        }

        private void ActualTestDelete(int amount)
        {
            var query = context.Set<CLIENT>();

            using (new TransactionScope(TransactionScopeOption.Required, options))
            {
                using (new ConnectionHandler(context.Database.Connection))
                {
                    PreInsertClients(amount + 100);
                    var items = contextItems.Materialize(query, context.Database.Connection).ToArray();
                    var watch = Stopwatch.StartNew();
                    contextItems.RangeDelete(items.Take(amount), context.Database.Connection);
                    watch.Stop();
                    Debug.WriteLine("delete " + amount + " entries: " + watch.Elapsed);
                    var items2 = contextItems.Materialize(query, context.Database.Connection);
                    Assert.AreEqual(items.Count() - amount, items2.Count());
                }
            }
        }

        [TestMethod]
        public void TestUpdate()
        {
            ActualTestUpdate(100);
            ActualTestUpdate(1000);
        }

        private void ActualTestUpdate(int amount)
        {
            var query = context.Set<CLIENT>();
            var updatedValue = "updated";
            using (new TransactionScope(TransactionScopeOption.Required, options))
            {
                using (new ConnectionHandler(context.Database.Connection))
                {
                    PreInsertClients(amount + 100);
                    var items = contextItems.Materialize(query, context.Database.Connection).ToArray();
                    var toUpdate = items.Take(amount).ToArray();

                    foreach (var taskItem in toUpdate)
                    {
                        taskItem.CLIENTONECODE = updatedValue;
                    }

                    var watch = Stopwatch.StartNew();
                    contextItems.RangeUpdate(toUpdate, context.Database.Connection);
                    watch.Stop();
                    Debug.WriteLine("update " + amount + " entries: " + watch.Elapsed);
                    var items2 = contextItems.Materialize(query, context.Database.Connection);
                    Assert.AreEqual(amount, items2.Count(x => x.CLIENTONECODE == updatedValue));
                }
            }
        }

        [TestMethod]
        public void TestInsert()
        {
            ActualTestInsert(1000);
        }

        private void PreInsertClients(int number)
        {
            var items = Enumerable.Range(1, number)
                    .Select(x => new CLIENT { CLIENTONECODE = x.ToString(),
                        CONTROL_ID = x.ToString(), 
                        CLIENT_NUMBER = x,
                        CLIENT_ID = Guid.NewGuid()
                        .ToString("N") })
                    .ToArray();
            contextItems.RangeInsert(items, context.Database.Connection);
        }

        private void ActualTestInsert(int amount)
        {
            var query = context.Set<CLIENT>();
            var updatedValue = "updated";

            using (new TransactionScope(TransactionScopeOption.Required, options))
            {
                using (new ConnectionHandler(context.Database.Connection))
                {
                    PreInsertClients(1);
                    var item = contextItems.Materialize(context.Set<CLIENT>(), context.Database.Connection).First();
                    var toInsert = Enumerable.Range(1, amount).Select(x => new CLIENT
                    {
                        CLIENT_ID = Guid.NewGuid()
                        .ToString("N"),
                        CLIENTONECODE = updatedValue,
                        CONTROL_ID = item.CONTROL_ID,
                        CLIENT_NUMBER = x
                    }).ToList();

                    var items = contextItems.Materialize(query, context.Database.Connection).ToList();

                    var watch = Stopwatch.StartNew();
                    contextItems.RangeInsert(toInsert, context.Database.Connection);
                    watch.Stop();
                    Debug.WriteLine("insert " + amount + " entries: " + watch.Elapsed);


                    foreach (var i in Enumerable.Range(1, 20))
                    {
                        DapperMaterialize(query);
                    }

                    foreach (var i in Enumerable.Range(1, 20))
                    {
                        Materialize(query);
                    }

                    var items2 = contextItems.Materialize(query, context.Database.Connection).ToList();
                    Assert.AreEqual(items.Count() + amount, items2.Count());
                    Assert.AreEqual(amount, items2.Count(x => x.CLIENTONECODE == updatedValue));
                }
            }
        }

        public void DapperMaterialize(IQueryable<CLIENT> query)
        {
            var watch = Stopwatch.StartNew();
            var oq = query.ToObjectQuery();
            var items3 = context.Database.Connection.Query<CLIENT>(oq.ToTraceString(), oq.Parameters).ToList();
            watch.Stop();
            Debug.WriteLine("dapper select " + items3.Count + " entries: " + watch.Elapsed);
        }

        public void Materialize(IQueryable<CLIENT> query)
        {
            var watch = Stopwatch.StartNew();
            var mymat = contextItems.Materialize(query, context.Database.Connection).ToList();
            watch.Stop();
            Debug.WriteLine("select " + mymat.Count + " entries: " + watch.Elapsed);
        }
    }
}
