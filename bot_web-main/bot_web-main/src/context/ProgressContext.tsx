"use client"

import { createContext, useContext, useEffect, useState } from "react"
import { fetchActivityById } from "@/lib/api"
import { formatCapitalization } from "@/utils/format"

interface ProgressContextType {
    currentQuestion: number
    setCurrentQuestion: (value: number) => void
    totalQuestions: number
    activityName: string
}

const ProgressContext = createContext<ProgressContextType | undefined>(
    undefined
)

export function ProgressProvider({
    children,
    studentId,
    activityId,
}: {
    children: React.ReactNode
    studentId: string
    activityId: string
}) {
    const [currentQuestion, setCurrentQuestion] = useState<number | null>(null)
    const [totalQuestions, setTotalQuestions] = useState<number>(60) // default
    const [activityName, setActivityName] = useState<string>("")

    useEffect(() => {
        if (!studentId || !activityId) {
            console.error("Faltan studentId o activityId en ProgressProvider")
            return
        }
        async function loadProgress() {
            try {
                const activityData = await fetchActivityById(
                    activityId,
                    studentId
                )
                if (activityData) {
                    const formattedActivityName = `Actividad ${
                        activityData.typeId
                    }: ${formatCapitalization(activityData.name)}`
                    setActivityName(formattedActivityName)
                    setCurrentQuestion(activityData.count)
                    setTotalQuestions(60) // TODO: Update when API returns value
                } else {
                    setCurrentQuestion(0)
                    setActivityName("Actividad no especificada")
                }
            } catch (error) {
                console.error("Error fetching initial progress:", error)
                setCurrentQuestion(0)
                setActivityName("Actividad no especificada")
            }
        }

        loadProgress()
    }, [studentId, activityId])

    // Loading
    if (currentQuestion === null) {
        return <p className="text-center text-gray-500">Cargando progreso...</p>
    }

    return (
        <ProgressContext.Provider
            value={{
                currentQuestion,
                setCurrentQuestion,
                totalQuestions,
                activityName,
            }}
        >
            {children}
        </ProgressContext.Provider>
    )
}

export function useProgress() {
    const context = useContext(ProgressContext)
    if (!context) {
        throw new Error("useProgress debe usarse dentro de un ProgressProvider")
    }
    return context
}
