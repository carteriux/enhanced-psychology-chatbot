import NavButton from "@/components/ui/NavButton"

export default async function PracticeResultsPage({
    params,
}: {
    params: Promise<{ studentId: string; activityId: string }>
}) {
    const studentId = (await params).studentId
    // const activityId = (await params).activityId

    return (
        <>
            {/* Main content */}
            <main className="md:mb-[70px]">
                <section className="flex flex-col justify-start items-center text-center mx-2 my-4 md:mt-[100px] md:mb-0 md:mx-0">
                    <div className="md:max-w-[658px]">
                        <h1 className="text-[20px] md:text-[40px] text-secondary text-balance font-montserrat font-bold leading-tight mb-6">
                            ¡Felicidades, has finalizado tu práctica!
                        </h1>
                        <p className="font-roboto text-[14px] md:text-[18px] mb-8">
                            Para ver el estado actualizado de tu progreso y
                            acceder a la información detallada de tus sesiones,
                            regresa al Dashboard. Desde allí, podrás consultar
                            tus avances y enviar los resultados directamente a
                            tu correo electrónico para mantener un registro
                            completo de tu entrenamiento.
                        </p>
                        <div className="flex justify-center items-center">
                            <NavButton
                                href={`/students/${studentId}/activities`}
                            >
                                Ir a Dashboard
                            </NavButton>
                        </div>
                    </div>
                </section>
            </main>
        </>
    )
}
