import { SendHorizonal } from "lucide-react"
import { useState, useRef } from "react"

interface ChatInputProps {
    disabled?: boolean
    onSendMessage: (message: string) => void
}

export default function ChatInput({ onSendMessage, disabled }: ChatInputProps) {
    const [message, setMessage] = useState("")
    const textareaRef = useRef<HTMLTextAreaElement>(null)

    const handleSubmit = (e?: React.FormEvent) => {
        if (e) e.preventDefault()
        if (!message.trim()) return
        onSendMessage(message)
        setMessage("")
    }

    const handleKeyDown = (e: React.KeyboardEvent<HTMLTextAreaElement>) => {
        if (e.key === "Enter" && !e.shiftKey) {
            e.preventDefault()
            handleSubmit()
        }
    }

    return (
        <form
            onSubmit={handleSubmit}
            className="flex items-center border border-gray-2 rounded-lg px-4 py-[8.5px] focus-within:ring-2 focus-within:ring-accent transition-all w-full gap-4 mt-6"
        >
            <textarea
                disabled={disabled}
                ref={textareaRef}
                name="student-message"
                id="student-message"
                rows={2}
                placeholder="Escribe aquí..."
                className="w-full h-max resize-none outline-none disabled:bg-transparent"
                value={message}
                onChange={(e) => setMessage(e.target.value)}
                onKeyDown={handleKeyDown}
            />
            <button
                disabled={disabled}
                className="flex justify-center items-center p-[4px] bg-accent hover:bg-accent/80 disabled:bg-accent/80 text-white rounded-full"
                type="submit"
            >
                <SendHorizonal size={14} />
            </button>
        </form>
    )
}
