import { Cpu } from "lucide-react"

interface ChatMessageProps {
    user: "AI" | "Student"
    message: string
}

export default function ChatMessage({ user, message }: ChatMessageProps) {
    return user === "AI" ? (
        <AIChatMessage message={message} />
    ) : (
        <UserChatMessage message={message} />
    )
}

function AIChatMessage({ message }: { message: string }) {
    return (
        <div className="flex flex-col gap-2 w-fit max-w-[80%] sm:max-w-[66%]">
            <div className="flex items-center gap-1">
                <div className="bg-gray-1/60 text-gray-3 rounded-full p-1">
                    <Cpu size={12} />
                </div>
                <small className="text-[12px] font-semibold font-montserrat text-gray-3">
                    Fabiola
                </small>
            </div>
            <div className="px-6 py-2 rounded-lg font-roboto text-[14px] bg-gray-1/60 text-gray-3">
                {message}
            </div>
        </div>
    )
}

export function AILoadingMessage() {
    return (
        <div className="flex flex-col gap-2">
            <div className="flex items-center gap-1 w-full">
                <div className="bg-gray-1/60 text-gray-3 rounded-full p-1">
                    <Cpu size={12} />
                </div>
                <small className="text-[12px] font-semibold font-montserrat text-gray-3">
                    Fabiola
                </small>
            </div>
            <div className="flex items-center w-fit px-6 py-2 rounded-lg font-roboto text-[14px] bg-gray-1/60 animate-pulse h-[36px]">
                <span className="flex gap-1">
                    <span className="w-2 h-2 bg-gray-3 rounded-full animate-bounce [animation-delay:0s]"></span>
                    <span className="w-2 h-2 bg-gray-3 rounded-full animate-bounce [animation-delay:0.2s]"></span>
                    <span className="w-2 h-2 bg-gray-3 rounded-full animate-bounce [animation-delay:0.4s]"></span>
                </span>
            </div>
        </div>
    )
}

function UserChatMessage({ message }: { message: string }) {
    return (
        <div className="self-end w-fit max-w-[80%] sm:max-w-[66%]">
            <div className="px-6 py-2 rounded-lg font-roboto text-[14px] bg-primary text-white">
                {message}
            </div>
        </div>
    )
}
