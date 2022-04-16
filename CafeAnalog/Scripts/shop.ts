(() => {
    const modal = new bootstrap.Modal(document.getElementById("buy-modal"));
    const buyResponse = document.getElementById("buy-response");
    const modalItemName = document.getElementById("item-name");
    const modalItemPrice = document.getElementById("item-price");

    let itemId = -1;

    Array.from(document.getElementsByClassName("shop-item")).forEach(shopItem => {
        const id = shopItem.getAttribute("data-id");
        const name = shopItem.getAttribute("data-name");
        const price = shopItem.getAttribute("data-price");
        const button = shopItem.getElementsByTagName("button")[0];

        button.addEventListener("click", () => {
            itemId = parseInt(id);
            modalItemName.innerText = name;
            modalItemPrice.innerText = price;
            modal.show();
        });
    });
})();
