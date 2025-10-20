"use client"
import { InputHTMLAttributes, useRef, useState } from "react"
import { Check, Upload } from "lucide-react"

interface FileInputProps extends InputHTMLAttributes<HTMLInputElement> {}

export default function FileInput({ className, id, ...rest }: FileInputProps) {
    const inputRef = useRef<HTMLInputElement>(null)
    const [fileName, setFileName] = useState<string>("")

    const handleClick = () => {
        inputRef.current?.click()
    }

    const handleFileChange = (event: React.ChangeEvent<HTMLInputElement>) => {
        const file = event.target.files?.[0]
        setFileName(file ? file.name : "")
    }

    return (
        <div className="flex flex-col items-center gap-2 font-roboto">
            <div
                tabIndex={0}
                onClick={handleClick}
                className={`border border-gray-2 text-[16px] rounded-lg px-4 py-[8.5px] flex items-center justify-between cursor-pointer hover:bg-gray-1/20 focus-within:ring-2 focus-within:ring-accent transition-all ${className} ${
                    fileName ? "" : "border-dashed hover:border-gray-3"
                }`}
            >
                <span
                    className={`truncate ${
                        fileName ? "text-foreground" : "text-gray-3"
                    }`}
                >
                    {fileName
                        ? "Archivo seleccionado"
                        : "Selecciona un archivo"}
                </span>
                {fileName ? (
                    <div className="ml-4 p-[2px] rounded-full transition-all border text-white bg-success border-success">
                        <Check size={12} />
                    </div>
                ) : (
                    <Upload size={20} className="ml-4 text-gray-3" />
                )}
            </div>
            {fileName && <p className="text-[14px] text-gray-3">{fileName}</p>}
            <input
                id={id}
                name={id}
                ref={inputRef}
                type="file"
                className="hidden"
                onChange={handleFileChange}
                {...rest}
            />
        </div>
    )
}
