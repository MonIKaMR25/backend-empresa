console.log("Custom Swagger script loaded.");

function init() {
    function addPutButton() {
        const putOp = document.querySelector('.opblock.opblock-put');
        if (!putOp) return;

        if (putOp.querySelector('.custom-put-btn')) return;

        const btn = document.createElement('button');
        btn.textContent = "Cargar datos de ejemplo";
        btn.className = "custom-put-btn";
        btn.style.margin = "10px";
        btn.style.padding = "8px 12px";
        btn.style.background = "#4CAF50";
        btn.style.color = "white";
        btn.style.border = "none";
        btn.style.cursor = "pointer";

        btn.onclick = () => {
            const textarea = putOp.querySelector('textarea');
            if (textarea) {
                textarea.value = JSON.stringify({
                    id: 1,
                    nombre: "Ejemplo PUT",
                    telefono: "555-123-4567",
                    correo: "ejemplo@correo.com"
                }, null, 2);
            }
        };

        const header = putOp.querySelector('.opblock-summary');
        if (header) header.appendChild(btn);
    }

    const observer = new MutationObserver(() => addPutButton());
    observer.observe(document.body, { childList: true, subtree: true });

    addPutButton();
}

if (document.readyState === "loading") {
    document.addEventListener("DOMContentLoaded", init);
} else {
    init();
}
