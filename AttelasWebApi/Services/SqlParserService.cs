
using Newtonsoft.Json;

namespace Attelas.Services;


/*
    SELECT invoice_number, due_date, status
    FROM t_invoices
    JOIN t_clients ON t_invoices.client_id = t_clients.client_id
    WHERE t_clients.name = 'Acme Corp'
    AND t_invoices.status = 0
*/

public class SqlParserService : ISqlParserService
{
    private const int MaxLength = 200;
    private bool _isQuery;
    private bool _isCreate;
    private string? _tableName;
    private string? _sqlStatement;
    
    
    public void Parse(string text)
    {
        // todo: prevent sql injection
        var clauses = new List<string>();
        if (MaxLength < text.Length)
        {
            throw new ArgumentException();
        }
        this._sqlStatement = text.Trim().Replace("```sql\n", string.Empty).Replace("\n```", string.Empty);
       
        if (this._sqlStatement.StartsWith("SELECT") || this._sqlStatement.StartsWith("select"))
        {
            this._isQuery = true;
        }
        else if (this._sqlStatement.StartsWith("INSERT") || this._sqlStatement.StartsWith("insert"))
        {
            this._isCreate = true;
            if (this._sqlStatement.Split(" ").Count() < 3)
            {
                throw new FormatException();
            }
            this._tableName = this._sqlStatement.Split(" ")[2].Replace(Environment.NewLine, string.Empty).Trim().ToLower();
        }

        foreach (var clause in this._sqlStatement.Split(Environment.NewLine))
        {
            if (clause.StartsWith("SELECT") || clause.StartsWith("select"))
            {
                clauses.Add("select *");
                continue;
            }
            if (clause.StartsWith("FROM") || clause.StartsWith("from"))
            {
                if (clause.Split(" ").Length < 2)
                {
                    throw new FormatException();
                }
                this._tableName = clause.Split(" ")[1].Replace(Environment.NewLine, string.Empty).Trim().ToLower();
            }
            clauses.Add(clause);
        }
        
        this._sqlStatement = string.Join(" ", clauses);;
        // if (this._sqlStatement.Contains("FROM") || this._sqlStatement.Contains("from"))
        // {
        //     int index = this._sqlStatement.IndexOf("FROM");
        //     this._sqlStatement = this._sqlStatement.Substring(index);
        //     if (this._sqlStatement.Split(" ").Length < 2)
        //     {
        //         throw new FormatException();
        //     }
        //     this._tableName = this._sqlStatement.Split(" ")[1].Replace("\n", string.Empty).Trim().ToLower();
        // }
    }
    
    public bool IsQuery()
    {
        return this._isQuery;
    }

    public bool IsCreate()
    {
        return this._isCreate;
    }

    public string GetTableName()
    {
        return this._tableName;
    }

    public string GetSqlStatement()
    {
        return this._sqlStatement;
    }
}