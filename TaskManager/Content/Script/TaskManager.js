$(document).ready(function () {

    // Редактирование проектов
    $(document).on('click', '.ico-edit', function (e) {
        e.preventDefault();

        var _id = $(this).data('id');

        $("#loading").show();

        $.ajaxSetup({ cache: false });

        $.ajax({
            type: "GET",
            url: "/Home/ProjectEdit/",
            data: { id: _id },
            success: function (data) {
                $("#loading").hide();
                $('.modals-dialog').html(data);
                $('.modal').modal('show');
            }
        });
    });

    // возврат
    $(document).on('click', '.ico-back', function () {
        var _id = $(this).data('id');
        window.location.host;
        window.location.href = '/Home/Index/';

        console.log(window.location.href);
    });

    // Данные пользователя
    $(document).on('click', '.ico-info', function () {
        var _id = $(this).data('id');
        window.location.host;
        window.location.href = '/Accaunt/UserDataDetails/' + _id;

        console.log(window.location.href);
    });

    // Удаление проекта
    $(document).on('click', '.ico-delete', function (e) {
        var _id = $(this).data('id');
        var el_tr = $(this).closest("tr");
        var el_tbody = $(el_tr).closest('tbody');

        if (confirm("Вы действительно хотите удалить?")) {

            $.ajax({
                type: "POST",
                url: "/Accaunt/DeleteProject/",
                // передача в качестве объекта
                // поля будут закодированые через encodeURIComponent автоматически
                data: { id: _id },
                success: function (data) {

                    if (data.result === true) {
                        el_tbody.find(el_tr).remove();
                    }
                    else {
                        alert("Данная запись не найдена.");
                    }
                   
                    console.log(data);
                    console.log(data.msg);
                    
                },
                error: function (data) {
                    alert('Нет ответа от сервера.');
                }
            });
        }
    });

    $(document).on('click', '#search', function (e) {
        e.preventDefault();

        var name = $('#name').val();

        $("#loading").show();

        $.ajaxSetup({ cache: false });

        $.ajax({
            type: "POST",
            url: "/Home/ProjectSearch/",
            data: { name: name },
            success: function (data) {
                $("#loading").hide();
                $('#results').html(data);
            }
        });

    });
});