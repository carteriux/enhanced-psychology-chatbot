"use server"
import { cookies } from "next/headers"
import { config } from "@/config/config"

export async function login(
    prevState: { success: boolean; error?: string },
    formData: FormData
) {
    const cookieStore = await cookies()
    const loginId = formData.get("login-id") as string
    const password = formData.get("password") as string

    try {
        const response = await fetch(
            `${config.BACKEND_URL}/api/Security/login`,
            {
                method: "POST",
                headers: { "Content-Type": "application/json" },
                body: JSON.stringify({ id: loginId, password }),
            }
        )

        const result = await response.json()

        console.log("Login result success: ", result.success)

        if (result.success && result.data?.token) {
            const { token, user } = result.data

            cookieStore.set("auth_token", token, {
                httpOnly: true,
                secure: config.COOKIE_SECURE,
                path: "/",
                maxAge: config.COOKIE_MAX_AGE,
            })

            // TODO: Delete when auth is properly handled
            cookieStore.set("is_admin", String(user.isAdmin), {
                httpOnly: true,
                secure: config.COOKIE_SECURE,
                path: "/",
                maxAge: config.COOKIE_MAX_AGE,
            })

            // Store user id for test-token flows
            cookieStore.set("user_id", String(user.idUser), {
                httpOnly: true,
                secure: config.COOKIE_SECURE,
                path: "/",
                maxAge: config.COOKIE_MAX_AGE,
            })

            const redirectUrl = user.isAdmin
                ? "/admin/students"
                : `/students/${user.idUser || 1}/activities`

            return {
                success: true,
                redirectUrl,
            }
        } else {
            return { success: false, error: result.error_Message }
        }
    } catch (error) {
        console.error("Error en login action: ", error)
        return { success: false, error: "Error en la conexión" }
    }
}
