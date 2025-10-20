export const ROUTE_MAP: Record<string, string | null> = {
    "/login": "/",
    // Admin
    "/admin/students": "/login",
    "/admin/students/new": "/admin/students",
    // Student
    "/students/[studentId]/activities": "/login",
    "/students/[studentId]/activities/[activityId]/description":
        "/students/[studentId]/activities",
    "/students/[studentId]/activities/[activityId]/chatbot":
        "/students/[studentId]/activities/[activityId]/description",
}

export function getPreviousPath(pathname: string): string | null {
    // Extraer valores dinámicos reales de la URL
    const studentMatch = pathname.match(/\/students\/(\d+)/)
    const activityMatch = pathname.match(/\/activities\/(\d+)/)

    const studentId = studentMatch ? studentMatch[1] : null
    const activityId = activityMatch ? activityMatch[1] : null

    // Normalizar la ruta en base a `ROUTE_MAP`
    const normalizedPath = pathname
        .replace(/\/students\/\d+/g, "/students/[studentId]")
        .replace(/\/activities\/\d+/g, "/activities/[activityId]")

    const previousPathTemplate = ROUTE_MAP[normalizedPath] || null

    if (!previousPathTemplate) return null

    // Restaurar valores dinámicos en la ruta anterior
    return previousPathTemplate
        .replace("[studentId]", studentId || "")
        .replace("[activityId]", activityId || "")
}
