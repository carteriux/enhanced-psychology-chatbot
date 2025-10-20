import NavButton from "@/components/ui/NavButton"
import { fetchActivityById } from "@/lib/api"

export default async function ActivityDescriptionPage({
    params,
}: {
    params: Promise<{ studentId: string; activityId: string }>
}) {
    const activityId = (await params).activityId
    const studentId = (await params).studentId
    const activityData = await fetchActivityById(activityId, studentId)

    if (!activityData) return <p>Error al cargar los datos del usuario</p>

    return (
        <section>
            {/* Instructions */}
            <article className="mb-8">
                <h4 className="text-[16px] font-montserrat font-semibold text-secondary mb-4">
                    Descripción de la actividad
                </h4>
                <p className="text-[14px] font-roboto leading-5 text-justify">
                    {activityData.description}
                </p>
            </article>
            <article className="flex justify-center items-center">
                <NavButton href="chatbot" size="sm">
                    Iniciar actividad
                </NavButton>
            </article>
        </section>
    )
}
