$(document).ready(function () {

    // Редактирование проектов
    $(document).on('click', '#projectEdit', function (e) {
        e.preventDefault();

        var _id = $(this).data('id');

        $("#loading").show();

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

    // Подробности пользователя.
    $(document).on('click', '#userDataDetails', function (e) {
        e.preventDefault();

        var _id = $(this).data('id');

        $("#loading").show();

        $.ajax({
            type: "GET",
            url: "/Accaunt/UserDataDetails/",
            data: { id: _id },
            success: function (data) {
                $("#loading").hide();
                $('.modals-dialog').html(data);
                $('.modal').modal('show');
            }
        });
    });

    // Редактирование данных пользователя.
    $(document).on('click', '#editUserData', function (e) {
        e.preventDefault();

        var _id = $(this).data('id');

        $("#loading").show();

        $.ajax({
            type: "GET",
            url: "/Accaunt/EditUserData/",
            data: { id: _id },
            success: function (data) {
                $("#loading").hide();
                $('.modals-dialog').html(data);
                $('.modal').modal('show');
            }
        });
    });

    // возврат, пока не используется.
    $(document).on('click', '.ico-back', function () {
        var _id = $(this).data('id');
        window.location.host;
        window.location.href = '/Home/Index/';

        console.log(window.location.href);
    });

    // Удаление задачи, пока не работает корректно. Допилить.
    $(document).on('click', '#TaskDelete', function (e) {
        var _id = $(this).data('id');
        var el_tr = $(this).closest("tr");
        var el_tbody = $(el_tr).closest('tbody');

        if (confirm("Вы действительно хотите удалить?")) {

            $.ajax({
                type: "POST",
                url: "/Home/TaskDelete/",
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

    // поиск
    $(document).on('click', '#search', function (e) {
        e.preventDefault();

        var name = $('#name').val();

        $("#loading").show();

        $.ajax({
            type: "POST",
            url: "/Home/ProjectSearch/",
            data: { name: name },
            success: function (data) {
                $('#results').html(data);
                $("#loading").hide();
            }
            });
        });

    // Список проектов
    $(document).on('click', '#projectList', function (e) {
        e.preventDefault();

        $("#loading").show();

        $.ajax({
            type: "GET",
            url: "/Home/ListProject/",
            success: function (data) {
                $('#results').html(data);
                $("#loading").hide();
            }
        });
    });

    // Список задач по проекту
    $(document).on('click', '#projectTaskList', function (e) {
        e.preventDefault();

        var _id = $(this).data('id');

        $("#loading").show();

        $.ajax({
            url: "/Home/ListTask/",
            data: { id: _id },
            success: function (data) {
                $('#taskRes').html(data);
                $("#loading").hide();
            }
        });
    });

    // Список пользователей.
    $(document).on('click', '#listUser', function (e) {
        e.preventDefault();

        $("#loading").show();

        $.ajax({
            type: "GET",
            url: "/Accaunt/ListUser/",
            success: function (data) {
                $('#results').html(data);
                $("#loading").hide();
            }
        });
    });

});