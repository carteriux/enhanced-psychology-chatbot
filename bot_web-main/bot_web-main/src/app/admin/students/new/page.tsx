"use client"

import Button from "@/components/ui/Button"
import FileInput from "@/components/ui/FileInput"
import Input from "@/components/ui/Input"
import { useActionState } from "react"
import { createStudent, bulkCreateStudents } from "@/actions/student"
import { useEffect } from "react"
import { useRouter } from "next/navigation"
import { useFormStatus } from "react-dom"

function SubmitButton({
    defaultText = "Registrar estudiante",
}: {
    defaultText?: string
}) {
    const { pending } = useFormStatus()
    return (
        <Button type="submit" size="sm" iconName="arrow-right">
            {pending ? "Cargando..." : defaultText}
        </Button>
    )
}

export default function NewStudentsPage() {
    const router = useRouter()
    // Single student
    const [state, formAction] = useActionState(createStudent, {
        success: false,
        errors: {} as Record<string, string>,
    })
    // Bulk register
    const [bulkState, bulkFormAction] = useActionState(bulkCreateStudents, {
        success: false,
        errors: null as string | null,
    })
    useEffect(() => {
        if (state.success || bulkState.success) {
            router.push("/admin/students")
        }
    }, [state, bulkState, router])

    return (
        <main>
            <section
                aria-labelledby="new-students-heading"
                className="flex justify-center items-center my-[70px]"
            >
                <div className="max-w-[1200px] w-full bg-white border border-gray-2 p-6 rounded-lg shadow-lg">
                    {/* Header */}
                    <header className="mb-6">
                        <h1
                            id="new-students-heading"
                            className="text-[24px] font-semibold font-montserrat text-secondary"
                        >
                            Registro de alumnos
                        </h1>
                    </header>
                    {/* Description */}
                    {/* <div className="my-6">
                        <h3 className="text-[16px] font-semibold font-montserrat text-gray-3 mb-2">
                            Lorem lorem ipsum
                        </h3>
                        <p className="text-[14px] font-roboto">
                            Lorem ipsum dolor sit amet, consectetur adipiscing
                            elit, sed do eiusmod tempor incididunt ut labore et
                            dolore magna aliqua. Ut enim ad minim veniam, quis
                            nostrud exercitation ullamco laboris.
                        </p>
                    </div> */}
                    {/* Register single student form */}
                    <form action={formAction}>
                        <div className="bg-gray-1 border border-gray-2 p-4">
                            <p className="text-[14px] font-semibold font-montserrat text-foreground">
                                Alta manual del alumno
                            </p>
                        </div>
                        <div className=" px-4 py-6 border border-gray-2">
                            {/* Input grid */}
                            <div className="grid grid-cols-3 gap-x-6 gap-y-4">
                                <div>
                                    <Input
                                        type="text"
                                        id="firstName"
                                        label="Nombre"
                                        name="firstName"
                                        placeholder="Nombre"
                                    />
                                    {state.errors.firstName && (
                                        <p className="text-danger text-sm mt-1">
                                            {state.errors.firstName}
                                        </p>
                                    )}
                                </div>

                                <div>
                                    <Input
                                        type="text"
                                        id="middleName"
                                        label="Apellido paterno"
                                        name="middleName"
                                        placeholder="Apellido paterno"
                                    />
                                    {state.errors.middleName && (
                                        <p className="text-danger text-sm mt-1">
                                            {state.errors.middleName}
                                        </p>
                                    )}
                                </div>

                                <div>
                                    <Input
                                        type="text"
                                        id="lastName"
                                        label="Apellido materno"
                                        name="lastName"
                                        placeholder="Apellido materno"
                                    />
                                    {state.errors.lastName && (
                                        <p className="text-danger text-sm mt-1">
                                            {state.errors.lastName}
                                        </p>
                                    )}
                                </div>

                                <div>
                                    <Input
                                        type="text"
                                        id="enrollmentNumber"
                                        label="Matrícula"
                                        name="enrollmentNumber"
                                        placeholder="Matrícula"
                                    />
                                    {state.errors.enrollmentNumber && (
                                        <p className="text-danger text-sm mt-1">
                                            {state.errors.enrollmentNumber}
                                        </p>
                                    )}
                                </div>

                                <div>
                                    <Input
                                        type="email"
                                        id="email"
                                        label="Correo electrónico"
                                        name="email"
                                        placeholder="Correo electrónico"
                                    />
                                    {state.errors.email && (
                                        <p className="text-danger text-sm mt-1">
                                            {state.errors.email}
                                        </p>
                                    )}
                                </div>

                                <div>
                                    <Input
                                        type="password"
                                        id="password"
                                        label="Contraseña"
                                        name="password"
                                        placeholder="Contraseña"
                                    />
                                    {state.errors.password && (
                                        <p className="text-danger text-sm mt-1">
                                            {state.errors.password}
                                        </p>
                                    )}
                                </div>
                            </div>
                            {/* General error message */}
                            {state.errors.general && (
                                <p className="text-danger text-sm mt-4 text-center">
                                    {state.errors.general}
                                </p>
                            )}
                            {/* Submit button */}
                            <div className="flex flex-row justify-end mt-6">
                                <SubmitButton />
                            </div>
                        </div>
                    </form>
                    {/* Register multiple students from file */}
                    <form action={bulkFormAction} className="mt-6">
                        <div className="bg-gray-1 border border-gray-2 p-4">
                            <p className="text-[14px] font-semibold font-montserrat text-foreground">
                                Alta masiva de alumnos
                            </p>
                        </div>
                        <div className=" px-4 py-6 border border-gray-2">
                            <div className="flex flex-col justify-center items-center">
                                <p className="text-[14] font-roboto text-center mb-4">
                                    Selecciona un archivo CSV para realizar el
                                    alta masiva de alumnos.
                                </p>
                                <FileInput
                                    id="file"
                                    accept=".csv"
                                    className="w-fit"
                                />
                            </div>
                            {bulkState.errors &&
                                (Array.isArray(bulkState.errors) ? (
                                    // Error list
                                    <div className="flex flex-col justify-center items-center">
                                        <p className="text-danger text-sm mt-4 text-center">
                                            Errores en el CSV:
                                        </p>
                                        <ul className="text-danger text-sm mt-4 text-left list-disc list-inside">
                                            {bulkState.errors.map(
                                                (err, idx) => (
                                                    <li key={idx}>{err}</li>
                                                )
                                            )}
                                        </ul>
                                    </div>
                                ) : (
                                    // General error message
                                    <p className="text-danger text-sm mt-4 text-center">
                                        {bulkState.errors}
                                    </p>
                                ))}
                            <div className="flex flex-row justify-center mt-6">
                                <SubmitButton defaultText="Confirmar registro" />
                            </div>
                        </div>
                    </form>
                </div>
            </section>
        </main>
    )
}
