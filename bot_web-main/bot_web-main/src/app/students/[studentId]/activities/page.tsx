import Image from "next/image"
import ActivityItem from "@/components/activities/ActivityItem"
import { fetchUserActivities, fetchStudentData } from "@/lib/api"
import Button from "@/components/ui/Button"
import { logout } from "@/actions/logout"

export default async function ActivitiesPage({
    params,
}: {
    params: Promise<{ studentId: string }>
}) {
    const studentId = (await params).studentId
    const studentData = await fetchStudentData(studentId)
    const activitiesData = await fetchUserActivities(studentId)

    if (!studentData) return <p>Error al cargar los datos del usuario</p> // TODO: Replace for error page --> Logout

    return (
        <main>
            <section
                className="flex justify-center items-center mx-2 my-4 md:mt-[70px] md:mb-0 md:mx-0"
                aria-labelledby="login-heading"
            >
                <article className="flex flex-col md:flex-row md:max-w-[658px] w-full gap-4">
                    {/* Profile side content */}
                    <div className="w-full md:w-1/3 bg-white border border-gray-2 p-6 rounded-lg shadow-lg h-full">
                        <h2 className="font-montserrat text-secondary font-semibold text-[16px] mb-6">
                            Mi perfil
                        </h2>
                        {/* TODO: Replace for image dynamically */}
                        <div className="flex md:flex-col items-center flex-row gap-4 md:gap-6">
                            <picture className="flex items-center justify-center rounded-full overflow-hidden size-[50px] md:size-[170px]">
                                <Image
                                    src={
                                        studentData.imageUrl ||
                                        "https://gravatar.com/avatar/123?f=y&s=250&r=pg&d=mp"
                                    }
                                    width={170}
                                    height={170}
                                    sizes="(max-width: 768px) 50px, 170px"
                                    alt="user-image"
                                />
                            </picture>
                            <div className="font-roboto text-left md:text-center">
                                <p className="text-[14px] font-semibold mb-2">
                                    {studentData.fullname}
                                </p>
                                <small className="block text-gray-3 text-[12px]">
                                    {studentData.enrollmentNumber}
                                </small>
                            </div>
                        </div>
                        {/* Logout button */}
                        <div className="flex justify-start md:justify-center mt-6">
                            <form action={logout}>
                                <Button type="submit" color="danger" size="sm">
                                    Cerrar sesión
                                </Button>
                            </form>
                        </div>
                    </div>
                    {/* Activities content */}
                    <div className="w-full md:w-2/3 bg-white border border-gray-2 py-6 rounded-lg shadow-lg h-full">
                        {/* Activities header */}
                        <div className="px-6">
                            <h2 className="font-montserrat text-secondary font-semibold text-[16px]">
                                Mis actividades
                            </h2>
                        </div>
                        {/* Activities list */}
                        <div className="font-roboto text-gray-3/60 text-[14px] my-6">
                            {activitiesData ? (
                                activitiesData.length > 0 ? (
                                    activitiesData.map((activity) => (
                                        <ActivityItem
                                            key={activity.id}
                                            href={`/students/${studentId}/activities/${activity.id}/description`}
                                            activity={activity}
                                            studentId={studentId}
                                        />
                                    ))
                                ) : (
                                    <div className="flex flex-row items-center justify-center border-t border-b border-gray-2 py-4 px-6">
                                        <p>No hay actividades cargadas</p>
                                    </div>
                                )
                            ) : (
                                <div className="flex flex-row items-center justify-center border-t border-b border-gray-2 py-4 px-6">
                                    <p>No hay actividades cargadas</p>
                                </div>
                            )}
                        </div>
                        {/* Activities instructions */}
                        <div className="px-6">
                            <h2 className="font-montserrat text-secondary font-semibold text-[16px] mb-4">
                                ¿Cómo funciona el agente?
                            </h2>
                            <p className="font-roboto text-[14px]">
                                Fabiola es un paciente virtual con depresión.
                                Puedes interactuar con Fabiola y hacerle
                                preguntas de la misma forma que lo harías con un
                                paciente.
                            </p>
                        </div>
                    </div>
                </article>
            </section>
        </main>
    )
}
