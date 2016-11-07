﻿using Artech.CodeGeneration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Artech.CodeGeneration
{
    public class DemoTemplate : Template
    {
        public string ClassName { get; private set; }
        public DemoTemplate(string className)
        {
            this.ClassName = className;
        }
        public override string TransformText()
        {
            //this.WriteLine("public class {0}", this.ClassName);
            //this.WriteLine("{");
            //this.WriteLine("}");

            this.WriteLine(@"
//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由T4模板自动生成
//	   生成时间 {0}
//     对此文件的更改可能会导致不正确的行为，并且如果
//     重新生成代码，这些更改将会丢失。
// </auto-generated>
//------------------------------------------------------------------------------            
", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));

            this.WriteLine("using System;");
            this.WriteLine(string.Format("namespace {0}", Config.NamespaceName));
            this.WriteLine("{");

            this.WriteLine(@"
     /// <summary>
     /// {0}
     /// DateTime:{1}
     /// </summary>",this.ClassName, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));

            this.WriteLine("\t public class {0}", this.ClassName);
            this.WriteLine("\t {");

            foreach (DbColumn column in DbHelper.GetDbColumns(Config.ConnectionString, Config.DbDatabase, this.ClassName))
            {
                this.WriteLine("\t\t /// <summary>");
                this.WriteLine("\t\t /// {0}", column.Remark);
                this.WriteLine("\t\t /// </summary>	");
                this.WriteLine("\t\t public {0} {1} {2} get;set; {3}", column.CSharpType, column.ColumnName, "{", "}");

                //换行
                this.WriteLine("\t\t \r\n");
            }


            this.WriteLine("\t }");
            this.WriteLine("}");

            return this.GenerationEnvironment.ToString();
        }
    }

    public class DemoGenerator : Generator
    {
        protected override IDictionary<string, Template> CreateTemplates()
        {
            Dictionary<string, Template> templates = new Dictionary<string, Template>();
            templates.Add("Foo.cs", new DemoTemplate("Foo"));
            templates.Add("Bar.cs", new DemoTemplate("Bar"));
            templates.Add("Baz.cs", new DemoTemplate("Baz"));
            return templates;
        }
    }

    public class EntityGenerator : Generator
    {
        protected override IDictionary<string, Template> CreateTemplates()
        {
            Dictionary<string, Template> templates = new Dictionary<string, Template>();
            //templates.Add("Foo.cs", new DemoTemplate("Foo"));
            //templates.Add("Bar.cs", new DemoTemplate("Bar"));
            //templates.Add("Baz.cs", new DemoTemplate("Baz"));

            foreach (var item in DbHelper.GetDbTables(Config.ConnectionString, Config.DbDatabase, Config.TableName))
            {
                string tableName = item.TableName;
                templates.Add(string.Format("{0}.cs", tableName), new DemoTemplate(tableName));
            }

            return templates;
        }
    }
}
