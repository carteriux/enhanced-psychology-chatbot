"use client"

interface CohortFilterProps {
    cohorts: string[]
    selectedCohort: string
    onCohortChange: (cohort: string) => void
}

export default function CohortFilter({ 
    cohorts, 
    selectedCohort, 
    onCohortChange 
}: CohortFilterProps) {
    return (
        <div className="flex flex-col gap-2">
            <label 
                htmlFor="cohort-filter" 
                className="text-sm font-medium text-gray-700"
            >
                Filtrar por cohorte:
            </label>
            <select
                id="cohort-filter"
                value={selectedCohort}
                onChange={(e) => onCohortChange(e.target.value)}
                className="px-3 py-2 border border-gray-300 rounded-md bg-white text-sm focus:outline-none focus:ring-2 focus:ring-accent focus:border-transparent"
            >
                <option value="">Todas las cohortes</option>
                {cohorts.map((cohort) => (
                    <option key={cohort} value={cohort}>
                        {cohort}
                    </option>
                ))}
            </select>
        </div>
    )
}