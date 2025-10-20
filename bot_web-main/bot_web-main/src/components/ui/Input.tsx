import { InputHTMLAttributes } from "react"

interface InputProps extends InputHTMLAttributes<HTMLInputElement> {
    label?: string
}

export default function Input({ className, label, id, ...rest }: InputProps) {
    return (
        <div className="flex flex-col gap-1 font-roboto">
            {label && (
                <label
                    htmlFor={id}
                    className="text-[12px] md:text-[14px] font-medium text-foreground"
                >
                    {label}
                </label>
            )}
            <input
                id={id}
                {...rest}
                className={`border border-gray-2 text-[14px] md:text-[16px] placeholder:text-gray-3 text-foreground rounded-lg px-4 py-[8.5px] focus:outline-none focus:ring-2 focus:ring-accent transition-all ${className}`}
            />
        </div>
    )
}
