"use server"

import { config } from "@/config/config"
import { parseCSV } from "@/utils/csv"
import { z } from "zod"
import { ApiStudentToCreate } from "@/types/student"
import fs from "fs"
import { CustomCSVError } from "@/utils/error"
import { fetchWithAuth } from "@/lib/fetch"

const studentSchema = z.object({
    firstName: z.string().min(1, "El nombre es obligatorio"),
    lastName: z.string().min(1, "Los apellidos son obligatorios"),
    middleName: z.string().optional(),
    enrollmentNumber: z.string().min(1, "La matrícula es obligatoria"),
    email: z.string().email("El correo electrónico no es válido"),
    password: z
        .string()
        .min(6, "La contraseña debe tener al menos 6 caracteres"),
    cohort: z.string().optional(),
})

export async function createStudent(prevState: any, formData: FormData) {
    try {
        const formDataObj = Object.fromEntries(formData) as Record<
            string,
            string
        >

        const validatedData: ApiStudentToCreate =
            studentSchema.parse(formDataObj)

        const response = await fetchWithAuth(`${config.BACKEND_URL}/api/User`, {
            method: "POST",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify(validatedData),
        })

        if (!response.ok) {
            const errorData = await response.json()
            throw new Error(
                errorData.message || "Error en el registro del usuario"
            )
        }

        return { success: true, errors: {} }
    } catch (error) {
        console.error("Error registrando estudiante:", error)

        if (error instanceof z.ZodError) {
            const formattedErrors = error.errors.reduce((acc, curr) => {
                acc[curr.path[0]] = curr.message
                return acc
            }, {} as Record<string, string>)

            return { success: false, errors: formattedErrors }
        }

        return {
            success: false,
            errors: { general: "Hubo un problema en el registro." },
        }
    }
}

export async function bulkCreateStudents(prevState: any, formData: FormData) {
    const file = formData.get("file") as File
    if (!file || file.size === 0) {
        return {
            success: false,
            errors: "Debes seleccionar un archivo CSV.",
        }
    }

    // (Temp) Store file on server
    const tempPath = `/tmp/${file.name}`
    try {
        const buffer = Buffer.from(await file.arrayBuffer())
        fs.writeFileSync(tempPath, buffer)
    } catch (error) {
        console.error(
            "Error al subir temporalmente el archivo al servidor: ",
            error
        )
        return {
            success: false,
            errors: "El archivo no pudo ser procesado.",
        }
    }

    // Process CSV file
    let students: ApiStudentToCreate[] = []
    try {
        students = await parseCSV(tempPath)

        if (students.length === 0) {
            return {
                success: false,
                errors: "El archivo está vacío o es inválido.",
            }
        }
    } catch (error) {
        let errorMessage: string | string[] =
            "Hubo un problema al procesar el archivo."

        if (error instanceof CustomCSVError) {
            errorMessage = error.errors
        } else if (error instanceof Error) {
            console.error("Error parsing CSV:", error)
            errorMessage = error.message
        }

        return {
            success: false,
            errors: errorMessage,
        }
    }

    // Send data to API
    try {
        const response = await fetchWithAuth(
            `${config.BACKEND_URL}/api/User/createmultipleusers`,
            {
                method: "POST",
                headers: { "Content-Type": "application/json" },
                body: JSON.stringify(students),
            }
        )

        if (!response.ok) {
            throw new Error("Error en el alta masiva de alumnos.")
        }

        return { success: true, errors: null }
    } catch (error) {
        console.error("Error al enviar datos para el alta masiva: ", error)
        return {
            success: false,
            errors: "Hubo un problema en el alta masiva.",
        }
    }
}

export async function deleteStudentById(id: number) {
    try {
        const res = await fetchWithAuth(
            `${config.BACKEND_URL}/api/User?id=${id}`,
            {
                method: "DELETE",
            }
        )

        if (!res.ok) {
            throw new Error("No se pudo eliminar el alumno.")
        }

        return { success: true }
    } catch (error) {
        console.error("Error eliminando alumno:", error)
        return {
            success: false,
            message: "Hubo un error al eliminar al alumno.",
        }
    }
}
