import { CircleAlert, LoaderCircle } from "lucide-react"

interface ModalProps {
    onConfirm: () => void
    onClose?: () => void
    isLoading?: boolean
    errorMessage?: string | null
}

export function PauseModal({ onConfirm, onClose }: ModalProps) {
    return (
        <>
            <h2 className="text-[16px] md:text-[18px] font-montserrat font-medium">
                ¿Estás seguro de que deseas pausar la práctica?
            </h2>
            {/* <p className="mt-2 text-[14px] font-roboto text-gray-3">
                Al cerrar la sesión lorem ipsum dolor sit amet, consectetur
                adipiscing elit.
            </p> */}
            <div className="flex flex-row items-end justify-end gap-4 mt-4">
                <button
                    onClick={onConfirm}
                    className="text-[12px] py-2 px-4 gap-2 bg-success hover:bg-success/80 text-white rounded-full shadow-md hover:shadow-none"
                >
                    Confirmar
                </button>
                <button
                    onClick={onClose}
                    className="text-[12px] py-2 px-4 gap-2 bg-danger hover:bg-danger/80 text-white rounded-full shadow-md hover:shadow-none"
                >
                    Cerrar
                </button>
            </div>
        </>
    )
}

export function FinishModal({
    onConfirm,
    onClose,
    isLoading,
    errorMessage,
}: ModalProps) {
    return (
        <>
            <h2 className="text-[16px] md:text-[18px] font-montserrat font-medium">
                ¿Estás seguro de que deseas finalizar la práctica?
            </h2>
            {/* <p className="mt-2 text-[14px] font-roboto text-gray-3">
                Al cerrar la sesión lorem ipsum dolor sit amet, consectetur
                adipiscing elit.
            </p> */}
            <div className="flex flex-row items-end justify-end gap-4 mt-4">
                <button
                    disabled={isLoading || errorMessage !== null}
                    onClick={onConfirm}
                    className="flex flex-row items-center text-[12px] py-2 px-4 gap-2 bg-success hover:bg-success/80 disabled:bg-success/80 text-white rounded-full shadow-md hover:shadow-none disabled:shadow-none"
                >
                    {isLoading ? "Cargando..." : "Confirmar"}
                    {isLoading && (
                        <LoaderCircle size={12} className="animate-spin" />
                    )}
                </button>
                <button
                    disabled={isLoading}
                    onClick={onClose}
                    className="text-[12px] py-2 px-4 gap-2 bg-danger hover:bg-danger/80 disabled:bg-danger/80 text-white rounded-full shadow-md hover:shadow-none disabled:shadow-none"
                >
                    Cerrar
                </button>
            </div>
            {errorMessage && <ErrorMessage message={errorMessage} />}
        </>
    )
}

export function CompleteModal({
    onConfirm,
    isLoading,
    errorMessage,
}: ModalProps) {
    return (
        <>
            <h2 className="text-[16px] md:text-[18px] font-montserrat font-medium">
                ¡Has finalizado la práctica!
            </h2>
            <p className="mt-2 text-[14px] font-roboto text-gray-3">
                Llegaste al límite de interacciones permitidas.
            </p>
            <div className="flex flex-row items-end justify-end gap-4 mt-4">
                <button
                    disabled={isLoading || errorMessage !== null}
                    onClick={onConfirm}
                    className="flex flex-row items-center text-[12px] py-2 px-4 gap-2 bg-success hover:bg-success/80 disabled:bg-success/80 text-white rounded-full shadow-md hover:shadow-none disabled:shadow-none"
                >
                    {isLoading ? "Cargando..." : "Continuar"}
                    {isLoading && (
                        <LoaderCircle size={12} className="animate-spin" />
                    )}
                </button>
            </div>
            {errorMessage && <ErrorMessage message={errorMessage} />}
        </>
    )
}

export function WarningModal({ onConfirm }: ModalProps) {
    return (
        <>
            <h2 className="text-[16px] md:text-[18px] font-montserrat font-medium">
                Estás llegando al límite de interacciones
            </h2>
            <p className="mt-2 text-[14px] font-roboto text-gray-3">
                A partir de este momento quedan 10 preguntas / interacciones. Se
                recomienda precaución.
            </p>
            <div className="flex flex-row items-end justify-end gap-4 mt-4">
                <button
                    onClick={onConfirm}
                    className="text-[12px] py-2 px-4 gap-2 bg-success hover:bg-success/80 text-white rounded-full shadow-md hover:shadow-none"
                >
                    Confirmar
                </button>
            </div>
        </>
    )
}

function ErrorMessage({ message }: { message: string }) {
    return (
        <div className="flex bg-danger/20 w-full px-2 py-1 rounded-md mt-4">
            <div className="flex flex-row items-center gap-1">
                <CircleAlert size={12} className="text-danger" />
                <p className="text-[12px] text-danger">{message}</p>
            </div>
        </div>
    )
}
