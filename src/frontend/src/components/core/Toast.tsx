import { useEffect, useState } from "react";

import type { ToastType } from "../../types/core";

import { SuccessIcon, FailureIcon } from "../Icons/ToastIcons"

interface ToastProps {
    message: string;
    type?: ToastType;
    onDone: () => void;
}

export function Toast({ message, type = "success", onDone }: ToastProps) {
    const [visible, setVisible] = useState(true);

    useEffect(() => {
        const fadeTimer = setTimeout(() => setVisible(false), 2200);
        const doneTimer = setTimeout(onDone, 2500);

        return () => {
            clearTimeout(fadeTimer);
            clearTimeout(doneTimer);
        };
    }, [onDone]);

    return (
        <div className={`toast toast--${type} ${visible ? "toast--in" : "toast--out"}`}>
            {type === "success" ? <SuccessIcon /> : <FailureIcon />}
            {message}
        </div>
    );
}