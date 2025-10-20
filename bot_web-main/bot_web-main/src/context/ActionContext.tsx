"use client"

import { createContext, useContext, useState } from "react"

interface ActionContextType {
    onPause?: () => void
    setOnPause: (callback: () => void) => void
    onFinish?: () => void
    setOnFinish: (callback: () => void) => void
}

const ActionContext = createContext<ActionContextType | undefined>(undefined)

export function ActionProvider({ children }: { children: React.ReactNode }) {
    const [onPause, setOnPause] = useState<(() => void) | undefined>(undefined)
    const [onFinish, setOnFinish] = useState<(() => void) | undefined>(
        undefined
    )

    return (
        <ActionContext.Provider
            value={{ onPause, setOnPause, onFinish, setOnFinish }}
        >
            {children}
        </ActionContext.Provider>
    )
}

export function useAction() {
    const context = useContext(ActionContext)
    if (!context) {
        throw new Error("useAction debe usarse dentro de un ActionProvider")
    }
    return context
}
