export class CustomCSVError extends Error {
    errors: string[]

    constructor(errors: string[]) {
        super("Errores en el CSV")
        this.errors = errors
    }
}
