(() => {
    const modal = new bootstrap.Modal(document.getElementById("buy-modal"));
    const buyError = document.getElementById("buy-error");
    const modalItemName = document.getElementById("item-name");
    const modalItemPrice = document.getElementById("item-price");

    let itemId = -1;

    Array.from(document.getElementsByClassName("item")).forEach(shopItem => {
        const id = shopItem.getAttribute("data-id");
        const name = shopItem.getAttribute("data-name");
        const price = shopItem.getAttribute("data-price");
        const button = shopItem.getElementsByTagName("button")[0];

        button.addEventListener("click", () => {
            itemId = parseInt(id);
            modalItemName.innerText = name;
            modalItemPrice.innerText = price;
            buyError.classList.add("d-none");
            modal.show();
        });
    });

    document.getElementById("buy-submit").addEventListener("click", async () => {
        const response = await fetch("/Shop/Buy", {
            method: "POST",
            mode: "same-origin",
            headers: {
                "Accept": "application/json",
                "Content-Type": "application/json",
                "X-XSRF-TOKEN": (<HTMLInputElement>document.getElementsByName("__RequestVerificationToken")[0]).value
            },
            body: JSON.stringify(itemId)
        });

        if (response.redirected) {
            console.log("Redirected!");
            window.location.href = response.url;
            return;
        }

        const message = await response.text();
        buyError.innerText = message;
        buyError.classList.remove("d-none");
    });
})();
