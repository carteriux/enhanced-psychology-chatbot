const ACTION_BUTTONS = {
    PAUSE: "pause",
    FINISH: "check",
} as const

const ACTIVITY_DESCRIPTION_MAP = new Map<number, string>([
    [
        1,
        "Lleva a cabo la entrevista inicial con el objetivo de llenar la historia clínica de Fabiola. Basándote en el formato y material visto en la clase de entrevista inicial, realiza las preguntas adecuadas para recopilar la información necesaria para completar la historia clínica de Fabiola y elaborar el diagnostico. EL LÍMITE ES DE 60 PREGUNTAS / INTERACCIONES con Fabiola. Es importante hacer las preguntas adecuadas, así como existe un límite de tiempo en una sesión, existe un límite de preguntas en este ejercicio, elígelas bien. En este ejercicio solo realizarás preguntas.",
    ],
    [
        2,
        "Conceptualiza al paciente dentro del modelo cognitivo de la depresión. Basándote en el material visto en la clase de modelo cognitivo de la depresión, hazle a Fabiola todas las preguntas necesarias para realizar la conceptualización dentro del modelo cognitivo de la depresión. Asegúrate de cubrir todos los componentes del modelo. EL LÍMITE ES DE 60 PREGUNTAS / INTERACCIONES con Fabiola. Es importante hacer las preguntas adecuadas, así como existe un límite de tiempo en una sesión, existe un límite de preguntas en este ejercicio, elígelas bien. En este ejercicio solo realizarás preguntas.",
    ],
    [
        3,
        "Estás en la 3ra sesión de terapia con Fabiola, el problema que se fija colaborativamente en la agenda es “me cuesta trabajo conservar amistades”. Tu misión es ayudarle a Fabiola con este problema a través de la reestructuración cognitiva de pensamientos automáticos. Asegurate de explorar el problema primero, identificar la cognición clave y seguir el procedimiento completo visto en clase para la eficaz aplicación de esta intervención. Si realizas la intervención de forma adecuada, Fabiola tendrá un resultado positivo en sus respuestas, de no llevar a cabo la intervención de forma adecuada, Fabiola NO presentará mejoría. EL LÍMITE ES DE 60 PREGUNTAS / INTERACCIONES con Fabiola. En esta actividad no tienes que limitarte a preguntas, las interacciones pueden ser afirmaciones, consejos, psicoeducación y preguntas.",
    ],
    [
        4,
        "La sesión previa le dejaste de tarea a Fabiola el formato de monitoreo de actividades, el cual comenzaron a llenar en sesión y te lo trae en esta sesión (documento con el formato de monitoreo de actividades completo llenado por Fabiola adjunto en la sección de recursos de la materia). Tu misión está sesión es ayudar a Fabiola a sentirse menos triste y menos apática a través de la activación conductual. Comienza tu interacción con Fabiola desde el paso de revisar el monitoreo de actividades y continua con el resto de los pasos para la correcta aplicación de la intervención según el material visto en clase y la demostración clínica de Rocío. EL LÍMITE ES DE 60 PREGUNTAS / INTERACCIONES con Fabiola. En esta actividad no tienes que limitarte a preguntas, las interacciones pueden ser afirmaciones, consejos, psicoeducación y preguntas.",
    ],
])

export { ACTION_BUTTONS, ACTIVITY_DESCRIPTION_MAP }
