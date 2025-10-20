interface ProgressBarProps {
    currentQuestion: number
    totalQuestions: number
}

export default function ProgressBar({
    currentQuestion,
    totalQuestions,
}: ProgressBarProps) {
    const progressPercentage = (currentQuestion / totalQuestions) * 100

    return (
        <div className="w-full flex flex-row items-center gap-4">
            {/* Progress bar */}
            <div className="w-full sm:w-[350px] bg-gray-1 h-[20px] rounded-full overflow-hidden">
                {/* Progress indicator */}
                <div
                    className="h-[20px] bg-success rounded-full"
                    style={{ width: `${progressPercentage}%` }}
                ></div>
            </div>
            {/* Question indicator */}
            <p className="text-gray-3">
                {currentQuestion}/{totalQuestions}
            </p>
        </div>
    )
}
