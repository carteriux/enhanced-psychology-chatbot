import NavButton from "@/components/ui/NavButton"
import Image from "next/image"
import Link from "next/link"

export default function HomePage() {
    return (
        <>
            {/* Main content */}
            <main>
                <section className="flex flex-col justify-start items-center text-center min-h-[calc(100vh-70px-8px-8px-16px)] md:min-h-[calc(100vh-70px-100px)] p-2 my-4 md:mt-[100px] md:p-0">
                    <div className="md:max-w-[600px]">
                        <h1 className="text-[20px] md:text-[40px] text-secondary font-bold leading-tight mb-6 md:mb-4">
                            Simulación de Pacientes en Psicoterapia Cognitiva
                        </h1>
                        <p className="font-roboto text-[14px] md:text-[18px] text-secondary mb-4">
                            Formación práctica de los futuros psicoterapeutas
                            mediante el desarrollo de un agente digital
                            inteligente
                        </p>
                        <p className="font-roboto text-[14px] md:text-[18px] mb-8 md:mb-6">
                            Diseñado para replicar las dinámicas de una sesión
                            terapéutica, respondiendo a las intervenciones de
                            los estudiantes con reacciones y comportamientos
                            consistentes con las características de un paciente
                            con depresión.
                        </p>
                        <div className="flex justify-center items-center">
                            <NavButton href="/login">Iniciar sesión</NavButton>
                        </div>
                    </div>
                </section>
            </main>
            {/* Footer */}
            <footer className="flex flex-col px-2 py-4 md:p-4 w-full bg-primary">
                <div className="flex flex-col justify-center items-center mb-8 md:my-8 gap-6">
                    <Image
                        src={"/logo.svg"}
                        alt="logo-img"
                        width={200}
                        // TODO: Implement sizes config
                        height={52}
                    />
                    <p className="text-white text-[12px] md:text-[14px] font-roboto max-w-[370px] text-center">
                        Supermanzana 313 Manzana 47 Lote 4-01 Calle Madroño y
                        Fresno CP. 77533, Cancún, Quintana Roo.
                    </p>
                </div>
                <nav className="flex gap-4 items-center justify-center md:justify-end text-center md:text-right text-white font-roboto text-[14px] md:text-[16px]">
                    <Link href="/">Aviso de privacidad</Link>
                    <Link href="/">Términos y condiciones</Link>
                </nav>
            </footer>
        </>
    )
}
