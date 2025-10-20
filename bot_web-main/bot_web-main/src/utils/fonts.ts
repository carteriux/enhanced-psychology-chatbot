import { Roboto, Montserrat } from "next/font/google"

// Roboto config
export const roboto = Roboto({
    subsets: ["latin"],
    display: "swap",
    variable: "--font-roboto",
    weight: ["100", "300", "400", "500", "700", "900"],
})

// Monserrat config
export const montserrat = Montserrat({
    subsets: ["latin"],
    display: "swap",
    variable: "--font-montserrat",
    weight: ["100", "300", "400", "500", "600", "700", "900"],
})
