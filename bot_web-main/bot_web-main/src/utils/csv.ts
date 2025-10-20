import fs from "fs"
import Papa, { ParseError, ParseResult } from "papaparse"
import { ApiStudentToCreate } from "@/types/student"
import { z } from "zod"
import { CustomCSVError } from "./error"

// Zod schema
const studentSchema = z.object({
    firstName: z.string().min(1, "El nombre es obligatorio"),
    middleName: z.string().min(1, "El apellido paterno es obligatorio"),
    lastName: z.string().min(1, "El apellido materno es obligatorio"),
    enrollmentNumber: z.string().min(1, "La matrícula es obligatoria"),
    email: z
        .string()
        .nonempty("El campo de correo no puede estar vacío")
        .email("El correo no es válido"),
    password: z
        .string()
        .nonempty("El campo de contraseña no puede estar vacío")
        .min(6, "La contraseña debe tener al menos 6 caracteres"),
})

type CsvRow = Record<string, string>

// Process and validate a CSV Row
function processCSVRow(row: CsvRow): ApiStudentToCreate | z.ZodError {
    const formattedRow = {
        firstName: row.firstName?.trim() || "",
        lastName: row.lastName?.trim() || "",
        middleName: row.middleName?.trim() || "",
        enrollmentNumber: row.enrollmentNumber?.trim() || "",
        email: row.email?.trim() || "",
        password: row.password?.trim() || "",
    }

    const parsed = studentSchema.safeParse(formattedRow)
    return parsed.success ? parsed.data : parsed.error
}

function isParseError(error: unknown): error is ParseError {
    return (
        typeof error === "object" &&
        error !== null &&
        "message" in error &&
        "type" in error
    )
}

// Extract unique errors per field
function extractFieldErrors(errors: z.ZodError[]): string[][] {
    return errors.map((err) => {
        const fieldErrors = new Map<string, string>()

        err.errors.forEach(({ message, path }) => {
            const field = path.join(".") // Convertir path en string

            if (!fieldErrors.has(field)) {
                fieldErrors.set(field, message)
            }
        })

        return Array.from(fieldErrors.values())
    })
}

// Count error ocurrances
function countErrorOccurrences(
    fieldErrorList: string[][]
): Map<string, number> {
    const errorCountMap = new Map<string, number>()

    fieldErrorList.flat().forEach((errorMessage) => {
        errorCountMap.set(
            errorMessage,
            (errorCountMap.get(errorMessage) || 0) + 1
        )
    })

    return errorCountMap
}

// Generate final errors array
function generateErrorsArray(errorCountMap: Map<string, number>): string[] {
    return Array.from(errorCountMap.entries()).map(([message, count]) =>
        count > 1 ? `${message} (${count} ocurrencias)` : message
    )
}

export async function parseCSV(
    filePath: string
): Promise<ApiStudentToCreate[]> {
    try {
        // Leer archivo desde el servidor
        const csvText = fs.readFileSync(filePath, "utf-8")

        return new Promise((resolve, reject) => {
            Papa.parse<CsvRow>(csvText, {
                header: true,
                skipEmptyLines: true,
                complete: (result: ParseResult<CsvRow>) => {
                    if (result.errors.length > 0) {
                        reject(
                            new Error(
                                `Error en el CSV: ${result.errors[0].message}`
                            )
                        )
                        return
                    }

                    const parsedResults = result.data.map(processCSVRow)

                    // Format error message
                    const errors = parsedResults.filter(
                        (res) => res instanceof z.ZodError
                    ) as z.ZodError[]

                    if (errors.length > 0) {
                        const fieldErrorList = extractFieldErrors(errors)
                        const errorCountMap =
                            countErrorOccurrences(fieldErrorList)
                        const errorsArr = generateErrorsArray(errorCountMap)

                        reject(new CustomCSVError(errorsArr))
                        return
                    }

                    // Filter and return only valid results
                    const validStudents = parsedResults.filter(
                        (res) => !(res instanceof z.ZodError)
                    ) as ApiStudentToCreate[]

                    resolve(validStudents)
                },
                error: (error: unknown) => {
                    if (isParseError(error)) {
                        console.error("Error en PapaParse:", error)
                        reject(
                            new Error(
                                `Error en el parseo CSV: ${error.message}`
                            )
                        )
                    } else {
                        console.error(
                            "Error desconocido en el parseo CSV:",
                            error
                        )
                        reject(
                            new Error(
                                "Error desconocido en el procesamiento del CSV"
                            )
                        )
                    }
                },
            })
        })
    } catch (error) {
        console.error("Error leyendo el archivo CSV en el servidor:", error)
        throw new Error("Error procesando el CSV en el servidor.")
    }
}
