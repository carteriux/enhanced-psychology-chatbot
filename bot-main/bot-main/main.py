from flask import Flask, request, jsonify,Response
import logging
from back import conversacion,generate_doc
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