"use client"
import Link from "next/link"
import { MessageSquare, Check, ArrowRight, Lock, Download } from "lucide-react"
import { ActivityPublic } from "@/types/activity"
import { downloadActivityFile } from "@/lib/api"

interface ActivityItemProps {
    href: string
    activity: ActivityPublic
    studentId: string
}

export default function ActivityItem({
    href,
    activity,
    studentId,
}: ActivityItemProps) {
    const handleDownload = async () => {
        if (!activity.filePath) return
        try {
            await downloadActivityFile({
                id: activity.id,
                idUser: studentId,
                fileName: activity.filePath,
            })
        } catch (error) {
            console.error("Error en la descarga:", error)
        }
    }

    return (
        <div className="flex flex-row items-center justify-between first-of-type:border-t border-b border-gray-2 py-4 px-6">
            {/* Icon & title */}
            <div
                className={`flex flex-row items-center gap-2 ${
                    activity.isCompleted || activity.isLocked
                        ? "text-gray-3/60"
                        : "text-gray-3"
                }`}
            >
                <MessageSquare size={18} />
                <p>Actividad {activity.typeId}</p>
            </div>

            {/* Progress & action buttons */}
            <div className="flex flex-row items-center gap-2">
                {/* TODO: Check if we need to fake the 100% on "isCompleted" */}
                <p>{activity.progress}%</p>{" "}
                <Link
                    // aria-disabled={!activity.inProgress || activity.isCompleted}
                    aria-disabled={activity.isCompleted}
                    className={`p-1 rounded-full transition-all border text-white ${
                        activity.isCompleted
                            ? "bg-success border-success pointer-events-none"
                            : activity.isLocked
                            ? "pointer-events-none bg-gray-3 border-gray-3"
                            : "bg-accent border-accent hover:bg-accent/80 hover:border-accent/80"
                    }`}
                    href={href}
                >
                    {activity.isCompleted ? (
                        <Check size={12} />
                    ) : activity.isLocked ? (
                        <Lock size={12} />
                    ) : (
                        <ArrowRight size={12} />
                    )}
                </Link>
                <button
                    disabled={!activity.isCompleted || !activity.filePath}
                    onClick={handleDownload}
                    className="bg-transparent border border-gray-1 p-1 rounded-full transition-all disabled:bg-transparent disabled:hover:cursor-not-allowed disabled:text-gray-1 text-gray-3 hover:bg-gray-1"
                >
                    <Download size={12} strokeWidth={3} />
                </button>
            </div>
        </div>
    )
}
