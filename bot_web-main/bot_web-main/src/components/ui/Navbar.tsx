"use client"

import { usePathname } from "next/navigation"
import { ArrowLeft } from "lucide-react"
import Image from "next/image"
import Link from "next/link"
import { getPreviousPath } from "@/utils/navigation"
import { useTransition } from "react"
import { logout } from "@/actions/logout"

export default function Navbar() {
    const pathname = usePathname()
    const [isPending, startTransition] = useTransition()

    // Obtener la ruta anterior con valores reales
    const previousPath = getPreviousPath(pathname)
    const isAdminPage = pathname.includes("admin")

    return (
        <header>
            <nav className="flex items-center justify-center h-[70px] bg-primary w-full relative">
                {/* Usar Link en lugar de botón para mejor navegación */}
                {previousPath && (
                    <Link
                        href={previousPath}
                        className="w-fit text-white absolute left-5 md:left-10 hover:text-white/80"
                    >
                        <ArrowLeft size={24} />
                    </Link>
                )}
                <Image
                    src="/logo.svg"
                    alt="logo-img"
                    width={200}
                    height={52}
                    priority
                    sizes="(max-width: 768px) 150px, 200px"
                />
                {isAdminPage && (
                    <form
                        action={() => startTransition(() => logout())}
                        className="absolute right-5 md:right-10"
                    >
                        <button
                            type="submit"
                            className="font-montserrat font-semibold text-[16px] text-white hover:text-white/80"
                            disabled={isPending}
                        >
                            {isPending ? "Cerrando sesión..." : "Cerrar sesión"}
                        </button>
                    </form>
                )}
            </nav>
        </header>
    )
}
