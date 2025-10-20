"use client"

import ChatInput from "@/components/chatbot/ChatInput"
import ChatMessage, { AILoadingMessage } from "@/components/chatbot/ChatMessage"
import { useEffect, useMemo, useRef, useState } from "react"
import { useModal } from "@/hooks/useModal"
import Modal from "@/components/ui/Modal"
import { useRouter, usePathname } from "next/navigation"
import {
    CompleteModal,
    FinishModal,
    PauseModal,
    WarningModal,
} from "@/components/chatbot/Modals"
import { useProgress } from "@/context/ProgressContext"
import { useAction } from "@/context/ActionContext"
import { sendActivityQuestion } from "@/lib/api"
import { endActivity } from "@/actions/activity"

interface Message {
    user: "AI" | "Student"
    message: string
}

export default function ChatbotPage() {
    const router = useRouter()
    const pathname = usePathname()
    const { setOnPause, setOnFinish } = useAction()
    const [modalLoading, setModalLoading] = useState(false)
    const [modalError, setModalError] = useState<string | null>(null)
    const [lockInput, setLockInput] = useState(false)
    const { studentId, activityId } = useMemo(() => {
        const segments = pathname.split("/")
        const studentIndex = segments.indexOf("students")
        const activitiesIndex = segments.indexOf("activities")

        return {
            studentId: studentIndex !== -1 ? segments[studentIndex + 1] : null,
            activityId:
                activitiesIndex !== -1 ? segments[activitiesIndex + 1] : null,
        }
    }, [pathname])

    const LOCAL_STORAGE_KEY = `chatbot_messages_${studentId}_${activityId}`

    const { currentQuestion, setCurrentQuestion, totalQuestions } =
        useProgress()
    const [loading, setLoading] = useState(false)
    const [messages, setMessages] = useState<Message[]>([])

    // Load persisted messages from localStorage
    useEffect(() => {
        const savedMessages = localStorage.getItem(LOCAL_STORAGE_KEY)
        if (savedMessages) {
            setMessages(JSON.parse(savedMessages))
        }
    }, [])

    const {
        isOpen: pauseModalIsOpen,
        openModal: openPauseModal,
        closeModal: closePauseModal,
    } = useModal()
    const {
        isOpen: finishModalIsOpen,
        openModal: openFinishModal,
        closeModal: closeFinishModal,
    } = useModal()
    const {
        isOpen: completeModalIsOpen,
        openModal: openCompleteModal,
        closeModal: closeCompleteModal,
    } = useModal()
    const {
        isOpen: warningModalIsOpen,
        openModal: openWarningModal,
        closeModal: closeWarningModal,
    } = useModal()

    const chatContainerRef = useRef<HTMLDivElement>(null)

    useEffect(() => {
        if (chatContainerRef.current) {
            chatContainerRef.current.scrollTop =
                chatContainerRef.current.scrollHeight
        }
    }, [messages])

    const onPause = () => {
        openPauseModal()
    }

    const onConfirmPause = () => {
        closePauseModal()
        router.push(`description`)
    }

    const onFinish = () => {
        openFinishModal()
    }

    useEffect(() => {
        setOnPause(() => onPause)
        setOnFinish(() => onFinish)
    }, [])

    const onConfirmFinish = async () => {
        setModalLoading(true)
        setModalError(null)

        let shouldCloseModal = true

        try {
            if (activityId && studentId) {
                const res = await endActivity(activityId, studentId)

                if (!res.success) {
                    setModalError(
                        "Ocurrió un error inesperado. Vuelve a intentar más tarde."
                    )
                    shouldCloseModal = false
                    return
                }

                router.push(`result`)
            }
        } catch (error) {
            console.error("Error al finalizar actividad", error)
            setModalError(
                "Ocurrió un error inesperado. Vuelve a intentar más tarde."
            )
            shouldCloseModal = false
        } finally {
            setLockInput(true) // Disable chat input
            setModalLoading(false)

            // Only close if there is no error
            if (shouldCloseModal) {
                handleCloseFinishModal()
            }
        }
    }

    const handleCloseFinishModal = () => {
        setModalError(null)
        closeFinishModal()
    }

    const onComplete = () => {
        openCompleteModal()
    }

    const onConfirmComplete = async () => {
        setModalLoading(true)
        setModalError(null)

        let shouldCloseModal = true

        try {
            if (activityId && studentId) {
                const res = await endActivity(activityId, studentId)

                if (!res.success) {
                    setModalError(
                        "Ocurrió un error inesperado. Vuelve a intentar más tarde."
                    )
                    shouldCloseModal = false
                    return
                }

                router.push(`result`)
            }
        } catch (error) {
            console.error("Error al finalizar actividad", error)
            setModalError(
                "Ocurrió un error inesperado. Vuelve a intentar más tarde."
            )
            shouldCloseModal = false
        } finally {
            setLockInput(true) // Disable chat input
            setModalLoading(false)

            // Only close if there is no error
            if (shouldCloseModal) {
                handleCloseCompleteModal()
            }
        }
    }

    const handleCloseCompleteModal = () => {
        setModalError(null)
        closeCompleteModal()
    }

    const onWarning = () => {
        openWarningModal()
    }

    const onConfirmWarning = () => {
        closeWarningModal()
    }

    const handleMessage = async (message: string) => {
        setMessages((prev: Message[]) => {
            const newMessages: Message[] = [
                ...prev,
                { user: "Student" as const, message },
            ]
            localStorage.setItem(LOCAL_STORAGE_KEY, JSON.stringify(newMessages)) // Store in localStorage
            return newMessages
        })

        setLoading(true)

        if (!activityId || !studentId) {
            console.error("Activity ID or Student ID is missing")
            return
        }

        try {
            const response = await sendActivityQuestion({
                activityId,
                studentId,
                question: message,
            })

            if (response.success && response.answer) {
                setMessages((prev: Message[]) => {
                    const newMessages: Message[] = [
                        ...prev,
                        {
                            user: "AI" as const,
                            message:
                                response.answer ?? "No se obtuvo respuesta.",
                        },
                    ]
                    localStorage.setItem(
                        LOCAL_STORAGE_KEY,
                        JSON.stringify(newMessages)
                    ) // Guardar en localStorage
                    return newMessages
                })
                // Last 10 interactions
                if (currentQuestion + 1 === totalQuestions - 10) {
                    onWarning() // Trigger warning modal
                }
                // Last interaction
                if (currentQuestion + 1 === totalQuestions) {
                    setCurrentQuestion(totalQuestions)
                    onComplete()
                } else {
                    setCurrentQuestion(currentQuestion + 1)
                }
            } else {
                setMessages((prev: Message[]) => {
                    const newMessages: Message[] = [
                        ...prev,
                        {
                            user: "AI" as const,
                            message: "No se pudo obtener una respuesta.",
                        },
                    ]
                    localStorage.setItem(
                        LOCAL_STORAGE_KEY,
                        JSON.stringify(newMessages)
                    ) // Guardar en localStorage
                    return newMessages
                })
            }
        } catch (error) {
            console.error("Error in chat:", error)
            setMessages((prev: Message[]) => {
                const newMessages: Message[] = [
                    ...prev,
                    {
                        user: "AI" as const,
                        message: "Hubo un error al procesar tu mensaje.",
                    },
                ]
                localStorage.setItem(
                    LOCAL_STORAGE_KEY,
                    JSON.stringify(newMessages)
                ) // Guardar en localStorage
                return newMessages
            })
        } finally {
            setLoading(false)
        }
    }

    return (
        <section className="flex flex-col h-full">
            {/* Chat content */}
            <article
                ref={chatContainerRef}
                role="log"
                className="flex flex-col gap-6 w-full overflow-y-auto pr-2 max-h-[calc(100%-67px-24px)] scroll-smooth"
            >
                {messages.map(({ message, user }, index) => (
                    <ChatMessage key={index} message={message} user={user} />
                ))}
                {loading && <AILoadingMessage />}
            </article>
            {/* Chat input */}
            <div className="absolute bottom-0 left-0 w-full px-6 pb-6">
                <ChatInput
                    disabled={
                        currentQuestion === totalQuestions ||
                        loading ||
                        lockInput
                    }
                    onSendMessage={handleMessage}
                />
            </div>
            {/* Pause Modal */}
            <Modal isOpen={pauseModalIsOpen} onClose={closePauseModal}>
                <PauseModal
                    onConfirm={onConfirmPause}
                    onClose={closePauseModal}
                />
            </Modal>
            {/* Stop Modal */}
            <Modal
                isOpen={finishModalIsOpen}
                onClose={modalLoading ? undefined : handleCloseFinishModal}
            >
                <FinishModal
                    isLoading={modalLoading}
                    errorMessage={modalError}
                    onConfirm={onConfirmFinish}
                    onClose={handleCloseFinishModal}
                />
            </Modal>
            {/* Complete Modal */}
            <Modal
                isOpen={completeModalIsOpen}
                onClose={
                    modalError !== null ? handleCloseCompleteModal : undefined
                }
            >
                <CompleteModal
                    isLoading={modalLoading}
                    errorMessage={modalError}
                    onConfirm={onConfirmComplete}
                />
            </Modal>
            {/* Warning Modal */}
            <Modal isOpen={warningModalIsOpen} onClose={closeWarningModal}>
                <WarningModal onConfirm={onConfirmWarning} />
            </Modal>
        </section>
    )
}
