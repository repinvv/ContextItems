﻿namespace ContextItems.MsSqlContextItemsGeneration.StaticFiles
{
    using System;
    using System.Collections.Generic;
    using ContextItems.CommonGeneration.ContextItems;
    using ContextItems.CommonGeneration.ContextItems.StaticFiles;

    internal class BulkInsertDataReaderGenerator : IFileGenerator
    {
        public string GetName()
        {
            return "BulkInsertDataReader";
        }

        public string FileContents(string outputNamespace, IEnumerable<DbModel> models)
        {
            return "namespace " + outputNamespace + Environment.NewLine +
@"{
    using System;
    using System.Collections.Generic;
    using System.Data;

    public class BulkInsertDataReader<T> : IDataReader
         where T : class, IMsSqlEntity
    {
        protected readonly List<T> Entities;
        private int current = -1;
        protected IMsSqlEntity CurrentEntity;
        private bool disposed;
        private List<T>.Enumerator dataEnumerator;

        public BulkInsertDataReader(List<T> entities)
        {
            Entities = entities;
            dataEnumerator = entities.GetEnumerator();
        }

        public void Dispose()
        {
            dataEnumerator.Dispose();
            disposed = true;
        }

        public string GetName(int i)
        {
            return null;
        }

        public string GetDataTypeName(int i)
        {
            return null;
        }

        public Type GetFieldType(int i)
        {
            return null;
        }

        public virtual object GetValue(int i)
        {
            return CurrentEntity.GetValue(i);
        }

        public int GetValues(object[] values)
        {
            return 0;
        }

        public int GetOrdinal(string name)
        {
            return 0;
        }

        public bool GetBoolean(int i)
        {
            return false;
        }

        public byte GetByte(int i)
        {
            return 0;
        }

        public long GetBytes(int i, long fieldOffset, byte[] buffer, int bufferoffset, int length)
        {
            return 0;
        }

        public char GetChar(int i)
        {
            return '\0';
        }

        public long GetChars(int i, long fieldoffset, char[] buffer, int bufferoffset, int length)
        {
            return 0;
        }

        public Guid GetGuid(int i)
        {
            return new Guid();
        }

        public short GetInt16(int i)
        {
            return 0;
        }

        public int GetInt32(int i)
        {
            return 0;
        }

        public long GetInt64(int i)
        {
            return 0;
        }

        public float GetFloat(int i)
        {
            return 0;
        }

        public double GetDouble(int i)
        {
            return 0;
        }

        public string GetString(int i)
        {
            return null;
        }

        public decimal GetDecimal(int i)
        {
            return 0;
        }

        public DateTime GetDateTime(int i)
        {
            return new DateTime();
        }

        public IDataReader GetData(int i)
        {
            return null;
        }

        public bool IsDBNull(int i)
        {
            return false;
        }

        public virtual int FieldCount { get { return Entities[0].GetNumberOfFields(); } }

        object IDataRecord.this[int i] { get { return null; } }

        object IDataRecord.this[string name] { get { return null; } }

        public void Close()
        {
            disposed = true;
        }

        public DataTable GetSchemaTable()
        {
            return null;
        }

        public bool NextResult()
        {
            return false;
        }

        public bool Read()
        {
            current++;
            if (current < Entities.Count)
            {
                CurrentEntity = Entities[current];
                return true;
            }

            return false;
        }

        public int Depth { get; private set; }

        public bool IsClosed { get { return disposed; } }

        public int RecordsAffected { get; private set; }
    }
}";
        }
    }
}
