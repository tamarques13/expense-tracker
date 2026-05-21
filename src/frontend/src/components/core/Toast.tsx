import { useEffect, useState } from "react";

interface ToastProps {
    message: string;
    type?: "success" | "error";
    onDone: () => void;
}

export function Toast({ message, type = "success", onDone }: ToastProps) {
    const [visible, setVisible] = useState(true);

    useEffect(() => {
        const timer = setTimeout(() => {
            setVisible(false);
            setTimeout(onDone, 300); // wait for fade-out then unmount
        }, 2500);
        return () => clearTimeout(timer);
    }, [onDone]);

    return (
        <div className={`toast toast--${type} ${visible ? "toast--in" : "toast--out"}`}>
            {type === "success" ? (
                <svg width="16" height="16" viewBox="0 0 16 16" fill="none" stroke="currentColor" strokeWidth="2" strokeLinecap="round" strokeLinejoin="round">
                    <path d="M3 8l3.5 3.5L13 4" />
                </svg>
            ) : (
                <svg width="16" height="16" viewBox="0 0 16 16" fill="none" stroke="currentColor" strokeWidth="2" strokeLinecap="round">
                    <path d="M4 4l8 8M12 4l-8 8" />
                </svg>
            )}
            {message}
        </div>
    );
}