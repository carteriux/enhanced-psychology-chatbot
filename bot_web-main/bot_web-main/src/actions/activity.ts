"use server"

import { config } from "@/config/config"
import { fetchWithAuth } from "@/lib/fetch"

export async function endActivity(activityId: string, studentId: string) {
    try {
        const res = await fetchWithAuth(
            `${config.BACKEND_URL}/api/UserActivities/EndActivity`,
            {
                method: "POST",
                headers: {
                    "Content-Type": "application/json",
                },
                body: JSON.stringify({ id: activityId, idUser: studentId }),
            }
        )

        if (!res.ok) {
            throw new Error("No se pudo finalizar la actividad.")
        }

        return { success: true }
    } catch (error) {
        console.error("Error al finalizar la actividad:", error)
        return { success: false, message: "Error al finalizar la actividad." }
    }
}
