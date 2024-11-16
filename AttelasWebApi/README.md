
# Introduction
This is a webApi application based .net8 offering 3 main functionalities via Invoice api and Client api shown in the screenshot below.
- enquiry invoice status
- submit new invoice
- enquiry client info

To be simple, we didn't define different types of datamodel in different layers, such as: VO, PO, DO.

The data will be stored in mysql based on entity framework core, but we will use in-memory database for unit test by commenting the codes in AttelasDbContex.cs like this:
```c#
//string? connectionString = GetConfigurations.GetConfiguration("ConnectionStrings:AttelasDataBase");//"Server=.;Database=AttelasWebApi;";
//optionsBuilder.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
optionsBuilder.UseInMemoryDatabase("AttelasDb");
```

Besides, it also provides llm interaction api which receives a text segment and invokes python service in flask application, outputs a
wise response for you.


![Screenshot 2024-11-14 at 20.24.17.png](../../attelas/AttelasWebApi/StaticContent/Screenshot%202024-11-14%20at%2020.24.17.png)

# Environment
- mysql 9.0
- python 3.8
- .net 8.0

# Authentication
- Call login api with this user: { "username": "validUser", "password": "validPassword" } and you will get a valid token.
- Call other apis using the token, otherwise you will be blocked. 

# How to use llm interaction in local

1. get your api-key and replace it in ./FlaskProject/llmGenerateSql.py, meanwhile install zhipuai module
![Screenshot 2024-11-14 at 21.06.18.png](../../attelas/AttelasWebApi/StaticContent/Screenshot%202024-11-14%20at%2021.06.18.png)

2. run python flask application
![Screenshot 2024-11-16 at 12.19.41.png](StaticContent/Screenshot%202024-11-16%20at%2012.19.41.png)

3. configure this url in appsettings.Development.json
![Screenshot 2024-11-14 at 20.50.53.png](../../attelas/AttelasWebApi/StaticContent/Screenshot%202024-11-14%20at%2020.50.53.png)

4. configure this mysql connection string
 ![Screenshot 2024-11-14 at 21.32.27.png](../../attelas/AttelasWebApi/StaticContent/Screenshot%202024-11-14%20at%2021.32.27.png)

5. run .net application

6. input your text such as "Could you check if INV-1020 has been processed?" via LLmInteraction api 
-> .net app receives it and calls python flask service 
-> call llm api generating corresponding sql 
-> .net app will receive the sql statement and parser it, finally will call xxxxFromSqlRaw api
-> trigger the predefined workflows

You can also build images to run by docker file, there is a docker file to run the .net app, you need also make a mysql docker file and a flask app docker. 










