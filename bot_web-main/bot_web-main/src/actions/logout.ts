"use server"

import { cookies } from "next/headers"
import { redirect } from "next/navigation"

export async function logout() {
    const cookieStore = await cookies()
    cookieStore.delete("auth_token")
    cookieStore.delete("is_admin")
    cookieStore.delete("user_id")
    redirect("/login")
}
