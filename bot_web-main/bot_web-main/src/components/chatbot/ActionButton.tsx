"use client"
import { Pause, Check } from "lucide-react"

type IconName = "pause" | "check"

interface ButtonProps {
    iconName?: IconName
    callback?: () => void
}

const iconMap = {
    pause: Pause,
    check: Check,
}

const ariaLabelMap = {
    pause: "Pausar la práctica",
    check: "Finalizar la práctica",
}

const labelMap = {
    pause: "Pausar",
    check: "Finalizar",
}

const colorVariants = {
    check: "bg-success hover:bg-success/80 focus:bg-success/80",
    pause: "bg-secondary hover:bg-secondary/80 focus:bg-secondary/80",
}

export default function ActionButton({
    iconName = "pause",
    callback,
}: ButtonProps) {
    const IconComponent = iconMap[iconName]

    return (
        <button
            type="button"
            onClick={callback}
            className={`${colorVariants[iconName]} w-full sm:w-fit flex flex-row items-center justify-center gap-2 text-[12px] text-white px-4 py-[2px] rounded-full focus:outline-none focus:ring-accent focus:ring-2 focus:ring-offset-2`}
            aria-label={labelMap[iconName]}
            title={ariaLabelMap[iconName]}
        >
            <IconComponent size={16} />
            <span className="sm:hidden">{labelMap[iconName]}</span>
        </button>
    )
}
