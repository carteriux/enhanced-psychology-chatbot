export type Activity = {
    id: number
    typeId: number // idActivity
    userId: number // idUser
    name: string //activityName
    progress: number
    filePath: string
    count: number
    inProgress?: boolean // progress > 0 || progress < 100
    isLocked?: boolean // previous activity isCompleted ?
    isCompleted?: boolean // progress === 100
    description?: string
}

export type ActivityPublic = Omit<Activity, "userId" | "count" | "description">

export type ActivityPractice = Pick<
    Activity,
    "name" | "progress" | "count" | "typeId" | "description"
>

export type ApiActivity = {
    id: number
    idUser: number
    idActivity: number
    count: number
    progressPercentage: number
    startDateTime: string // Formato ISO 8601
    endDateTime: string | null // Puede ser `null`
    filePath: string
    activityName: string
}
