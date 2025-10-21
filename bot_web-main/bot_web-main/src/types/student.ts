export type Student = {
    id: number
    fullname?: string // Puede ser opcional porque se genera dinámicamente
    imageUrl?: string // Puede ser opcional porque se genera dinámicamente
    firstName: string
    lastName: string
    middleName?: string // Es opcional porque puede no existir
    enrollmentNumber: string
    cohort?: string // Es opcional porque puede no existir
}

export type ApiStudent = {
    idUser: number
    email: string
    firstName: string
    lastName: string
    middleName?: string
    enrollmentNumber: string
    isFirstTime: boolean | null
    lastAccessDate: string | null
    isAdmin: boolean
    password?: string
    cohort?: string
}

export type ApiStudentToCreate = Omit<
    ApiStudent,
    "idUser" | "isFirstTime" | "lastAccessDate" | "isAdmin"
>

export type StudentPublicAdmin = Omit<Student, "fullname" | "imageUrl">

export type StudentPublic = Pick<
    Student,
    "fullname" | "imageUrl" | "firstName" | "lastName" | "enrollmentNumber"
>
