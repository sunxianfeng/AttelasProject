namespace Attelas.Services;

public interface ISqlParserService
{
    void Parse(string text);
    bool IsQuery();
    bool IsCreate();
    string GetTableName();
    string GetSqlStatement();
}