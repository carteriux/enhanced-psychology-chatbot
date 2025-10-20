import { NextRequest, NextResponse } from "next/server"
import { getUser } from "@/lib/auth"

export async function middleware(req: NextRequest) {
    const user = await getUser()

    if (!user) {
        return NextResponse.redirect(new URL("/login", req.url))
    }

    // Redirigir a admin si el usuario es administrador e intenta acceder a estudiantes
    if (user.isAdmin && req.nextUrl.pathname.startsWith("/students")) {
        return NextResponse.redirect(new URL("/admin", req.url))
    }

    // Redirigir a estudiantes si no es admin e intenta acceder a admin
    if (!user.isAdmin && req.nextUrl.pathname.startsWith("/admin")) {
        return NextResponse.redirect(
            new URL(`/students/${user.userId}/activities`, req.url)
        )
    }

    // Redirigir a la actividad correcta si el usuario intenta acceder a /students
    if (!user.isAdmin && req.nextUrl.pathname === "/students") {
        return NextResponse.redirect(
            new URL(`/students/${user.userId}/activities`, req.url)
        )
    }

    return NextResponse.next()
}

// Aplicar middleware en rutas protegidas
export const config = {
    matcher: ["/students/:path*", "/admin/:path*"],
}
