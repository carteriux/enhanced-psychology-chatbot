"use client"

import Input from "@/components/ui/Input"
import { login } from "@/actions/login"
import { useActionState } from "react"
import { useRouter } from "next/navigation"
import { useEffect } from "react"
import Button from "@/components/ui/Button"
import { useFormStatus } from "react-dom"

function SubmitButton() {
    const { pending } = useFormStatus()
    return (
        <Button type="submit" iconName="arrow-right" disabled={pending}>
            {pending ? "Cargando..." : "Continuar"}
        </Button>
    )
}

export default function LoginPage() {
    const router = useRouter()
    const [state, formAction] = useActionState(login, {
        success: false,
        error: "",
    })

    useEffect(() => {
        if (state.success && state.redirectUrl) {
            router.push(state.redirectUrl)
        }
    }, [state, router])

    return (
        <main>
            <section
                className="flex justify-center items-center mx-2 my-4 md:mt-[70px] md:mb-0 md:mx-0"
                aria-labelledby="login-heading"
            >
                <div className="md:max-w-[650px] w-full bg-white border border-gray-2 px-4 py-6 md:py-10 md:px-14 text-center rounded-lg shadow-lg">
                    <h1
                        id="login-heading"
                        className="text-[20px] md:text-[24px] font-montserrat text-secondary font-semibold tracking-wide mb-6 md:mb-4"
                    >
                        Iniciar sesión
                    </h1>
                    <p className="font-roboto text-[14px] md:text-[16px] mb-8">
                        Ingresa tu matrícula y contraseña para comenzar la
                        actividad de aprendizaje.
                    </p>
                    <form
                        action={formAction}
                        className="flex flex-col md:mx-[100px] gap-4 md:gap-6"
                    >
                        <Input
                            type="text"
                            name="login-id"
                            placeholder="Matrícula o correo"
                            required
                        />
                        <Input
                            type="password"
                            name="password"
                            placeholder="Contraseña"
                            required
                        />
                        {state.error && (
                            <p className="text-danger text-sm">{state.error}</p>
                        )}
                        <div className="flex items-center justify-center mt-2 md:mt-0">
                            <SubmitButton />
                        </div>
                    </form>
                </div>
            </section>
        </main>
    )
}
