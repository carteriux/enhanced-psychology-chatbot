import type { Config } from "tailwindcss"

export default {
    content: [
        "./src/pages/**/*.{js,ts,jsx,tsx,mdx}",
        "./src/components/**/*.{js,ts,jsx,tsx,mdx}",
        "./src/app/**/*.{js,ts,jsx,tsx,mdx}",
    ],
    theme: {
        extend: {
            fontFamily: {
                roboto: ["var(--font-roboto)"],
                montserrat: ["var(--font-montserrat)"],
            },
            colors: {
                background: "var(--background)",
                foreground: "var(--foreground)",
                primary: "#336699",
                secondary: "#17376E",
                accent: "#6EC1E4",
                success: "#6EE4B3",
                danger: "#E46E6E",
                gray: {
                    1: "#D9D9D9",
                    2: "#CBD0DC",
                    3: "#75767A",
                },
            },
        },
    },
    plugins: [],
} satisfies Config
