import { ReactNode } from "react"
import { createPortal } from "react-dom"
import { motion } from "framer-motion"
import { X } from "lucide-react"
import { useEffect, useState } from "react"

interface ModalProps {
    isOpen: boolean
    onClose?: () => void
    children: ReactNode
}

export default function Modal({ isOpen, onClose, children }: ModalProps) {
    const [modalRoot, setModalRoot] = useState<HTMLDivElement | null>(null)

    useEffect(() => {
        const div = document.createElement("div")
        document.body.appendChild(div)
        setModalRoot(div)

        return () => {
            document.body.removeChild(div)
        }
    }, [])

    useEffect(() => {
        if (!isOpen) return

        const handleKeyDown = (event: KeyboardEvent) => {
            if (event.key === "Escape" && onClose) {
                onClose()
            }
        }

        document.addEventListener("keydown", handleKeyDown)
        return () => document.removeEventListener("keydown", handleKeyDown)
    }, [isOpen, onClose])

    const handleModalClick = (event: React.MouseEvent) => {
        event.stopPropagation()
    }

    const handleBackdropClick = () => {
        if (onClose) {
            onClose()
        }
    }

    if (!isOpen || !modalRoot) return null

    return createPortal(
        <div
            className="fixed inset-0 flex items-center justify-center bg-black/50 z-50"
            onClick={handleBackdropClick}
        >
            <motion.div
                role="dialog"
                aria-modal="true"
                className="bg-white p-6 rounded-lg shadow-lg mx-2 md:mx-0 max-w-lg w-full relative"
                onClick={handleModalClick}
                initial={{ opacity: 0, y: 50, scale: 0.9 }}
                animate={{ opacity: 1, y: 0, scale: 1 }}
                exit={{ opacity: 0, y: 50, scale: 0.9 }}
                transition={{ duration: 0.3, ease: "easeOut" }}
            >
                {onClose && (
                    <button
                        type="button"
                        className="absolute top-3 right-3 text-gray-500 hover:text-gray-800"
                        onClick={onClose}
                        aria-label="Cerrar modal"
                    >
                        <X size={16} strokeWidth={3} />
                    </button>
                )}
                {children}
            </motion.div>
        </div>,
        modalRoot
    )
}
