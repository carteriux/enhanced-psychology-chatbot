export const config = {
    BACKEND_URL: process.env.NEXT_PUBLIC_BACKEND_URL || "http://localhost:4000",
    // JWT_SECRET: process.env.JWT_SECRET || "default_secret",
    COOKIE_SECURE: process.env.NODE_ENV === "production",
    COOKIE_MAX_AGE: 60 * 60 * 24, // 1 day
}
