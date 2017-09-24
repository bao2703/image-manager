var btnAll = $("#btn-all");
var btnDelete = $("#btn-delete");
var btnCancel = $("#btn-cancel");
var btnEdit = $("#btn-edit");
var checkbox = $("input[type=checkbox]");
var img = $(".img-fluid");
hideBtn();

btnEdit.click(function() {
    showBtn();
    checkbox.attr("disabled", false);
    img.on("click",
        function() {
            $(this).toggleClass("p-1");
        });
});

btnAll.click(function() {
    checkbox.prop("checked", !checkbox.prop("checked"));
});

btnDelete.click(function() {
    $.ajax({
        url: "/Image/Delete",
        type: "post",
        data: $("input[name=selectedImages]:checked"),
        success: function() {
            location.reload();
        }
    });
});

btnCancel.click(function() {
    clear();
});

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
    checkbox.prop("checked", false);
    checkbox.attr("disabled", true);
    img.removeClass("p-1");
    img.off("click");
    hideBtn();
}