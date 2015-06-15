# Context Items

Tool to enhance Entity Framework with BulkInsert, BulkUpdate and BulkDelete operations for Ms SQL and Array-bound insert, update and delete operations for Oracle.

# Installation

1. To install Context Items, add one of 2 NuGet packages, either "Context Items for MsSql" or "Context Items for Oracle" to the project where you already have Entity Framework schema (.edmx file).
Package will add either MsSqlContextItems.tt or OracleContextItems.tt file to the project and, at this point, will produce a generation error.
2. Open installed *ContextItems.tt file, find and correct var nameSpace to contain the correct namespace for your project and var edmxName to contain the name of Entity Framework schema.
3. Save *ContextItems.tt file.

note - every time you update your Entity Framework schema, *ContextItems.tt will automatically regenerate to reflect the changes.

# Usage (MsSql)
1. Materialization example
```C#
var contextItems = new MsSqlContextItems();
var context = new TaskTrackerEntities(); // this is your entity framework context
var query = context.Set<task_item>();
var result = contextItems.Materialize(query, context.Database.Connection);
```

2. Insert example
```C#
var contextItems = new MsSqlContextItems();
var context = new TaskTrackerEntities(); // this is your entity framework context
var items = Enumerable.Range(1, 1000)
                    .Select(x => new task_item {})  // put field values inside brackets
                    .ToArray();
contextItems.RangeInsertSequenced(items, context.Database.Connection); 
// notice that sequence is used for insertion, read about that below
```

# Usage (Oracle)
1. Materialization usage is the same as for MsSql
2. Insert example
```C#
var contextItems = new OracleContextItems("schemaName"); 
// schema name in the constructor is what makes the difference for oracle implementation,
// read about that below
var context = new AppConfigContext();
var items = Enumerable.Range(1, 1000)
                      .Select(x => new CLIENT { CLIENTONECODE = x.ToString(),
                          CONTROL_ID = x.ToString(), 
                          CLIENT_NUMBER = x,
                          CLIENT_ID = Guid.NewGuid().ToString("N") })
                      .ToArray();
contextItems.RangeInsert(items, context.Database.Connection);
```

# Motive
Motive stands for "why would i make such a component, let alone publish it".
The reason is the way Entity Framework does insertions. Sometimes you need to insert, update or delete a lot of entites.
EF does that one by one, sending separate request for each insertion, which is dead slow as you could have guessed already.<br/>
Also, EF 5.0.0 has a nasty issue with Oracle, where it can't insert long(over 2K chars) strings to Clob/Xml field. 
This one was enough stimulus for one of the projects we had in our company to switch all the persistence operations to ContextItems.

# Functionality
<h3>1. Equality members</h3>
Context items generates overriden Equals and GetHashCode methods for each entity. 
Have no idea why EF does not do that. Now every 2 entities of the same type will be equal if their primary keys match<br/>

<h3>2. MsSql Bulk Insert</h3>
<b>Scenario:</b><br/>
a. Create a number of entities<br/>
b. Call RangeInsert method<br/>

<b>Implementation:</b><br/>
If entity does not have Identity primary key, then SqlBulkCopy is used behind the scenes which is the fastest insertion method ever.<br/>
Otherwise multi-insert statement is generated, input collection is split into batches of 50-200 items, depending on number of fields the entity has, and every batch is inserted within one multi-insert statement.
This is done because there is no way to retrieve identity after bulk insert. So if you want real speed, do not use identity. Period.<br/>
Alternative to identities are Guid keys, this solves insertion performance, since it allows bulk inserts, but they have the downside of their own - 
joins to that table will work significatly slower, due to 4 times longer keys compared to 32bit integers. Also Guids are non-sequential, therefore clustered indexes are either impossible or ineffective, not sure which. 
You can read about that on the internets.<br/>
Salvation is the sequence. It takes best of 2 approaches, it is sequential(as the name implies) integer key and it allows to do bulk insert directly into the table.<br/>

<h3>3. MsSql Sequenced Bulk Insert</h3>
Scenario is the same as for regular Bulk Insert.<br/>
<b>Implementation:</b><br/>
First, ContextItems retrieve a set of sequence numbers from MsSql server using stored procedure "sp_sequence_get_range". Note that only acyclic sequences are supported here, with increment of 1.
For instance if we want to insert 100 entities, and stored procedure returns 704, entities will have numbers 704 through 803 assigned as their integer primary key.
After that, bulk insert is performed into the table.<br/>
Note that there is a convention - sequence should share the name with the table and have suffix "_seq", for example "task_item" and "task_item_seq". Otherwise there is a public static string SequenceName field in every entity,
which can be overriden.<br/>
Seuences are only supported since MsSql Server 2012.

<h3>4. MsSql Bulk Update</h3>
<b>Scenario:</b><br/>
a. Get a set of entities from the database.<br/>
b. Using user input or some calculations modify field values in those entities.<br/>
c. Update the database records with new values.<br/>
<b>Implementation:</b><br/>
If there is less than 150 entities(predefined constant), ContextItems will generate multi-update statement i.e. update is done as a single operation.
Otherwise, update is performed in 4 steps.<br/>
a. Temp table is created, that has the same fields as the target table.<br/>
b. Bulk Insert to temp table is performed.<br/>
c. Join-Update statement is sent to pass values from temp table to target table.<br/>
d. Temp table is dropped.<br/>
<br/>
Note that there is no change tracking is performed, you have to control it yourself. If you did not change the entity - do not send it to RangeUpdate method, instead exclude it from the collection. 
There is no harm in it if you don't control that, but it will cut your performance trying to set values that database already has.<br/>

<h3>5. MsSql Bulk Delete</h3>
<b>Scenario:</b><br/>
a. Get a set of entities from the database.<br/>
b. With some application logic, decide that you have to delete some of them, and create new collection of items to delete.<br/>
c. Delete records using RangeDelete command.<br/>
<b>Implementation:</b><br/>
If there is less than 150 entities(predefined constant), ContextItems will generate multi-delete statement i.e. deletion is done as a single operation.
Otherwize, deletion is performed in 4 steps.<br/>
a. Temp table is created, that has primary key fields of the target table.<br/>
b. Bulk Insert of key values is performed to temp table.<br/>
c. Join-Delete statement is sent to delete records from target table which keys are present in temp table.<br/>
d. Temp table is dropped.<br/>

Like Bulk Update, this operation is not atomic, so it is better to use either of these 2 operations in transaction.

<h3>6. Materialization</h3>
Do i have to say that EF materialization is slow? I guess i don't. Even if you do .AsNoTracking() before doing .ToList(), it will be slow, and even slower if you don't.
Materialization, done by ContextItems is approximately 40% faster than that of EF. <br/> 
Also, there is a known high performance micro-ORM Dapper, that can also do this way faster than EF.
Performance comparision between Dapper and ContextItems can be found in this repository, files CiTestResultsMsSql.txt and CiTestResultsOracle.txt.
For those who did not check those files out: ContextItems is slightly, but noticeably faster.

<h3>7. Oracle array bound insert, update and delete operations</h3>
Functionality here is mostly the same as for MsSql, but implementation is different. Unlike MsSql, Bulk Insert for Oracle can't be used to insert directly into the table,
OracleBulkCopy does not regard any triggers, constraints and even primary keys. Also, it does not support transactions, which is the worst sin of it.
I could try and implement it the same way as for MsSql, i.e. insert into temp table first and then do a joined statement, but instead i used a method recommended by Oracle.<br/>
Method is called array binding. More info on that method is given on official Oracle site: http://www.oracle.com/technetwork/issue-archive/2009/09-sep/o59odpnet-085168.html <br/>
Long story short: Sql statement used for insert, update or delete operation is the same as if you want to do operation on a single entity, but instead of binding 
a set of fields as a parameters to that statement, a set of arrays is bound, thus inserting, updating or deleting an array of entities instead of one entity.

There is another difference to Oracle worth mentioning: sometimes you can not access tables using simple "Insert into CLIENT ..." statement. You will have to append schema/owner name in front of it,
like "Insert into DBOWNER.CLIENT ...". For some reason DBA's refuse to create synonims. Anyway, as a solution to that, when you create OracleContextItems instance, you can provide a schema name to be used in all operations.

# Sample Project
There is a sample project in Citest folder, which you can look at and run if you have MsSql and/or Oracle server around.
There are database scripts to insert couple tables somewhere inside, and there are integration tests, wrapped in transactions, to demonstrate insert, update and delete operations as 
well as materialization performance comparision tests.

# Supported Entity Framework versions
1. "Context Items for MsSql" package supports EF 5.0.0 - 6.1.3, was not tested with versions in between these two, but it would be a safe bet that it will be working.
2. "Context Items for Oracle" package supports EF 5.0.0 and was not tested with any other version, since i could not make EF 6 work with Oracle. Not sure if EF supports Oracle anymore.
Anyway, if you can make second line of Oracle insert example(in Usage(Oracle) section) work, the rest will most likely work too, as EF is not needed for anything but connection.
3. Only Database-first approach is supported. 
4. Entity names should match tables in database, i.e. no renaming after you create a schema. I wanted to fix this limitation, but could not spare time for it. And it was not so important a limitation for my projects anyway.