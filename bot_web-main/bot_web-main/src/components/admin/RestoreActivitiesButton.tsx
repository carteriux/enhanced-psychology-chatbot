"use client"

import { RotateCcw } from "lucide-react"
import { useTransition } from "react"
import { resetUserActivities } from "@/lib/api"
import { useRouter } from "next/navigation"

export default function RestoreActivitiesButton({ 
    id, 
    userName 
}: { 
    id: number
    userName: string 
}) {
    const [isPending, startTransition] = useTransition()
    const router = useRouter()

    const handleRestore = () => {
        if (confirm(`¿Está seguro de que desea restaurar todas las actividades de ${userName}? Esta acción eliminará todo su progreso.`)) {
            startTransition(async () => {
                const result = await resetUserActivities(id)

                if (result.success) {
                    alert(`Actividades de ${userName} restauradas exitosamente.`)
                    router.refresh()
                } else {
                    alert(`Error al restaurar actividades: ${result.error}`)
                }
            })
        }
    }

    return (
        <button
            type="button"
            onClick={handleRestore}
            disabled={isPending}
            className={`bg-yellow-500 hover:bg-yellow-600 focus:bg-yellow-600 w-full sm:w-fit flex flex-row items-center justify-center gap-2 text-[12px] text-white px-4 py-[2px] rounded-full focus:outline-none focus:ring-accent focus:ring-2 focus:ring-offset-2 ${
                isPending ? "opacity-50 cursor-not-allowed" : ""
            }`}
            aria-label="Restaurar actividades del usuario"
            title="Restaurar actividades del usuario"
        >
            <RotateCcw size={16} />
            <span>{isPending ? "Restaurando..." : "Restaurar"}</span>
        </button>
    )
}