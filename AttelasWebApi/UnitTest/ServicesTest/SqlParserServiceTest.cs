using Attelas.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using Assert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace Attelas.UnitTest.ServicesTest;

[TestClass]
public class SqlParserServiceTest
{
    private SqlParserService _sqlParserService;
    
    [SetUp]
    public void Init()
    {
        this._sqlParserService = new SqlParserService();
    }

    
    [Test]
    public void TestQuerySqlParserService()
    {
        string sql = """
                     ```sql
                     SELECT invoice_number, due_date, status
                     FROM t_invoices
                     JOIN t_clients ON t_invoices.client_id = t_clients.client_id
                     WHERE t_clients.name = 'Acme Corp'
                     AND t_invoices.status = 0
                     """;
        this._sqlParserService.Parse(sql);
        Assert.IsTrue(this._sqlParserService.IsQuery());
        Assert.IsTrue("t_invoices" == this._sqlParserService.GetTableName());
    }
    
    [Test]
    public void TestCreateSqlParserService()
    {
        string sql = """
                     ```sql
                     insert into t_invoices (client_id, due_date, status) 
                     values ('Acme Corp', '00:00:00', 1)"
                     """;
        this._sqlParserService.Parse(sql);
            
        Assert.IsTrue(this._sqlParserService.IsCreate());
        Assert.IsTrue("t_invoices" == this._sqlParserService.GetTableName());
    }
    
    // [Test]
    // [ExpectedException(typeof(ArgumentNullException))]
    // public void TestSqlParserServiceWithException()
    // {
    //     string sql = """
    //                  ```sql
    //                  insert into"
    //                  """;
    //     this._sqlParserService.Parse(sql);
    // }
    
}