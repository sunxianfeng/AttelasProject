
from zhipuai import ZhipuAI


class LLmGenerateSql:
    client = ZhipuAI(api_key="767fcacedb549d241c4ef4266c639e2f.APN2TEbi615ZYabc")
    schema_clients = """
    CREATE TABLE t_clients (
        client_id VARCHAR(10) PRIMARY KEY,
        name VARCHAR(20) NOT NULL,
        email VARCHAR(100) NOT NULL,
        address VARCHAR(100) NOT NULL
    );
    """

    schema_invoices = """
        CREATE TABLE t_invoices (
        invoice_number VARCHAR(10) PRIMARY KEY,
        client_id VARCHAR(10) NOT NULL,
        due_date datetime DEFAULT now(),
        status INT NOT NULL comment '0: Pending, 1: Paid, 2: Overdue', 
        line_items json DEFAULT NULL,
        FOREIGN KEY(client_id) REFERENCES clients(client_id)
    );
    """

    def generate_sql(self, query):
        prompt = f"""
                    There is a schema for a database table named t_invoice: {LLmGenerateSql.schema_clients}.
                    There is another schema for a database table named t_clients: {LLmGenerateSql.schema_invoices}.
                    Can you output an SQL query/update/insert based on this schema to answer the following question? 
                    Only output SQL queries, do not output any other content. Question: {query}
                  """

        response = LLmGenerateSql.client.chat.completions.create(
            model="glm-4",
            messages=[
                {"role": "user", "content": prompt}
            ],
        )
        return response.choices[0].message.content


if __name__ == "__main__":
    question = "Could you check if INV-1020 has been processed?"
    print(LLmGenerateSql().generate_sql(question))

    """
    ```sql
    SELECT invoice_number, status
    FROM t_invoices
    WHERE invoice_number = 'INV-1020';
    ```
    """

    # question = "Do we have any invoices due for Acme Corp"
    # print("---------------------")
    # print(generate_sql(question))
