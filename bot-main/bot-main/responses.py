
RESPONSE_400 = {
    "code":400,
    "message": "Parámetros Inválidos.",
    "parameters":[]
}


RESPONSE_500 = {
    "code":500,
    "message": "Error Técnico, Intente de nuevo, si el error persiste reportar a Soporte."
}


RESPONSE_200 = {
    "code":200,
    "message":"ok"
}

RESPONSE_204 = {
    "code":204,
    "message":"no info"
}


def build_successful_response(data):
    
    response = {
        "result":RESPONSE_200,
        "data": str(data)
    }

    return response