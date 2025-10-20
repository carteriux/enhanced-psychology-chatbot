from langchain_core.prompts import ChatPromptTemplate
from langchain_core.messages import HumanMessage
from langchain_google_vertexai import VertexAIEmbeddings,ChatVertexAI
from langchain_google_firestore import FirestoreVectorStore
from google.cloud import firestore
from operator import itemgetter
import dotenv
import os
from activities_desc import ACTIVITIES
from langchain_core.output_parsers import StrOutputParser
dotenv.load_dotenv()

PROJECT_ID = os.getenv("PROJECT_ID")
COLLECTION_ID = os.getenv("COLLECTION_ID")
HISTORY_COLLECTION = os.getenv("CHAT_HISTORY")
DATABASE = os.getenv("DATABASE")
LOCATION = os.getenv("LOCATION")

llm = ChatVertexAI(
    model="gemini-2.0-flash-lite",
    temperature=0.2,
    max_tokens=None,
    max_retries=6,
    stop=None,    
)

client = firestore.Client(
        project=PROJECT_ID,)

embeddings = VertexAIEmbeddings(model_name="text-multilingual-embedding-002", location=LOCATION, project=PROJECT_ID)

vectorstore = FirestoreVectorStore(collection=COLLECTION_ID,
                     embedding_service=embeddings, 
                     client=client)
retriever = vectorstore.as_retriever()


template = """Actua como un paciente en terapia psicológica. 
Tu objetivo es ayudar a los estudiantes de psicología a practicar sus habilidades antes de que se les asigne un paciente real.
La personalidad del paciente que vas a personificar esta caracterizado por los siguientes diagnosticos psiquiatricos:

Trastorno depresivo mayor:
Más de dos semanas de tristeza, inactividad, falta de energía, culpa y dificultad para dormir.
Fobia social:
Miedo intenso a ser evaluada en situaciones de desempeño, ansiedad por mostrar síntomas de nerviosismo y ser rechazada por demostrarlos. 
Trastorno de personalidad evitativa:
Baja autoestima crónica, comportamientos evitativos y sensación de incompetencia desde temprana edad.

La descripción de la práctica es la siguiente:

{Act_description}

La información del paciente que vas a personificar es la siguiente:

{context}

Aqui te dejo el historial de la conversación que has tenido con el estudiante, tambien tomalo como referencia para poder crear tus respuestas:

{chat_history}

Siempre responde las preguntas del estudiante en primera persona del singular, con un lenguaje casual y manteniendo la personalidad del paciente. No termines tus respuestas haciendo preguntas al estudiante.

"""

resume_template = """Escriba un resumen conciso de la siguiente conversación de un terapeuta con su paciente Fabiola:\n\n{contexto}"""

resume_prompt = ChatPromptTemplate.from_template(resume_template)


prompt = ChatPromptTemplate(messages=[ 
    ("system",template),
    ("user","Entrada del usuario:{input}"),
])

def format_docs(docs):
    return "\n\n".join(doc.page_content for doc in docs)

chain = {
    "input": itemgetter("question"),
    "context": itemgetter("question") | retriever | format_docs,
    "chat_history": itemgetter("history"),
    "Act_description": itemgetter("act_description")} | prompt | llm
    
resume_chain = resume_prompt | llm |StrOutputParser()

def create_bot(user_id, query,chat_history, activity_id):
    
    print("Creando respuesta")

    activity_description = ACTIVITIES[activity_id][f"{activity_id}_desc"]
    
    question = HumanMessage(query, user_id = user_id )

    history = resume_chain.invoke(chat_history)

    # print(history)

    answer = chain.invoke({"question": question.content ,"history":history,"act_description":activity_description})

    answer.name = activity_id

    answer.user_id = user_id


    print("Devolviendo respuesta")

    return answer

if __name__ == "__main__":

    from back import get_session_history

    user_id = os.getenv("TEST_USER_ID")

    history = get_session_history(user_id,os.getenv("PROJECT_ID"))

    chat_history ="\n".join( message.content for message in history.messages)

    question = input("Ingrse su pregunta al paciente")

    response = create_bot(user_id=user_id,query=question,chat_history=chat_history,activity_id="DA1")

    print(response.content)


