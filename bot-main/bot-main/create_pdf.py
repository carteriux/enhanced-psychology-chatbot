import os
from reportlab.pdfgen import canvas
from reportlab.lib.pagesizes import letter
from reportlab.lib.utils import simpleSplit
from langchain_core.messages import filter_messages
from get_chat_history import get_session_history
from pathlib import Path
from datetime import datetime
import pytz

def crear_pdf_de_strings(user_id,activityID,nombre_archivo="output.pdf"):
    c = canvas.Canvas(nombre_archivo, pagesize=letter)
    width, height = letter
    margen_x = 50
    margen_y = 50
    max_ancho_texto = width - 2 * margen_x  # Espacio máximo para texto
    y_position = height - margen_y  # Posición inicial

    # Obtener historial de mensajes
    history = get_session_history(session_id=user_id, project_id=os.getenv("PROJECT_ID"))
    messages = history.messages

    # Filtrar mensajes de usuario y bot
    user_messages = filter_messages(messages, include_types=("human"))
    bot_messages = filter_messages(messages, include_types=("ai"))

    conversation = [mes for mes in list(zip(user_messages, bot_messages)) if mes[1].name == activityID ]

    c.setFont("Times-Roman", 12)

    tz=pytz.timezone("America/Mexico_City")

    date = datetime.now(tz=tz).strftime("%d-%m-%Y:%I:%M:%S:%p")

    header = {
        "Número de matrícula":user_id,
        "ID de la actividad": activityID,
        "Fecha de finalización del archivo": date,
    }

    for key,item in header.items():

        header_text = f"{key}: {item}"

        header_line = simpleSplit(header_text, c._fontname, c._fontsize, max_ancho_texto)

        for line in header_line + [""]:  # Agrega líneas en blanco para separación
            if y_position < margen_y:
                c.showPage()
                c.setFont("Times-Roman", 12)
                y_position = height - margen_y  # Reiniciar la posición en la nueva página

            c.drawString(margen_x, y_position, line)
            y_position -= 10  # Espaciado entre líneas

    y_position -= 20
    for mes in conversation:
        user_text = f"{mes[0].user_id}: {mes[0].content}"
        bot_text = f"Fabiola: {mes[1].content}"

        # Dividir el texto en líneas que quepan en el ancho de la página
        user_lines = simpleSplit(user_text, c._fontname, c._fontsize, max_ancho_texto)
        bot_lines = simpleSplit(bot_text, c._fontname, c._fontsize, max_ancho_texto)

        # Imprimir las líneas asegurando que caben en la página
        for line in user_lines + [""] + bot_lines + [""]:  # Agrega líneas en blanco para separación
            if y_position < margen_y:
                c.showPage()
                c.setFont("Times-Roman", 12)
                y_position = height - margen_y  # Reiniciar la posición en la nueva página

            c.drawString(margen_x, y_position, line)
            y_position -= 20  # Espaciado entre líneas

    c.save()
    print(f"PDF guardado como {nombre_archivo}")

    return Path(nombre_archivo)
# crear_pdf_de_strings(TestActivity3[ 'activityID' ],f"{TestActivity3['activityID']}_test.pdf")
