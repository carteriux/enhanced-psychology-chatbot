"use server"
import { cookies } from "next/headers"
import jwt from "jsonwebtoken"

type JwtPayload = {
    IdUser: string
    Email: string
    EnrollmentNumber: string
    exp: number
}

export async function getUser() {
    const cookieStore = await cookies()

    const token = cookieStore.get("auth_token")?.value
    const isAdmin = cookieStore.get("is_admin")?.value === "true"

    if (!token) return null

    // For testing with "test-token", bypass JWT validation
    if (token === "test-token") {
        const userIdCookie = cookieStore.get("user_id")?.value
        return { token, isAdmin, userId: userIdCookie ?? "1" }
    }

    try {
        const decoded = jwt.decode(token) as JwtPayload

        return { token, isAdmin, userId: decoded.IdUser }
    } catch (error) {
        console.error("Token inválido:", error)
        return null
    }
}
