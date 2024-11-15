from flask import Flask, request

from llmGenerateSql import LLmGenerateSql

app = Flask(__name__)


@app.route('/v1/llmGenerateSql', methods=['POST'])
def get_sql():
    text = request.json
    res = LLmGenerateSql().generate_sql(text)
    print(res)
    return res


if __name__ == '__main__':
    app.run()
