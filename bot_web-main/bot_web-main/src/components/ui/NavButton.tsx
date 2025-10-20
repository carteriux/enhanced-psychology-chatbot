import { ReactNode } from "react"
import Link from "next/link"
import { DynamicIcon } from "lucide-react/dynamic"

type IconName = "arrow-right"

interface NavButtonProps {
    children: ReactNode
    href: string
    iconName?: IconName
    size?: "sm" | "md"
}

const styles = {
    base: "font-montserrat w-fit font-medium bg-accent hover:bg-accent/80 text-white rounded-full flex items-center justify-between shadow-md hover:shadow-none",
    size: {
        md: "text-[20px] py-4 px-14 gap-4",
        sm: "text-[12px] py-2 px-4 gap-2",
    },
    iconSize: {
        md: 24,
        sm: 16,
    },
}

export default function NavButton({
    children,
    href,
    iconName,
    size = "md",
}: NavButtonProps) {
    return (
        <Link href={href} className={`${styles.base} ${styles.size[size]}`}>
            <span className="flex-1 text-center">{children}</span>
            {iconName && (
                <DynamicIcon name={iconName} size={styles.iconSize[size]} />
            )}
        </Link>
    )
}
