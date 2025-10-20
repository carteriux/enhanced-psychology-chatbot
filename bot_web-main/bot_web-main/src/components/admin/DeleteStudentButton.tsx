"use client"

import { Trash2 } from "lucide-react"
import { useTransition } from "react"
import { deleteStudentById } from "@/actions/student"
import { useRouter } from "next/navigation"

export default function DeleteStudentButton({ id }: { id: number }) {
    const [isPending, startTransition] = useTransition()
    const router = useRouter()

    const handleDelete = () => {
        startTransition(async () => {
            const result = await deleteStudentById(id)

            if (result.success) {
                router.refresh()
            }
        })
    }

    return (
        <button
            type="button"
            onClick={handleDelete}
            disabled={isPending}
            className={`bg-danger hover:bg-danger/80 focus:bg-danger/80 w-full sm:w-fit flex flex-row items-center justify-center gap-2 text-[12px] text-white px-4 py-[2px] rounded-full focus:outline-none focus:ring-accent focus:ring-2 focus:ring-offset-2 ${
                isPending ? "opacity-50 cursor-not-allowed" : ""
            }`}
            aria-label="Eliminar usuario"
            title="Eliminar usuario"
        >
            <Trash2 size={16} />
            <span>{isPending ? "Eliminando..." : "Eliminar"}</span>
        </button>
    )
}
