(() => {
    const slider = document.getElementById("slider");
    const sliderInput = <HTMLInputElement>document.getElementById("slider-input");
    const sliderConfirmation = document.getElementById("slider-confirmation")
    const sliderItemNames = Array.from(document.getElementsByClassName("item-name"));

    // The browser remembers slider position on reload, so we reset it.
    sliderInput.value = "0";

    let ticketId = -1;

    Array.from(document.getElementsByClassName("item")).forEach(shopItem => {
        const id = shopItem.getAttribute("data-id");
        const name = shopItem.getAttribute("data-name");
        const button = shopItem.getElementsByTagName("button")[0];

        button.addEventListener("click", () => {
            ticketId = parseInt(id);
            sliderItemNames.forEach(itemName => (<HTMLElement>itemName).innerText = name);
            slider.classList.remove("d-none");
        });
    });

    document.getElementById("slider-close").addEventListener("click", () => {
        sliderInput.value = "0";
        sliderInput.classList.remove("d-none");
        sliderConfirmation.classList.add("d-none");
        slider.classList.add("d-none");
    });

    sliderInput.addEventListener("change", async () => {
        const value = parseInt(sliderInput.value);

        if (value < 100) {
            sliderInput.value = "0";
            return;
        }

        await useTicket();
    });

    const useTicket = async () => {
        const response = await fetch("/Inventory/UseTicket", {
            method: "POST",
            mode: "same-origin",
            headers: {
                "Accept": "application/json",
                "Content-Type": "application/json",
                "X-XSRF-TOKEN": (<HTMLInputElement>document.getElementsByName("__RequestVerificationToken")[0]).value
            },
            body: JSON.stringify(ticketId)
        });

        if (response.ok) {
            sliderInput.classList.add("d-none");
            sliderConfirmation.classList.remove("d-none");

            const ticketCount = document.getElementById(`ticket-count-${ticketId}`);
            const count = parseInt(ticketCount.innerText);

            if (count === 1) {
                // No more tickets, remove the element from the DOM tree.
                ticketCount.parentElement.parentElement.remove();
            } else {
                ticketCount.innerText = String(count - 1);
            }
        } else {
            // Showing alerts is a bad user experience, but we use it here for the sake of simplicity.
            alert("Something went wrong when attempting to use ticket.");
        }
    }
})();
