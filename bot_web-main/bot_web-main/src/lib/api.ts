import { config } from "@/config/config"
import { ActivityPractice, ActivityPublic, ApiActivity } from "@/types/activity"
import { ApiStudent, StudentPublicAdmin, StudentPublic } from "@/types/student"
import { ACTIVITY_DESCRIPTION_MAP } from "@/utils/constants"
import { fetchWithAuth } from "./fetch"

export async function fetchUserActivities(
    userId: string
): Promise<ActivityPublic[] | null> {
    try {
        const response = await fetchWithAuth(
            `${config.BACKEND_URL}/api/UserActivities/GetActivitiesByUserId/${userId}`,
            { cache: "no-store" }
        )

        if (!response.ok) {
            throw new Error("Failed to fetch user activities")
        }

        const responseData = await response.json()
        const data: ApiActivity[] = responseData.data

        const activityArr: ActivityPublic[] = data
            .map(
                ({
                    id,
                    progressPercentage,
                    filePath,
                    idActivity,
                    activityName,
                    endDateTime,
                }) => {
                    return {
                        id,
                        progress: progressPercentage,
                        filePath,
                        typeId: idActivity,
                        name: activityName,
                        isCompleted:
                            progressPercentage === 100 || endDateTime !== null,
                        inProgress:
                            progressPercentage > 0 && progressPercentage < 100,
                    }
                }
            )
            .sort((a: ActivityPublic, b: ActivityPublic) => a.typeId - b.typeId)
            .map((activity, index, array) => {
                // Locks activities if previous is not completed
                const isFirstActivity = index === 0
                const previousActivity = array[index - 1]

                return {
                    ...activity,
                    isLocked: isFirstActivity
                        ? false // First activity is never locked
                        : !previousActivity?.isCompleted,
                }
            })

        return activityArr
    } catch (error) {
        console.error("Error fetching user activities:", error)
        return null
    }
}

export async function fetchActivityById(
    activityId: string,
    studentId: string
): Promise<ActivityPractice | null> {
    if (!activityId || !studentId) {
        return null
    }

    try {
        const url = `${config.BACKEND_URL}/api/UserActivities/GetActivityById?id=${activityId}&idUser=${studentId}`
        const response = await fetchWithAuth(url, { cache: "no-store" })

        if (!response.ok) {
            throw new Error("Failed to fetch activity data")
        }

        const responseData = await response.json()

        if (!responseData || !responseData.data) {
            return null
        }

        const apiActivity: ApiActivity = responseData.data

        const activityDescription = ACTIVITY_DESCRIPTION_MAP.get(
            apiActivity.idActivity
        )

        const activity: ActivityPractice = {
            name: apiActivity.activityName,
            progress: apiActivity.progressPercentage,
            count: apiActivity.count ?? 0,
            typeId: apiActivity.idActivity,
            description: activityDescription,
        }

        return activity
    } catch (error) {
        console.error("Error fetching activity:", error)
        return null
    }
}

export async function fetchStudentData(
    userId: string
): Promise<StudentPublic | null> {
    try {
        const response = await fetchWithAuth(
            `${config.BACKEND_URL}/api/User/${userId}`,
            { cache: "no-store" }
        )

        if (!response.ok) {
            throw new Error("Failed to fetch user data")
        }

        const responseData = await response.json()
        const data = responseData.data.user

        // Generate fullname dynamically
        const fullname = [data.firstName, data.middleName, data.lastName]
            .filter(Boolean) // Delete `undefined` if no `middleName`
            .join(" ")

        const imageParams = new URLSearchParams({
            name: `${data.firstName} ${data.middleName}`,
            size: "250",
            background: "D9D9D9",
            color: "75767A",
        })

        const imageUrl = `https://ui-avatars.com/api/?${imageParams.toString()}`

        // Formatear la respuesta
        const formattedUser: StudentPublic = {
            fullname,
            imageUrl, // Dynamic avatar image
            firstName: data.firstName,
            lastName: data.lastName,
            enrollmentNumber: data.enrollmentNumber,
        }

        return formattedUser
    } catch (error) {
        console.error("Error fetching user data:", error)
        return null
    }
}

// TODO: Evaluate if needs to be sent to actions
export async function sendActivityQuestion({
    activityId,
    studentId,
    question,
}: {
    activityId: string
    studentId: string
    question: string
}): Promise<{ success: boolean; answer?: string; error?: string }> {
    try {
        const response = await fetchWithAuth(
            `${config.BACKEND_URL}/api/UserActivities/ActivityQuestions`,
            {
                method: "POST",
                headers: { "Content-Type": "application/json" },
                body: JSON.stringify({
                    id: activityId,
                    idUser: studentId,
                    question,
                }),
            }
        )

        if (!response.ok) {
            throw new Error("Failed to send question")
        }

        const result = await response.json()
        const responseData = result.data

        const data = { success: true, answer: responseData.message }

        return data
    } catch (error) {
        console.error("Error sending question:", error)
        return { success: false, error: "Error en la conexión" }
    }
}

export async function fetchUsers(): Promise<StudentPublicAdmin[] | null> {
    try {
        const response = await fetchWithAuth(`${config.BACKEND_URL}/api/students`, {
            cache: "no-store",
        })

        if (!response.ok) {
            throw new Error("Failed to fetch users")
        }

        const responseData = await response.json()
        const data: ApiStudent[] = responseData.data.users

        const studentsArr: StudentPublicAdmin[] = data.map(
            ({ idUser, firstName, lastName, middleName, enrollmentNumber, cohort }) => {
                return {
                    id: idUser,
                    firstName,
                    lastName,
                    middleName,
                    enrollmentNumber,
                    cohort,
                }
            }
        )

        return studentsArr
    } catch (error) {
        console.error("Error fetching user activities:", error)
        return null
    }
}

export async function downloadActivityFile({
    id,
    idUser,
    fileName,
}: {
    id: number
    idUser: string
    fileName: string
}) {
    try {
        const apiUrl = `${
            config.BACKEND_URL
        }/api/UserActivities/GetFileActivity?id=${id}&idUser=${idUser}&fileName=${encodeURIComponent(
            fileName
        )}`
        const response = await fetchWithAuth(apiUrl)

        if (!response.ok) {
            throw new Error("No se pudo descargar el archivo.")
        }

        const blob = await response.blob()

        // Temp URL for document download
        const url = window.URL.createObjectURL(blob)
        const a = document.createElement("a")
        a.href = url
        a.download = fileName
        document.body.appendChild(a)
        a.click()
        a.remove()
        window.URL.revokeObjectURL(url)
    } catch (error) {
        console.error("Error al descargar el archivo:", error)
        throw new Error("Hubo un problema al descargar el archivo.")
    }
}

export async function fetchUsersByCohort(cohort?: string): Promise<StudentPublicAdmin[] | null> {
    try {
        const url = cohort 
            ? `${config.BACKEND_URL}/api/students?cohort=${encodeURIComponent(cohort)}`
            : `${config.BACKEND_URL}/api/students`
            
        const response = await fetchWithAuth(url, {
            cache: "no-store",
        })

        if (!response.ok) {
            throw new Error("Failed to fetch users by cohort")
        }

        const responseData = await response.json()
        const data: ApiStudent[] = responseData.data.users

        const studentsArr: StudentPublicAdmin[] = data.map(
            ({ idUser, firstName, lastName, middleName, enrollmentNumber, cohort }) => {
                return {
                    id: idUser,
                    firstName,
                    lastName,
                    middleName,
                    enrollmentNumber,
                    cohort,
                }
            }
        )

        return studentsArr
    } catch (error) {
        console.error("Error fetching users by cohort:", error)
        return null
    }
}

export async function resetUserActivities(userId: number): Promise<{ success: boolean; message?: string; error?: string }> {
    try {
        const response = await fetchWithAuth(
            `${config.BACKEND_URL}/api/UserActivities/ResetActivities`,
            {
                method: "POST",
                headers: { "Content-Type": "application/json" },
                body: JSON.stringify(userId),
            }
        )

        if (!response.ok) {
            throw new Error("Failed to reset user activities")
        }

        const result = await response.json()
        const responseData = result.data

        return { success: true, message: responseData.message }
    } catch (error) {
        console.error("Error resetting user activities:", error)
        return { success: false, error: "Error resetting activities" }
    }
}

export async function getCohorts(): Promise<string[]> {
    try {
        const response = await fetchWithAuth(`${config.BACKEND_URL}/api/students`, {
            cache: "no-store",
        })

        if (!response.ok) {
            throw new Error("Failed to fetch users")
        }

        const responseData = await response.json()
        const data: ApiStudent[] = responseData.data.users

        // Extract unique cohorts
        const cohorts = [...new Set(data.map(user => user.cohort).filter(Boolean))] as string[]
        
        return cohorts.sort()
    } catch (error) {
        console.error("Error fetching cohorts:", error)
        return []
    }
}
