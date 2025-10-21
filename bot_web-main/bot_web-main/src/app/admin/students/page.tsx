"use client"

import NavButton from "@/components/ui/NavButton"
import { fetchUsers, fetchUsersByCohort, getCohorts } from "@/lib/api"
import { StudentPublicAdmin } from "@/types/student"
import DeleteStudentButton from "@/components/admin/DeleteStudentButton"
import RestoreActivitiesButton from "@/components/admin/RestoreActivitiesButton"
import CohortFilter from "@/components/admin/CohortFilter"
import { useEffect, useState } from "react"

export default function StudentsPage() {
    const [studentsData, setStudentsData] = useState<StudentPublicAdmin[] | null>(null)
    const [cohorts, setCohorts] = useState<string[]>([])
    const [selectedCohort, setSelectedCohort] = useState<string>("")
    const [isLoading, setIsLoading] = useState(true)

    useEffect(() => {
        loadInitialData()
    }, [])

    useEffect(() => {
        if (selectedCohort === "") {
            loadAllUsers()
        } else {
            loadUsersByCohort(selectedCohort)
        }
    }, [selectedCohort])

    const loadInitialData = async () => {
        try {
            const [users, cohortsData] = await Promise.all([
                fetchUsers(),
                getCohorts()
            ])
            setStudentsData(users)
            setCohorts(cohortsData)
        } catch (error) {
            console.error("Error loading initial data:", error)
        } finally {
            setIsLoading(false)
        }
    }

    const loadAllUsers = async () => {
        try {
            const users = await fetchUsers()
            setStudentsData(users)
        } catch (error) {
            console.error("Error loading users:", error)
        }
    }

    const loadUsersByCohort = async (cohort: string) => {
        try {
            const users = await fetchUsersByCohort(cohort)
            setStudentsData(users)
        } catch (error) {
            console.error("Error loading users by cohort:", error)
        }
    }

    if (isLoading) {
        return (
            <main className="flex justify-center items-center min-h-[400px]">
                <div className="text-lg">Cargando estudiantes...</div>
            </main>
        )
    }

    return (
        <main>
            <section
                aria-labelledby="students-heading"
                className="flex justify-center items-center my-[70px]"
            >
                <div className="max-w-[1200px] max-h-[calc(100vh-210px)] flex flex-col w-full bg-white border border-gray-2 p-6 rounded-lg shadow-lg">
                    {/* Header */}
                    <header className="flex flex-col sm:flex-row items-start sm:items-center justify-between mb-6 gap-4">
                        <h1
                            id="students-heading"
                            className="text-[24px] font-semibold font-montserrat text-secondary"
                        >
                            Registro de alumnos
                        </h1>
                        <div className="flex flex-col sm:flex-row items-start sm:items-center gap-4">
                            <CohortFilter 
                                cohorts={cohorts}
                                selectedCohort={selectedCohort}
                                onCohortChange={setSelectedCohort}
                            />
                            <NavButton
                                size="sm"
                                href="/admin/students/new"
                                iconName="arrow-right"
                            >
                                Alta de alumno
                            </NavButton>
                        </div>
                    </header>
                    {/* Scrollable container */}
                    <div className="overflow-y-auto flex-1">
                        {/* Students table */}
                        <table className="border-collapse table-auto w-full text-sm">
                            {/* Fixed table header */}
                            <thead className="sticky top-0 z-10">
                                <tr className="text-[14px] font-montserrat text-foreground bg-gray-1 border border-gray-2">
                                    <th
                                        scope="col"
                                        className="p-4 text-left font-semibold"
                                    >
                                        ID
                                    </th>
                                    <th
                                        scope="col"
                                        className="p-4 text-left font-semibold"
                                    >
                                        Nombre
                                    </th>
                                    <th
                                        scope="col"
                                        className="p-4 text-left font-semibold"
                                    >
                                        Apellidos
                                    </th>
                                    <th
                                        scope="col"
                                        className="p-4 text-left font-semibold"
                                    >
                                        Matrícula
                                    </th>
                                    <th
                                        scope="col"
                                        className="p-4 text-left font-semibold"
                                    >
                                        Cohorte
                                    </th>
                                    <th
                                        scope="col"
                                        className="p-4 text-left font-semibold"
                                    >
                                        Acciones
                                    </th>
                                </tr>
                            </thead>
                            {/* Scrollable table body */}
                            <tbody className="bg-white text-[14px] font-roboto">
                                {studentsData &&
                                    studentsData.map(
                                        ({
                                            id,
                                            firstName,
                                            middleName,
                                            lastName,
                                            enrollmentNumber,
                                            cohort,
                                        }: StudentPublicAdmin) => (
                                            <tr
                                                key={id}
                                                className="even:bg-gray-2/30 hover:bg-accent/30 transition-all"
                                            >
                                                <td className="border-b border-gray-2 p-4 font-normal">
                                                    {id}
                                                </td>
                                                <td className="border-b border-gray-2 p-4 font-normal">
                                                    {firstName}
                                                </td>
                                                <td className="border-b border-gray-2 p-4 font-normal">
                                                    {lastName}
                                                </td>
                                                <td className="border-b border-gray-2 p-4 font-normal">
                                                    {enrollmentNumber}
                                                </td>
                                                <td className="border-b border-gray-2 p-4 font-normal">
                                                    {cohort || "Sin asignar"}
                                                </td>
                                                <td className="border-b border-gray-2 p-4 font-normal">
                                                    <div className="flex flex-col sm:flex-row gap-2">
                                                        <RestoreActivitiesButton
                                                            id={id}
                                                            userName={`${firstName} ${lastName}`}
                                                        />
                                                        <DeleteStudentButton
                                                            id={id}
                                                        />
                                                    </div>
                                                </td>
                                            </tr>
                                        )
                                    )}
                            </tbody>
                        </table>
                    </div>
                </div>
            </section>
        </main>
    )
}
