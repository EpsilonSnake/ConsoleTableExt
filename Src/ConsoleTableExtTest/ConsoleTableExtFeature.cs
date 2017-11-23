﻿using ConsoleTableExt;
using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Http.Headers;

namespace ConsoleTableExtTest
{
    [TestFixture]
    public class ConsoleTableExtFeature
    {
        [Test]
        public void Export_Test()
        {
            var optionDefault = new ConsoleTableExportOption {IncludeRowCount = IncludeRowCountType.Top};
            var optionAlternative = new ConsoleTableExportOption {ExportFormat = ConsoleTableFormat.Alternative};
            var optionMarkDown = new ConsoleTableExportOption { ExportFormat = ConsoleTableFormat.MarkDown};
            var optionMinimal = new ConsoleTableExportOption {ExportFormat = ConsoleTableFormat.Minimal, IncludeRowCount = IncludeRowCountType.Bottom};

            var result0 = ConsoleTableBuilder.From(GetTable()).Export().ToString();
            var result1 = ConsoleTableBuilder.From(GetTable()).Export(optionDefault).ToString();
            var result2 = ConsoleTableBuilder.From(GetTable()).Export(optionAlternative).ToString();
            var result3 = ConsoleTableBuilder.From(GetTable()).Export(optionMarkDown).ToString();
            var result4 = ConsoleTableBuilder.From(GetTable()).Export(optionMinimal).ToString();

            var rows = Enumerable.Repeat(new Something("I am using a sample project for Entity Framework"), 1000).ToList();
            rows.AddRange(Enumerable.Repeat(new Something("GetCustomAttribute is not recognized"), 1000).ToList());
            rows.AddRange(Enumerable.Repeat(new Something("Sending email in .NET through Gmail"), 1000).ToList());
            rows.AddRange(Enumerable.Repeat(new Something("Be sure to use System.Net.Mail, not the deprecated System.Web.Mail"), 1000).ToList());
            rows = rows.OrderBy(elem => Guid.NewGuid()).ToList();
            var x = ConsoleTableBuilder.From(rows).Export().ToString();

            var result5 = ConsoleTableBuilder.From(rows)
                .AddColumn(new List<string> {"A", "B", "C"}, true)
                .AddRow("1", "2", "3")
                .AddRow("11", "22", "33")
                .AddRow("111", "222", "333")
                .AddColumn(new List<string> { "ColA", "ColB", "ColC" }, true)
                .Export().ToString();
        }

        static DataTable GetTable()
        {
            // Here we create a DataTable with four columns.
            DataTable table = new DataTable();
            table.Columns.Add("Dosage", typeof(int));
            table.Columns.Add("Drug", typeof(string));
            table.Columns.Add("Patient", typeof(string));
            table.Columns.Add("Date", typeof(DateTime));

            // Here we add five DataRows.
            table.Rows.Add(25, "Indocin", "David", DateTime.Now);
            table.Rows.Add(50, "Enebrel", "Sam", DateTime.Now);
            table.Rows.Add(10, "Hydralazine", "Christoff", DateTime.Now);
            table.Rows.Add(21, "Combivent", "Janet", DateTime.Now);
            table.Rows.Add(100, "Dilantin", "Melanie", DateTime.Now);
            return table;
        }

        public class Something : IConsoleTableDataStore
        {
            public Something(string name)
            {
                Id = Guid.NewGuid().ToString("N");
                Name = name;
                Date = DateTime.Now;
                System.Threading.Thread.Sleep(12);
            }

            [ConsoleTableColumnAttributes(1, "Name")]
            public string Name { get; set; }

            [ConsoleTableColumnAttributes(3, "Dxxxx")]
            public string Id { get; set; }

            [ConsoleTableColumnAttributes(2)]
            public DateTime Date { get; set; }
        }
    }
}