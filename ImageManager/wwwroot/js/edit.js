let btnAll = $('#btn-all');
let btnDelete = $('#btn-delete');
let btnCancel = $('#btn-cancel');
let btnEdit = $('#btn-edit');
let checkBoxes = $('input[type=checkbox]');
let card = $('.img-card');
let aCard = card.parent('a');
hideBtn();

btnEdit.click(() => {
    showBtn();
    aCard.on('click', (event) => event.preventDefault());
});

btnAll.click(() => toggleCheckBox(checkBoxes));

btnDelete.click(() => {
    $.ajax({
        url: $('#script-edit').data('url'),
        type: 'post',
        data: $('input[name=selectedItems]:checked'),
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
    card.off('click');
    card.on('click',
        function() {
            $('#modal').modal('show');
            $('.carousel-item').removeClass('active');
            $(`#slide-${$(this).find('.card-img-top').data('id')}`).first().addClass('active');
        });
}

function showBtn() {
    btnAll.show();
    btnDelete.show();
    btnCancel.show();
    btnEdit.hide();
    card.off('click');
    card.on('click',
        function() {
            const currCheck = $(this).find('input');
            toggleCheckBox(currCheck);
        });
}

function clear() {
    checkBoxes.prop('checked', false);
    aCard.off('click');
    hideBtn();
}

function toggleCheckBox(checkBox) {
    checkBox.prop('checked', !checkBox.prop('checked'));
}

var carousel = $('#carousel');
$(document).keydown((e) => {
    if (e.keyCode === 37) {
        carousel.carousel('prev');
    } else if (e.keyCode === 39) {
        carousel.carousel('next');
    }
});