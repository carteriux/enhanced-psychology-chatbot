from flask import Flask, request, jsonify,Response
import logging
from back import conversacion,generate_doc,reset_user_history
from responses import *
import json


app = Flask(__name__)

@app.route("/bot", methods=['POST'])

def conversation():

    logging.info("Iniciando conversación.")
    data = conversacion(request.json)#.decode()
    logging.info("Finalizando conversación.")
    response = build_successful_response(data)

    return json.dumps(response, ensure_ascii=False).encode(encoding="utf-8")

@app.route("/reset_history", methods=['POST'])
def reset_history():
    data = request.json
    user_id = data.get("user_id")
    if not user_id:
        return json.dumps({"success": False, "message": "user_id requerido"}), 400
    try:
        reset_user_history(user_id)
        return json.dumps({"success": True, "message": f"Historial limpiado para {user_id}"})
    except Exception as e:
        logging.error(f"Error al limpiar historial de {user_id}: {e}")
        return json.dumps({"success": False, "message": str(e)}), 500


@app.route("/generate_chat_pdf",methods=["POST"])
def create_pdf():
    req = request.json

    activity_name = req["activity_id"]
    generated_doc = generate_doc(req)

    if generated_doc is None:
        return json.dumps(RESPONSE_500)
    # return generate_doc

    return Response(generated_doc, mimetype="application/pdf",
                    headers={"Content-Disposition": 
                             f"attachment; filename={activity_name}.pdf"})

if __name__ == "__main__":
    app.run(debug=True, host="0.0.0.0", port="8000")