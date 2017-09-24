let btnAll = $("#btn-all");
let btnDelete = $("#btn-delete");
let btnCancel = $("#btn-cancel");
let btnEdit = $("#btn-edit");
let checkBoxes = $("input[type=checkbox]");
let card = $(".card");
let aCard = card.parent("a");
hideBtn();

btnEdit.click(() => {
    showBtn();
    card.on("click",
        function() {
            const currCheck = $(`#${$(this).find("img").data("id")}`);
            toggleCheckBox(currCheck);
        });
    aCard.on("click", (event) => event.preventDefault());
});

btnAll.click(() => toggleCheckBox(checkBoxes));

btnDelete.click(() => {
    $.ajax({
        url: $("#script-edit").data("url"),
        type: "post",
        data: $("input[name=selectedItems]:checked"),
        success: function() {
            location.reload();
        }
    });
});

btnCancel.click(clear);

function hideBtn() {
    btnAll.hide();
    btnDelete.hide();
    btnCancel.hide();
    btnEdit.show();
}

function showBtn() {
    btnAll.show();
    btnDelete.show();
    btnCancel.show();
    btnEdit.hide();
}

function clear() {
    checkBoxes.prop("checked", false);
    card.off("click");
    aCard.off("click");
    hideBtn();
}

function toggleCheckBox(checkBox) {
    checkBox.prop("checked", !checkBox.prop("checked"));
}