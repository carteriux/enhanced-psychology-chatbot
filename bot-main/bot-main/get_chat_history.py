from firebase_admin import firestore
import os
from langchain_google_firestore import FirestoreChatMessageHistory

def get_session_history(session_id, project_id):
    
    client = firestore.Client(
        project=project_id,
        database= os.getenv("DATABASE") 
        )

    firestore_chat_history = FirestoreChatMessageHistory(
        session_id=session_id,
        collection= os.getenv("CHAT_HISTORY") ,
        client=client)

    return firestore_chat_history
