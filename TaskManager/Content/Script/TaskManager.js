$(document).ready(function () {

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

    // Удаление задачи.
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

        window.location.host;
        window.location.href = '/Home/ListProject/';

    });

    // Открытие создания проекта.
    $(document).on('click', '#getProjectCreate', function (e) {
        e.preventDefault();

        $("#loading").show();

        $.ajax({
            type: "GET",
            url: "/Home/CreateProject/",
            success: function (data) {

                $("#loading").hide();
                $('.modals-dialog').html(data);
                $('.modal').modal('show');
            },
            error: function (data) {
                alert('Нет ответа от сервера.');
            }
        });
    });

    // Добавление созданного проекта в представление.
    $(document).on('click', '#postProjectCreate', function (e) {
        e.preventDefault();

        var container = this.closest('.modal-content');

        var name = $(container).find('#Name').val();
        var userId = $(container).find('#UserId').val();

        console.log(name);
        
        $.ajax({
            type: "POST",
            url: "/Home/CreateProject/",
            data: { Name: name, UserId: userId },
            success: function (data) {

                if (data.result == true) {
                    $('.modal').modal('hide');
                    $(document).find('tbody').append(
                        '<tr><td>' + data.ProjectName + 
                        '</td><td>' + data.ProjectManager + 
                        '</td><td>' + data.DateCreate +
                        '</td><td></td>' +
                        '<td><span hidden = "hidden" id="idProject" data-id=' + data.ProjectId + '></span >' +
                        '<span title="Редактировать проект" id="getProjectEdit" class="ico-edit icon-button">' +
                        '</span><span title="Список задач" id="projectTaskList" class="ico-tasklist icon-button"></span>' +
                        '<span title="Удалить проект" id="projectDelete" class="ico-delete icon-button"></span></td >'
                    );
                }
                else (data.result == false)
                {
                    var abc = $(container).find
                    var _id = $(this).data('id');

                    alert('Ошибка валидации.');
                }
            },
            error: function (data) {
                alert(data);
            }
        });
    });

    // открытие окна редактирование проектов.
    $(document).on('click', '#getProjectEdit', function (e) {
        e.preventDefault();

        var container = this.closest('td');
        var _id = $(container).find('#idProject').data('id');

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

    // Изменение редактируемого проекта в представление. Необходимо допилить.
    $(document).on('click', '#postProjectEdit', function (e) {
        e.preventDefault();

        var container = this.closest('.modal-content');

        var name = $(container).find('#Name').val();
        var userId = $(container).find('#UserId').val();

        console.log(name);

        $.ajax({
            type: "POST",
            url: "/Home/ProjectEdit/",
            data: { Name: name, UserId: userId },
            success: function (data) {

                if (data.result == true) {
                    $('.modal').modal('hide');
                    $(document).find('tbody').append(
                        '<tr><td>' + data.ProjectName +
                        '</td><td>' + data.ProjectManager +
                        '</td><td>' + data.DateCreate +
                        '</td><td></td>' +
                        '<td><span hidden = "hidden" id="idProject" data-id=' + data.ProjectId + '></span >' +
                        '<span title="Редактировать проект" id="getProjectEdit" class="ico-edit icon-button">' +
                        '</span><span title="Список задач" id="projectTaskList" class="ico-tasklist icon-button"></span>' +
                        '<span title="Удалить проект" id="projectDelete" class="ico-delete icon-button"></span></td >'
                    );
                }
                else {
                    alert('Ошибка валидации.');
                }
            },
            error: function (data) {
                alert(data);
            }
        });
    });

    // Список задач по проекту
    $(document).on('click', '#projectTaskList', function (e) {
        e.preventDefault();

        var container = this.closest('td');
        var _id = $(container).find('#idProject').data('id');

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

    // Создание задачи по проекту.
    $(document).on('click', '#createTask', function (e) {
        e.preventDefault();

        var _id = $(this).data('id');

        $("#loading").show();

        $.ajax({
            type: "GET",
            url: "/Home/CreateTask/",
            data: { id: _id },
            success: function (data) {
                $("#loading").hide();
                $('.modals-dialog').html(data);
                $('.modal').modal('show');
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