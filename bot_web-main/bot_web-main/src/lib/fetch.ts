import { getUser } from "@/lib/auth"

export async function fetchWithAuth(
    input: RequestInfo | URL,
    init: RequestInit = {}
): Promise<Response> {
    const user = await getUser()

    // Throw error if no token available
    if (!user?.token) {
        throw new Error("Token no disponible al hacer fetch")
    }

    const headers = {
        ...(init.headers || {}),
        Authorization: `Bearer ${user.token}`,
    }

    return fetch(input, {
        ...init,
        headers,
    })
}
