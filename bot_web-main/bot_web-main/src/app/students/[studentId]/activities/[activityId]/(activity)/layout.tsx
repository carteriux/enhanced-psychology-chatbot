"use client"

import ProgressBar from "@/components/chatbot/ProgressBar"
import ActionButton from "@/components/chatbot/ActionButton"
import { ACTION_BUTTONS } from "@/utils/constants"
import { usePathname } from "next/navigation"
import { ProgressProvider, useProgress } from "@/context/ProgressContext"
import { useAction, ActionProvider } from "@/context/ActionContext"
import { use } from "react"
import { Cpu } from "lucide-react"

type ActivityParams = {
    studentId: string
    activityId: string
}

export default function ActivityLayout({
    children,
    params,
}: {
    children: React.ReactNode
    params: Promise<ActivityParams>
}) {
    const { studentId, activityId } = use(params)
    return (
        <ActionProvider>
            <ProgressProvider studentId={studentId} activityId={activityId}>
                <LayoutContent>{children}</LayoutContent>
            </ProgressProvider>
        </ActionProvider>
    )
}

function LayoutContent({ children }: { children: React.ReactNode }) {
    const { currentQuestion, totalQuestions, activityName } = useProgress()
    const { onPause, onFinish } = useAction()
    const pathname = usePathname()

    const isChatbotPage = pathname.endsWith("/chatbot")

    return (
        <main>
            <section
                // (Original) className="flex justify-center items-center mx-2 my-4 md:mt-[70px] md:mb-0 md:mx-0"
                className="flex justify-center items-center mx-2 my-4 md:mt-[35px] md:mb-0 md:mx-0"
                aria-labelledby="activity-layout"
            >
                <div className="max-w-[650px] w-full">
                    {/* Barra de progreso */}
                    <div className="flex flex-col sm:flex-row gap-2 md:gap-0 md:items-center md:justify-between mb-4">
                        <ProgressBar
                            currentQuestion={currentQuestion}
                            totalQuestions={totalQuestions}
                        />

                        {/* Botones de acción (Solo en chatbot) */}
                        {isChatbotPage && (
                            <div className="w-full sm:w-fit flex flex-row items-center gap-2">
                                <ActionButton
                                    callback={onPause}
                                    iconName={ACTION_BUTTONS.PAUSE}
                                />
                                <ActionButton
                                    callback={onFinish}
                                    iconName={ACTION_BUTTONS.FINISH}
                                />
                            </div>
                        )}
                    </div>
                    {/* Nombre de la avtividad */}
                    <div className="flex flex-col sm:flex-row items-center justify-start gap-4 bg-white border border-gray-2 p-6 rounded-lg shadow-lg mb-4">
                        <div className="bg-gray-1/60 text-gray-3 rounded-full p-4 md:p-2">
                            <Cpu className=" size-[32px] sm:size-[20px]" />
                        </div>
                        <p className="text-[16px] text-pretty font-montserrat font-semibold text-gray-3/60">
                            {activityName}
                        </p>
                    </div>
                    {/* Contenido dinámico */}
                    <div
                        className={
                            isChatbotPage
                                ? // (Original) ? "h-[calc(100vh-70px-24px-22px-8px-16px-16px-16px-16px-178px)] sm:h-[calc(100vh-70px-24px-16px-16px-16px-16px-86px-16px)] md:h-[calc(100vh-70px-70px-24px-16px-70px-16px-86px)] bg-white border border-gray-2 p-6 rounded-lg shadow-lg relative"
                                  "h-[calc(100vh-70px-24px-22px-8px-16px-16px-16px-16px-178px)] sm:h-[calc(100vh-70px-24px-16px-16px-16px-16px-86px-16px)] md:h-[calc(100vh-35px-35px-24px-16px-70px-16px-86px)] bg-white border border-gray-2 p-6 rounded-lg shadow-lg relative"
                                : "bg-white border border-gray-2 p-6 rounded-lg shadow-lg relative"
                        }
                    >
                        {children}
                    </div>
                </div>
            </section>
        </main>
    )
}
