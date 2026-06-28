// Calendario para elegir el rango de la estadía. Combina la disponibilidad que
// define el dueño (availableRanges) con las fechas ya reservadas (reservedRanges)
// para deshabilitar todo lo que no se puede reservar, y avisa al padre del rango
// elegido mediante onRangeSelected.
import { useState } from "react";
import { DayPicker } from "react-day-picker";
import "react-day-picker/dist/style.css";
import { eachDayOfInterval, parseISO, addYears, startOfToday, startOfMonth, addMonths } from "date-fns";
import "./AvailabilityCalendar.css";

export default function AvailabilityCalendar({ availableRanges, reservedRanges, onRangeSelected }) {
    const [selectedRange, setSelectedRange] = useState();

    // Expandimos cada rango a la lista de días individuales que abarca.
    // Días reservados (no se pueden seleccionar)
    const reservedDays = reservedRanges.flatMap(range =>
        eachDayOfInterval({
            start: parseISO(range.startDate),
            end: parseISO(range.endDate),
        })
    );

    // Días disponibles de la propiedad
    const availableDays = availableRanges.flatMap(range =>
        eachDayOfInterval({
            start: parseISO(range.startDate),
            end: parseISO(range.endDate),
        })
    );

    // Para deshabilitar lo "no disponible" calculamos, sobre el próximo año, qué
    // días NO están dentro de ningún rango disponible del dueño.
    const allPossibleDays = eachDayOfInterval({
        start: startOfToday(),
        end: addYears(startOfToday(), 1), // rango de 1 año
    });

    const unavailableDays = allPossibleDays.filter(
        day => !availableDays.some(av => av.toDateString() === day.toDateString())
    );

    const today = startOfToday();

    // Conjunto de días no seleccionables (lo usamos también para "cortar" rangos).
    const disabledDays = [...reservedDays, ...unavailableDays];

    // Igual que disabledDays pero agregando "antes de hoy", en el formato que
    // espera DayPicker para deshabilitar fechas pasadas.
    const disabledDaysForPicker = [{ before: today },...reservedDays, ...unavailableDays];

    // Maneja la selección del rango. La idea clave: si el usuario arrastra un
    // rango que cruza un día deshabilitado, recortamos la selección hasta el día
    // anterior al primer día bloqueado (no se permiten rangos con huecos).
    const handleSelectRange = (range) => {
        if (!range?.from) {
            setSelectedRange(range);
            onRangeSelected?.(range);
            return;
        }

        if (!range?.to) {
            setSelectedRange(range);
            onRangeSelected?.(range);
            return;
        }

        const allDays = eachDayOfInterval({ start: range.from, end: range.to });

        // Buscamos el primer día bloqueado dentro del rango elegido (si lo hay).
        const firstDisabled = allDays.find(day =>
            disabledDays.some(d => d.toDateString() === day.toDateString())
        );

        if (firstDisabled) {
            // Recortamos la salida al día previo al primer bloqueo.
            const validEnd = new Date(firstDisabled);
            validEnd.setDate(validEnd.getDate() - 1);

            // Solo aceptamos el rango recortado si sigue teniendo al menos un día.
            if (validEnd >= range.from) {
                setSelectedRange({ from: range.from, to: validEnd });
                onRangeSelected?.({ from: range.from, to: validEnd });
            } else {
                setSelectedRange(undefined);
                onRangeSelected?.(undefined);
            }
            return;
        }

        setSelectedRange(range);
        onRangeSelected?.(range);
    };

    return (
        <div className="border rounded p-2 bg-light">

            <DayPicker
                mode="range"
                selected={selectedRange}
                onSelect={handleSelectRange}
                disabled={disabledDaysForPicker}
                fromMonth={startOfMonth(today)} // primer mes visible
                toMonth={addMonths(today, 11)}  // último mes visible (11 meses más)
                numberOfMonths={2}              // 2 meses se muestran a la vez
                modifiers={{
                    checkIn: selectedRange?.from,
                    checkOut: selectedRange?.to,
                }}
                modifiersAttributes={{
                    checkIn: { title: "Día de check-in" },
                    checkOut: { title: "Día de check-out" },
                }}
                modifiersClassNames={{
                    selected: "selected-day",
                    disabled: "disabled-day",
                    range_start: "rdp-day_range_start",
                    range_end: "rdp-day_range_end",
                    range_middle: "rdp-day_range_middle",
                }}
                styles={{
                    caption: { color: "#333", fontWeight: "bold" },
                    head_cell: { color: "#555", fontWeight: 500 },
                    day: { borderRadius: 0 },
                }}
            />

            <div className="mt-2">
                <span className="badge bg-info me-2 small">Seleccionado</span>
                <span className="badge bg-secondary me-2 small">Reservado</span>
            </div>
        </div>
    );
}
