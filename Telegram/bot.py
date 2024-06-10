import os
import telebot
import pyodbc

BOT_TOKEN = os.environ.get('BOT_TOKEN')

bot = telebot.TeleBot(BOT_TOKEN)

# Database connection details
connection_string = (
    'DRIVER={ODBC Driver 17 for SQL Server};'
    'SERVER=RegistosRetro.mssql.somee.com;'
    'DATABASE=RegistosRetro;'
    'UID=mowt_SQLLogin_1;'
    'PWD=kbeho1tzig;'
    'TrustServerCertificate=YES;'
)

def execute_query(sql_query):
    try:
        # Establish the connection
        with pyodbc.connect(connection_string) as conn:
            cursor = conn.cursor()
            cursor.execute(sql_query)
            # Fetch all results
            rows = cursor.fetchall()
            return rows
    except Exception as e:
        return f"Erro ao conectar ao banco de dados: {e}"

@bot.message_handler(commands=['procurar'])
def search_person(message):
    try:
        search_query = message.text.split(' ', 1)[1] 
        trimmed_name = search_query.strip().upper()

        sql_query = f"""
        SELECT * FROM debts 
        WHERE UPPER(NameClient) LIKE '%{trimmed_name}%' 
        OR UPPER(phone) LIKE '%{trimmed_name}%' 
        OR UPPER(email) LIKE '%{trimmed_name}%'
        """

        rows = execute_query(sql_query)

        if isinstance(rows, str):  # Error message
            response = rows
        elif not rows:  # No results found
            response = "Nenhum resultado encontrado."
        else:
            # response = f"{sql_query} \n\n"
            response = "Resultados da pesquisa:\n"
            for row in rows:
                client_name = row[2]
                debt = row[3]
                response += f"{client_name}: {debt}€\n"

        bot.reply_to(message, response)

    except IndexError:
        bot.reply_to(message, "Adicione algum texto para procurar.")
    except Exception as e:
        bot.reply_to(message, f"Erro: {e}")

@bot.message_handler(func=lambda msg: True)
def echo_all(message):
    bot.reply_to(message, message.text)

bot.infinity_polling()