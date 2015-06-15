using System;
using System.Transactions;
using EntityFramework.Extensions;
using EntityFramework.Utilities;

namespace tests
{
    using System.Data;
    using System.Data.Common;
    using System.Diagnostics;
    using System.Linq;
    using Citest;
    using Dapper;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class AdoOperationsTest
    {
        private MsSqlContextItems contextItems;
        private TaskTrackerEntities context;

        [TestInitialize]
        public void Prepare()
        {
            contextItems = new MsSqlContextItems();
            context = new TaskTrackerEntities();
        }

        [TestMethod]
        public void TestDeletion()
        {
            ActualTestDelete(100);
            context = new TaskTrackerEntities();
            ActualTestDelete(100000);
        }

        private void ActualTestDelete(int amount)
        {
            var query = context.Set<task_item>();

            using (var trans = context.Database.BeginTransaction(IsolationLevel.ReadCommitted))
            {
                var under = trans.UnderlyingTransaction;
                PreInsertTaskItems(amount + 100, under);
                var items = contextItems.Materialize(query, context.Database.Connection, under).ToArray();
                var watch = Stopwatch.StartNew();
                contextItems.RangeDelete(items.Take(amount), context.Database.Connection, under);
                watch.Stop();
                Debug.WriteLine("delete " + amount + " entries: " + watch.Elapsed);
                var items2 = contextItems.Materialize(query, context.Database.Connection, under);
                Assert.AreEqual(items.Count() - amount, items2.Count());
            }
        }

        [TestMethod]
        public void TestUpdate()
        {
            ActualTestUpdate(100);
            context = new TaskTrackerEntities();
            ActualTestUpdate(100000);
        }

        private void ActualTestUpdate(int amount)
        {
            var query = context.Set<task_item>();
            var updatedValue = "updated value";
            using (var trans = context.Database.BeginTransaction(IsolationLevel.ReadCommitted))
            {
                var under = trans.UnderlyingTransaction;
                PreInsertTaskItems(amount + 100, under);

                var items = contextItems.Materialize(query, context.Database.Connection, under).ToArray();
                var toUpdate = items.Take(amount).ToArray();

                foreach (var taskItem in toUpdate)
                {
                    taskItem.task_parameters = updatedValue;
                }

                var watch = Stopwatch.StartNew();
                contextItems.RangeUpdate(toUpdate, context.Database.Connection, under);
                watch.Stop();
                Debug.WriteLine("update " + amount + " entries: " + watch.Elapsed);
                var items2 = contextItems.Materialize(query, context.Database.Connection, under);
                Assert.AreEqual(amount, items2.Count(x => x.task_parameters == updatedValue));
            }
        }

        [TestMethod]
        public void TestInsert()
        {
            ActualTestInsert(200000);
        }

        private void PreInsertTaskItems(int number, DbTransaction under)
        {
            var type = new task_type { task_name = "naem", assembly_location = "asm", class_name = "class" };
            var batch = new batch { task_type = type };
            context.task_type.Add(type);
            context.batch.Add(batch);
            context.SaveChanges();

            var items = Enumerable.Range(1, number)
                    .Select(x => new task_item { batch_id = batch.batch_id, task_parameters = "old value" })
                    .ToArray();
            contextItems.RangeInsertSequenced(items, context.Database.Connection, under);
        }

        private void ActualTestInsert(int amount)
        {
            var query = context.Set<task_item>();
            var updatedValue = "updated value";
            using (var trans = context.Database.BeginTransaction(IsolationLevel.ReadCommitted))
            {
                DbTransaction under = trans.UnderlyingTransaction;
                PreInsertTaskItems(1, under);
                var item = context.Set<task_item>().First();
                var toInsert = Enumerable.Range(1, amount)
                                         .Select(x => new task_item())
                                         .ToList();

                var items = contextItems.Materialize(query, context.Database.Connection, under)
                                        .ToList();

                foreach (var taskItem in toInsert)
                {
                    taskItem.task_parameters = updatedValue;
                    taskItem.batch_id = item.batch_id;
                    taskItem.task_parameters = updatedValue;
                    taskItem.group_key = "grp";
                    taskItem.exclusion_key = "exc";
                }

                var watch = Stopwatch.StartNew();
                contextItems.RangeInsertSequenced(toInsert, context.Database.Connection, under);
                watch.Stop();
                Debug.WriteLine("insert " + amount + " entries: " + watch.Elapsed);


                foreach (var i in Enumerable.Range(1, 20))
                {
                    DapperMaterialize(query, under);
                }

                foreach (var i in Enumerable.Range(1, 20))
                {
                    Materialize(query, under);
                }

                var items2 = contextItems.Materialize(query, context.Database.Connection, under).ToList();
                Assert.AreEqual(items.Count() + amount, items2.Count());
                Assert.AreEqual(amount, items2.Count(x => x.task_parameters == updatedValue));

            }
        }

        public void DapperMaterialize(IQueryable<task_item> query, DbTransaction under)
        {
            var watch = Stopwatch.StartNew();
            var oq = query.ToObjectQuery();
            var items3 = context.Database.Connection.Query<task_item>(oq.ToTraceString(), oq.Parameters, under).ToList();
            watch.Stop();
            Debug.WriteLine("dapper select " + items3.Count + " entries: " + watch.Elapsed);
        }

        public void Materialize(IQueryable<task_item> query, DbTransaction under)
        {
            var watch = Stopwatch.StartNew();
            var mymat = contextItems.Materialize(query, context.Database.Connection, under).ToList();
            watch.Stop();
            Debug.WriteLine("select " + mymat.Count + " entries: " + watch.Elapsed);
        }

        private int PreInsertTaskType()
        {
            var type = new task_type { task_name = "naem", assembly_location = "asm", class_name = "class" };
            context.task_type.Add(type);
            context.SaveChanges();
            return type.task_type_id;
        }

        [TestMethod]
        public void EfUtilBatchInsert()
        {
            var amount = 200000;

            using (var trans = new TransactionScope(TransactionScopeOption.Required))
            {
                var type = PreInsertTaskType();
                var batches = Enumerable.Range(1, amount)
                    .Select(x => new batch
                    {
                        batch_id = Guid.NewGuid(),
                        task_type_id = type,
                        batch_complete_task_parameters = "some",
                        chunk_init_parameters = "also",
                        batch_complete_task_type_id = type
                    }).ToList();

                var watch = Stopwatch.StartNew();
                EFBatchOperation.For(context, context.batch).InsertAll(batches);
                watch.Stop();
                Console.WriteLine("insert " + amount + " entries: " + watch.Elapsed);
            }
        }

        [TestMethod]
        public void MyBatchInsert()
        {
            var amount = 200000;

            using (var trans = new TransactionScope(TransactionScopeOption.Required))
            {
                var type = PreInsertTaskType();
                var batches = Enumerable.Range(1, amount)
                    .Select(x => new batch
                    {
                        batch_id = Guid.NewGuid(),
                        task_type_id = type,
                        batch_complete_task_parameters = "some",
                        chunk_init_parameters = "also",
                        batch_complete_task_type_id = type
                    }).ToList();

                var watch = Stopwatch.StartNew();
                contextItems.RangeInsert(batches, context.Database.Connection, null);
                watch.Stop();
                Console.WriteLine("insert " + amount + " entries: " + watch.Elapsed);
            }
        }
    }
}
