"use client"
import { ReactNode } from "react"
import { DynamicIcon } from "lucide-react/dynamic"

type IconName = "arrow-right"

interface ButtonProps extends React.ButtonHTMLAttributes<HTMLButtonElement> {
    children: ReactNode
    callback?: () => void
    iconName?: IconName
    size?: "sm" | "md"
    color?: "accent" | "danger"
}

export default function Button({
    children,
    callback,
    iconName,
    size = "md",
    color = "accent",
    ...props
}: ButtonProps) {
    const handleClick = () => {
        if (callback) callback()
    }

    const styles = {
        base: "font-montserrat w-fit font-medium text-white rounded-full flex items-center justify-between shadow-md hover:shadow-none",
        size: {
            md: "text-[20px] py-4 px-14 gap-4",
            sm: "text-[12px] py-2 px-4 gap-2",
        },
        color: {
            accent: "bg-accent hover:bg-accent/80",
            danger: "bg-danger hover:bg-danger/80",
        },
        iconSize: {
            md: 24,
            sm: 16,
        },
    }

    return (
        <button
            onClick={handleClick}
            className={`${styles.base} ${styles.size[size]} ${styles.color[color]}`}
            {...props}
        >
            <span className="flex-1 text-center">{children}</span>
            {iconName && (
                <DynamicIcon name={iconName} size={styles.iconSize[size]} />
            )}
        </button>
    )
}
