document.addEventListener('DOMContentLoaded', () => {
    const $toggleUserModalButton = $('.js-toggle-user-modal');

    $toggleUserModalButton.on('click', () => {
        $('.js-user-modal').modal('toggle');
    });

    feather.replace();
});