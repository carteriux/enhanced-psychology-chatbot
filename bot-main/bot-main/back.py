import logging
from create_bot import create_bot
from langchain_core.messages import HumanMessage
import os
from langchain_core.messages import filter_messages
from google.cloud import storage
from create_pdf import crear_pdf_de_strings
from pathlib import Path
from get_chat_history import get_session_history
from activities_desc import ACTIVITIES
from dotenv import load_dotenv

load_dotenv()


def conversacion(data):
    
    question = data["question"]

    user_id = data["user_id"]

    activity_id = data["activity_id"]
    
    history = get_session_history(user_id,os.getenv("PROJECT_ID"))

    if user_id != os.getenv("TEST_USER_ID"):
        # [mes for mes in list(zip(user_messages, bot_messages)) if mes[1].name == activityID ]

        if len([mes for mes in filter_messages(history.messages, include_types=("human")) if mes.name == activity_id]) > 60: 
            
            answer = """Has alcanzado el número máximo de interacciones con el simulador."""
            return answer
  

    try:      
        chat_history ="\n".join( message.content for message in history.messages if message.name in ACTIVITIES[activity_id][f"{activity_id}_hist"])

        for i in range(3):

            answer = create_bot(user_id = user_id,query=question,chat_history=chat_history,activity_id=activity_id)
            print(answer.content)
            # print(answer)
            if answer.content == "":
                print("No se pudo obtener una respuesta")
            else:
                history.add_messages([HumanMessage(question, user_id = user_id, name = activity_id), answer])
                return answer.content
            if answer.content == "" or i==2:
                return "Lo siento no me siento comoda con esa pregunta"
            
        
    except Exception as ex:
        logging.warning(ex)
        logging.warning("pregunta del usuario: "+question)

        logging.warning(answer)

def reset_user_history(user_id):
    history = get_session_history(user_id, os.getenv("PROJECT_ID"))
    history.clear()
    logging.info(f"Historial de Firestore limpiado para usuario: {user_id}")


def generate_doc(data):

    user_id = data['user_id']

    activityID = data["activity_id"]

    client = storage.Client()

    bucket = client.bucket(os.getenv("BUCKET_NAME"))

    blob = bucket.blob(f"{user_id}_files/{activityID}.pdf")

    file_path = crear_pdf_de_strings(user_id = user_id ,
                               activityID = activityID,
                               nombre_archivo = f"{activityID}.pdf")

    with open(file_path,"rb") as f:
        pdf_bytes = f.read()

    blob.upload_from_filename(filename=file_path.absolute(),content_type="pdf")
    
    if os.path.exists( f"{activityID}.pdf" ):
        os.remove( f"{activityID}.pdf" )

    print(f"Archivo '{activityID}.pdf' subido a '{user_id}_files/' en el bucket '{os.getenv('BUCKET_NAME')}'.")
     
    return pdf_bytes

    # update = tblHistory.update_entity(mode = UpdateMode.REPLACE, entity = entity)
if  __name__ == "__main__":
    question = "¿Trabajas?"
    user_id = "Test-id"
    activity_id = "DA1"
    data = {
        "question":question,
        "user_id":user_id,
        "activity_id": activity_id
    }
    print(conversacion(data))
    